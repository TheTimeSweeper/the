using RoR2;
using UnityEngine;
using PlagueMod.Modules;
using System;
using RoR2.Projectile;
using PlagueMod.Survivors.Plague.Components;

namespace PlagueMod.Survivors.Plague
{
    public static class PlagueAssets
    {
        private static AssetBundle _assetBundle;

        public static GameObject SimpleBombCasingProjectile;
        public static GameObject SimpleBombCasingSquareProjectile;

        public static GameObject SimpleImpactPowderProjectile;
        public static GameObject SimpleImpactPowderProjectile2;

        public static PlagueBombSelectUI bombSelectUI;
        public static BombSelectSkillIcon skillIconPrefab;

        public static void Init(AssetBundle assetBundle)
        {
            _assetBundle = assetBundle;

            SimpleBombCasingProjectile = assetBundle.LoadAndAddProjectilePrefab("PlagueBombSimpleCasing");
            SimpleBombCasingSquareProjectile = assetBundle.LoadAndAddProjectilePrefab("PlagueBombSimpleCasingSquare");

            SimpleImpactPowderProjectile = assetBundle.LoadAndAddProjectilePrefab("PlagueExplosionSimplePowder");
            SimpleImpactPowderProjectile.GetComponent<ProjectileImpactExplosion>().explosionEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXGreaterWisp");

            SimpleImpactPowderProjectile2 = assetBundle.LoadAndAddProjectilePrefab("PlagueExplosionSimplePowder2 Variant");
            SimpleImpactPowderProjectile2.GetComponent<ProjectileImpactExplosion>().explosionEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosionGolem");

            bombSelectUI = assetBundle.LoadAsset<GameObject>("PlagueUI").GetComponent<PlagueBombSelectUI>();
            skillIconPrefab = assetBundle.LoadAsset<GameObject>("PlagueSkillIcon").GetComponent<BombSelectSkillIcon>();
        }
    }
}
