using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections.Generic;
using RoR2.UI;
using System;
using System.Linq;

namespace Modules {
    public static class Assets
    {
        // the assetbundle to load assets from
        // survivor todo: separate assets between survivors
        public static AssetBundle mainAssetBundle;
        public static AssetBundle teslaAssetBundle;

        //HENRY: indev
        #region indev
        // lists of assets to add to contentpack

        // cache these and use to create our own materials
        public static Shader hotpoo = Resources.Load<Shader>("Shaders/Deferred/HGStandard");
        private static string[] assetNames = new string[0];
        #endregion

        #region MY NAME'S NOT HENRY
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;
        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;
        #endregion
        
        // CHANGE THIS
        private const string assetbundleName = "joe";
        
        //jerry don't you know
        public static GameObject JoeFireball = null;
        public static GameObject JoeImpactEffect = null;
        public static GameObject JoeJumpSwingEffect = null;
        
        public static GameObject TeslaCoil = null;
        public static GameObject TeslaCoilBlueprint = null;

        public static void Initialize()
        {
            //HENRY: check this somewhere else secretly
            if (assetbundleName == "myassetbundle")
            {
                Debug.LogError("AssetBundle name hasn't been changed- not loading any assets to avoid conflicts");
                return;
            }

            LoadAssetBundle();

            LoadSoundbank();
            //PopulateHenrysAssetsThatNoLongerExist();
            PopulateAss();
        }

        private static void PopulateAss() {

            JoeFireball = mainAssetBundle.LoadAsset<GameObject>("JoeFireballBasic");

            JoeImpactEffect = LoadEffect("JoeImpactEffectBasic");
            JoeJumpSwingEffect = LoadEffect("JoeJumpSwingParticlesesEffect");

            TeslaCoil = teslaAssetBundle.LoadAsset<GameObject>("TeslaCoil");
            TeslaCoilBlueprint = teslaAssetBundle.LoadAsset<GameObject>("TeslaCoilBlueprint");

            swordHitSoundEvent = CreateNetworkSoundEventDef("HenrySwordHit");
        }

        public static void PopulateHenrysAssetsThatNoLongerExist()
        {
            if (!mainAssetBundle)
            {
                Debug.LogError("There is no AssetBundle to load assets from.");
                return;
            }
            
            // feel free to delete everything in here and load in your own assets instead
            // it should work fine even if left as is- even if the assets aren't in the bundle

            swordHitSoundEvent = CreateNetworkSoundEventDef("HenrySwordHit");

            bombExplosionEffect = LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (bombExplosionEffect)
            {
                ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
                shakeEmitter.amplitudeTimeDecay = true;
                shakeEmitter.duration = 0.5f;
                shakeEmitter.radius = 200f;
                shakeEmitter.scaleShakeRadiusWithLocalScale = false;

                shakeEmitter.wave = new Wave
                {
                    amplitude = 1f,
                    frequency = 40f,
                    cycleOffset = 0f
                };
            }

            swordSwingEffect = Assets.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = Assets.LoadEffect("ImpactHenrySlash");
        }

        public static void LoadAssetBundle()
        {
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("JoeMod." + assetbundleName))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            if (teslaAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("JoeMod.teslatrooper"))
                {
                    teslaAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            assetNames = mainAssetBundle.GetAllAssetNames();
            assetNames.Concat(teslaAssetBundle.GetAllAssetNames());
        }

        public static T Load<T>(string assString) where T : UnityEngine.Object => LoadAsset<T>(assString);
        public static T LoadAsset<T>(string assString) where T : UnityEngine.Object
        {
            T loadedAss = RoR2.LegacyResourcesAPI.Load<T>(assString);


            if (loadedAss == null) {
                loadedAss = mainAssetBundle.LoadAsset<T>(assString);
            }
            if(loadedAss == null)
            {
                loadedAss = teslaAssetBundle.LoadAsset<T>(assString);
            }
            
            if(loadedAss == null)
            {
                Debug.LogError($"Null asset: {assString}.\nAttempt to load asset '{assString}' from assetbundles returned null");
            }

            return loadedAss;
        }

        public static GameObject LoadSurvivorModel(string modelName) {
            GameObject model = Modules.Assets.LoadAsset<GameObject>(modelName);
            if (model == null) {
                Debug.LogError("Trying to load a null model- check to see if the name in your code matches the name of the object in Unity");
                return null;
            }

            return PrefabAPI.InstantiateClone(model, model.name, false);
        }

