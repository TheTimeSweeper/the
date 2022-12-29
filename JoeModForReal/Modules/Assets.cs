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
using ModdedEntityStates.Joe;
using UnityEngine.AddressableAssets;

namespace Modules {

    internal static class Assets
    {
        public static AssetBundle mainAssetBundle;

        //HENRY: indev
        #region indev

        // cache these and use to create our own materials
        public static Shader hotpoo = RoR2.LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/HGStandard");
        #endregion

        public static GameObject JoeFireball => Projectiles.JoeFireball;
        public static GameObject JoeImpactEffect;
        public static GameObject JoeJumpSwingEffect;
        public static GameObject TenticlesSpelledWrong;

        public static GameObject MercSwordSlash;

        public static NetworkSoundEventDef FleshSliceSound;

        public static void Initialize()
        {
            LoadAssetBundle();

            PopulateAss();
        }

        public static void LoadAssetBundle()
        {
            if (mainAssetBundle == null)
            {
                mainAssetBundle = AssetBundle.LoadFromFile(Files.GetPluginFilePath("AssetBundles", "joe"));
            }
        }
        
        private static void PopulateAss() {

            JoeJumpSwingEffect = LoadEffect("JoeJumpSwingParticlesesEffect");
            JoeImpactEffect = LoadEffect("JoeImpactEffectBasic");

            //todo wtf
            FleshSliceSound = CreateNetworkSoundEventDef("play_joe_fleshSlice");

            MercSwordSlash = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/MercSwordSlash.prefab").WaitForCompletion();
        }

        public static void LateInitialize() {

            TenticlesSpelledWrong = CreateTenticles();
        }


        private static GameObject CreateTenticles() {
            GameObject tenticles = LoadAsset<GameObject>("Tenticles");

            ConvertAllRenderersToHopooShader(tenticles);

            BuffWard buffWard = tenticles.GetComponent<BuffWard>();
            buffWard.buffDef = Buffs.TenticleBuff;
            buffWard.Networkradius = Special1Tenticles.Range;

            tenticles.transform.Find("Tenticles").transform.localScale = Vector3.one * Special1Tenticles.Range * 2;

            GameObject warbanner = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/WarbannerWard");
            Material warbannerMaterial = warbanner.transform.Find("Indicator/IndicatorSphere").GetComponent<MeshRenderer>().material;
            Material material = tenticles.transform.Find("Indicator/Sphere").GetComponent<MeshRenderer>().material = new Material(warbannerMaterial);
            material.SetColor("_TintColor", Color.green);

            PrefabAPI.RegisterNetworkPrefab(tenticles);
            return tenticles;
        }

        public static T LoadAsset<T>(string assString) where T : UnityEngine.Object
        {
            T loadedAss = mainAssetBundle.LoadAsset<T>(assString);
            
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

        public static Texture LoadCharacterIconGeneric(string characterName)
        {
            return LoadAsset<Texture>("tex" + characterName + "Icon");
        }
        public static Texture LoadCharacterIcon(string name) {
            return LoadAsset<Texture>(name);
        }
        public static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            Modules.Content.AddNetworkSoundEventDef(networkSoundEventDef);

            return networkSoundEventDef;
        }

        private static GameObject CloneLightningOrbEffect(string path, string name, Color beamColor, Color? lineColor = null, float width = 1) {
            GameObject newEffect = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>(path), name, false);

            foreach (LineRenderer rend in newEffect.GetComponentsInChildren<LineRenderer>()) {
                if (rend) {
                    Material mat = UnityEngine.Object.Instantiate<Material>(rend.material);
                    mat.SetColor("_TintColor", beamColor);
                    rend.material = mat;

                    if (lineColor != null) {
                        rend.startColor = lineColor.Value;
                        rend.endColor = lineColor.Value;
                    }
                    rend.widthMultiplier = width;
                }
            }
            AddNewEffectDef(newEffect);

            return newEffect;
        }

        private static GameObject CloneTracer(string originalTracerName, string newTracerName, Color? color = null, float widthMultiplierMultiplier = 1, float? speed = null, float? length = null)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = speed.HasValue? speed.Value : newTracer.GetComponent<Tracer>().speed;
            newTracer.GetComponent<Tracer>().length = length.HasValue ? length.Value : newTracer.GetComponent<Tracer>().length;

