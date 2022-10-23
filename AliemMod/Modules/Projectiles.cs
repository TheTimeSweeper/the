using UnityEngine;

namespace Modules {
    internal static class Projectiles {
        internal static GameObject RayGunProjectilePrefab;
        internal static GameObject RayGunProjectilePrefabBig;

        public static void Init() {
            RayGunProjectilePrefab = Content.AddProjectilePrefab(Assets.LoadAsset<GameObject>("AliemLemonProjectile"));
            RayGunProjectilePrefabBig = Content.AddProjectilePrefab(Assets.LoadAsset<GameObject>("AliemLemonProjectileBig"));
        }
    }
}