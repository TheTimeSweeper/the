﻿using EntityStates;

namespace RA2Mod.Survivors.Conscript.States
{
    public class ThrowMolotov : GenericProjectileBaseState
    {
        public static float BaseDuration = 0.65f;
        public static float BaseDelayDuration = 0.0f;

        public static float DamageCoefficient = 5f;

        public override void OnEnter()
        {
            projectilePrefab = ConscriptAssets.Molotov;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            attackSoundString = "HenryBombThrow";

            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = BaseDelayDuration;

            damageCoefficient = DamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            force = 80f;

            //base.projectilePitchBonus = 1;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            recoilAmplitude = 0.1f;
            bloom = 10;

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void PlayAnimation(float duration)
        {

            if (GetModelAnimator())
            {
                PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
            }
        }
    }
}
