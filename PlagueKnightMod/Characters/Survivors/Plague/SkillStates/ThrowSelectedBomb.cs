using EntityStates;
using PlagueMod.Survivors.Plague.Components;
using PlagueMod.Survivors.Plague.SkillDefs;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.SkillStates
{
    public class ThrowSelectedBomb : GenericProjectileBaseState, PlagueBombSelectionSkillDef.IPlagueBombSetSelector
    {
        //the news didn't work. set them in onenter
        public static float throwBombBaseDuration = 0.5f;
        public static float throwBombBaseDelayDuration = 0.08f;

        public static float throwBombDamage = 1f;
        //needs to be set in the projectilecontroller component
        //public static float procCoefficient = 1f;

        public static float throwBombProjectilePitch= 0f;

        public static float throwBombThrowForce = 80f;
        public static float throwBombBoomForce = 100f;
        
        public float lowGravMultiplier = 0.3f;
        public float smallhopVelocity = 5f;

        private PlagueBombSelectorController _selectorComponent;

        public void SetPlagueComponent(PlagueBombSelectorController component)
        {
            _selectorComponent = component;
        }

        public override void OnEnter() {

            base.projectilePrefab = _selectorComponent.GetSelectedProjectile();
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
                base.SmallHop(base.characterMotor, smallhopVelocity / Mathf.Sqrt(this.attackSpeedStat));
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (!isGrounded) {
                //mess with slowing horizontal velocity
                ref float ySpeed = ref characterMotor.velocity.y;
                if (ySpeed < 0) {
                    ySpeed += Physics.gravity.y * -lowGravMultiplier * Time.fixedDeltaTime;
                }

                //why did I ever think this was a good idea
                ////actuallylook into other movement affecting options
                ////what slows you in vanilla? well also how'd moff do it too.
                //ref float xSpeed = ref characterMotor.velocity.x;
                //xSpeed *= lateralSlowMultiplier;
                //ref float zSpeed = ref characterMotor.velocity.z;
                //zSpeed *= lateralSlowMultiplier;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }

        public override void PlayAnimation(float duration) {

            if (base.GetModelAnimator()) {
                base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
            }
        }
    }
}