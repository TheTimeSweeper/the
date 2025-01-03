﻿using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KatamariMod.Modules.Characters
{
    public abstract class CharacterBase<T> where T : CharacterBase<T>, new()
    {
        public abstract string assetBundleName { get; }

        public abstract string bodyName { get; }
        public abstract string modelPrefabName { get; }

        private BodyInfo _cachedBodyInfo;
        public BodyInfo bodyInfo
        {
            get
            {
                if(_cachedBodyInfo == null)
                {
                    _cachedBodyInfo = newBodyInfo;
                }
                return _cachedBodyInfo;
            }
        }
        protected abstract BodyInfo newBodyInfo { get; }

        public virtual CustomRendererInfo[] customRendererInfos { get; }

        public virtual ItemDisplaysBase itemDisplays { get; }

        public static T instance { get; private set; }

        public AssetBundle assetBundle { get; private set; }

        public GameObject bodyPrefab;
        public CharacterBody prefabCharacterBody;
        public GameObject characterModelObject;
        public CharacterModel prefabCharacterModel;

        public void Initialize()
        {
            instance = this as T;
            assetBundle = Asset.LoadAssetBundle(assetBundleName);

            InitializeCharacter();
        }

        public virtual void InitializeCharacter()
        {
            InitializeCharacterBodyPrefab();

            InitializeItemDisplays();
        }

        protected virtual void InitializeCharacterBodyPrefab()
        {
            //ability to pass in an existing model
            if(characterModelObject == null)
            {
                characterModelObject = Prefabs.LoadCharacterModel(assetBundle, modelPrefabName);
            }

            bodyPrefab = Modules.Prefabs.CreateBodyPrefab(characterModelObject, bodyInfo);
            prefabCharacterBody = bodyPrefab.GetComponent<CharacterBody>();

            prefabCharacterModel = Modules.Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfos);
        }

        public virtual void InitializeItemDisplays() {
            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            itemDisplayRuleSet.name = "idrs" + bodyName;
            
            prefabCharacterModel.itemDisplayRuleSet = itemDisplayRuleSet;

            if (itemDisplays != null) {
                RoR2.ContentManagement.ContentManager.onContentPacksAssigned += SetItemDisplays;
            }
        }

        public void SetItemDisplays(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj) {
            itemDisplays.SetItemDisplays(prefabCharacterModel.itemDisplayRuleSet);
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

        /// <summary> body prefab you're cloning for your character- commando is the safest </summary>
        public string bodyNameToClone = "Commando";

        public Color bodyColor = Color.white;

        public Texture characterPortrait = null;

        public float sortPosition = 100f;

        public GameObject crosshair = null;
        public GameObject podPrefab = null;
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
        #endregion camera
    }
}
