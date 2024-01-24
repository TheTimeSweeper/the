using RoR2;
using UnityEngine;
using RA2Mod.Modules;
using System;
using RoR2.Projectile;
using RA2Mod.Survivors.Chrono.Components;
using UnityEngine.AddressableAssets;
using RoR2.Audio;
using RoR2.Skills;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoAssets {

        public static ChronoProjectionMotor markerPrefab;
        public static GameObject chronoBombProjectile;

        public static GameObject chronoIndicatorIvan;
        public static GameObject chronoIndicatorVanish;
        public static GameObject chronoIndicatorPhase;

        public static ChronoTether chronoVanishTether;
        public static GameObject vanishEffect;

        public static ChronosphereProjection chronosphereProjection;

        public static GameObject endPointivsualizer;
        public static GameObject arcvisualizer;

        public static SkillDef cancelSKillDef;

        public static List<Texture2D> testTextures = new List<Texture2D>();

        public static void Init(AssetBundle assetBundle)
        {
            Log.CurrentTime("SYNC START");

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

            GameObject sprintProjectionPrefabObject = assetBundle.LoadAsset<GameObject>("ChronoProjection");
            R2API.PrefabAPI.RegisterNetworkPrefab(sprintProjectionPrefabObject);
            markerPrefab = sprintProjectionPrefabObject.GetComponent<ChronoProjectionMotor>();

            chronoBombProjectile = assetBundle.LoadAsset<GameObject>("ChronoIvanBombProjectile");
            R2API.PrefabAPI.RegisterNetworkPrefab(chronoBombProjectile);
            chronoBombProjectile.GetComponent<ProjectileController>().ghostPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/StickyBomb").GetComponent<ProjectileController>().ghostPrefab;
            chronoBombProjectile.GetComponent<ProjectileImpactExplosion>().explosionEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosionLunarSun");
            Content.AddProjectilePrefab(chronoBombProjectile);

            chronoIndicatorIvan = assetBundle.LoadAsset<GameObject>("IndicatorChronoIvan");
            chronoIndicatorVanish = assetBundle.LoadAsset<GameObject>("IndicatorChronoVanish");
            chronoIndicatorPhase = assetBundle.LoadAsset<GameObject>("IndicatorChronoPhaseCooldown");

            GameObject beamObject = assetBundle.LoadAsset<GameObject>("ChronoTether");
            Material beamMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/ClayBoss/matTrailSiphonHealth.mat").WaitForCompletion();
            beamMat = new Material(beamMat);
            Texture2D lightningRamp = Addressables.LoadAssetAsync<Texture2D>("RoR2/Base/Common/ColorRamps/texRampLightning2.png").WaitForCompletion();
            beamMat.SetTexture("_RemapTex", lightningRamp);
            beamObject.GetComponent<LineRenderer>().sharedMaterial = beamMat;
            chronoVanishTether = beamObject.GetComponent<ChronoTether>();

            vanishEffect = assetBundle.LoadAsset<GameObject>("ChronoVanishVFX");

            endPointivsualizer = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Huntress/HuntressArrowRainIndicator.prefab").WaitForCompletion();
            arcvisualizer = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/BasicThrowableVisualizer.prefab").WaitForCompletion();

            GameObject chronosphereProjectionObject = assetBundle.LoadAsset<GameObject>("ChronosphereProjection");
            chronosphereProjection = chronosphereProjectionObject.GetComponent<ChronosphereProjection>();
            Material sphereMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/Icicle/matIceAuraSphere.mat").WaitForCompletion();
            sphereMat.SetFloat("_Boost", 1.85f);
            sphereMat.SetFloat("_RimPower", 1.36f);
            sphereMat.SetFloat("_RimStrength", 0.84f);
            sphereMat.SetFloat("_AlphaBoost", 0.51f);
            sphereMat.SetFloat("_IntersectionStrength", 12.86f);
            sphereMat.SetTexture("_Cloud2Tex", Addressables.LoadAssetAsync<Texture2D>("RoR2/Base/Common/texCloudLightning1.png").WaitForCompletion());
            sphereMat.SetTextureScale("_Cloud2Tex", new Vector2(0.01f, 0.01f));
            sphereMat.SetTextureScale("_Cloud1Tex", new Vector2(0.02f, 0.02f));
            sphereMat.SetTexture("_RemapTex", lightningRamp);

            chronosphereProjection.sphereRenderer.sharedMaterial = sphereMat;
            R2API.PrefabAPI.RegisterNetworkPrefab(chronosphereProjectionObject);
            Content.AddNetworkedObject(chronosphereProjectionObject);

            for (int i = 1; i < 21; i++)
            {
                testTextures.Add(assetBundle.LoadAsset<Texture2D>("testTexture " + i));
            }

            Log.CurrentTime("SYNC FINISH");
        }

        public static IEnumerator InitAsync(AssetBundle assetBundle)
        {
            Log.CurrentTime("ASYNC START");

            yield return assetBundle.LoadAssetAsync("texIconChronoCancel", (Sprite result) =>
            {
                cancelSKillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
                {
                    skillName = "chronoCancel",
                    skillNameToken = ChronoSurvivor.CHRONO_PREFIX + "CANCEL_NAME",
                    skillDescriptionToken = ChronoSurvivor.CHRONO_PREFIX + "CANCEL_DESC",
                    keywordTokens = new string[] { "KEYWORD_AGILE" },
                    skillIcon = result,

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
            });

            //projection
            yield return assetBundle.LoadAssetAsync("ChronoProjection", (GameObject result) =>
            {
                markerPrefab = result.GetComponent<ChronoProjectionMotor>();
                R2API.PrefabAPI.RegisterNetworkPrefab(markerPrefab.gameObject);
            });

            //ivan bomb
            yield return assetBundle.LoadAssetAsync("ChronoIvanBombProjectile", (GameObject result) =>
            {
                chronoBombProjectile = result;
            });
            R2API.PrefabAPI.RegisterNetworkPrefab(chronoBombProjectile);
            yield return Assets.LoadAddressableAssetAsync<GameObject>("RoR2/Base/StickyBomb/StickyBombGhost.prefab", (result) =>
            {
                chronoBombProjectile.GetComponent<ProjectileController>().ghostPrefab = result;
            });
            yield return Assets.LoadAddressableAssetAsync<GameObject>("RoR2/DLC1/LunarSun/ExplosionLunarSun.prefab", (result) =>
            {
                chronoBombProjectile.GetComponent<ProjectileImpactExplosion>().explosionEffect = result;
            });
            Content.AddProjectilePrefab(chronoBombProjectile);

            //indicators
            yield return assetBundle.LoadAssetAsync<GameObject>("IndicatorChronoIvan", (result) =>
            {
                chronoIndicatorIvan = result;
            });
            yield return assetBundle.LoadAssetAsync<GameObject>("IndicatorChronoVanish", (result) =>
            {
                chronoIndicatorVanish = result;
            });
            yield return assetBundle.LoadAssetAsync<GameObject>("IndicatorChronoPhaseCooldown", (result) =>
            {
                chronoIndicatorPhase = result;
            });

            Texture2D lightningRamp = null;
            yield return Assets.LoadAddressableAssetAsync<Texture2D>("RoR2/Base/Common/ColorRamps/texRampLightning2.png", (result) =>
            {
                lightningRamp = result;
            });

            //tether
            yield return assetBundle.LoadAssetAsync<GameObject>("ChronoTether", (result) =>
            {
                chronoVanishTether = result.GetComponent<ChronoTether>();
            });
            yield return Assets.LoadAddressableAssetAsync<Material>("RoR2/Base/ClayBoss/matTrailSiphonHealth.mat", (result) =>
            {
                Material beamMat = new Material(result);
                beamMat.SetTexture("_RemapTex", lightningRamp);
                chronoVanishTether.GetComponent<LineRenderer>().sharedMaterial = beamMat;
            });


            //vanish vfx
            yield return assetBundle.LoadAssetAsync<GameObject>("ChronoVanishVFX", (result) =>
            {
                vanishEffect = result;
            });

            //visualizers
            yield return Assets.LoadAddressableAssetAsync<GameObject>("RoR2/Base/Huntress/HuntressArrowRainIndicator.prefab", (result) =>
            {
                endPointivsualizer = result;
            });
            yield return Assets.LoadAddressableAssetAsync<GameObject>("RoR2/Base/Common/VFX/BasicThrowableVisualizer.prefab", (result) =>
            {
                arcvisualizer = result;
            });

            //chronosphere here we go
            yield return assetBundle.LoadAssetAsync<GameObject>("ChronosphereProjection", (result) =>
            {
                chronosphereProjection = result.GetComponent<ChronosphereProjection>();
            });

            Texture2D lightningCloud = null;
            yield return Assets.LoadAddressableAssetAsync<Texture2D>("RoR2/Base/Common/ColorRamps/texRampLightning2.png", (result) =>
            {
                lightningCloud = result;
            });
            yield return Assets.LoadAddressableAssetAsync<Material>("RoR2/Base/Icicle/matIceAuraSphere.mat", (result) =>
            {
                Material sphereMat = new Material(result);
                sphereMat.SetFloat("_Boost", 1.85f);
                sphereMat.SetFloat("_RimPower", 1.36f);
                sphereMat.SetFloat("_RimStrength", 0.84f);
                sphereMat.SetFloat("_AlphaBoost", 0.51f);
                sphereMat.SetFloat("_IntersectionStrength", 12.86f);
                sphereMat.SetTexture("_Cloud2Tex", lightningCloud);
                sphereMat.SetTextureScale("_Cloud2Tex", new Vector2(0.01f, 0.01f));
                sphereMat.SetTextureScale("_Cloud1Tex", new Vector2(0.02f, 0.02f));
                sphereMat.SetTexture("_RemapTex", lightningRamp);
                chronosphereProjection.sphereRenderer.sharedMaterial = sphereMat;
            });
            R2API.PrefabAPI.RegisterNetworkPrefab(chronosphereProjection.gameObject);
            Content.AddNetworkedObject(chronosphereProjection.gameObject);

            for (int i = 1; i < 21; i++)
            {
                yield return assetBundle.LoadAssetAsync<Texture2D>("testTexture " + i, (result) =>
                {
                    testTextures.Add(result);
                });
            }

            Log.CurrentTime("ASYNC FINISH");
        }
    }
}
