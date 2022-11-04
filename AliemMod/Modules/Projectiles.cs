using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Modules {
    internal static class Projectiles {
        internal static GameObject RayGunProjectilePrefab;
        internal static GameObject RayGunProjectilePrefabBig;
        internal static GameObject GrenadeProjectile;

        public static void Init() {

            RayGunProjectilePrefab = JankyLoadAliemPrefab("AliemLemonProjectile");
            RayGunProjectilePrefab.GetComponent<ProjectileSingleTargetImpact>().impactEffect = Assets.m1EffectPrefab;

            RayGunProjectilePrefabBig = JankyLoadAliemPrefab("AliemLemonProjectileBig");
            RayGunProjectilePrefabBig.GetComponent<ProjectileImpactExplosion>().impactEffect = Assets.m2EffectPrefab;

            GrenadeProjectile = JankyLoadAliemPrefab("AliemGrenadeProjectile");
            GrenadeProjectile.GetComponent<ProjectileImpactExplosion>().impactEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXToolbotQuick");
        }

        public static GameObject JankyLoadAliemPrefab(string assName) {

            GameObject prefab = Assets.LoadAsset<GameObject>(assName);
            R2API.PrefabAPI.RegisterNetworkPrefab(prefab);

            Content.AddProjectilePrefab(prefab);

            return prefab;
        }
    }
}