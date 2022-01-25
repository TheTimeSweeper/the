﻿using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections.Generic;
using RoR2.UI;
using System;

namespace HenryMod.Modules
{
    internal static class Assets
    {
        // the assetbundle to load assets from
        // survivor todo: separate assets between survivors
        internal static AssetBundle mainAssetBundle;
        internal static AssetBundle teslaAssetBundle;

        //HENRY: indev
        #region indev
        // lists of assets to add to contentpack
        internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();
        internal static List<EffectDef> effectDefs = new List<EffectDef>();

        // cache these and use to create our own materials
        internal static Shader hotpoo = Resources.Load<Shader>("Shaders/Deferred/HGStandard");
        internal static Material commandoMat;
        private static string[] assetNames = new string[0];
        #endregion

        #region MY NAME'S NOT HENRY
        // particle effects
        internal static GameObject swordSwingEffect;
        internal static GameObject swordHitImpactEffect;

        internal static GameObject bombExplosionEffect;
        // networked hit sounds
        internal static NetworkSoundEventDef swordHitSoundEvent;
        #endregion

        // CHANGE THIS
        private const string assetbundleName = "joe";

        //jerry don't you know
        public static GameObject JoeFireball = null;
        public static GameObject JoeImpactEffect = null;
        public static GameObject JoeJumpSwingEffect = null;

        public static GameObject TestlaCoil = null;
        public static GameObject TestlaCoilBlueprint = null;

        // icons
        public static Texture JoePortrait = null;
        public static Sprite Skill1Icon = null;
        public static Sprite Skill2Icon = null;

        internal static void Initialize()
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

            JoePortrait = mainAssetBundle.LoadAsset<Sprite>("joe_icon").texture;
            Skill1Icon = mainAssetBundle.LoadAsset<Sprite>("skill1_icon");
            Skill2Icon = mainAssetBundle.LoadAsset<Sprite>("skill2_icon");

            JoeFireball = mainAssetBundle.LoadAsset<GameObject>("JoeFireballBasic");

            JoeImpactEffect = LoadEffect("JoeImpactEffectBasic");
            JoeJumpSwingEffect = LoadEffect("JoeJumpSwingParticlesesEffect");

            TestlaCoil = mainAssetBundle.LoadAsset<GameObject>("TeslaCoil");
            TestlaCoilBlueprint = mainAssetBundle.LoadAsset<GameObject>("TeslaCoilBlueprint");

            swordHitSoundEvent = CreateNetworkSoundEventDef("HenrySwordHit");


        }

        internal static void PopulateHenrysAssetsThatNoLongerExist()
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

        internal static void LoadAssetBundle()
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
        }
        
        internal static T LoadAsset<T>(string assString) where T : UnityEngine.Object
        {
            T assBundle = mainAssetBundle.LoadAsset<T>(assString);

            if(assBundle == null)
            {
                assBundle = teslaAssetBundle.LoadAsset<T>(assString);
            }

            return assBundle;
        }

        internal static void LoadSoundbank()
        {
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("JoeMod.HenryBank.bnk"))
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

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            networkSoundEventDefs.Add(networkSoundEventDef);

            return networkSoundEventDef;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (MeshRenderer i in objectToConvert.GetComponentsInChildren<MeshRenderer>())
            {
                if (i)
                {
                    if (i.material)
                    {
                        i.material.shader = hotpoo;
                    }
                }
            }

            foreach (SkinnedMeshRenderer i in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (i)
                {
                    if (i.material)
                    {
                        i.material.shader = hotpoo;
                    }
                }
            }
        }

        internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
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

        internal static Texture LoadCharacterIcon(string characterName)
        {
            return LoadAsset<Texture>("tex" + characterName + "Icon");
        }

        internal static Texture LoadCharacterIconButRetarded(string name) {
            return LoadAsset<Texture>(name);
        }

        internal static GameObject LoadCrosshair(string crosshairName)
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

            effectDefs.Add(newEffectDef);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!commandoMat) commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            Material mat = UnityEngine.Object.Instantiate<Material>(commandoMat);
            Material tempMat = Assets.LoadAsset<Material>(materialName);

            if (!tempMat)
            {
                Debug.LogError("Failed to load material: " + materialName + " - Check to see that the name in your Unity project matches the one in this code");
                return commandoMat;
            }

            mat.name = materialName;
            mat.SetColor("_Color", tempMat.GetColor("_Color"));
            mat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            mat.SetColor("_EmColor", emissionColor);
            mat.SetFloat("_EmPower", emission);
            mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            mat.SetFloat("_NormalStrength", normalStrength);

            return mat;
        }

        public static Material CreateMaterial(string materialName)
        {
            return Assets.CreateMaterial(materialName, 0f);
        }

        public static Material CreateMaterialFull(string materialName)
        {
            Material mat = Assets.CreateMaterial(materialName, 1, Color.white, 1);
            mat.SetInt("_Cull", 0);
            return mat;
        }

        public static Material CreateMaterial(string materialName, float emission)
        {
            return Assets.CreateMaterial(materialName, emission, Color.white);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor)
        {
            return Assets.CreateMaterial(materialName, emission, emissionColor, 0f);
        }

        public class MaterialSetup
        {
            Material _mat;

            public MaterialSetup(Material mat)
            {
                this._mat = mat;
            }

            public static MaterialSetup GenerateMaterial(string materialName)
            {
                if (!commandoMat) commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

                Material mat = UnityEngine.Object.Instantiate<Material>(commandoMat);
                Material tempMat = Assets.LoadAsset<Material>(materialName);

                if (!tempMat)
                {
                    Debug.LogError("Failed to load material: " + materialName + " - Check to see that the name in your Unity project matches the one in this code");
                    return new MaterialSetup(commandoMat);
                }

                mat.SetColor("_Color", tempMat.GetColor("_Color"));
                mat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
                mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));

                return new MaterialSetup(mat);
            }

            public MaterialSetup SetNormal(float normalStrength = 1)
            {
                _mat.SetFloat("_NormalStrength", normalStrength);
                return this;
            }

            public MaterialSetup SetEmission()
            {
                return SetEmission(1, Color.white);
            }
            public MaterialSetup SetEmission(float emission)
            {
                return SetEmission(emission, Color.white);
            }
            public MaterialSetup SetEmission(float emission, Color emissionColor)
            {
                _mat.SetFloat("_EmPower", emission);
                _mat.SetColor("_EmColor", emissionColor);
                return this;
            }
            public MaterialSetup SetCull (bool cull = false)
            {
                _mat.SetInt("_Cull", cull ? 1 : 0);
                return this;
            }
            public Material ReturnMaterial()
            {
                return _mat;
            }
        }
    }
}