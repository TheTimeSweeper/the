using RoR2;
using UnityEngine;
using RA2Mod.Modules;
using System;
using RoR2.Projectile;
using RA2Mod.Survivors.Chrono.Components;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoAssets {

        public static ChronoProjectionMotor markerPrefab;
        public static GameObject chronoBombProjectile;

        public static GameObject chronoIndicatorIvan;
        public static GameObject chronoIndicatorVanish;
        public static GameObject chronoIndicatorPhase;

        public static void Init(AssetBundle assetBundle) {

            GameObject markerPrefabObject = assetBundle.LoadAsset<GameObject>("ChronoProjection");
            R2API.PrefabAPI.RegisterNetworkPrefab(markerPrefabObject);
            markerPrefab = markerPrefabObject.GetComponent<ChronoProjectionMotor>();

            chronoBombProjectile = assetBundle.LoadAsset<GameObject>("ChronoIvanBombProjectile");
            R2API.PrefabAPI.RegisterNetworkPrefab(chronoBombProjectile);
            chronoBombProjectile.GetComponent<ProjectileController>().ghostPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/StickyBomb").GetComponent<ProjectileController>().ghostPrefab;
            chronoBombProjectile.GetComponent<ProjectileImpactExplosion>().explosionEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosionLunarSun");

            chronoIndicatorIvan = assetBundle.LoadAsset<GameObject>("IndicatorChronoIvan");
            chronoIndicatorVanish = assetBundle.LoadAsset<GameObject>("IndicatorChronoVanish");
            chronoIndicatorPhase = assetBundle.LoadAsset<GameObject>("IndicatorChronoPhaseCooldown");
        }
    }
}
