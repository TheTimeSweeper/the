using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace HenryMod.EntityStates.Joe {

    public class ThroBoomButCoolerQuestionMaark : GenericProjectileBaseState {

        public new static float damageCoefficient = 30f;
        public static float procCoefficient = 1f;
        public static float throwForce = 80f;
        public static float boomForce = 100f;
        public new float projectilePitchBonus = 0f;
        public new float baseDelayBeforeFiringProjectile = 0.1f;
        
        public new static float baseDuration = 0.5f;
        public static float lowGravMultiplier = 0.1f;
        public static float smallhopVelocity = 5f;

        public override void OnEnter() {

            projectilePrefab = Modules.Projectiles.totallyNewBombPrefab;
            //damageCoefficient = bombDamageCoefficient;
            force = throwForce;
            //baseDuration = bombBaseDuration
            base.OnEnter();


            if (!base.isGrounded) {
                base.SmallHop(base.characterMotor, smallhopVelocity);
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (!isGrounded) {
                ref float ySpeed = ref characterMotor.velocity.y;
                if (ySpeed < 0) {
                    ySpeed += Physics.gravity.y * -lowGravMultiplier * Time.deltaTime;
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Any;
        }


        public override void PlayAnimation(float duration) {

            if (base.GetModelAnimator()) {
                base.PlayAnimation("Arms, Override", "swing2 v2", "swing.playbackRate", this.duration);
            }
        }
    }


    public class ThrowBoom : BaseSkillState {

        public static float damageCoefficient = 3f;
        public static float procCoefficient = 1f;
        public static float throwForce = 80f;
        public static float boomForce = 100f;

        public static float baseDuration = 0.5f;
        public static float lowGravMultiplier = 0.1f;
        public static float smallhopVelocity = 5f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter() {
            base.OnEnter();
            this.duration = ThrowBoom.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.1f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            if (!base.isGrounded) {
                base.SmallHop(base.characterMotor, smallhopVelocity);
            }
                        //base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);

            base.PlayAnimation("Arms, Override", "swing2 v2", "swing.playbackRate", this.duration);
        }

        public override void OnExit() {
            base.OnExit();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (!isGrounded) {
                ref float ySpeed = ref characterMotor.velocity.y;
                if (ySpeed < 0) {
                    ySpeed += Physics.gravity.y * -lowGravMultiplier * Time.deltaTime;
                }
            }

            if (base.fixedAge >= this.fireTime) {
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
                Util.PlaySound("HenryBombThrow", base.gameObject);

                if (base.isAuthority) {
                    Ray aimRay = base.GetAimRay();

                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.totallyNewBombPrefab,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(aimRay.direction),
                        base.gameObject,
                        ThrowBoom.damageCoefficient * this.damageStat,
                        ThrowBoom.boomForce,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        ThrowBoom.throwForce);
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}