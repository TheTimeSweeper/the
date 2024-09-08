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
using KinematicCharacterController;

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
        public static int noises = 0;

        private static AssetBundle assetBundle;

        public static List<IEnumerator> GetAssetBundleInitializedCoroutines(AssetBundle assetBundle)
        {
            return Asset.PreLoadAssetsAsyncCoroutines<Sprite>(assetBundle,
                "texIconChrono",
                "texIconChronoRA2",
                "texIconChronoPassive",
                "texIconChronoPrimary",
                "texIconChronoSecondary",
                "texIconChronoUtility",
                "texIconChronoUtilityAlt",
                "texIconChronoSpecial");

            //List<IEnumerator> subEnumerators = new List<IEnumerator>();
            
            ////subEnumerators.Add(assetBundle.LoadAssetAsync<Sprite>("texIconChrono",   null));
            ////subEnumerators.Add(assetBundle.LoadAssetAsync<Sprite>("texIconChronoRA2", null));
            //subEnumerators.Add(assetBundle.LoadAssetCoroutine<Sprite>("texIconChronoPassive", null));
            //subEnumerators.Add(assetBundle.LoadAssetCoroutine<Sprite>("texIconChronoPrimary", null));
            //subEnumerators.Add(assetBundle.LoadAssetCoroutine<Sprite>("texIconChronoSecondary", null));
            //subEnumerators.Add(assetBundle.LoadAssetCoroutine<Sprite>("texIconChronoUtility", null));
            //subEnumerators.Add(assetBundle.LoadAssetCoroutine<Sprite>("texIconChronoUtilityAlt", null));
            //subEnumerators.Add(assetBundle.LoadAssetCoroutine<Sprite>("texIconChronoSpecial", null));
            ////well this was equally as fast. guess just skip a dictionary then
            ////subEnumerators.Add(assetBundle.LoadAssetToCollection<Sprite>("texIconChrono", skillIcons));
            ////subEnumerators.Add(assetBundle.LoadAssetToCollection<Sprite>("texIconChronoRA2", skillIcons));
            ////subEnumerators.Add(assetBundle.LoadAssetToCollection<Sprite>("texIconChronoPassive", skillIcons));
            ////subEnumerators.Add(assetBundle.LoadAssetToCollection<Sprite>("texIconChronoPrimary", skillIcons));
            ////subEnumerators.Add(assetBundle.LoadAssetToCollection<Sprite>("texIconChronoSecondary", skillIcons));
            ////subEnumerators.Add(assetBundle.LoadAssetToCollection<Sprite>("texIconChronoUtility", skillIcons));
            ////subEnumerators.Add(assetBundle.LoadAssetToCollection<Sprite>("texIconChronoUtilityAlt", skillIcons));
            ////subEnumerators.Add(assetBundle.LoadAssetToCollection<Sprite>("texIconChronoSpecial", skillIcons));

            //return subEnumerators;
        }

        public static void OnCharacterInitializedE(AssetBundle assetBundle_)
        {
            Log.CurrentTime("INIT ASYNC");
            assetBundle = assetBundle_;

            //this didn't change with any new asyncasset stuff
            //TestNoisesTogether();

            assetBundle.LoadAssetAsync("texIconChronoUtilityCancel", 
                (Sprite result) =>
                {
                    cancelSKillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
                    {
                        skillName = "chronoCancel",
                        skillNameToken = ChronoSurvivor.TOKEN_PREFIX + "CANCEL_NAME",
                        skillDescriptionToken = ChronoSurvivor.TOKEN_PREFIX + "CANCEL_DESC",
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
            assetBundle.LoadAssetAsync("ChronoProjection",
                (GameObject result) =>
                {
                    markerPrefab = result.GetComponent<ChronoProjectionMotor>();
                    markerPrefab.GetComponent<KinematicCharacterMotor>().playerCharacter = true;//todo set in editor when ror2 lib is updated
                    R2API.PrefabAPI.RegisterNetworkPrefab(markerPrefab.gameObject);
                    Modules.Content.AddNetworkedObject(markerPrefab.gameObject);
                });

            //Ivan bomb
            Asset.LoadAssetsAsync(
                new AsyncAsset<GameObject>(assetBundle, "ChronoIvanBombProjectile"),
                new AsyncAsset<GameObject>("RoR2/DLC1/LunarSun/ExplosionLunarSun.prefab"),
                (ivanResult, result) =>
                {
                    chronoBombProjectile = ivanResult;
                    chronoBombProjectile.GetComponent<ProjectileController>().ghostPrefab.AddComponent<VFXAttributes>().DoNotPool = true;
                    R2API.PrefabAPI.RegisterNetworkPrefab(chronoBombProjectile);
                    Content.AddProjectilePrefab(chronoBombProjectile);
                    
                    GameObject bombExplosion = result.InstantiateClone(result.name + "ChronoBomb", false);
                    bombExplosion.GetComponent<EffectComponent>().soundName = "Play_IvanBomb_Explode";
                    Content.CreateAndAddEffectDef(bombExplosion);

                    chronoBombProjectile.GetComponent<ProjectileExplosion>().explosionEffect = bombExplosion;

                    lunarSunExplosion = result.InstantiateClone(result.name + "Chrono", false);
                    lunarSunExplosion.GetComponent<EffectComponent>().soundName = "";
                    Content.CreateAndAddEffectDef(lunarSunExplosion);

                });

            //indicators
            assetBundle.LoadAssetAsync("IndicatorChronoIvan", (GameObject result) => chronoIndicatorIvan = result);
            assetBundle.LoadAssetAsync("IndicatorChronoVanish", (GameObject result) => chronoIndicatorVanish = result);
            assetBundle.LoadAssetAsync("IndicatorChronoPhaseCooldown", (GameObject result) => chronoIndicatorPhase = result);

            //vanish vfx
            assetBundle.LoadAssetAsync("ChronoVanishVFX", 
                (GameObject result) =>
                {
                    vanishEffect = result;
                    Modules.Content.CreateAndAddEffectDef(vanishEffect);
                });

            //visualizers
            Asset.LoadAssetAsync("RoR2/Base/Huntress/HuntressArrowRainIndicator.prefab", (GameObject result) => endPointivsualizer = result);
            Asset.LoadAssetAsync("RoR2/Base/Common/VFX/BasicThrowableVisualizer.prefab", (GameObject result) => arcvisualizer = result);

            //tether
            Asset.LoadAssetsAsync(
                new AsyncAsset<Material>("RoR2/Base/ClayBoss/matTrailSiphonHealth.mat"),
                new AsyncAsset<GameObject>(assetBundle, "ChronoTether"),
                new AsyncAsset<GameObject>(assetBundle, "ChronoTracer"),
                (beamMatResult, tetherResult, tracerResult) =>
                {
                    chronoVanishTether = tetherResult.GetComponent<ChronoTether>();
                    LineRenderer tetherLine = chronoVanishTether.GetComponent<LineRenderer>();
                    tetherLine.sharedMaterial.SetTexture("_NormalTex", beamMatResult.GetTexture("_NormalTex"));
                    tetherLine.sharedMaterial.SetTexture("_Cloud1Tex", beamMatResult.GetTexture("_Cloud1Tex"));
                    tetherLine.sharedMaterial.SetTexture("_Cloud2Tex", beamMatResult.GetTexture("_Cloud2Tex"));

                    chronoTracer = tracerResult;
                    LineRenderer tracerLine = chronoTracer.GetComponentInChildren<LineRenderer>();
                    tracerLine.sharedMaterial.SetTexture("_NormalTex", beamMatResult.GetTexture("_NormalTex"));
                    tracerLine.sharedMaterial.SetTexture("_Cloud1Tex", beamMatResult.GetTexture("_Cloud1Tex"));
                    tracerLine.sharedMaterial.SetTexture("_Cloud2Tex", beamMatResult.GetTexture("_Cloud2Tex"));
                    Content.CreateAndAddEffectDef(chronoTracer);
                });

            //tether 2
            Asset.LoadAssetAsync("RoR2/Base/ClayBoss/matTrailSiphonHealth.mat", 
                (Material beamMatResult) =>
                {
                    Material beamMat = beamMatResult;

                    assetBundle.LoadAssetAsync("ChronoTether", 
                        (GameObject result) =>
                        {
                            chronoVanishTether = result.GetComponent<ChronoTether>();
                            LineRenderer line = chronoVanishTether.GetComponent<LineRenderer>();
                            line.sharedMaterial.SetTexture("_NormalTex", beamMat.GetTexture("_NormalTex"));
                            line.sharedMaterial.SetTexture("_Cloud1Tex", beamMat.GetTexture("_Cloud1Tex"));
                            line.sharedMaterial.SetTexture("_Cloud2Tex", beamMat.GetTexture("_Cloud2Tex"));
                        });

                    assetBundle.LoadAssetAsync("ChronoTracer",
                        (GameObject result) =>
                        {
                            chronoTracer = result;
                            LineRenderer line = chronoTracer.GetComponentInChildren<LineRenderer>();
                            line.sharedMaterial.SetTexture("_NormalTex", beamMat.GetTexture("_NormalTex"));
                            line.sharedMaterial.SetTexture("_Cloud1Tex", beamMat.GetTexture("_Cloud1Tex"));
                            line.sharedMaterial.SetTexture("_Cloud2Tex", beamMat.GetTexture("_Cloud2Tex"));
                            Content.CreateAndAddEffectDef(chronoTracer);
                        });
                });

            //chronospheres here we go
            assetBundle.LoadAssetAsync("ChronosphereProjection", 
                (GameObject result) => chronosphereProjection = result.GetComponent<ChronosphereProjection>());

            assetBundle.LoadAssetAsync("chronosphereProjectionFreeze",
                (GameObject result) =>
                {
                    chronosphereProjectionFreeze = result.GetComponent<ChronosphereProjection>();
                    chronosphereProjectionFreeze.GetComponentInChildren<SphereCollider>().radius = ChronoConfig.M3_Freezosphere_Radius.Value;
                });

            //overlay
            assetBundle.LoadAssetAsync("matChronosphereFreezeOverlay", (Material result) => frozenOverlayMaterial = result);
            assetBundle.LoadAssetAsync("matChronosphere1", (Material result) => phaseOverlayMaterial = result);

            Log.CurrentTime("END ASYNC");
        }

        public static void OnCharacterInitialized(AssetBundle assetBundle_)
        {
            Log.CurrentTime("INIT ASYNC");

            assetBundle = assetBundle_;

            TestNoisesTogether();

            loads.Add(assetBundle.LoadAssetCoroutine("texIconChronoUtilityCancel", (Sprite result) =>
            {
                cancelSKillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
                {
                    skillName = "chronoCancel",
                    skillNameToken = ChronoSurvivor.TOKEN_PREFIX + "CANCEL_NAME",
                    skillDescriptionToken = ChronoSurvivor.TOKEN_PREFIX + "CANCEL_DESC",
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
            loads.Add(assetBundle.LoadAssetCoroutine("ChronoProjection", (GameObject result) =>
            {
                markerPrefab = result.GetComponent<ChronoProjectionMotor>();
                markerPrefab.GetComponent<KinematicCharacterMotor>().playerCharacter = true;//todo set in editor when ror2 lib is updated
                R2API.PrefabAPI.RegisterNetworkPrefab(markerPrefab.gameObject);
                Modules.Content.AddNetworkedObject(markerPrefab.gameObject);
            }));

            //ivan bomb
            loads.Add(assetBundle.LoadAssetCoroutine("ChronoIvanBombProjectile", (GameObject ivanResult) =>
            {
                chronoBombProjectile = ivanResult;
                chronoBombProjectile.GetComponent<ProjectileController>().ghostPrefab.AddComponent<VFXAttributes>().DoNotPool = true;
                R2API.PrefabAPI.RegisterNetworkPrefab(chronoBombProjectile);
                Content.AddProjectilePrefab(chronoBombProjectile);

                loads.Add(Asset.LoadAssetCoroutine<GameObject>("RoR2/DLC1/LunarSun/ExplosionLunarSun.prefab", (result) =>
                {
                    GameObject bombExplosion = result.InstantiateClone(result.name + "ChronoBomb", false);
                    bombExplosion.GetComponent<EffectComponent>().soundName = "Play_IvanBomb_Explode";
                    Content.CreateAndAddEffectDef(bombExplosion);

                    chronoBombProjectile.GetComponent<ProjectileExplosion>().explosionEffect = bombExplosion;

                    lunarSunExplosion = result.InstantiateClone(result.name + "Chrono", false);
                    lunarSunExplosion.GetComponent<EffectComponent>().soundName = "";
                    Content.CreateAndAddEffectDef(lunarSunExplosion);
                }));
            }));

            //indicators
            loads.Add(assetBundle.LoadAssetCoroutine<GameObject>("IndicatorChronoIvan", (result) =>
            {
                chronoIndicatorIvan = result;
            }));
            loads.Add(assetBundle.LoadAssetCoroutine<GameObject>("IndicatorChronoVanish", (result) =>
            {
                chronoIndicatorVanish = result;
            }));
            loads.Add(assetBundle.LoadAssetCoroutine<GameObject>("IndicatorChronoPhaseCooldown", (result) =>
            {
                chronoIndicatorPhase = result;
            }));
            //vanish vfx
            loads.Add(assetBundle.LoadAssetCoroutine<GameObject>("ChronoVanishVFX", (result) =>
            {
                vanishEffect = result;
                Modules.Content.CreateAndAddEffectDef(vanishEffect);
            }));
            //visualizers
            loads.Add(Asset.LoadAssetCoroutine<GameObject>("RoR2/Base/Huntress/HuntressArrowRainIndicator.prefab", (result) =>
            {
                endPointivsualizer = result;
            }));
            loads.Add(Asset.LoadAssetCoroutine<GameObject>("RoR2/Base/Common/VFX/BasicThrowableVisualizer.prefab", (result) =>
            {
                arcvisualizer = result;
            }));
            //tether
            loads.Add(Asset.LoadAssetCoroutine<Material>("RoR2/Base/ClayBoss/matTrailSiphonHealth.mat", (Material beamMatResult) =>
            {
                Material beamMat = beamMatResult;

                loads.Add(assetBundle.LoadAssetCoroutine<GameObject>("ChronoTether", (result) =>
                {
                    chronoVanishTether = result.GetComponent<ChronoTether>();
                    LineRenderer line = chronoVanishTether.GetComponent<LineRenderer>();
                    line.sharedMaterial.SetTexture("_NormalTex", beamMat.GetTexture("_NormalTex"));
                    line.sharedMaterial.SetTexture("_Cloud1Tex", beamMat.GetTexture("_Cloud1Tex"));
                    line.sharedMaterial.SetTexture("_Cloud2Tex", beamMat.GetTexture("_Cloud2Tex"));
                }));

                loads.Add(assetBundle.LoadAssetCoroutine<GameObject>("ChronoTracer", (result) =>
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
            loads.Add(assetBundle.LoadAssetCoroutine<GameObject>("ChronosphereProjection", (result) =>
            {
                chronosphereProjection = result.GetComponent<ChronosphereProjection>();
            }));
            loads.Add(assetBundle.LoadAssetCoroutine<GameObject>("chronosphereProjectionFreeze", (result) =>
            {
                chronosphereProjectionFreeze = result.GetComponent<ChronosphereProjection>();
                chronosphereProjectionFreeze.GetComponentInChildren<SphereCollider>().radius = ChronoConfig.M3_Freezosphere_Radius.Value;
            }));

            //overlay
            loads.Add(assetBundle.LoadAssetCoroutine<Material>("matChronosphereFreezeOverlay", (result) =>
            {
                frozenOverlayMaterial = result;
            }));
            loads.Add(assetBundle.LoadAssetCoroutine<Material>("matChronosphere1", (result) =>
            {
                phaseOverlayMaterial = result;
                Log.CurrentTime("LAST ASYNC LOAD");
            }));

            Log.CurrentTime("END ASYNC");
        }

        private static void TestNoisesTogether()
        {
            if (noises > 0)
            {
                List<IEnumerator> testSubEnumerators = new List<IEnumerator>();

                loads.Add(loadSubEnumerators());

                IEnumerator loadSubEnumerators()
                {
                    for (int i = 1; i <= noises; i++)
                    {
                        /*loads.Add*/
                        testSubEnumerators.Add(assetBundle.LoadAssetCoroutine("NOISE" + i, (Texture2D result) =>
                        {
                            testTextures.Add(result);
                            Log.Warning(result);
                        }));
                    }
                    for (int i = 0; i < testSubEnumerators.Count; i++)
                    {
                        while (testSubEnumerators[i].MoveNext()) yield return null;
                    }
                }
            }
        }

        private static void TestNoisesAsyncAsset()
        {
            if (noises > 0)
            {
                List<AsyncAsset<Texture2D>> testAsyncAssets = new List<AsyncAsset<Texture2D>>();

                for (int i = 1; i <= noises; i++)
                {
                    testAsyncAssets.Add(new AsyncAsset<Texture2D>(assetBundle, "NOISE" + i).AddCoroutine());
                }
            }
        }
    }
}
