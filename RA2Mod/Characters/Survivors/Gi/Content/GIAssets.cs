using RoR2;
using UnityEngine;
using RA2Mod.Modules;
using System;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using R2API;
using UnityEngine;
using EntityStates;
using RA2Mod.General.Components;
using System.Collections;
using System.Collections.Generic;

namespace RA2Mod.Survivors.GI
{
    public static class GIAssets
    {
        public static GameObject missilePrefab;
        public static GameObject heavyMissilePrefab;

        public static GameObject caltropsPrefab;
        public static GameObject caltropsPrefabOpti;
        public static GameObject minePrefab;
        internal static GameObject heavyGunTracer;
        internal static GameObject gunTracer;
        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {
            _assetBundle = assetBundle;

            //missile
            missilePrefab = _assetBundle.LoadAsset<GameObject>("GIToolbotRocketProjectile").InstantiateClone("GIToolbotRocketProjectile");
            ProjectileController missileController = missilePrefab.GetComponent<ProjectileController>();
            missileController.ghostPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotGrenadeGhost.prefab").WaitForCompletion();

            ProjectileImpactExplosion missileImpact = missilePrefab.GetComponent<ProjectileImpactExplosion>();
            missileImpact.impactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/OmniExplosionVFXToolbotQuick.prefab").WaitForCompletion();
            missileImpact.blastRadius = GIConfig.M1_Missile_ExplosionRadius.Value;

            Content.NetworkAndAddProjectilePrefab(missilePrefab);

            //heavy missile
            heavyMissilePrefab = _assetBundle.LoadAsset<GameObject>("GIHeavyMissileProjectile").InstantiateClone("GIHeavyMissileProjectile");
            heavyMissilePrefab.GetComponent<ProjectileController>().ghostPrefab = missileController.ghostPrefab;
            
            ProjectileImpactExplosion heavyMissileImpact = heavyMissilePrefab.GetComponent<ProjectileImpactExplosion>();
            heavyMissileImpact.falloffModel = BlastAttack.FalloffModel.None;
            heavyMissileImpact.blastRadius = GIConfig.M1_HeavyMissile_ExplosionRadius.Value;
            heavyMissileImpact.impactEffect = missileImpact.impactEffect;

            Content.NetworkAndAddProjectilePrefab(heavyMissilePrefab);

            //caltropses
            caltropsPrefab = _assetBundle.LoadAsset<GameObject>("CaltropsProjectile");
            Content.NetworkAndAddProjectilePrefab(caltropsPrefab);

            GameObject caltropsDotZone = _assetBundle.LoadAsset<GameObject>("CaltropsDotZone");
            InitCaltropsDotZone(caltropsDotZone);

            caltropsPrefabOpti = _assetBundle.LoadAsset<GameObject>("CaltropsProjectileOpti");
            Content.NetworkAndAddProjectilePrefab(caltropsPrefabOpti);

            GameObject caltropsDotZoneOpti = _assetBundle.LoadAsset<GameObject>("CaltropsDotZoneOpti");
            InitCaltropsDotZone(caltropsDotZoneOpti);

            //mine
            minePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiMine.prefab").WaitForCompletion(), "GIEngiMine");

            UnityEngine.Object.Destroy(minePrefab.GetComponent<ProjectileDeployToOwner>());
            UnityEngine.Object.Destroy(minePrefab.GetComponent<Deployable>());
            UnityEngine.Object.Destroy(minePrefab.GetComponent<ApplyTorqueOnStart>());
            minePrefab.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s;
            minePrefab.GetComponent<ProjectileStickOnImpact>().ignoreCharacters = false;

            EntityStateMachine mainMachine = EntityStateMachine.FindByCustomName(minePrefab, "Main");
            SerializableEntityStateType MainState = new SerializableEntityStateType(typeof(SkillStates.Mine.WaitForStickMutiny));
            mainMachine.initialStateType = MainState;

            EntityStateMachine armingMachine = EntityStateMachine.FindByCustomName(minePrefab, "Arming");
            SerializableEntityStateType mineArmingState = new SerializableEntityStateType(typeof(SkillStates.Mine.MineArmingMutiny));
            armingMachine.initialStateType = mineArmingState;
            armingMachine.mainStateType = mineArmingState;

            heavyGunTracer = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/TracerToolbotRebar.prefab").WaitForCompletion();

            gunTracer = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

            Content.AddProjectilePrefab(minePrefab);
        }

        private static void InitCaltropsDotZone(GameObject caltropsDotZone)
        {

            //DamageAPI.ModdedDamageTypeHolderComponent damageTypeHolder = caltropsDotZone.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            //damageTypeHolder.Add(GIDamageTypes.CaltropsSlow);
            //caltropsDotZone.transform.Find("Scaler").transform.localScale = Vector3.one * GIConfig.M2_Caltrops_Scale.Value;
            //caltropsDotZone.GetComponent<ProjectileDotZone>().lifetime = GIConfig.M2_Caltrops_DotDuration.Value;
            Content.NetworkAndAddProjectilePrefab(caltropsDotZone);
            Content.NetworkAndAddProjectilePrefab(caltropsDotZone.GetComponent<ProjectileSpawnDotzoneChildren>().childPrefab.gameObject);
        }
    }
}
