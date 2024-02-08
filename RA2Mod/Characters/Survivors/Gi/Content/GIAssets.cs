using RoR2;
using UnityEngine;
using RA2Mod.Modules;
using System;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using R2API;
using UnityEngine;
using EntityStates;

namespace RA2Mod.Survivors.GI
{
    public static class GIAssets
    {
        public static GameObject missilePrefab;
        public static GameObject heavyMissilePrefab;

        public static GameObject caltropsPrefab;
        public static GameObject minePrefab;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {
            _assetBundle = assetBundle;

            //missile
            missilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotGrenadeLauncherProjectile.prefab").WaitForCompletion();
            missilePrefab = PrefabAPI.InstantiateClone(missilePrefab, "GIToolbotGrenadeLauncherProjectile", true);

            ProjectileImpactExplosion missileImpact = missilePrefab.GetComponent<ProjectileImpactExplosion>();
            missileImpact.blastRadius = GIConfig.M1MissileExplosionRadius.Value;

            Content.AddProjectilePrefab(missilePrefab);

            //heavy missile
            heavyMissilePrefab = _assetBundle.LoadAsset<GameObject>("GIMissileProjectile").InstantiateClone("GIMissileProjectile");
            heavyMissilePrefab.GetComponent<ProjectileController>().ghostPrefab = missilePrefab.GetComponent<ProjectileController>().ghostPrefab;

            ProjectileImpactExplosion heavyMissileImpact = heavyMissilePrefab.GetComponent<ProjectileImpactExplosion>();
            heavyMissileImpact.falloffModel = BlastAttack.FalloffModel.None;
            heavyMissileImpact.blastRadius = GIConfig.M1HeavyMissileExplosionRadius.Value;
            heavyMissileImpact.impactEffect = missileImpact.impactEffect;
            
            Content.NetworkAndAddProjectilePrefab(heavyMissilePrefab);

            //caltropses
            caltropsPrefab = _assetBundle.LoadAsset<GameObject>("CaltropsProjectile");
            Content.NetworkAndAddProjectilePrefab(caltropsPrefab);

            GameObject caltropsDotZone = _assetBundle.LoadAsset<GameObject>("CaltropsDotZone");
            //DamageAPI.ModdedDamageTypeHolderComponent damageTypeHolder = caltropsDotZone.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            //damageTypeHolder.Add(GIDamageTypes.CaltropsSlow);
            Content.NetworkAndAddProjectilePrefab(caltropsDotZone);

            //mine
            minePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiMine.prefab").WaitForCompletion(), "GIEngiMine");

            UnityEngine.Object.Destroy(minePrefab.GetComponent<ProjectileDeployToOwner>());
            UnityEngine.Object.Destroy(minePrefab.GetComponent<Deployable>());
            UnityEngine.Object.Destroy(minePrefab.GetComponent<ApplyTorqueOnStart>());
            minePrefab.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s;

            EntityStateMachine mainMachine = EntityStateMachine.FindByCustomName(minePrefab, "Main");
            SerializableEntityStateType MainState = new SerializableEntityStateType(typeof(SkillStates.Mine.WaitForStickMutiny));
            mainMachine.initialStateType = MainState;

            EntityStateMachine armingMachine = EntityStateMachine.FindByCustomName(minePrefab, "Arming");
            SerializableEntityStateType mineArmingState = new SerializableEntityStateType(typeof(SkillStates.Mine.MineArmingMutiny));
            armingMachine.initialStateType = mineArmingState;
            armingMachine.mainStateType = mineArmingState;

            Content.AddProjectilePrefab(minePrefab);
        }
    }
}
