﻿using EntityStates;
using PlagueMod.Survivors.Plague.Components;
using PlagueMod.Survivors.Plague.SkillDefs;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.SkillStates
{
    public class ThrowSelectedBomb : GenericProjectileBaseState, PlagueBombSelectionSkillDef.IPlagueBombSelector
    {
        //the news didn't work. set them in onenter
        public static float throwBombBaseDuration = 0.5f;
        public static float throwBombBaseDelayDuration = 0.08f;

        public static float throwBombDamage = 2f;
        //needs to be set in the projectilecontroller component
        //public static float procCoefficient = 1f;

        public static float throwBombProjectilePitch = 0f;
        
        public float lowGravMultiplier = 0.3f;
        public float smallhopVelocity => PlagueConfig.bombSmallHop.Value;

        public float bombAircontrol => PlagueConfig.bombAirControl.Value;

        public PlagueBombSelectorController selectorComponent { get; set ; }

        public override void OnEnter() {

            base.projectilePrefab = selectorComponent.GetSelectedProjectile();
            //base.effectPrefab

            EntityStateMachine.FindByCustomName(gameObject, "Body").SetNextStateToMain();

            base.baseDuration = throwBombBaseDuration;
            base.baseDelayBeforeFiringProjectile = throwBombBaseDelayDuration;

            base.damageCoefficient = throwBombDamage;
            //base.force = throwBombThrowForce;
            base.projectilePitchBonus = -5;
            //min/maxSpread
            base.recoilAmplitude = 0.1f;
            base.bloom = 10;

            //targetmuzzle
            base.attackSoundString = "HenryBombThrow";

            base.OnEnter();

            if (!base.isGrounded) {
                base.SmallHop(base.characterMotor, smallhopVelocity / Mathf.Sqrt(this.attackSpeedStat));
            }

            characterMotor.airControl = bombAircontrol;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (!isGrounded) {
                ref float ySpeed = ref characterMotor.velocity.y;
                if (ySpeed < 0) {
                    ySpeed += Physics.gravity.y * -lowGravMultiplier * Time.fixedDeltaTime;
                }

                //why did I ever think this was a good idea
                ////mess with slowing horizontal velocity
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