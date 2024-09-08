using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RA2Mod.Modules;
using R2API;
using RoR2;
using RoR2.Projectile;
using RA2Mod.Survivors.Tesla.States;

namespace RA2Mod.Survivors.Tesla 
{ 
    public class TeslaAssets
    {
        public static GameObject TeslaCoil;
        public static GameObject TeslaCoilBlueprint;

        public static GameObject TeslaIndicatorPrefab;

        public static GameObject TeslaIndicatorPrefabDash;

        public static Material ChainLightningMaterial;
        private static Texture nodRampTex;

        public static GameObject TeslaLoaderZapConeProjectile;
        public static GameObject TeslaZapConeEffect;

        public static GameObject TeslaLightningOrbEffectRed;
        public static GameObject TeslaMageLightningOrbEffectRed;
        public static GameObject TeslaMageLightningOrbEffectRedThick;


        public static Sprite[] rangeSprites = null;// new Sprite[] { Modules.Assets.LoadAsset<Sprite>("texIndicator1Close"),
                                                         //Modules.Assets.LoadAsset<Sprite>("texIndicator2Med"),
                                                        // Modules.Assets.LoadAsset<Sprite>("texIndicator3Far") };
        public static Sprite allySprite = null;// Modules.Assets.LoadAsset<Sprite>("texIndicatorAlly");
        public static Sprite towerSprite = null;// Modules.Assets.LoadAsset<Sprite>("texIndicatorTowerIcon");


        public static List<IEnumerator> loads => ContentPacks.asyncLoadCoroutines;

        private static AssetBundle assetBundle;

        internal static List<IEnumerator> GetAssetBundleInitializedCoroutines(AssetBundle assetBundle)
        {
            List<IEnumerator> loads = new List<IEnumerator>();
            loads.Add(Asset.LoadAssetCoroutine<Material>("RoR2/Base/Common/VFX/matLightningLongBlue.mat", (mat) => ChainLightningMaterial = mat));

            return loads;
        }

        internal static void OnCharacterInitialized(AssetBundle assetBundle)
        {
            TeslaAssets.assetBundle = assetBundle;

            TeslaCoilBlueprint = assetBundle.LoadAsset<GameObject>("TeslaCoilBlueprint");

            TeslaIndicatorPrefab = CreateTeslaTrackingIndicator();

            TeslaIndicatorPrefabDash = CreateTeslaDashTrackingIndicator();

            nodRampTex = assetBundle.LoadAsset<Texture2D>("texTeslaRampLightning2");

            TeslaLightningOrbEffectRed =
                CloneLightningOrbNod("Prefabs/Effects/OrbEffects/LightningOrbEffect",
                                        "NodLightningOrbEffect",
                                        1.2f);
            TeslaMageLightningOrbEffectRed =
                CloneLightningOrbNod("Prefabs/Effects/OrbEffects/MageLightningOrbEffect",
                                        "NodMageLightningOrbEffect");
            TeslaMageLightningOrbEffectRedThick =
                CloneLightningOrbNod("Prefabs/Effects/OrbEffects/MageLightningOrbEffect",
                                        "NodMageThickLightningOrbEffect",
                                        2);
            TeslaMageLightningOrbEffectRedThick.GetComponentInChildren<AnimateShaderAlpha>().timeMax = 0.5f;

            TeslaLoaderZapConeProjectile = CreateZapConeProjectile();

            rangeSprites = new Sprite[] { assetBundle.LoadAsset<Sprite>("texIndicator1Close"),
                           assetBundle.LoadAsset<Sprite>("texIndicator2Med"),
                            assetBundle.LoadAsset<Sprite>("texIndicator3Far") };

            allySprite = assetBundle.LoadAsset<Sprite>("texIndicatorAlly");
            towerSprite = assetBundle.LoadAsset<Sprite>("texIndicatorTowerIcon");
        }

        private static GameObject CreateTeslaTrackingIndicator()
        {
            GameObject indicatorPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/LightningIndicator"), "TeslaIndicator", false);

            UnityEngine.Object.DestroyImmediate(indicatorPrefab.transform.Find("TextMeshPro").gameObject);
            UnityEngine.Object.DestroyImmediate(indicatorPrefab.transform.Find("Holder/Brackets").gameObject);

            indicatorPrefab.transform.localScale = Vector3.one * .15f;
            indicatorPrefab.transform.localPosition = Vector3.zero;
            indicatorPrefab.transform.Find("Holder").rotation = Quaternion.identity;
            indicatorPrefab.transform.Find("Holder/Brackets").rotation = Quaternion.identity;

            TeslaIndicatorView indicatorViewComponent = indicatorPrefab.AddComponent<TeslaIndicatorView>();

            SpriteRenderer spriteRenderer = indicatorPrefab.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = assetBundle.LoadAsset<Sprite>("texIndicator1Close");
            spriteRenderer.color = Color.cyan;
            spriteRenderer.transform.localRotation = Quaternion.identity;
            spriteRenderer.transform.localPosition = Vector3.zero;

            indicatorViewComponent.indicatorRenderer = spriteRenderer;

            SpriteRenderer towerIndicator = UnityEngine.Object.Instantiate(spriteRenderer, spriteRenderer.transform.parent);
            towerIndicator.sprite = assetBundle.LoadAsset<Sprite>("texIndicatorTower2RedWide");
            towerIndicator.color = Color.red;

            indicatorViewComponent.towerIndicator = towerIndicator.gameObject;

            return indicatorPrefab;
        }

