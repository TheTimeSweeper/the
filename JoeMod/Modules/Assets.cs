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
using RoR2.Projectile;

namespace Modules {
    internal static class Assets
    {
        public static AssetBundle mainAssetBundle;
        public static AssetBundle teslaAssetBundle;

        //HENRY: indev
        #region indev
        // lists of assets to add to contentpack

        // cache these and use to create our own materials
        public static Shader hotpoo = RoR2.LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/HGStandard");
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
        
        private const string assetbundleName = "joe";
        #region joe
        //jerry don't you know
        public static GameObject JoeFireball;
        public static GameObject JoeImpactEffect;
        public static GameObject JoeJumpSwingEffect;
        #endregion

        #region tesla
        public static GameObject TeslaCoil;
        public static GameObject TeslaCoilBlueprint;

        public static GameObject TeslaIndicatorPrefab;

        public static GameObject TeslaLoaderZapConeProjectile;
        public static GameObject TeslaZapConeEffect;

        public static Material ChainLightningMaterial;
        #endregion

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

        public static void PopulateHenrysAssetsThatNoLongerExist() {
            if (!mainAssetBundle) {
                Debug.LogError("There is no AssetBundle to load assets from.");
                return;
            }

            // feel free to delete everything in here and load in your own assets instead
            // it should work fine even if left as is- even if the assets aren't in the bundle

            swordHitSoundEvent = CreateNetworkSoundEventDef("HenrySwordHit");

            bombExplosionEffect = LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (bombExplosionEffect) {
                ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
                shakeEmitter.amplitudeTimeDecay = true;
                shakeEmitter.duration = 0.5f;
                shakeEmitter.radius = 200f;
                shakeEmitter.scaleShakeRadiusWithLocalScale = false;

                shakeEmitter.wave = new Wave {
                    amplitude = 1f,
                    frequency = 40f,
                    cycleOffset = 0f
                };
            }

            swordSwingEffect = Assets.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = Assets.LoadEffect("ImpactHenrySlash");
        }

        private static void PopulateAss() {

            JoeFireball = mainAssetBundle.LoadAsset<GameObject>("JoeFireballBasic");

            JoeImpactEffect = LoadEffect("JoeImpactEffectBasic");
            JoeJumpSwingEffect = LoadEffect("JoeJumpSwingParticlesesEffect");

            TeslaCoilBlueprint = teslaAssetBundle.LoadAsset<GameObject>("TeslaCoilBlueprint");

            TeslaIndicatorPrefab = CreateTeslaTrackingIndicator();

            ChainLightningMaterial = FindChainLightningMaterial();

            TeslaLoaderZapConeProjectile = CreateZapConeProjectile();

            //swordHitSoundEvent = CreateNetworkSoundEventDef("HenrySwordHit");
        }

        private static GameObject CreateZapConeProjectile() {
            GameObject zapConeProjectile = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LoaderZapCone"), "TeslaLoaderZapCone");
            
            ProjectileProximityBeamController beamController = zapConeProjectile.GetComponent<ProjectileProximityBeamController>();
            beamController.attackFireCount = ModdedEntityStates.TeslaTrooper.ZapPunch.OrbCasts;
            beamController.attackRange = ModdedEntityStates.TeslaTrooper.ZapPunch.OrbDistance;
            beamController.maxAngleFilter = 50;
            beamController.procCoefficient = ModdedEntityStates.TeslaTrooper.ZapPunch.ProcCoefficient;
            beamController.damageCoefficient = ModdedEntityStates.TeslaTrooper.ZapPunch.OrbDamageCoefficient;
            beamController.lightningType = RoR2.Orbs.LightningOrb.LightningType.MageLightning;

            UnityEngine.Object.DestroyImmediate(zapConeProjectile.transform.Find("Effect").GetComponent<ShakeEmitter>());

            TeslaZapConeEffect = CreateTeslaZapConeEffect(zapConeProjectile);

            UnityEngine.Object.Destroy(zapConeProjectile.transform.Find("Effect/Distortion, 3D").gameObject);
            UnityEngine.Object.Destroy(zapConeProjectile.transform.Find("Effect/RadialMesh").gameObject);
            UnityEngine.Object.Destroy(zapConeProjectile.transform.Find("Effect/Flash").gameObject);

            Content.AddProjectilePrefab(zapConeProjectile);

            return zapConeProjectile;
        }