        public static void LoadSoundbank()
        {
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("JoeMod.HenryBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("JoeMod.Tesla_Trooper.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        private static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if (Resources.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = 250f;
            newTracer.GetComponent<Tracer>().length = 50f;

            AddNewEffectDef(newTracer);

            return newTracer;
        }

        public static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            Modules.Content.AddNetworkSoundEventDef(networkSoundEventDef);

            return networkSoundEventDef;
        }

        public static void ConvertAllRenderersToHopooShader(GameObject objectToConvert) {
            if (!objectToConvert) return;

            foreach (MeshRenderer i in objectToConvert.GetComponentsInChildren<MeshRenderer>()) {
                if (i?.sharedMaterial != null) {
                    i.sharedMaterial.SetHotpooMaterial();
                }
            }

            foreach (SkinnedMeshRenderer i in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                if (i?.sharedMaterial != null) {
                    i.sharedMaterial.SetHotpooMaterial();
                }
            }
        }

        public static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] rendererInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                rendererInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            return rendererInfos;
        }

        public static Texture LoadCharacterIconGeneric(string characterName)
        {
            return LoadAsset<Texture>("tex" + characterName + "Icon");
        }

        public static Texture LoadCharacterIcon(string name) {
            return LoadAsset<Texture>(name);
        }
        /// <summary>
        /// search for crosshair prefabs here. plug in the character or crosshair name
        /// </summary>
        /// <para>https://xiaoxiao921.github.io/GithubActionCacheTest/assetPathsDump.html</para>
        public static GameObject LoadCrosshair(string crosshairName)
        {
            if (Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return Resources.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName)
        {
            return LoadEffect(resourceName, "", false);
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            return LoadEffect(resourceName, soundName, false);
        }

        private static GameObject LoadEffect(string resourceName, bool parentToTransform)
        {
            return LoadEffect(resourceName, "", parentToTransform);
        }

        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform)
        {
            bool assetExists = false;
            for (int i = 0; i < assetNames.Length; i++)
            {
                if (assetNames[i].Contains(resourceName.ToLower()))
                {
                    assetExists = true;
                    i = assetNames.Length;
                }
            }

            if (!assetExists)
            {
                Debug.LogError("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            GameObject newEffect = LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;

            EffectComponent effect = newEffect.GetComponent<EffectComponent>();
            if (!effect) effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = true;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddNewEffectDef(newEffect, soundName);

            return newEffect;
        }

        private static void AddNewEffectDef(GameObject effectPrefab)
        {
            AddNewEffectDef(effectPrefab, "");
        }

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName)
        {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = soundName;

            Modules.Content.AddEffectDef(newEffectDef);
        }

        #region materials(old)
        private const string obsolete = "use `Materials.CreateMaterial` instead, or use the extension `Material.SetHotpooMaterial` directly on a material";
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName) => Assets.CreateMaterial(materialName, 0f);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission) => Assets.CreateMaterial(materialName, emission, Color.white);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission, Color emissionColor) => Assets.CreateMaterial(materialName, emission, emissionColor, 0f);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            return Materials.CreateHotpooMaterial(materialName)
                            .MakeUnique()
                            .SetEmission(emission, emissionColor)
                            .SetNormal(normalStrength);
        }
        #endregion materials(old)

        #region materials new (simple)(example)
        //public static Material CreateHotpooMaterial(string materialName) {

        //    //Material mat = UnityEngine.Object.Instantiate<Material>(Assets.commandoMat);
        //    Material tempMat = Assets.LoadAsset<Material>(materialName);

        //    if (!tempMat) {
        //        Debug.LogError("Failed to load material: " + materialName + " - Check to see that the name in your Unity project matches the one in this code");
        //        return new Material(Assets.hotpoo);
        //    }

        //    return new Material(tempMat).SetHotpooMaterial;
        //}

        //public static Material SetHotpooMaterial(this Material tempMat) {

        //    float? bumpScale = null;
        //    Color? emissionColor = null;

        //    //grab values before the shader changes
        //    if (tempMat.IsKeywordEnabled("_NORMALMAP")) {
        //        bumpScale = tempMat.GetFloat("_BumpScale");
        //    }
        //    if (tempMat.IsKeywordEnabled("_EMISSION")) {
        //        emissionColor = tempMat.GetColor("_EmissionColor");
        //    }

        //    tempMat.shader = Assets.hotpoo;

        //    tempMat.SetColor("_Color", tempMat.GetColor("_Color"));
        //    tempMat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
        //    tempMat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));

        //    if (bumpScale != null) {
        //        tempMat.SetFloat("_NormalStrength", (float)bumpScale);
        //    }
        //    if (emissionColor != null) {
        //        tempMat.SetColor("_EmColor", (Color)emissionColor);
        //        tempMat.SetFloat("_EmPower", 1);
        //    }

        //    return tempMat;
        //}
        #endregion materials new (simple)(example)
    }
}