        private static GameObject CreateTeslaDashTrackingIndicator()
        {
            GameObject indicatorPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/LightningIndicator"), "TeslaIndicator", false);

            UnityEngine.Object.DestroyImmediate(indicatorPrefab.transform.Find("TextMeshPro").gameObject);
            UnityEngine.Object.DestroyImmediate(indicatorPrefab.transform.Find("Holder/Brackets").gameObject);

            indicatorPrefab.transform.localScale = Vector3.one * .15f;
            indicatorPrefab.transform.localPosition = Vector3.zero;
            indicatorPrefab.transform.Find("Holder").rotation = Quaternion.identity;
            indicatorPrefab.transform.Find("Holder/Brackets").rotation = Quaternion.identity;

            TeslaIndicatorView indicatorViewComponent = indicatorPrefab.AddComponent<TeslaIndicatorView>();

            SpriteRenderer spriteRenderer = indicatorPrefab.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = assetBundle.LoadAsset<Sprite>("texIndicatorDash3");
            spriteRenderer.color = Color.white;
            spriteRenderer.transform.localRotation = Quaternion.identity;
            spriteRenderer.transform.localPosition = Vector3.zero;

            indicatorViewComponent.indicatorRenderer = spriteRenderer;

            //SpriteRenderer towerIndicator = UnityEngine.Object.Instantiate(spriteRenderer, spriteRenderer.transform.parent);
            //towerIndicator.sprite = Modules.Assets.LoadAsset<Sprite>("texIndicatorDash");
            //towerIndicator.color = Color.white;

            //indicatorViewComponent.towerIndicator = towerIndicator.gameObject;

            return indicatorPrefab;
        }

        private static GameObject CloneLightningOrbNod(string path, string name, float width = 1)
        {
            GameObject newEffect = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>(path), name, false);

            foreach (LineRenderer rend in newEffect.GetComponentsInChildren<LineRenderer>())
            {
                if (rend)
                {
                    Material mat = new Material(rend.sharedMaterial);
                    mat.SetTexture("_RemapTex", nodRampTex);
                    rend.sharedMaterial = mat;

                    rend.widthMultiplier = width;
                }
            }
            Content.CreateAndAddEffectDef(newEffect);

            return newEffect;
        }

        private static GameObject CreateZapConeProjectile()
        {
            GameObject zapConeProjectile = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LoaderZapCone"), "TeslaLoaderZapCone");

            ProjectileProximityBeamController beamController = zapConeProjectile.GetComponent<ProjectileProximityBeamController>();
            beamController.attackFireCount = ZapPunch.OrbCasts;
            beamController.attackRange = ZapPunch.OrbDistance;
            beamController.maxAngleFilter = 50;
            beamController.procCoefficient = ZapPunch.ProcCoefficient;
            beamController.damageCoefficient = 1;
            beamController.lightningType = RoR2.Orbs.LightningOrb.LightningType.MageLightning;
            //beamController.inheritDamageType = true;

            //DamageAPI.AddModdedDamageType(zapConeProjectile.GetComponent<ProjectileDamage>(), Modules.DamageTypes.conductive);
            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = zapConeProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(TeslaDamageTypes.Conductive);


            UnityEngine.Object.DestroyImmediate(zapConeProjectile.transform.Find("Effect").GetComponent<ShakeEmitter>());

            TeslaZapConeEffect = CreateTeslaZapConeEffect(zapConeProjectile);

            UnityEngine.Object.Destroy(zapConeProjectile.transform.Find("Effect/Distortion, 3D").gameObject);
            UnityEngine.Object.Destroy(zapConeProjectile.transform.Find("Effect/RadialMesh").gameObject);
            UnityEngine.Object.Destroy(zapConeProjectile.transform.Find("Effect/Flash").gameObject);

            Content.AddProjectilePrefab(zapConeProjectile);

            return zapConeProjectile;
        }

        private static GameObject CreateTeslaZapConeEffect(GameObject zapConeProjectile)
        {
            GameObject zapConeEffect = PrefabAPI.InstantiateClone(zapConeProjectile.transform.Find("Effect").gameObject, "TeslaPunchConeEffect", false);
            zapConeEffect.SetActive(true);

            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Sparks, Single").gameObject);
            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Lines").gameObject);
            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Point Light").gameObject);
            UnityEngine.Object.Destroy(zapConeEffect.transform.Find("Impact Shockwave").gameObject);

            ParticleSystem shockwaveParticle = zapConeEffect.transform.Find("RadialMesh").GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = shockwaveParticle.main;
            mainModule.startSpeed = 0.5f;
            zapConeEffect.transform.Find("RadialMesh").localScale = new Vector3(0.5f, 0.5f, 1);

            shockwaveParticle.GetComponent<ParticleSystemRenderer>().material.color = Color.cyan;

            Asset.CreateEffectFromObject(zapConeEffect, "", false);

            return zapConeEffect;
        }
    }
}
