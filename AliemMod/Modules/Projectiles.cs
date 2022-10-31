using RoR2;
using UnityEngine;

namespace Modules {
    internal static class Projectiles {
        internal static GameObject RayGunProjectilePrefab;
        internal static GameObject RayGunProjectilePrefabBig;
        internal static GameObject GrenadeProjectile;

        public static void Init() {

            RayGunProjectilePrefab = Assets.LoadAsset<GameObject>("AliemLemonProjectile");
            RayGunProjectilePrefab.GetComponent<RoR2.Projectile.ProjectileSingleTargetImpact>().impactEffect = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");

            Content.AddProjectilePrefab(RayGunProjectilePrefab);
            
            RayGunProjectilePrefabBig = Assets.LoadAsset<GameObject>("AliemLemonProjectileBig");
            RayGunProjectilePrefabBig.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().impactEffect = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");

            Content.AddProjectilePrefab(RayGunProjectilePrefabBig);

            GrenadeProjectile = Assets.LoadAsset<GameObject>("AliemGrenadeProjectile");
            GrenadeProjectile.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().impactEffect = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");

            Content.AddProjectilePrefab(GrenadeProjectile);
        }
    }
}