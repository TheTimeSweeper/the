using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace Modules {
    internal static class Projectiles {
        internal static GameObject RayGunProjectilePrefab;
        internal static GameObject RayGunProjectilePrefabBig;
        internal static GameObject SwordProjectilePrefab;
        internal static GameObject SwordProjectilePrefabBig;
        internal static GameObject SawedOffProjectilePrefabBig;

        internal static GameObject GrenadeProjectile;
        internal static GameObject GrenadeProjectileScepter;

        public static void Init()
        {
            RayGunProjectilePrefab = JankyLoadAliemPrefab("AliemLemonProjectile");
            RayGunProjectilePrefab.GetComponent<ProjectileSingleTargetImpact>().impactEffect = Assets.m1EffectPrefab;

            RayGunProjectilePrefabBig = JankyLoadAliemPrefab("AliemLemonProjectileBig");
            RayGunProjectilePrefabBig.GetComponent<ProjectileImpactExplosion>().impactEffect = Assets.m2EffectPrefab;

            SwordProjectilePrefab = JankyLoadAliemPrefab("AliemSwordProjectile");
            SwordProjectilePrefabBig = JankyLoadAliemPrefab("AliemSwordProjectileBig");

            SawedOffProjectilePrefabBig = JankyLoadAliemPrefab("AliemSawedOffProjectileBig", true);
            Assets.ConvertAllRenderersToHopooShader(SawedOffProjectilePrefabBig.GetComponent<ProjectileController>().ghostPrefab);
            SawedOffProjectilePrefabBig.GetComponent<ProjectileOverlapAttack>().impactEffect = Assets.nemforcerImpactEffect;

            GrenadeProjectile = JankyLoadAliemPrefab("AliemGrenadeProjectile");
            GrenadeProjectile.GetComponent<ProjectileImpactExplosion>().impactEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXToolbotQuick");

            GrenadeProjectileScepter = JankyLoadAliemPrefab("AliemGrenadeProjectileScepter");
            GrenadeProjectileScepter.GetComponent<ProjectileImpactExplosion>().impactEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXGreaterWisp");

            FunnyMaterial("AliemSwordProjectileGhost");
            FunnyMaterial("AliemSwordProjectileGhostBig");
        }

        private static void FunnyMaterial(string assName)
        {
            GameObject prefab = Assets.LoadAsset<GameObject>(assName);//.InstantiateClone(assName, false);
            
            prefab.GetComponentInChildren<Renderer>().sharedMaterial.SetHotpooMaterial();
            prefab.GetComponentInChildren<Renderer>().sharedMaterial.SetCull();
        }

        public static GameObject JankyLoadAliemPrefab(string assName, bool clone = false) {

            GameObject prefab = Assets.LoadAsset<GameObject>(assName);
            R2API.PrefabAPI.RegisterNetworkPrefab(prefab);
            if (clone)
            {
                prefab = prefab.InstantiateClone(assName, true);
            }
            Content.AddProjectilePrefab(prefab);

            return prefab;
        }
    }
}