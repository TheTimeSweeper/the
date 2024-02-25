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
using R2API;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoAssets {

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

        public static GameObject endPointivsualizer;
        public static GameObject arcvisualizer;

        public static SkillDef cancelSKillDef;

        public static List<Texture2D> testTextures = new List<Texture2D>();
        public static int noises = 4;

        //public static List<Texture2D> testTextures = new List<Texture2D>();

        public static void Init(AssetBundle assetBundle)
        {
            Log.CurrentTime("SYNC START");

            for (int i = 1; i <= noises; i++)
            {
                testTextures.Add(assetBundle.LoadAsset<Texture2D>("NOISE" + i));
            }

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
            chronoBombProjectile.GetComponent<ProjectileExplosion>().explosionEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosionLunarSun");
            Content.AddProjectilePrefab(chronoBombProjectile);

            chronoIndicatorIvan = assetBundle.LoadAsset<GameObject>("IndicatorChronoIvan");
            chronoIndicatorVanish = assetBundle.LoadAsset<GameObject>("IndicatorChronoVanish");
            chronoIndicatorPhase = assetBundle.LoadAsset<GameObject>("IndicatorChronoPhaseCooldown");

            vanishEffect = assetBundle.LoadAsset<GameObject>("ChronoVanishVFX");
            Modules.Content.CreateAndAddEffectDef(vanishEffect);

            endPointivsualizer = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Huntress/HuntressArrowRainIndicator.prefab").WaitForCompletion();
            arcvisualizer = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/BasicThrowableVisualizer.prefab").WaitForCompletion();

            Material beamMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/ClayBoss/matTrailSiphonHealth.mat").WaitForCompletion();
            beamMat = new Material(beamMat);

            chronoVanishTether = assetBundle.LoadAsset<GameObject>("ChronoTether").GetComponent<ChronoTether>();
            LineRenderer vanishLine = chronoVanishTether.GetComponent<LineRenderer>();
            vanishLine.sharedMaterial.SetTexture("_NormalTex", beamMat.GetTexture("_NormalTex"));
            vanishLine.sharedMaterial.SetTexture("_Cloud1Tex", beamMat.GetTexture("_Cloud1Tex"));
            vanishLine.sharedMaterial.SetTexture("_Cloud2Tex", beamMat.GetTexture("_Cloud2Tex"));

            chronoTracer = assetBundle.LoadAsset<GameObject>("ChronoTracer");
            LineRenderer tracerLine = chronoTracer.GetComponentInChildren<LineRenderer>();
            tracerLine.sharedMaterial.SetTexture("_NormalTex", beamMat.GetTexture("_NormalTex"));
            tracerLine.sharedMaterial.SetTexture("_Cloud1Tex", beamMat.GetTexture("_Cloud1Tex"));
            tracerLine.sharedMaterial.SetTexture("_Cloud2Tex", beamMat.GetTexture("_Cloud2Tex"));
            Content.CreateAndAddEffectDef(chronoTracer);

            chronosphereProjection = assetBundle.LoadAsset<GameObject>("ChronosphereProjection").GetComponent<ChronosphereProjection>();

            chronosphereProjectionFreeze = assetBundle.LoadAsset<GameObject>("chronosphereProjectionFreeze").GetComponent<ChronosphereProjection>();
            chronosphereProjectionFreeze.GetComponentInChildren<SphereCollider>().radius = ChronoConfig.M3_Freezosphere_Radius.Value;

            frozenOverlayMaterial = assetBundle.LoadAsset<Material>("matChronosphereFreezeOverlay");

            Log.CurrentTime("SYNC FINISH");
        }

        public static IEnumerator InitAsync(AssetBundle assetBundle)
        {
            for (int i = 1; i <= noises; i++)
            {
                yield return assetBundle.LoadAssetAsyncYielding("NOISE" + i, (Texture2D result) =>
                {
                    testTextures.Add(result);
                });
            }

            yield return assetBundle.LoadAssetAsyncYielding("texIconChronoCancel", (Sprite result) =>
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
            yield return assetBundle.LoadAssetAsyncYielding("ChronoProjection", (GameObject result) =>
            {
                markerPrefab = result.GetComponent<ChronoProjectionMotor>();
                R2API.PrefabAPI.RegisterNetworkPrefab(markerPrefab.gameObject);
            });

            //ivan bomb
            yield return assetBundle.LoadAssetAsyncYielding<GameObject>("ChronoIvanBombProjectile", subLoadIvanBomb);
            IEnumerator subLoadIvanBomb(GameObject ivanResult)
            {
                chronoBombProjectile = ivanResult;
                R2API.PrefabAPI.RegisterNetworkPrefab(chronoBombProjectile);
                Content.AddProjectilePrefab(chronoBombProjectile);

                yield return Assets.LoadAddressableAssetAsyncYielding<GameObject>("RoR2/Base/StickyBomb/StickyBombGhost.prefab", (result) =>
                {
                    chronoBombProjectile.GetComponent<ProjectileController>().ghostPrefab = result;
                });
                yield return Assets.LoadAddressableAssetAsyncYielding<GameObject>("RoR2/DLC1/LunarSun/ExplosionLunarSun.prefab", (result) =>
                {
                    lunarSunExplosion = result;
                    chronoBombProjectile.GetComponent<ProjectileExplosion>().explosionEffect = lunarSunExplosion;
                });
            }

            //indicators
            yield return assetBundle.LoadAssetAsyncYielding<GameObject>("IndicatorChronoIvan", (result) =>
            {
                chronoIndicatorIvan = result;
            });
            yield return assetBundle.LoadAssetAsyncYielding<GameObject>("IndicatorChronoVanish", (result) =>
            {
                chronoIndicatorVanish = result;
            });
            yield return assetBundle.LoadAssetAsyncYielding<GameObject>("IndicatorChronoPhaseCooldown", (result) =>
            {
                chronoIndicatorPhase = result;
            });
            //vanish vfx
            yield return assetBundle.LoadAssetAsyncYielding<GameObject>("ChronoVanishVFX", (result) =>
            {
                vanishEffect = result;
                Modules.Content.CreateAndAddEffectDef(vanishEffect);
            });
            //visualizers
            yield return Assets.LoadAddressableAssetAsyncYielding<GameObject>("RoR2/Base/Huntress/HuntressArrowRainIndicator.prefab", (result) =>
            {
                endPointivsualizer = result;
            });
            yield return Assets.LoadAddressableAssetAsyncYielding<GameObject>("RoR2/Base/Common/VFX/BasicThrowableVisualizer.prefab", (result) =>
            {
                arcvisualizer = result;
            });

            //tether
            yield return Assets.LoadAddressableAssetAsyncYielding<Material>("RoR2/Base/ClayBoss/matTrailSiphonHealth.mat", loadBeamMat);
            IEnumerator loadBeamMat(Material beamMatResult)
            {
                Material beamMat = beamMatResult;

                yield return assetBundle.LoadAssetAsyncYielding<GameObject>("ChronoTether", (result) =>
                {
                    chronoVanishTether = result.GetComponent<ChronoTether>();
                    LineRenderer line = chronoVanishTether.GetComponent<LineRenderer>();
                    line.sharedMaterial.SetTexture("_NormalTex", beamMat.GetTexture("_NormalTex"));
                    line.sharedMaterial.SetTexture("_Cloud1Tex", beamMat.GetTexture("_Cloud1Tex"));
                    line.sharedMaterial.SetTexture("_Cloud2Tex", beamMat.GetTexture("_Cloud2Tex"));
                });

                yield return assetBundle.LoadAssetAsyncYielding<GameObject>("ChronoTracer", (result) =>
                {
                    chronoTracer = result;
                    LineRenderer line = chronoTracer.GetComponentInChildren<LineRenderer>();
                    line.sharedMaterial.SetTexture("_NormalTex", beamMat.GetTexture("_NormalTex"));
                    line.sharedMaterial.SetTexture("_Cloud1Tex", beamMat.GetTexture("_Cloud1Tex"));
                    line.sharedMaterial.SetTexture("_Cloud2Tex", beamMat.GetTexture("_Cloud2Tex"));
                    Content.CreateAndAddEffectDef(chronoTracer);
                });
            }

            //chronosphere here we go
            yield return assetBundle.LoadAssetAsyncYielding<GameObject>("ChronosphereProjection", loadChronoProjection);
            /*IEnumerator */ void loadChronoProjection(GameObject chronosphereResult)
            {
                chronosphereProjection = chronosphereResult.GetComponent<ChronosphereProjection>();

                //Texture2D lightningCloud = null;
                //yield return Assets.LoadAddressableAssetAsync<Texture2D>("RoR2/Base/Common/texCloudLightning1.png", (result) =>
                //{
                //    lightningCloud = result;
                //});
                //Texture2D magmaCloud = null;
                //yield return Assets.LoadAddressableAssetAsync<Texture2D>("RoR2/Base/Common/texMagmaCloud.png", (result) =>
                //{
                //    magmaCloud = result;
                //});

                //for (int i = 0; i < chronosphereProjection.sphereRenderers.Length; i++)
                //{
                //    Renderer rend = chronosphereProjection.sphereRenderers[i];
                //    rend.sharedMaterial.SetTexture("_Cloud1Tex", magmaCloud);
                //    rend.sharedMaterial.SetTexture("_Cloud2Tex", lightningCloud);
                //}
            }
            //freezosphere
            yield return assetBundle.LoadAssetAsyncYielding<GameObject>("chronosphereProjectionFreeze", loadChronoProjectionFreeze);
            /*IEnumerator */ void loadChronoProjectionFreeze(GameObject chronosphereResult)
            {
                chronosphereProjectionFreeze = chronosphereResult.GetComponent<ChronosphereProjection>();
                
                chronosphereProjectionFreeze.GetComponentInChildren<SphereCollider>().radius = ChronoConfig.M3_Freezosphere_Radius.Value;
                //Texture2D lightningCloud = null;
                //yield return Assets.LoadAddressableAssetAsync<Texture2D>("RoR2/Base/Common/texCloudLightning1.png", (result) =>
                //{
                //    lightningCloud = result;
                //});
                //Texture2D magmaCloud = null;
                //yield return Assets.LoadAddressableAssetAsync<Texture2D>("RoR2/Base/Common/texMagmaCloud.png", (result) =>
                //{
                //    magmaCloud = result;
                //});

                //for (int i = 0; i < chronosphereProjection.sphereRenderers.Length; i++)
                //{
                //    Renderer rend = chronosphereProjection.sphereRenderers[i];
                //    rend.sharedMaterial.SetTexture("_Cloud1Tex", magmaCloud);
                //    rend.sharedMaterial.SetTexture("_Cloud2Tex", lightningCloud);
                //}
            }
            //overlay
            yield return assetBundle.LoadAssetAsyncYielding<Material>("matChronosphereFreezeOverlay", (result) =>
            {
                frozenOverlayMaterial = result;
            });
        }

        public static List<IEnumerator> InitAsync2(AssetBundle assetBundle)
        {
            List<IEnumerator> loads = new List<IEnumerator>();
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

            return loads;
        }
    }
}
