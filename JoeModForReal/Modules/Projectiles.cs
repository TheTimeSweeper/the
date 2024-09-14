﻿using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules {

    internal static class Projectiles {

        public static GameObject JoeFireball;
        public static GameObject JoeSwordBeam;

        public static GameObject totallyNewBombPrefab;

        public static void Init() {
            
            JoeFireball = LoadAndRegisterProjectileAsIs("JoeFireballBasic");
            //JoeFireball.GetComponent<ProjectileSingleTargetImpact>().impactEffect = Assets.JoeImpactEffect;

            totallyNewBombPrefab = CloneProjectilePrefab("EngiGrenadeProjectile", "TotallyNotPlagueKnightBomb");

            JoeSwordBeam = CreateJoeSwordBeam();
        }

        private static GameObject CreateJoeSwordBeam() {
            
            GameObject prefab = Asset.LoadAsset<GameObject>("SwordBeam");

            DamageAPI.ModdedDamageTypeHolderComponent damageTypeComponent = prefab.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DamageTypes.TenticleLifeStealing);

            GameObject ghostPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/EvisProjectile").GetComponent<ProjectileController>().ghostPrefab;
            ghostPrefab = PrefabAPI.InstantiateClone(ghostPrefab, "JoeGreenEvisProjectile", false);
            Modules.Asset.recolorEffects(Color.green, ghostPrefab);

            prefab.GetComponent<ProjectileController>().ghostPrefab = ghostPrefab;

            R2API.PrefabAPI.RegisterNetworkPrefab(prefab);

            Content.AddProjectilePrefab(prefab);

            return prefab;
        }

        public static GameObject LoadAndRegisterProjectileAsIs(string assName) {

            GameObject prefab = Asset.LoadAsset<GameObject>(assName);
            R2API.PrefabAPI.RegisterNetworkPrefab(prefab);

            Content.AddProjectilePrefab(prefab);

            return prefab;
        }

        public static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName) {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}