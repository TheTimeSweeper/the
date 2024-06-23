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
using UnityEngine.AddressableAssets;
using AliemMod.Content;
using Path = System.IO.Path;

namespace AliemMod.Modules
{

    public static class Assets
    {
        public static AssetBundle mainAssetBundle;

        //HENRY: indev
        #region indev

        // cache these and use to create our own materials
        public static Shader hotpoo = LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/HGStandard");
        #endregion

        private const string assetbundleName = "aliem";

        public static GameObject bloodEffect;
        public static GameObject nemforcerImpactEffect;
        public static NetworkSoundEventDef nemforcerImpactSound;

        public static GameObject m1EffectPrefab;
        public static GameObject m2EffectPrefab;

        public static GameObject knifeSwingEffect;
        public static GameObject knifeImpactEffect;
        public static GameObject LunarChargedProjectile;

        public static GameObject rifleTracer;
        public static GameObject rifleTracerBig;

        public static GameObject sawedOffTracerThin;
        public static GameObject sawedOffTracer;
        public static GameObject sawedOffMuzzleFlash;

        public static GameObject swirlCharge;
        public static GameObject swirlChargeMax;

        public static GameObject burrowPopOutEffect;

        public static GameObject BBOrbEffect;

        public static void Initialize()
        {
            //HENRY: check this somewhere else secretly
            if (assetbundleName == "myassetbundle")
            {
                Debug.LogError("AssetBundle name hasn't been changed- not loading any assets to avoid conflicts");
                return;
            }

            LoadAssetBundle();

            LoadSoundBank();

            PopulateAss();
        }

        private static void LoadSoundBank()
        {
            if (UnityEngine.Application.isBatchMode) return;

            AkSoundEngine.AddBasePath(Path.Combine(Path.GetDirectoryName(AliemPlugin.instance.Info.Location), "SoundBanks"));
            AkSoundEngine.LoadBank("aliem.bnk", out var _soundBankId);
        }

