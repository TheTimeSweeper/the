using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace HenryMod.ModdedEntityStates.Joe {

    public class ThroBoomButCoolerQuestionMaark : GenericProjectileBaseState {

        //the news didn't work. set them in onenter
        public static float throwBombBaseDuration = 0.5f;
        public static float throwBombBaseDelayDuration = 0.08f;

        public static float throwBombDamage = 1f;
        //needs to be set in the projectilecontroller component
        //public static float procCoefficient = 1f;

        public static float throwBombProjectilePitch= 0f;

        public static float throwBombThrowForce = 80f;
        public static float throwBombBoomForce = 100f;
        
        public float lowGravMultiplier = 0.1f;
        public float smallhopVelocity = 5f;
        public static float lateralSlowMultiplier = 0.85f;

        public override void OnEnter() {

            base.projectilePrefab = Modules.Projectiles.totallyNewBombPrefab;
            //base.effectPrefab

            base.baseDuration = throwBombBaseDuration;
            base.baseDelayBeforeFiringProjectile = throwBombBaseDelayDuration;

            base.damageCoefficient = throwBombDamage;
            base.force = throwBombThrowForce;
            //base.projectilePitchBonus = 0;
            //min/maxSpread
            base.recoilAmplitude = 0.1f;
            base.bloom = 10;

            //targetmuzzle
            base.attackSoundString = "HenryBombThrow";


            base.OnEnter();

            if (!base.isGrounded) {
                base.SmallHop(base.characterMotor, smallhopVelocity);
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (!isGrounded) {
                //mess with slowing horizontal velocity
                ref float ySpeed = ref characterMotor.velocity.y;
                if (ySpeed < 0) {
                    ySpeed += Physics.gravity.y * -lowGravMultiplier * Time.deltaTime;
                }

                //actuallylook into other movement affecting options
                //what slows you in vanilla? well also how'd moff do it too.
                ref float xSpeed = ref characterMotor.velocity.x;
                xSpeed *= lateralSlowMultiplier;
                ref float zSpeed = ref characterMotor.velocity.z;
                zSpeed *= lateralSlowMultiplier;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }


        public override void PlayAnimation(float duration) {

            if (base.GetModelAnimator()) {
                base.PlayAnimation("Arms, Override", "swing2 v2", "swing.playbackRate", this.duration);
            }
        }
    }


    public class ThrowBoom : BaseSkillState {

        public static float damageCoefficient = 3f;
        //needs to be set in the projectilecontroller component
        //public static float procCoefficient = 1f;
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