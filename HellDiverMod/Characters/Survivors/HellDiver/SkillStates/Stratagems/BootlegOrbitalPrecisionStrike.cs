using EntityStates;
using RoR2.Projectile;
using UnityEngine;

namespace HellDiverMod.Survivors.HellDiver.SkillStates
{
    public class BootlegOrbitalPrecisionStrike : BaseState
    {
        private GameObject orbitalPrecisionStrikeProjectile => UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Captain/CaptainAirstrikeProjectile1.prefab").WaitForCompletion();

        public override void OnEnter()
        {
            base.OnEnter();

            FireProjectileInfo projectileInfo = new FireProjectileInfo
            {
                projectilePrefab = orbitalPrecisionStrikeProjectile,
                position = transform.position,
                rotation = Quaternion.identity,
                owner = characterBody.gameObject,
                damage = damageStat * 10,
                //force = 0,
                crit = base.RollCrit(),
                //damageColorIndex = damageColorIndex,
                //target = target,
                //speedOverride = speedOverride,
                //fuseOverride = -1f,
                //damageTypeOverride = null
            };
            ProjectileManager.instance.FireProjectile(projectileInfo);
            outer.SetNextStateToMain();
        }
    }
}
