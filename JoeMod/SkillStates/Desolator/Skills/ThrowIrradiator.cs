using RoR2;
using RoR2.Projectile;
using UnityEngine;
using ModdedEntityStates.BaseStates;

namespace ModdedEntityStates.Desolator {
    public class ThrowIrradiator : BaseTimedSkillState {

        public static float DamageCoefficient = 1;

        public static float BaseDuration = 1f;
        public static float StartTime = 0.0f;

        public override void OnEnter() {
            base.OnEnter();

            InitDurationValues(BaseDuration, StartTime);

            if (base.isAuthority) {
                Ray aimRay = base.GetAimRay();
                FireProjectileInfo fireProjectileInfo = new FireProjectileInfo {
                    crit = base.RollCrit(),
                    damage = this.damageStat * DamageCoefficient,
                    damageColorIndex = DamageColorIndex.Default,
                    force = 0f,
                    owner = base.gameObject,
                    position = aimRay.origin,
                    procChainMask = default(ProcChainMask),
                    projectilePrefab = Modules.Assets.DesolatorIrradiatorProjectile,
                    rotation = Quaternion.LookRotation(aimRay.direction),
                    target = null
                };
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
            Util.PlaySound("Play_Desolator_Deploy", base.gameObject);
        }
    }
}
