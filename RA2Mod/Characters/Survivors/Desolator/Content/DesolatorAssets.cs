using R2API;
using RA2Mod.General.Components;
using RA2Mod.Modules;
using RA2Mod.Survivors.Desolator.Components;
using RA2Mod.Survivors.Desolator.States;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using ThreeEyedGames;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorAssets
    {
        public static GameObject DesolatorTracerRebar;
        public static GameObject DesolatorTracerSnipe;

        public static TeamAreaIndicator DesolatorTeamAreaIndicatorPrefab;

        public static GameObject IrradiatedImpactEffect;

        public static GameObject DesolatorIrradiatorProjectile;
        public static GameObject DesolatorIrradiatorProjectileScepter;

        public static GameObject DesolatorDeployProjectile;
        public static GameObject DesolatorDeployProjectileScepter;
        public static GameObject DesolatorDeployProjectileEmote;

        public static GameObject DesolatorCrocoLeapProjectile;

        public static GameObject DesolatorAuraPrefab;

        private static AssetBundle assetBundle;

        public static List<IEnumerator> GetAssetBundleInitializedCoroutines(AssetBundle assetBundle)
        {
            List<IEnumerator> loads = new List<IEnumerator>();
            //irradiator projectile
            return loads;
        }

        public static void OnCharacterInitialized(AssetBundle assetBundle)
        {
            DesolatorAssets.assetBundle = assetBundle;

            DesolatorTracerRebar = CreateDesolatorTracerRebar();
            DesolatorTracerSnipe = CreateDesolatorTracerSnipe();

            DesolatorTeamAreaIndicatorPrefab = CreateDesolatorTeamAreaIndicator();

            DesolatorIrradiatorProjectile = CreateIrradiatorProjectile();

            DesolatorIrradiatorProjectileScepter = CreateIrradiatorProjectileScepter();

            IrradiatedImpactEffect = DesolatorIrradiatorProjectile.GetComponent<ProjectileDotZone>().impactEffect;

            DesolatorCrocoLeapProjectile = CreateDesolatorCrocoLeapProjectile();
            Asset.CreateEffectFromObject(IrradiatedImpactEffect, "", false);

            DesolatorAuraPrefab = CreateDesolatorAura();

            DesolatorDeployProjectile = CreateDesolatorDeployProjectile();
            DesolatorDeployProjectileScepter = CreateDesolatorDeployProjectileScepter();
            DesolatorDeployProjectileEmote = CreateDesolatorDeployProjectileEmote();
        }

        #region desolator stuff

        private static GameObject CreateDesolatorTracerRebar()
        {
            GameObject tracer = CloneTracer("TracerToolbotRebar", "TracerDeslotorRebar", Color.green, 3);

            UnityEngine.Object.Destroy(tracer.transform.Find("StickEffect").gameObject);

            return tracer;
        }

        private static GameObject CreateDesolatorTracerSnipe()
        {
            GameObject tracer = CloneTracer("TracerHuntressSnipe", "TracerDeslotorHuntressSnipe", Color.green, 3);

            UnityEngine.Object.Destroy(tracer.transform.Find("TracerHead").gameObject);

            return tracer;
        }

        private static TeamAreaIndicator CreateDesolatorTeamAreaIndicator()
        {
            GameObject impvoidspike = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ImpVoidspikeProjectile");

            TeamAreaIndicator teamAreaIndicator = PrefabAPI.InstantiateClone(impvoidspike.transform.Find("ImpactEffect/TeamAreaIndicator, FullSphere").gameObject, "DesolatorTeamAreaIndicator", false).GetComponent<TeamAreaIndicator>();

            teamAreaIndicator.teamMaterialPairs[1].sharedMaterial = new Material(teamAreaIndicator.teamMaterialPairs[1].sharedMaterial);
            teamAreaIndicator.teamMaterialPairs[1].sharedMaterial.SetColor("_TintColor", Color.green);

            return teamAreaIndicator;
        }

        private static GameObject CreateDesolatorDeployProjectile()
        {

            GameObject DeployProjectile = PrefabAPI.InstantiateClone(assetBundle.LoadAsset<GameObject>("DeployProjectile"), "DeployProjectile", true);

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = DeployProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DesolatorDamageTypes.DesolatorDot);

            TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, DeployProjectile.transform);
            areaIndicator.teamFilter = DeployProjectile.GetComponent<TeamFilter>();
            areaIndicator.transform.localScale = Vector3.one * DeployIrradiate.Range;

            DeployProjectile.transform.Find("Hitboxes").localScale = Vector3.one * DeployIrradiate.Range;

            DeployProjectile.GetComponentInChildren<LightRadiusScale>().sizeMultiplier = ThrowIrradiator.Range;

            Content.AddProjectilePrefab(DeployProjectile);

            return DeployProjectile;
        }


        private static GameObject CreateDesolatorDeployProjectileEmote()
        {

            GameObject DeployProjectile = PrefabAPI.InstantiateClone(assetBundle.LoadAsset<GameObject>("DeployProjectileEmote"), "DeployProjectileEmote", true);

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = DeployProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DesolatorDamageTypes.DesolatorDot);

            //TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, DeployProjectile.transform);
            //areaIndicator.teamFilter = DeployProjectile.GetComponent<TeamFilter>();
            //areaIndicator.transform.localScale = Vector3.one * DeployIrradiate.Range;

            DeployProjectile.transform.Find("Hitboxes").localScale = Vector3.one * EmoteRadiationProjectile.Range;

            DeployProjectile.GetComponentInChildren<LightRadiusScale>().sizeMultiplier = EmoteRadiationProjectile.Range;

            Content.AddProjectilePrefab(DeployProjectile);

            return DeployProjectile;
        }

        private static GameObject CreateDesolatorDeployProjectileScepter()
        {

            GameObject DeployProjectile = PrefabAPI.InstantiateClone(assetBundle.LoadAsset<GameObject>("DeployProjectile"), "DeployProjectileScepter", true);

            DeployProjectile.GetComponent<ProjectileDotZone>().resetFrequency = 1.5f;

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = DeployProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DesolatorDamageTypes.DesolatorDot);

            TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, DeployProjectile.transform);
            areaIndicator.teamFilter = DeployProjectile.GetComponent<TeamFilter>();
            areaIndicator.transform.localScale = Vector3.one * ScepterDeployIrradiate.ScepterRange;

            DeployProjectile.transform.Find("Hitboxes").localScale = Vector3.one * ScepterDeployIrradiate.ScepterRange;

            DeployProjectile.GetComponentInChildren<LightRadiusScale>().sizeMultiplier = ThrowIrradiator.Range;

            Content.AddProjectilePrefab(DeployProjectile);

            return DeployProjectile;
        }

        private static GameObject CreateIrradiatorProjectile()
        {

            GameObject irradiatorProjectile = PrefabAPI.InstantiateClone(assetBundle.LoadAsset<GameObject>("IrradiatorProjectile"), "IrradiatorProjectile", true);

            Renderer ghostRenderer = irradiatorProjectile.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<Renderer>();
            ghostRenderer.material = ghostRenderer.material.ConvertDefaultShaderToHopoo();

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = irradiatorProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DesolatorDamageTypes.DesolatorDot);

            //todo deso fix this shit
            //Log.Warning($"irradiatorDeployableSlot {DesolatorDeployables.irradiatorDeployableSlot}");
            //irradiatorProjectile.GetComponent<ProjectileDeployToOwner>().deployableSlot = DesolatorDeployables.irradiatorDeployableSlot;
            //Log.Warning($"deployableSlot {DesolatorAssets.DesolatorIrradiatorProjectile.GetComponent<ProjectileDeployToOwner>().deployableSlot}");
            //UnityEngine.Object.Destroy(irradiatorProjectile.GetComponent<Deployable>());
            //UnityEngine.Object.Destroy(irradiatorProjectile.GetComponent<ProjectileDeployToOwner>());

            TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, irradiatorProjectile.transform);
            areaIndicator.teamFilter = irradiatorProjectile.GetComponent<TeamFilter>();
            areaIndicator.transform.localScale = Vector3.one * ThrowIrradiator.Range;

            irradiatorProjectile.transform.Find("Hitboxes").localScale = Vector3.one * ThrowIrradiator.Range;

            irradiatorProjectile.GetComponentInChildren<LightRadiusScale>().sizeMultiplier = ThrowIrradiator.Range;

            Content.AddProjectilePrefab(irradiatorProjectile);

            return irradiatorProjectile;
        }

        private static GameObject CreateIrradiatorProjectileScepter()
        {
            GameObject irradiatorProjectileScepter = PrefabAPI.InstantiateClone(assetBundle.LoadAsset<GameObject>("IrradiatorProjectileScepter"), "IrradiatorProjectileScepter", true);

            //todo deso fix this shit
            //irradiatorProjectileScepter.GetComponent<ProjectileDeployToOwner>().deployableSlot = DesolatorDeployables.irradiatorDeployableSlot;
            //UnityEngine.Object.Destroy(irradiatorProjectileScepter.GetComponent<Deployable>());
            //UnityEngine.Object.Destroy(irradiatorProjectileScepter.GetComponent<ProjectileDeployToOwner>());

            Renderer ghostRenderer = irradiatorProjectileScepter.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<Renderer>();
            ghostRenderer.material = ghostRenderer.material.ConvertDefaultShaderToHopoo();

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = irradiatorProjectileScepter.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DesolatorDamageTypes.DesolatorDot);

            TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, irradiatorProjectileScepter.transform);
            areaIndicator.teamFilter = irradiatorProjectileScepter.GetComponent<TeamFilter>();
            areaIndicator.transform.localScale = Vector3.one * ThrowIrradiator.Range;

            irradiatorProjectileScepter.transform.Find("Hitboxes").localScale = Vector3.one * ThrowIrradiator.Range;

            irradiatorProjectileScepter.GetComponentInChildren<LightRadiusScale>().sizeMultiplier = ThrowIrradiator.Range;

            ProjectileImpactExplosion impactExplosion = irradiatorProjectileScepter.GetComponent<ProjectileImpactExplosion>();
            impactExplosion.blastRadius = ThrowIrradiator.Range;
            impactExplosion.blastDamageCoefficient = ScepterThrowIrradiator.explosionDamageCoefficient;
            impactExplosion.explosionEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXGreaterWisp");

            Content.AddProjectilePrefab(irradiatorProjectileScepter);

            return irradiatorProjectileScepter;
        }

        private static GameObject CreateDesolatorCrocoLeapProjectile()
        {
            GameObject leapAcidProjectile = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Croco/CrocoLeapAcid.prefab").WaitForCompletion(), "DesolatorLeapAcid");

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = leapAcidProjectile.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DesolatorDamageTypes.DesolatorDot);
            ProjectileDotZone projectileDotZone = leapAcidProjectile.GetComponent<ProjectileDotZone>();
            projectileDotZone.impactEffect = IrradiatedImpactEffect;
            projectileDotZone.lifetime = AimBigRadBeam.DotZoneLifetime;
            projectileDotZone.damageCoefficient = 1;
            projectileDotZone.overlapProcCoefficient = 0.5f;
            projectileDotZone.resetFrequency = 2F;

            leapAcidProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            ProjectileController projectileController = leapAcidProjectile.GetComponent<ProjectileController>();
            GameObject leapAcidProjectileGhost = PrefabAPI.InstantiateClone(projectileController.ghostPrefab, "DesolatorLeapAcidGhost");
            projectileController.ghostPrefab = leapAcidProjectileGhost;
            
            Transform transformFindFX = leapAcidProjectileGhost.transform.Find("FX");
            transformFindFX.transform.localScale = Vector3.one * AimBigRadBeam.BaseAttackRadius * 1.2f;
            UnityEngine.Object.Destroy(transformFindFX.GetComponent<AlignToNormal>());

            Transform transformFindDecal = leapAcidProjectileGhost.transform.Find("FX/Decal");
            float scale = AimBigRadBeam.BaseAttackRadius * 0.13f;
            transformFindDecal.localScale = new Vector3(scale, 0.85f, scale);
            transformFindDecal.GetComponent<Decal>().Material.SetTexture("_MainTexture", assetBundle.LoadAsset<Texture2D>("texDesolatorDecal"));

            AlignToNormal alignComponent = transformFindDecal.gameObject.AddComponent<AlignToNormal>();
            alignComponent.maxDistance = AimBigRadBeam.BaseAttackRadius;
            alignComponent.offsetDistance = 1.5f;

            TeamAreaIndicator areaIndicator = UnityEngine.Object.Instantiate(DesolatorTeamAreaIndicatorPrefab, leapAcidProjectile.transform);
            areaIndicator.teamFilter = leapAcidProjectile.GetComponent<TeamFilter>();
            areaIndicator.transform.parent = leapAcidProjectile.transform.Find("FX");
            areaIndicator.transform.localScale = Vector3.one;
            leapAcidProjectile.transform.Find("FX/Hitbox (1)").localScale = Vector3.one * 1.8f;
            Transform transformFindFX2 = leapAcidProjectile.transform.Find("FX");
            transformFindFX2.transform.localScale = Vector3.one * AimBigRadBeam.BaseAttackRadius;
            
            Content.AddProjectilePrefab(leapAcidProjectile);

            return leapAcidProjectile;
        }

        private static GameObject CreateDesolatorAura()
        {
            GameObject aura = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/IcicleAura"), "DesolatorAura", true);
            
            UnityEngine.Object.Destroy(aura.GetComponent<IcicleAuraController>());
            aura.AddComponent<DesolatorAuraController>();

            UnityEngine.Object.Destroy(aura.transform.Find("Particles/Chunks").gameObject);
            UnityEngine.Object.Destroy(aura.transform.Find("Particles/SpinningSharpChunks").gameObject);
            UnityEngine.Object.Destroy(aura.transform.Find("Particles/Ring, Procced").gameObject);
            UnityEngine.Object.Destroy(aura.GetComponent<AkGameObj>());

            BuffWard buffWard = aura.GetComponent<BuffWard>();
            buffWard.buffDef = LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Weak");
            buffWard.radius = RadiationAura.Radius;

            greenifyRing(aura.transform.Find("Particles/Ring, Outer").GetComponent<ParticleSystem>());
            greenifyRing(aura.transform.Find("Particles/Ring, Core").GetComponent<ParticleSystem>());
            
            ParticleSystemRenderer shitthatgetsremovedbyfuckingriskytweaksbrojusthideit = aura.transform.Find("Particles/Area").GetComponent<ParticleSystemRenderer>();
            if(shitthatgetsremovedbyfuckingriskytweaksbrojusthideit != null)
            {
                shitthatgetsremovedbyfuckingriskytweaksbrojusthideit.material = DesolatorTeamAreaIndicatorPrefab.teamMaterialPairs[1].sharedMaterial;
            }

            return aura;
        }

        private static void greenifyRing(ParticleSystem particleSystem)
        {
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = Color.green;
            particleSystem.transform.localScale = new Vector3(0.8f, 1, 0.8f);
        }

        //todo deso move ugh
        private static GameObject CloneTracer(string originalTracerName, string newTracerName, Color? color = null, float widthMultiplierMultiplier = 1, float? speed = null, float? length = null)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            newTracer.GetComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = speed.HasValue ? speed.Value : newTracer.GetComponent<Tracer>().speed;
            newTracer.GetComponent<Tracer>().length = length.HasValue ? length.Value : newTracer.GetComponent<Tracer>().length;

            if (color.HasValue || widthMultiplierMultiplier != 1)
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
                        colorKey[0].color = Color.green;
                        colorKey[0].time = 0.0f;
                        colorKey[1].color = Color.green;
                        colorKey[1].time = 1.0f;

                        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
                        alphaKey[0].alpha = 1.0f;
                        alphaKey[0].time = 0.0f;
                        alphaKey[1].alpha = 1.0f;
                        alphaKey[1].time = 1.0f;

                        gradient.SetKeys(colorKey, alphaKey);

                        trailModule.colorOverLifetime = new ParticleSystem.MinMaxGradient(gradient);
                    }
                }
            }

            if (color.HasValue)
            {
                foreach (var rend in newTracer.GetComponentsInChildren<ParticleSystemRenderer>())
                {
                    rend.material.SetColor("_MainColor", color.Value);
                    rend.material.SetColor("_Color", color.Value);
                    rend.material.SetColor("_TintColor", color.Value);
                }
            }

            Content.CreateAndAddEffectDef(newTracer);

            return newTracer;
        }

        #endregion desolator stuff

    }
}