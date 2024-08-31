using AliemMod.Content;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace AliemMod.Modules
{
    public static class Projectiles
    {
        internal static GameObject RayGunProjectilePrefab;
        internal static GameObject RayGunProjectilePrefabBig;
        internal static GameObject SwordProjectilePrefab;
        internal static GameObject SwordProjectilePrefabBig;
        internal static GameObject SawedOffProjectilePrefabBig;
        internal static GameObject BBGunProjectilePrefabBig;

        internal static GameObject GrenadeProjectile;
        internal static GameObject GrenadeProjectileScepter;

        public static void Init()
        {
            RayGunProjectilePrefab = JankyLoadAliemPrefab("AliemLemonProjectile", true);
            RayGunProjectilePrefab.GetComponent<ProjectileSingleTargetImpact>().impactEffect = AliemAssets.m1EffectPrefab;

            RayGunProjectilePrefabBig = JankyLoadAliemPrefab("AliemLemonProjectileBig");
            RayGunProjectilePrefabBig.GetComponent<ProjectileImpactExplosion>().impactEffect = AliemAssets.m2EffectPrefab;
            RayGunProjectilePrefabBig.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>().Add(DamageTypes.FuckinChargedKillAchievementTracking);

            SwordProjectilePrefab = JankyLoadAliemPrefab("AliemSwordProjectile");
            SwordProjectilePrefabBig = JankyLoadAliemPrefab("AliemSwordProjectileBig");
            SwordProjectilePrefabBig.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>().Add(DamageTypes.FuckinChargedKillAchievementTracking);

            SawedOffProjectilePrefabBig = JankyLoadAliemPrefab("AliemSawedOffProjectileBig", true, true);
            AliemAssets.ConvertAllRenderersToHopooShader(SawedOffProjectilePrefabBig.GetComponent<ProjectileController>().ghostPrefab);
            SawedOffProjectilePrefabBig.GetComponent<ProjectileOverlapAttack>().impactEffect = AliemAssets.nemforcerImpactEffect;
            SawedOffProjectilePrefabBig.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>().Add(DamageTypes.FuckinChargedKillAchievementTracking);

            BBGunProjectilePrefabBig = JankyLoadAliemPrefab("AliemBBGunProjectileBig", true);
            ProjectileOverlapAttack BBOverlapAttack = BBGunProjectilePrefabBig.GetComponent<ProjectileOverlapAttack>();
            BBOverlapAttack.impactEffect = AliemAssets.nemforcerImpactEffect;
            BBOverlapAttack.resetInterval = AliemConfig.M1_BBGunCharged_HitInterval.Value;
            BBOverlapAttack.fireFrequency = 1 / AliemConfig.M1_BBGunCharged_HitInterval.Value;
            AliemAssets.ConvertAllRenderersToHopooShader(BBGunProjectilePrefabBig.GetComponent<ProjectileController>().ghostPrefab);

            GrenadeProjectile = JankyLoadAliemPrefab("AliemGrenadeProjectile");
            GrenadeProjectile.GetComponent<ProjectileImpactExplosion>().impactEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXToolbotQuick");

            GrenadeProjectileScepter = JankyLoadAliemPrefab("AliemGrenadeProjectileScepter");
            GrenadeProjectileScepter.GetComponent<ProjectileImpactExplosion>().impactEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXGreaterWisp");

            FunnyMaterial("AliemSwordProjectileGhost");
            FunnyMaterial("AliemSwordProjectileGhostBig");
        }

        private static void FunnyMaterial(string assName)
        {
            GameObject prefab = AliemAssets.LoadAsset<GameObject>(assName);//.InstantiateClone(assName, false);

            prefab.GetComponentInChildren<Renderer>().sharedMaterial.SetHotpooMaterial();
            prefab.GetComponentInChildren<Renderer>().sharedMaterial.SetCull();
        }

        //suppose "janky" is an addmittance about the nature of non-thunderkit modding in general
        public static GameObject JankyLoadAliemPrefab(string assName, bool clone = false, bool cloneGhost = false)
        {
            GameObject prefab = AliemAssets.LoadAsset<GameObject>(assName);
            prefab.RegisterNetworkPrefab();

            ProjectileController projectileController = prefab.GetComponent<ProjectileController>();
            projectileController.ghostPrefab.AddComponent<VFXAttributes>().DoNotPool = true;
            //so you can mess with it in runtimeinspector
            if (clone)
            {
                prefab = prefab.DebugClone(true);
            }
            if (cloneGhost)
            {
                projectileController.ghostPrefab = projectileController.ghostPrefab.DebugClone(false);
            }
            Content.AddProjectilePrefab(prefab);

            return prefab;
        }
    }
}