            if (color.HasValue || widthMultiplierMultiplier != 1) {
                foreach (var lineREnderer in newTracer.GetComponentsInChildren<LineRenderer>()) {
                    if (color.HasValue) {
                        lineREnderer.startColor = color.Value;
                        lineREnderer.endColor = color.Value;
                    }
                    if(widthMultiplierMultiplier != 1){
                        lineREnderer.widthMultiplier *= widthMultiplierMultiplier;
                    }
                }

                foreach (ParticleSystem particles in newTracer.GetComponentsInChildren<ParticleSystem>()) {
                    ParticleSystem.MainModule mainModule = particles.main;
                    mainModule.startSize = new ParticleSystem.MinMaxCurve(mainModule.startSize.constant * widthMultiplierMultiplier);
                    mainModule.startColor = new ParticleSystem.MinMaxGradient(color.Value);

                    ParticleSystem.TrailModule trailModule = particles.trails;
                    if (trailModule.enabled) {

                        //Gradient gradient = trailModule.colorOverLifetime.gradientMin;
                        //gradient.colorKeys[0].color = Color.green;
                        Gradient gradient = new Gradient();
                        GradientColorKey[] colorKey = new GradientColorKey[2];
                        colorKey[0].color = Color.green;
                        colorKey[0].time = 0.0f;
                        colorKey[1].color = Color.green;
                        colorKey[1].time = 1.0f;

                        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
                        alphaKey[0].alpha = 1.0f;
                        alphaKey[0].time = 0.0f;
                        alphaKey[1].alpha = 0.0f;
                        alphaKey[1].time = 1.0f;

                        gradient.SetKeys(colorKey, alphaKey);

                        trailModule.colorOverLifetime = new ParticleSystem.MinMaxGradient(gradient);
                    }
                }
            }
            
            if (color.HasValue) {
                foreach(var rend in newTracer.GetComponentsInChildren<ParticleSystemRenderer>()) {
                    rend.material.SetColor("_MainColor", color.Value);
                    rend.material.SetColor("_Color", color.Value);
                    rend.material.SetColor("_TintColor", color.Value);
                }
            }

            AddNewEffectDef(newTracer);

            return newTracer;
        }

        /// <summary>
        /// search for crosshair prefabs here. plug in the character or crosshair name
        /// </summary>
        /// <para>https://xiaoxiao921.github.io/GithubActionCacheTest/assetPathsDump.html</para>
        public static GameObject LoadCrosshair(string crosshairName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) 
                return Assets.LoadAsset<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName) => LoadEffect(resourceName, "", false);
        private static GameObject LoadEffect(string resourceName, string soundName) => LoadEffect(resourceName, soundName, false);
        private static GameObject LoadEffect(string resourceName, bool parentToTransform) => LoadEffect(resourceName, "", parentToTransform);
        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform) {

            GameObject newEffect = LoadAsset<GameObject>(resourceName);

            if (!newEffect) {
                Debug.LogError("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            CreateEffectFromObject(newEffect, soundName, parentToTransform);

            return newEffect;
        }

        private static void CreateEffectFromObject(GameObject newEffect) => CreateEffectFromObject(newEffect, "", false);

        private static void CreateEffectFromObject(GameObject newEffect, string soundName, bool parentToTransform) {
            newEffect.AddComponent<DestroyOnTimer>().duration = 6;
            newEffect.AddComponent<NetworkIdentity>();
            if (!newEffect.GetComponent<VFXAttributes>()) {
                newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            }

            EffectComponent effect = newEffect.GetComponent<EffectComponent>();
            if (!effect) {
                effect = newEffect.AddComponent<EffectComponent>();
                effect.applyScale = true;
                effect.effectIndex = EffectIndex.Invalid;
                effect.parentToReferencedTransform = parentToTransform;
                effect.positionAtReferencedTransform = true;
                effect.soundName = soundName;
            }
            
            AddNewEffectDef(newEffect, soundName);
        }

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName = "")
        {
            EffectDef newEffectDef = new EffectDef(effectPrefab);
            //newEffectDef.prefab = effectPrefab;
            //newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            //newEffectDef.prefabName = effectPrefab.name;
            //newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            //newEffectDef.spawnSoundEventName = soundName;

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