        public static void LoadAssetBundle()
        {
            try
            {
                mainAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(AliemPlugin.instance.Info.Location), "AssetBundles", "aliem"));
                AliemPlugin.instance.StartCoroutine(ShaderSwapper.ShaderSwapper.UpgradeStubbedShadersAsync(mainAssetBundle));
            } catch { }
        }

        private static void PopulateAss()
        {
            bloodEffect = LoadEffect("BloodParticle", true);
            nemforcerImpactEffect = LoadEffect("ImpactNemforcer", "Play_Leap_Impact", true);

            //nemforcerImpactSound = CreateNetworkSoundEventDef("Play_Leap_Impact");

            m1EffectPrefab = CreateM1Effect();
            m2EffectPrefab = CreateM2Effect();

            knifeSwingEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/MercSwordSlash.prefab").WaitForCompletion().InstantiateClone("AliemHunkKnifeSwing", false);
            knifeSwingEffect.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = Addressables.LoadAssetAsync<Material>("RoR2/Base/Huntress/matHuntressSwingTrail.mat").WaitForCompletion();
            knifeSwingEffect.transform.GetChild(0).localScale = new Vector3(0.9f, 2f, 6);
            knifeSwingEffect.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.9f);
            knifeSwingEffect.transform.GetChild(0).localEulerAngles = new Vector3(-90, 0, 0);

            mainAssetBundle.LoadAsset<Material>("matRifle")
                .SetHotpooMaterial()
                .SetSpecular(0.3f, 10);
            mainAssetBundle.LoadAsset<Material>("matRevolver")
                .SetHotpooMaterial()
                .SetSpecular(0.3f, 10);

            CreateChargedLunarProjectile();

            HunkKnifeImpactEffect();

            rifleTracer = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/GoldGat/TracerGoldGat.prefab").WaitForCompletion().InstantiateClone("TracerGoldGatAliemRifle", false);
            ColorTracer(rifleTracer, new Color(0.3581327f, 0.0518868f, 1, 1), 2);
            AddNewEffectDef(rifleTracer);

            rifleTracerBig = mainAssetBundle.LoadAsset<GameObject>("RifleTracerThick").InstantiateClone("okimgonnarenamethatonebeforeIfuckinforgetitandmakeahugemistake", false);
            rifleTracerBig.transform.Find("BeamTrails").GetComponent<ParticleSystemRenderer>().trailMaterial = Addressables.LoadAssetAsync<Material>("RoR2/DLC1/VoidRaidCrab/matVoidRaidCrabTripleBeam3.mat").WaitForCompletion();
            rifleTracerBig.transform.Find("VolumeTracer").transform.localScale = Vector3.one * 1.6f;//radius
            AddNewEffectDef(rifleTracerBig);

            sawedOffTracerThin = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ClayBruiser/TracerClayBruiserMinigun.prefab").WaitForCompletion().InstantiateClone("TracerClayBruiserMinigunAliemSawedOff", false);
            ColorTracer(sawedOffTracerThin, null, 3);
            AddNewEffectDef(sawedOffTracerThin);

            sawedOffTracer = mainAssetBundle.LoadAsset<GameObject>("SawedOffTracer");
            sawedOffTracer.transform.Find("BeamTrails/ScaledSmoke, Billboard").GetComponent<ParticleSystemRenderer>().sharedMaterial =
                Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matOpaqueDust.mat").WaitForCompletion();
            sawedOffTracer.transform.Find("BeamTrails/Unscaled Flames").GetComponent<ParticleSystemRenderer>().sharedMaterial =
                Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matOmniExplosion1.mat").WaitForCompletion();
            sawedOffTracer.transform.Find("SmokeLine").GetComponent<LineRenderer>().sharedMaterial =
                Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matSmokeTrail.mat").WaitForCompletion();

            sawedOffTracer = sawedOffTracer.DebugClone(false);
            AddNewEffectDef(sawedOffTracer);

            sawedOffMuzzleFlash = mainAssetBundle.LoadAsset<GameObject>("Muzzleflash1Shotgun");
            Material flashmat = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matSparkCutout00.mat").WaitForCompletion();
            sawedOffMuzzleFlash.transform.Find("Starburst").GetComponent<ParticleSystemRenderer>().sharedMaterial = flashmat;
            sawedOffMuzzleFlash.transform.Find("Starburst2").GetComponent<ParticleSystemRenderer>().sharedMaterial = flashmat;
            AddNewEffectDef(sawedOffMuzzleFlash);

            swirlCharge = mainAssetBundle.LoadAsset<GameObject>("SwirlParticles").DebugClone(false); //not an effect def
            swirlChargeMax = mainAssetBundle.LoadAsset<GameObject>("SwirlParticlesMax").DebugClone(false);
            AddNewEffectDef(swirlChargeMax);

            burrowPopOutEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bell/BellBodyPartsImpact.prefab").WaitForCompletion();

            BBOrbEffect = mainAssetBundle.LoadAsset<GameObject>("BBGunOrbEffect");
            if (!AliemConfig.M1_BBGun_VFXAlways.Value)
            {
                BBOrbEffect.GetComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Medium;
            }
            BBOrbEffect = DebugClone(BBOrbEffect, false);
            AddNewEffectDef(BBOrbEffect);
        }

        private static void CreateChargedLunarProjectile()
        {
            GameObject lunarNeedle = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LunarSkillReplacements/LunarNeedleProjectile.prefab").WaitForCompletion();

            LunarChargedProjectile = lunarNeedle.InstantiateClone("AliemChargedLunarNeedle", true);

            ProjectileController projectileController = LunarChargedProjectile.GetComponent<ProjectileController>();
            GameObject newGhost = projectileController.ghostPrefab.InstantiateClone(projectileController.ghostPrefab.name + "AliemCharged", false);
            GameObject newCore = mainAssetBundle.LoadAsset<GameObject>("ChargedLunarNeedleCore").InstantiateClone("ChargedLunarNeedleCore", false);
            Material matBig = new Material(Addressables.LoadAssetAsync<Material>("RoR2/Base/LunarSkillReplacements/matLunarNeedleImpactEffect.mat").WaitForCompletion());
            matBig.SetColor("_TintColor", new Color(0.4615285f, 0.0514418f, 0.6415094f));
            newCore.GetComponent<Renderer>().sharedMaterial = matBig;
            newCore.transform.SetParent(newGhost.transform, false);
            newCore.transform.localPosition = Vector3.zero;
            projectileController.ghostPrefab = newGhost;

            ProjectileImpactExplosion chargedImpactExplosion = LunarChargedProjectile.GetComponent<ProjectileImpactExplosion>();
            chargedImpactExplosion.fireChildren = true;
            GameObject lunarNeedleSimple = lunarNeedle.InstantiateClone(lunarNeedle.name + "AliemSimple", true);
            chargedImpactExplosion.childrenProjectilePrefab = lunarNeedleSimple;
            chargedImpactExplosion.childrenCount = 8;
            chargedImpactExplosion.childrenDamageCoefficient = 1;
            chargedImpactExplosion.rangePitchDegrees = 360;
            chargedImpactExplosion.rangeRollDegrees = 360;
            chargedImpactExplosion.lifetimeAfterImpact = 0.5f;

            ProjectileSimple projectileSimple = lunarNeedleSimple.GetComponent<ProjectileSimple>();
            projectileSimple.enableVelocityOverLifetime = false;
            projectileSimple.desiredForwardSpeed = 15f;
            UnityEngine.Object.Destroy(lunarNeedleSimple.GetComponent<ProjectileDirectionalTargetFinder>());
            UnityEngine.Object.Destroy(lunarNeedleSimple.GetComponent<ProjectileSteerTowardTarget>());
            UnityEngine.Object.Destroy(lunarNeedleSimple.GetComponent<ProjectileTargetComponent>());
            UnityEngine.Object.Destroy(lunarNeedleSimple.GetComponent<ProjectileStickOnImpact>());
            UnityEngine.Object.Destroy(lunarNeedleSimple.GetComponent<SphereCollider>());
            Content.AddProjectilePrefab(lunarNeedleSimple);

            ProjectileImpactExplosion simpleImpactExplosion = lunarNeedleSimple.GetComponent<ProjectileImpactExplosion>();
            simpleImpactExplosion.lifetime = 0.2f;
            simpleImpactExplosion.blastRadius = 3;

            Content.AddProjectilePrefab(LunarChargedProjectile);
        }

        private static void HunkKnifeImpactEffect()
        {
            knifeImpactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion().InstantiateClone("HunkKnifeImpact", false);
            knifeImpactEffect.GetComponent<OmniEffect>().enabled = false;

            Material hitsparkMat = UnityEngine.Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/Base/Merc/matOmniHitspark3Merc.mat").WaitForCompletion());
            hitsparkMat.SetColor("_TintColor", Color.white);

            knifeImpactEffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystemRenderer>().material = hitsparkMat;

            //knifeImpactEffect.transform.GetChild(2).localScale = Vector3.one * 1.5f;
            knifeImpactEffect.transform.GetChild(2).gameObject.GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Huntress/matOmniRing2Huntress.mat").WaitForCompletion();

            Material slashMat = UnityEngine.Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matOmniRadialSlash1Generic.mat").WaitForCompletion());

            knifeImpactEffect.transform.GetChild(5).gameObject.GetComponent<ParticleSystemRenderer>().material = slashMat;

            knifeImpactEffect.transform.GetChild(6).GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/LunarWisp/matOmniHitspark1LunarWisp.mat").WaitForCompletion();
            knifeImpactEffect.transform.GetChild(6).gameObject.GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matOmniHitspark2Generic.mat").WaitForCompletion();

            //knifeImpactEffect.transform.GetChild(1).localScale = Vector3.one * 1.5f;

            knifeImpactEffect.transform.GetChild(1).gameObject.SetActive(true);
            knifeImpactEffect.transform.GetChild(2).gameObject.SetActive(true);
            knifeImpactEffect.transform.GetChild(3).gameObject.SetActive(true);
            knifeImpactEffect.transform.GetChild(4).gameObject.SetActive(true);
            knifeImpactEffect.transform.GetChild(5).gameObject.SetActive(true);
            knifeImpactEffect.transform.GetChild(6).gameObject.SetActive(true);
            knifeImpactEffect.transform.GetChild(6).GetChild(0).gameObject.SetActive(true);

            //knifeImpactEffect.transform.GetChild(6).transform.localScale = new Vector3(1f, 1f, 3f);
            //knifeImpactEffect.transform.localScale = Vector3.one * 1.5f;

            AddNewEffectDef(knifeImpactEffect);
        }

        private static GameObject CreateM2Effect()
        {

            GameObject gunBigImpact = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX").InstantiateClone("AliemM2OmniExplosionVFX", false);
            gunBigImpact.GetComponent<EffectComponent>().soundName = "Play_engi_M2_explo";

            CreateEffectFromObject(gunBigImpact);
            return gunBigImpact;
        }

        private static GameObject CreateM1Effect()
        {
            GameObject gunImpact = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX").InstantiateClone("AliemM1OmniExplosionVFX", false);
            gunImpact.GetComponent<EffectComponent>().soundName = "Play_engi_M1_explo";

            CreateEffectFromObject(gunImpact);
            return gunImpact;
        }

        public static GameObject DebugClone(this GameObject clonee, bool net)
        {
            if (AliemConfig.Debug.Value)
            {
                return clonee.InstantiateClone(clonee.name, net);
            }
            else
            {
                return clonee;
            }
        }

        public static T LoadAsset<T>(string assString) where T : UnityEngine.Object
        {
            T loadedAss = LegacyResourcesAPI.Load<T>(assString);


            if (loadedAss == null)
            {
                loadedAss = mainAssetBundle.LoadAsset<T>(assString);
            }

            if (loadedAss == null)
            {
                Debug.LogError($"Null asset: {assString}.\nAttempt to load asset '{assString}' from assetbundles returned null");
            }

            return loadedAss;
        }

        public static GameObject LoadSurvivorModel(string modelName)
        {
            GameObject model = LoadAsset<GameObject>(modelName);
            if (model == null)
            {
                Debug.LogError("Trying to load a null model- check to see if the name in your code matches the name of the object in Unity");
                return null;
            }

            return model.InstantiateClone(model.name, false);
        }

        public static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (MeshRenderer i in objectToConvert.GetComponentsInChildren<MeshRenderer>())
            {
                if (i?.sharedMaterial != null)
                {
                    i.sharedMaterial.SetHotpooMaterial();
                }
            }

            foreach (SkinnedMeshRenderer i in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (i?.sharedMaterial != null)
                {
                    i.sharedMaterial.SetHotpooMaterial();
                }
            }
        }

        public static Texture LoadCharacterIconGeneric(string characterName)
        {
            return LoadAsset<Texture>("tex" + characterName + "Icon");
        }
        public static Texture LoadCharacterIcon(string name)
        {
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

        private static GameObject CloneLightningOrbEffect(string path, string name, Color beamColor, Color? lineColor = null, float width = 1)
        {
            GameObject newEffect = LegacyResourcesAPI.Load<GameObject>(path).InstantiateClone(name, false);

            foreach (LineRenderer rend in newEffect.GetComponentsInChildren<LineRenderer>())
            {
                if (rend)
                {
                    Material mat = UnityEngine.Object.Instantiate(rend.sharedMaterial);
                    mat.SetColor("_TintColor", beamColor);
                    rend.sharedMaterial = mat;

                    if (lineColor != null)
                    {
                        rend.startColor = lineColor.Value;
                        rend.endColor = lineColor.Value;
                    }
                    rend.widthMultiplier = width;
                }
            }
            AddNewEffectDef(newEffect);

            return newEffect;
        }

        private static GameObject ColorTracer(GameObject newTracer, Color? color = null, float widthMultiplierMultiplier = 1, float? speed = null, float? length = null)
        {
            //if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            //GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            Tracer tracer = newTracer.GetComponent<Tracer>();
            if (tracer != null)
            {
                tracer.speed = speed.HasValue ? speed.Value : tracer.speed;
                tracer.length = length.HasValue ? length.Value : tracer.length;
            }

            if (color != null || widthMultiplierMultiplier != 1)
            {
                foreach (var lineREnderer in newTracer.GetComponentsInChildren<LineRenderer>())
                {
                    if (color.HasValue)
                    {
                        lineREnderer.startColor = color.Value;
                        lineREnderer.endColor = color.Value;
                    }
                    if (widthMultiplierMultiplier != 1)
                    {
                        lineREnderer.widthMultiplier *= widthMultiplierMultiplier;
                    }
                }

                foreach (ParticleSystem particles in newTracer.GetComponentsInChildren<ParticleSystem>())
                {
                    ParticleSystem.MainModule mainModule = particles.main;
                    mainModule.startSize = new ParticleSystem.MinMaxCurve(mainModule.startSize.constant * widthMultiplierMultiplier);
                    mainModule.startColor = new ParticleSystem.MinMaxGradient(color.Value);

                    ParticleSystem.TrailModule trailModule = particles.trails;
                    if (trailModule.enabled)
                    {

                        //Gradient gradient = trailModule.colorOverLifetime.gradientMin;
                        //gradient.colorKeys[0].color = Color.green;
                        Gradient gradient = new Gradient();
                        GradientColorKey[] colorKey = new GradientColorKey[2];
                        colorKey[0].color = color.Value;
                        colorKey[0].time = 0.0f;
                        colorKey[1].color = color.Value;
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

            if (color != null)
            {
                foreach (var rend in newTracer.GetComponentsInChildren<ParticleSystemRenderer>())
                {
                    Material mat = new Material(rend.material);

                    mat.SetColor("_MainColor", color.Value);
                    mat.SetColor("_Color", color.Value);
                    mat.SetColor("_TintColor", color.Value);

                    rend.sharedMaterial = mat;
                }
            }

            //AddNewEffectDef(newTracer);

            return newTracer;
        }

        /// <summary>
        /// search for crosshair prefabs here. plug in the character or crosshair name
        /// </summary>
        /// <para>https://xiaoxiao921.github.io/GithubActionCacheTest/assetPathsDump.html</para>
        public static GameObject LoadCrosshair(string crosshairName)
        {
            if (LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return LoadAsset<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName) => LoadEffect(resourceName, "", false);
        private static GameObject LoadEffect(string resourceName, string soundName) => LoadEffect(resourceName, soundName, false);
        private static GameObject LoadEffect(string resourceName, bool parentToTransform) => LoadEffect(resourceName, "", parentToTransform);
        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform)
        {

            GameObject newEffect = LoadAsset<GameObject>(resourceName);

            if (!newEffect)
            {
                Debug.LogError("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            CreateEffectFromObject(newEffect, soundName, parentToTransform);

            return newEffect;
        }

        private static void CreateEffectFromObject(GameObject newEffect) => CreateEffectFromObject(newEffect, "", false);

        private static void CreateEffectFromObject(GameObject newEffect, string soundName, bool parentToTransform)
        {
            newEffect.AddComponent<DestroyOnTimer>().duration = 6;
            newEffect.AddComponent<NetworkIdentity>();
            if (!newEffect.GetComponent<VFXAttributes>())
            {
                newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            }

            EffectComponent effect = newEffect.GetComponent<EffectComponent>();
            if (!effect)
            {
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
        public static Material CreateMaterial(string materialName) => CreateMaterial(materialName, 0f);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission) => CreateMaterial(materialName, emission, Color.white);
        [Obsolete(obsolete)]
        public static Material CreateMaterial(string materialName, float emission, Color emissionColor) => CreateMaterial(materialName, emission, emissionColor, 0f);
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