using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Modules {
    internal static class Projectiles {

        public static GameObject JoeFireball;
        public static GameObject JoeSwordBeam;

        public static GameObject totallyNewBombPrefab;

        public static void Init() {
            
            JoeFireball = JankyLoadAliemPrefab("JoeFireballBasic");
            //JoeFireball.GetComponent<ProjectileSingleTargetImpact>().impactEffect = Assets.JoeImpactEffect;

            totallyNewBombPrefab = CloneProjectilePrefab("EngiGrenadeProjectile", "TotallyNotPlagueKnightBomb");

        }

        public static GameObject JankyLoadAliemPrefab(string assName) {

            GameObject prefab = Assets.LoadAsset<GameObject>(assName);
            R2API.PrefabAPI.RegisterNetworkPrefab(prefab);

            Content.AddProjectilePrefab(prefab);

            return prefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName) {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}