using RoR2;
using RoR2.Projectile;
using UnityEngine;
using ModdedEntityStates.BaseStates;
using System;

namespace ModdedEntityStates.Desolator {

    public class ThrowIrradiator : BaseTimedSkillState {
        public static float DamageCoefficient = 0.3f;
        public static float Range = 35;
        
        public static float BaseDuration = 1f;
        public static float StartTime = 0.0f;

        public override void OnEnter() {
            base.OnEnter();

            InitDurationValues(BaseDuration, StartTime);

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
                    projectilePrefab = Modules.Assets.DesolatorIrradiatorProjectile,
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
