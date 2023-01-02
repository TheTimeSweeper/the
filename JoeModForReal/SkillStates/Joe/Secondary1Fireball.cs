using EntityStates;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace ModdedEntityStates.Joe {

    public class Secondary1Fireball : BaseSkillState {

        public static float damageCoefficient => TestValueManager.value1;// 5.5f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.3f;
        //public static float throwForce = 80f;

        private float duration;
        private float castTime;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter() {
            base.OnEnter();
            this.duration = Secondary1Fireball.baseDuration / this.attackSpeedStat;
            this.castTime = 0;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            base.PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", this.duration);
        }

        public override void OnExit() {
            base.OnExit();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (base.fixedAge >= this.castTime) {
                this.Fire();
            }

            if (base.fixedAge >= this.duration && base.isAuthority) {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire() {

            if (!this.hasFired) {
                this.hasFired = true;
                Util.PlaySound("play_joe_fireShoot", base.gameObject);

                if (base.isAuthority) {
                    Ray aimRay = base.GetAimRay();

                    ProjectileManager.instance.FireProjectile(Modules.Assets.JoeFireball,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(aimRay.direction),
                        base.gameObject,
                        Secondary1Fireball.damageCoefficient * this.damageStat,
                        4000f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        100);
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return base.GetMinimumInterruptPriority();
        }
    }
}