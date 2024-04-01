using EntityStates;
using HellDiverMod.Modules.BaseStates;
using HellDiverMod.Survivors.HellDiver.Components;
using RoR2.Projectile;
using UnityEngine;

namespace HellDiverMod.Survivors.HellDiver.SkillStates
{
    public class ThrowStratagem : BaseTimedSkillState
    {
        public override float TimedBaseDuration => 1;

        public override float TimedBaseCastStartPercentTime => 1;

        private GameObject stratagemProjectilePrefab => HellDiverAssets.stratagemProjectile;

        public override void OnEnter()
        {
            base.OnEnter();

            StratagemInputController stratagemInputController = GetComponent<StratagemInputController>();
            stratagemInputController.QueueStratagem();
            stratagemInputController.Reset();

            if (isAuthority)
            {
                FireStratagemProjectile();
            }
        }

        private void FireStratagemProjectile()
        {
            var aimRay = GetAimRay();
            FireProjectileInfo projectileInfo = new FireProjectileInfo
            {
                projectilePrefab = stratagemProjectilePrefab,
                position = aimRay.origin,
                rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction),
                owner = gameObject,
                damage = damageStat,
                //force = 0,
                //crit = false,
                //damageColorIndex = damageColorIndex,
                //target = target,
                //speedOverride = 100,
                //fuseOverride = -1f,
                //damageTypeOverride = null 
            };
            ProjectileManager.instance.FireProjectile(projectileInfo);
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            skillLocator.primary.UnsetSkillOverride(gameObject, HellDiverSurvivor.throwStratagemSkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
