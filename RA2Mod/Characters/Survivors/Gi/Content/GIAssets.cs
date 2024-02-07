using RoR2;
using UnityEngine;
using RA2Mod.Modules;
using System;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using R2API;
using UnityEngine;

namespace RA2Mod.Survivors.GI
{
    public static class GIAssets
    {
        public static GameObject caltropsPrefab;
        public static GameObject missilePrefab;
        public static GameObject heavyMissilePrefab;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {
            _assetBundle = assetBundle;

            //missile
            missilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotGrenadeLauncherProjectile.prefab").WaitForCompletion();
            missilePrefab = PrefabAPI.InstantiateClone(missilePrefab, "GIToolbotGrenadeLauncherProjectile", true);

            ProjectileImpactExplosion missileImpact = missilePrefab.GetComponent<ProjectileImpactExplosion>();
            missileImpact.blastRadius = GIConfig.M1HeavyMissileExplosionRadius.Value;

            Content.AddProjectilePrefab(missilePrefab);

            //heavy missile
            heavyMissilePrefab = _assetBundle.LoadAsset<GameObject>("GIMissileProjectile").InstantiateClone("GIMissileProjectile");
            heavyMissilePrefab.GetComponent<ProjectileController>().ghostPrefab = missilePrefab.GetComponent<ProjectileController>().ghostPrefab;

            ProjectileImpactExplosion heavyMissileImpact = heavyMissilePrefab.GetComponent<ProjectileImpactExplosion>();
            heavyMissileImpact.falloffModel = BlastAttack.FalloffModel.None;
            heavyMissileImpact.blastRadius = GIConfig.M1MissileExplosionRadius.Value;
            heavyMissileImpact.impactEffect = missileImpact.impactEffect;
            
            Content.NetworkAndAddProjectilePrefab(heavyMissilePrefab);

            //caltropses
            caltropsPrefab = _assetBundle.LoadAsset<GameObject>("CaltropsProjectile");
            Content.NetworkAndAddProjectilePrefab(caltropsPrefab);

            GameObject caltropsDotZone = _assetBundle.LoadAsset<GameObject>("CaltropsDotZone");
            //DamageAPI.ModdedDamageTypeHolderComponent damageTypeHolder = caltropsDotZone.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            //damageTypeHolder.Add(GIDamageTypes.CaltropsSlow);
            Content.NetworkAndAddProjectilePrefab(caltropsDotZone);
        }
    }
}
