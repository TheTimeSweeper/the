﻿using RoR2;
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
using R2API;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoAssets {

        public static List<IEnumerator> loads => Modules.ContentPacks.asyncLoadCoroutines;

        public static ChronoProjectionMotor markerPrefab;
        public static GameObject chronoBombProjectile;
        public static GameObject lunarSunExplosion;

        public static GameObject chronoIndicatorIvan;
        public static GameObject chronoIndicatorVanish;
        public static GameObject chronoIndicatorPhase;

        public static ChronoTether chronoVanishTether;
        public static GameObject chronoTracer;
        public static GameObject vanishEffect;

        public static ChronosphereProjection chronosphereProjection;
        public static ChronosphereProjection chronosphereProjectionFreeze;
        public static Material frozenOverlayMaterial;
        public static Material phaseOverlayMaterial;

        public static GameObject endPointivsualizer;
        public static GameObject arcvisualizer;

        public static SkillDef cancelSKillDef;

        public static List<Texture2D> testTextures = new List<Texture2D>();
        public static int noises = 4;

        public static void InitAsync(AssetBundle assetBundle)
        {
            Log.CurrentTime("INIT ASYNC");

            for (int i = 1; i <= noises; i++)
            {
                loads.Add(assetBundle.LoadAssetAsync("NOISE" + i, (Texture2D result) =>
                {
                    testTextures.Add(result);
                }));
            }

            loads.Add(assetBundle.LoadAssetAsync("texIconChronoCancel", (Sprite result) =>
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
            }));

            //projection
            loads.Add(assetBundle.LoadAssetAsync("ChronoProjection", (GameObject result) =>
            {
                markerPrefab = result.GetComponent<ChronoProjectionMotor>();
                R2API.PrefabAPI.RegisterNetworkPrefab(markerPrefab.gameObject);
                Modules.Content.AddNetworkedObject(markerPrefab.gameObject);
            }));

            //ivan bomb
            loads.Add(assetBundle.LoadAssetAsync("ChronoIvanBombProjectile", (GameObject ivanResult) =>
            {
                chronoBombProjectile = ivanResult;
                R2API.PrefabAPI.RegisterNetworkPrefab(chronoBombProjectile);
                Content.AddProjectilePrefab(chronoBombProjectile);

                loads.Add(Assets.LoadAddressableAssetAsync<GameObject>("RoR2/Base/StickyBomb/StickyBombGhost.prefab", (result) =>
                {
                    chronoBombProjectile.GetComponent<ProjectileController>().ghostPrefab = result;
                }));
                loads.Add(Assets.LoadAddressableAssetAsync<GameObject>("RoR2/DLC1/LunarSun/ExplosionLunarSun.prefab", (result) =>
                {
                    lunarSunExplosion = result;
                    chronoBombProjectile.GetComponent<ProjectileExplosion>().explosionEffect = lunarSunExplosion;
                }));
            }));

            //indicators
            loads.Add(assetBundle.LoadAssetAsync<GameObject>("IndicatorChronoIvan", (result) =>
            {
                chronoIndicatorIvan = result;
            }));
            loads.Add(assetBundle.LoadAssetAsync<GameObject>("IndicatorChronoVanish", (result) =>
            {
                chronoIndicatorVanish = result;
            }));
            loads.Add(assetBundle.LoadAssetAsync<GameObject>("IndicatorChronoPhaseCooldown", (result) =>
            {
                chronoIndicatorPhase = result;
            }));
            //vanish vfx
            loads.Add(assetBundle.LoadAssetAsync<GameObject>("ChronoVanishVFX", (result) =>
            {
                vanishEffect = result;
                Modules.Content.CreateAndAddEffectDef(vanishEffect);
            }));
            //visualizers
            loads.Add(Assets.LoadAddressableAssetAsync<GameObject>("RoR2/Base/Huntress/HuntressArrowRainIndicator.prefab", (result) =>
            {
                endPointivsualizer = result;
            }));
            loads.Add(Assets.LoadAddressableAssetAsync<GameObject>("RoR2/Base/Common/VFX/BasicThrowableVisualizer.prefab", (result) =>
            {
                arcvisualizer = result;
            }));
            //tether
            loads.Add(Assets.LoadAddressableAssetAsync<Material>("RoR2/Base/ClayBoss/matTrailSiphonHealth.mat", (Material beamMatResult) =>
            {
                Material beamMat = beamMatResult;

                loads.Add(assetBundle.LoadAssetAsync<GameObject>("ChronoTether", (result) =>
                {
                    chronoVanishTether = result.GetComponent<ChronoTether>();
                    LineRenderer line = chronoVanishTether.GetComponent<LineRenderer>();
                    line.sharedMaterial.SetTexture("_NormalTex", beamMat.GetTexture("_NormalTex"));
                    line.sharedMaterial.SetTexture("_Cloud1Tex", beamMat.GetTexture("_Cloud1Tex"));
                    line.sharedMaterial.SetTexture("_Cloud2Tex", beamMat.GetTexture("_Cloud2Tex"));
                }));

                loads.Add(assetBundle.LoadAssetAsync<GameObject>("ChronoTracer", (result) =>
                {
                    chronoTracer = result;
                    LineRenderer line = chronoTracer.GetComponentInChildren<LineRenderer>();
                    line.sharedMaterial.SetTexture("_NormalTex", beamMat.GetTexture("_NormalTex"));
                    line.sharedMaterial.SetTexture("_Cloud1Tex", beamMat.GetTexture("_Cloud1Tex"));
                    line.sharedMaterial.SetTexture("_Cloud2Tex", beamMat.GetTexture("_Cloud2Tex"));
                    Content.CreateAndAddEffectDef(chronoTracer);
                }));
            }));

            //chronospheres here we go
            loads.Add(assetBundle.LoadAssetAsync<GameObject>("ChronosphereProjection", (result) =>
            {
                chronosphereProjection = result.GetComponent<ChronosphereProjection>();
            }));
            loads.Add(assetBundle.LoadAssetAsync<GameObject>("chronosphereProjectionFreeze", (result) =>
            {
                chronosphereProjectionFreeze = result.GetComponent<ChronosphereProjection>();
                chronosphereProjectionFreeze.GetComponentInChildren<SphereCollider>().radius = ChronoConfig.M3_Freezosphere_Radius.Value;
            }));

            //overlay
            loads.Add(assetBundle.LoadAssetAsync<Material>("matChronosphereFreezeOverlay", (result) =>
            {
                frozenOverlayMaterial = result;
            }));
            loads.Add(assetBundle.LoadAssetAsync<Material>("matChronosphere1", (result) =>
            {
                phaseOverlayMaterial = result;
                Log.CurrentTime("LAST ASYNC LOAD");
            }));

            Log.CurrentTime("END ASYNC");
        }
    }
}
