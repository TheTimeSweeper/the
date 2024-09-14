using UnityEngine;
using Modules;
using RoR2.Projectile;

namespace JoeModForReal.Content.Survivors {

    public static class GenjiProjectiles {
        public static GameObject genjiShuriken;

        public static void Init() {

            genjiShuriken = CloneShuriken();
        }

        private static GameObject CloneShuriken() {
            GameObject shuriken = Modules.Projectiles.CloneProjectilePrefab("ShurikenProjectile", "TotallyNewShuriken");

            UnityEngine.Object.Destroy(shuriken.GetComponent<ProjectileDirectionalTargetFinder>());
            UnityEngine.Object.Destroy(shuriken.GetComponent<ProjectileSteerTowardTarget>());
            UnityEngine.Object.Destroy(shuriken.GetComponent<ProjectileTargetComponent>());

            shuriken.GetComponent<ProjectileController>().ghostPrefab = Asset.LoadAsset<GameObject>("GenjiProjectileGhostOW1Shuriken");

            return shuriken;
        }
    }
}