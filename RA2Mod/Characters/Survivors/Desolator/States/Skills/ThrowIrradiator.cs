using RA2Mod.Modules.BaseStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RA2Mod.Survivors.Desolator.States
{

    public class ThrowIrradiator : BaseTimedSkillState {
        public static float DamageCoefficient = 0.15f;
        public static float Range = 35;

        public override float TimedBaseDuration => 1;

        public override float TimedBaseCastStartPercentTime => 0;

        public override void OnEnter() {
            base.OnEnter();

            PlayCrossfade("Gesture, Override", "DoPlace", 0.1f);

            if (base.isAuthority) {
                Ray aimRay = base.GetAimRay();

                FireProjectileInfo fireProjectileInfo = new FireProjectileInfo {
                    crit = base.RollCrit(),
                    damage = this.damageStat * DamageCoefficient,
                    damageColorIndex = DamageColorIndex.Default,
                    //damageTypeOverride = DamageType.WeakOnHit,
                    force = 0f,
                    owner = base.gameObject,
                    position = aimRay.origin,
                    procChainMask = default(ProcChainMask),
                    projectilePrefab = DesolatorAssets.DesolatorIrradiatorProjectile,
                    rotation = Quaternion.LookRotation(aimRay.direction),
                    target = null
                };
                ModifyProjectile(ref fireProjectileInfo);
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
            Util.PlaySound("Play_Desolator_Deploy", base.gameObject);
        }

        protected virtual void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo) { }
    }
}
