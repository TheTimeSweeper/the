using R2API;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.Modules.Characters
{
    public abstract class CharacterBase<T> where T : CharacterBase<T>, new()
    {
        public abstract string assetBundleName { get; }

        public abstract string bodyName { get; }
        public abstract string modelPrefabName { get; }
        
        public abstract BodyInfo bodyInfo { get; }
        private BodyInfo cachedBodyInfo;
        protected BodyInfo _bodyInfo
        {
            get
            {
                if(cachedBodyInfo == null)
                {
                    cachedBodyInfo = bodyInfo;
                }
                return cachedBodyInfo;
            }
            set
            {
                cachedBodyInfo = value;
            }
        }

        public virtual CustomRendererInfo[] customRendererInfos { get; }

        public virtual ItemDisplaysBase itemDisplays { get; }

        public static T instance { get; private set; }

        public virtual AssetBundle assetBundle { get; protected set; }

        public virtual GameObject bodyPrefab                 {get; protected set;}
        public virtual CharacterBody prefabCharacterBody     {get; protected set;}
        public virtual GameObject characterModelObject       {get; protected set;}
        public virtual CharacterModel prefabCharacterModel   {get; protected set;}

        private BodyIndex? _lazyBodyIndex;
        public BodyIndex bodyIndex
        {
            get
            {
                if (_lazyBodyIndex == null)
                {
                    if(bodyPrefab == null)
                    {
                        Log.Error("Cannot get BodyIndex. Body has not been created.");
                        return default(BodyIndex);
                    }
                    BodyIndex bodyIndex = bodyPrefab.GetComponent<CharacterBody>().bodyIndex;
                    if (bodyIndex == default(BodyIndex)) {
                        Log.Error("Cannot get BodyIndex. Body has not been registered.");
                        return bodyIndex;
                    }

                    _lazyBodyIndex = bodyIndex;
                }
                return _lazyBodyIndex.Value;
            }
        }

        public virtual void Initialize()
        {
            instance = this as T;

            Log.CurrentTime($"{bodyName} init");

            Asset.LoadAssetBundleAsync(assetBundleName, (loadedAssetBundle) => {

                Log.CurrentTime($"{bodyName} assetbundle loaded");

                assetBundle = loadedAssetBundle;
                ContentPacks.asyncLoadCoroutines.Add(LoadAssetsThenInitializeCharacter());

                if (_bodyInfo == null)
                    return;

                ContentPacks.asyncLoadCoroutines.Add(_bodyInfo.FinalizeBodyInfoAsync(assetBundle));
                if(_bodyInfo.asyncLoads != null)
                {
                    ContentPacks.asyncLoadCoroutines.AddRange(_bodyInfo.asyncLoads);
                }
            });
        } 

        public virtual List<IEnumerator> GetAssetBundleInitializedCoroutines()
        {
            return null;
        }

        public virtual IEnumerator LoadAssetsThenInitializeCharacter()
        {
            List<IEnumerator> subEnumerators = GetAssetBundleInitializedCoroutines();
            if (subEnumerators != null)
            {
                for (int i = 0; i < subEnumerators.Count; i++)
                {
                    while (subEnumerators[i].MoveNext()) yield return null;
                }
            }
            InitializeCharacter();
            yield break;
        }

        public virtual void InitializeCharacter()
        {
            InitializeCharacterBodyPrefab();
        }

        public abstract void OnCharacterInitialized();

        protected virtual void InitializeCharacterBodyPrefab()
        {
            ContentPacks.asyncLoadCoroutines.Add(Prefabs.LoadCharacterModelAsync(assetBundle, modelPrefabName, (modelResult) =>
            {
                characterModelObject = modelResult.InstantiateClone(modelPrefabName);

                ContentPacks.asyncLoadCoroutines.Add(Prefabs.CloneCharacterBodyAsync(characterModelObject, _bodyInfo, (bodyResult) =>
                {
                    bodyPrefab = bodyResult;
                    prefabCharacterBody = bodyPrefab.GetComponent<CharacterBody>();

                    prefabCharacterModel = Modules.Prefabs.SetupCharacterModel(bodyPrefab, _bodyInfo, customRendererInfos);

                    Log.CurrentTime($"{bodyName} body prefab complete");
                    
                    InitializeItemDisplays();

                    ContentPacks.asyncLoadCoroutines.Add(LoadAssetsThenFinalizeCharacter());
                }));
            }));

            //characterModelObject = Prefabs.LoadCharacterModel(assetBundle, modelPrefabName);

            //bodyPrefab = Modules.Prefabs.CreateBodyPrefab(characterModelObject, bodyInfo);
            //prefabCharacterBody = bodyPrefab.GetComponent<CharacterBody>();

            //prefabCharacterModel = Modules.Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfos);
        }

        public virtual void InitializeItemDisplays() {
            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            itemDisplayRuleSet.name = "idrs" + bodyName;
            
            prefabCharacterModel.itemDisplayRuleSet = itemDisplayRuleSet;

            if (itemDisplays != null)
            {
                itemDisplays.SetItemDisplays(prefabCharacterModel.itemDisplayRuleSet);
            }
        }

        public virtual List<IEnumerator> GetCoroutinesAfterPrefabCreation()
        {
            return null;
        }

        public virtual IEnumerator LoadAssetsThenFinalizeCharacter()
        {
            List<IEnumerator> subEnumerators = GetCoroutinesAfterPrefabCreation();
            if (subEnumerators != null)
            {
                for (int i = 0; i < subEnumerators.Count; i++)
                {
                    while (subEnumerators[i].MoveNext()) yield return null;
                }
            }
            OnCharacterInitialized();
            yield break;
        }

        public abstract void InitializeEntityStateMachines();

        public abstract void InitializeSkills();

        public abstract void InitializeSkins();

        public abstract void InitializeCharacterMaster();

    }

    // for simplifying characterbody creation
    public class BodyInfo
    {
        #region Character
        public string bodyName = "";
        public string bodyNameToken = "";
        public string subtitleNameToken = "";

        /// <summary> the body prefab you're cloning for your character- commando is the safest </summary>
        [Obsolete("I recommend loading asynchronously instead, using bodyToClonePath.")]
        public string bodynameToClone = "Commando";

        /// <summary> addressable path for the body prefab you're cloning for your character- commando is the safest </summary>
        public string bodyToClonePath = "RoR2/Base/Commando/CommandoBody.prefab";

        public Color bodyColor = Color.white;

        public float sortPosition = 100f;

        public CharacterBody.BodyFlags bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;

        /// <summary>
        /// pass in instead a bundle path or addressable path to load async
        /// </summary>
        public Texture characterPortrait = null;
        public string characterPortraitBundlePath = null;
        public string characterPortraitAddressablePath = null;

        /// <summary>
        /// pass in instead a bundle path or addressable path to load async
        /// </summary>
        public GameObject crosshair = null;
        public string crosshairBundlePath = null;
        public string crosshairAddressablePath = null;

        /// <summary>
        /// pass in instead a bundle path or addressable path to load async
        /// </summary>
        public GameObject podPrefab = null;
        public string podPrefabBundlePath = null;
        public string podPrefabAddressablePath = null;

        public List<IEnumerator> asyncLoads = null;
        #endregion Character

        #region Stats
        //main stats
        public float maxHealth = 100f;
        public float healthRegen = 1f;
        public float armor = 0f;
        public float shield = 0f; // base shield is a thing apparently. neat

        public int jumpCount = 1;

        //conventional base stats, consistent for all survivors
        public float damage = 12f;
        public float attackSpeed = 1f;
        public float crit = 1f;

        //misc stats
        public float moveSpeed = 7f;
        public float acceleration = 80f;
        public float jumpPower = 15f;

        //stat growth
        /// <summary>
        /// Leave this alone, and you don't need to worry about setting any of the stat growth values. They'll be set at the consistent ratio that all vanilla survivors have.
        /// <para>If You do, healthGrowth should be maxHealth * 0.3f, regenGrowth should be healthRegen * 0.2f, damageGrowth should be damage * 0.2f</para>
        /// </summary>
        public bool autoCalculateLevelStats = true;

        public float healthGrowth = 100f * 0.3f;
        public float regenGrowth = 1f * 0.2f;
        public float armorGrowth = 0f;
        public float shieldGrowth = 0f;

        public float damageGrowth = 12f * 0.2f;
        public float attackSpeedGrowth = 0f;
        public float critGrowth = 0f;

        public float moveSpeedGrowth = 0f;
        public float jumpPowerGrowth = 0f;// jump power per level exists for some reason
        #endregion Stats

        #region Camera
        public Vector3 aimOriginPosition = new Vector3(0f, 1.6f, 0f);
        public Vector3 modelBasePosition = new Vector3(0f, -0.92f, 0f);

        /// <summary> basically the "height" of your camera </summary>
        public Vector3 cameraPivotPosition = new Vector3(0f, 0.8f, 0f);
        
        /// <summary> how far relative to the pivot is your camera's center </summary>
        public float cameraParamsVerticalOffset = 1.37f;

        /// <summary> large characters like loader are -12. for smaller characters like commando go for -10 maybe -9 </summary>
        public float cameraParamsDepth = -10;

        internal bool hasAimAnimator = true;
        internal bool hasCharacterDirection = true;
        internal bool hasFoostepController = true;
        internal bool hasRagdoll = true;
        internal bool hasSkinController = true;

        private CharacterCameraParams _cameraParams;
        public CharacterCameraParams cameraParams
        {
            get
            {
                if (_cameraParams == null)
                {
                    _cameraParams = ScriptableObject.CreateInstance<CharacterCameraParams>();
                    _cameraParams.data.minPitch = -70;
                    _cameraParams.data.maxPitch = 70;
                    _cameraParams.data.wallCushion = 0.1f;
                    _cameraParams.data.pivotVerticalOffset = cameraParamsVerticalOffset;
                    _cameraParams.data.idealLocalCameraPos = new Vector3(0, 0, cameraParamsDepth);
                }
                return _cameraParams;
            }
            set => _cameraParams = value;
        }

        public IEnumerator FinalizeBodyInfoAsync(AssetBundle assetBundle)
        {
            List<IEnumerator> bodyInfoLoads = new List<IEnumerator>();

            if (characterPortrait == null)
            {
                bodyInfoLoads.Add(Asset.LoadFromAddressableOrBundle<Texture>(
                    assetBundle,
                    characterPortraitBundlePath,
                    characterPortraitAddressablePath,
                    (result) => characterPortrait = result));
            }
            if (crosshair == null)
            {
                bodyInfoLoads.Add(Asset.LoadFromAddressableOrBundle<GameObject>(
                    assetBundle,
                    crosshairBundlePath,
                    crosshairAddressablePath,
                    (result) => crosshair = result));
            }
            if (podPrefab == null)
            {
                bodyInfoLoads.Add(Asset.LoadFromAddressableOrBundle<GameObject>(
                    assetBundle,
                    podPrefabBundlePath,
                    podPrefabAddressablePath,
                    (result) => podPrefab = result));
            }

            for (int i = 0; i < bodyInfoLoads.Count; i++)
            {
                while (bodyInfoLoads[i] != null && bodyInfoLoads[i].MoveNext())
                {
                    yield return null;
                }
            }
        }
        #endregion camera
    }
}