        private static GameObject CreateTeslaZapConeEffect(GameObject zapConeProjectile) {
            GameObject zapConeEffect = PrefabAPI.InstantiateClone(zapConeProjectile.transform.Find("Effect").gameObject, "TeslaPunchConeEffect", false);
            zapConeEffect.SetActive(true);

            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Sparks, Single").gameObject);
            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Lines").gameObject);
            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Point Light").gameObject);
            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Impact Shockwave").gameObject);

            ParticleSystem shockwaveParticle = zapConeEffect.transform.Find("RadialMesh").GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = shockwaveParticle.main;
            mainModule.startSpeed = 0.5f;
            zapConeEffect.transform.Find("RadialMesh").localScale = new Vector3( 0.5f, 0.5f, 1);

            shockwaveParticle.GetComponent<ParticleSystemRenderer>().material.color = Color.cyan;

            CreateEffectFromObject(zapConeEffect, "", false);

            return zapConeEffect;
        }

        private static GameObject CreateTeslaTrackingIndicator() {

            GameObject indicatorPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/LightningIndicator"), "TeslaIndicator", false);

            UnityEngine.Object.DestroyImmediate(indicatorPrefab.transform.Find("TextMeshPro").gameObject);
            UnityEngine.Object.DestroyImmediate(indicatorPrefab.transform.Find("Holder/Brackets").gameObject);

            indicatorPrefab.transform.localScale = Vector3.one * .15f;
            indicatorPrefab.transform.localPosition = Vector3.zero;
            indicatorPrefab.transform.Find("Holder").rotation = Quaternion.identity;
            indicatorPrefab.transform.Find("Holder/Brackets").rotation = Quaternion.identity;

            TeslaIndicatorView indicatorViewComponent = indicatorPrefab.AddComponent<TeslaIndicatorView>();

            SpriteRenderer spriteRenderer = indicatorPrefab.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = Modules.Assets.LoadAsset<Sprite>("texIndicator1Close");
            spriteRenderer.color = Color.cyan;
            spriteRenderer.transform.localRotation = Quaternion.identity;
            spriteRenderer.transform.localPosition = Vector3.zero;

            indicatorViewComponent.indicatorRenderer = spriteRenderer;

            SpriteRenderer towerIndicator = UnityEngine.Object.Instantiate(spriteRenderer, spriteRenderer.transform.parent);
            towerIndicator.sprite = Modules.Assets.LoadAsset<Sprite>("texIndicatorTower2RedWide");
            towerIndicator.color = Color.red;

            indicatorViewComponent.towerIndicator = towerIndicator.gameObject;

            return indicatorPrefab;
        }

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

        private static Material FindChainLightningMaterial() {
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningOrbEffect").GetComponentInChildren<LineRenderer>().material;
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
            //using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("JoeMod.HenryBank.bnk"))
            //{
            //    byte[] array = new byte[manifestResourceStream2.Length];
            //    manifestResourceStream2.Read(array, 0, array.Length);
            //    SoundAPI.SoundBanks.Add(array);
            //}

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("JoeMod.Tesla_Trooper.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        private static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

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
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return Assets.LoadAsset<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName) => LoadEffect(resourceName, "", false);
        private static GameObject LoadEffect(string resourceName, string soundName) => LoadEffect(resourceName, soundName, false);
        private static GameObject LoadEffect(string resourceName, bool parentToTransform) => LoadEffect(resourceName, "", parentToTransform);
        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform) {
            bool assetExists = false;
            for (int i = 0; i < assetNames.Length; i++) {
                if (assetNames[i].Contains(resourceName.ToLowerInvariant())) {
                    assetExists = true;
                    break;
                }
            }

            if (!assetExists) {
                Debug.LogError("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            GameObject newEffect = LoadAsset<GameObject>(resourceName);
            CreateEffectFromObject(newEffect, soundName, parentToTransform);

            return newEffect;
        }

        private static void CreateEffectFromObject(GameObject newEffect, string soundName, bool parentToTransform) {
            newEffect.AddComponent<DestroyOnTimer>().duration = 6;
            newEffect.AddComponent<NetworkIdentity>();
            if (!newEffect.GetComponent<VFXAttributes>()) {
                newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            }

            EffectComponent effect = newEffect.GetComponent<EffectComponent>();
            if (!effect) effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = true;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddNewEffectDef(newEffect, soundName);
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