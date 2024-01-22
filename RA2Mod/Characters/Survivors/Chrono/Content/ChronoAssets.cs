using RoR2;
using UnityEngine;
using RA2Mod.Modules;
using System;
using RoR2.Projectile;
using RA2Mod.Survivors.Chrono.Components;
using UnityEngine.AddressableAssets;
using RoR2.Audio;
using RoR2.Skills;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoAssets {

        public static ChronoProjectionMotor markerPrefab;
        public static GameObject chronoBombProjectile;

        public static GameObject chronoIndicatorIvan;
        public static GameObject chronoIndicatorVanish;
        public static GameObject chronoIndicatorPhase;

        public static ChronoTether chronoVanishTether;

        public static ChronosphereProjection chronosphereProjection;

        public static GameObject visualizer;

        public static SkillDef cancelSKillDef;

        public static void Init(AssetBundle assetBundle) {
            cancelSKillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "chronoCancel",
                skillNameToken = ChronoSurvivor.CHRONO_PREFIX + "CANCEL_NAME",
                skillDescriptionToken = ChronoSurvivor.CHRONO_PREFIX + "CANCEL_DESC",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Any,

                baseRechargeInterval = 1f,
                baseMaxStock = 0,

                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 0,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            GameObject markerPrefabObject = assetBundle.LoadAsset<GameObject>("ChronoProjection");
            R2API.PrefabAPI.RegisterNetworkPrefab(markerPrefabObject);
            markerPrefab = markerPrefabObject.GetComponent<ChronoProjectionMotor>();

            chronoBombProjectile = assetBundle.LoadAsset<GameObject>("ChronoIvanBombProjectile");
            R2API.PrefabAPI.RegisterNetworkPrefab(chronoBombProjectile);
            chronoBombProjectile.GetComponent<ProjectileController>().ghostPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/StickyBomb").GetComponent<ProjectileController>().ghostPrefab;
            chronoBombProjectile.GetComponent<ProjectileImpactExplosion>().explosionEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosionLunarSun");

            chronoIndicatorIvan = assetBundle.LoadAsset<GameObject>("IndicatorChronoIvan");
            chronoIndicatorVanish = assetBundle.LoadAsset<GameObject>("IndicatorChronoVanish");
            chronoIndicatorPhase = assetBundle.LoadAsset<GameObject>("IndicatorChronoPhaseCooldown");

            GameObject beamObject = assetBundle.LoadAsset<GameObject>("ChronoTether");
            Material beamMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/ClayBoss/matTrailSiphonHealth.mat").WaitForCompletion();
            beamMat = new Material(beamMat);
            beamMat.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture2D>("RoR2/Base/Common/ColorRamps/texRampLightning2.png").WaitForCompletion());
            beamObject.GetComponent<LineRenderer>().sharedMaterial = beamMat;
            chronoVanishTether = beamObject.GetComponent<ChronoTether>();

            visualizer = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Huntress/HuntressArrowRainIndicator.prefab").WaitForCompletion();

            GameObject gob = assetBundle.LoadAsset<GameObject>("ChronosphereProjection");
            chronosphereProjection = gob.GetComponent<ChronosphereProjection>();
            chronosphereProjection.sphereRenderer.sharedMaterial = Addressables.LoadAssetAsync<Material>("RoR2/Base/Icicle/matIceAuraSphere.mat").WaitForCompletion();
            Content.AddNetworkedObject(chronosphereProjection.gameObject);
        }
    }
}
