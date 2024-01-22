using EntityStates;
using RA2Mod.Survivors.Chrono;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using RA2Mod.Survivors.Chrono.SkillDefs;
using RA2Mod.Survivors.Chrono.Components;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class ChronoBomb : GenericProjectileBaseState, IHasSkillDefComponent<ChronoTrackerBomb>
    {
        public static float BaseDuration = 0.65f;
        //delays for projectiles feel absolute ass so only do this if you know what you're doing, otherwise it's best to keep it at 0
        public static float BaseDelayDuration = 0.0f;

        public static float DamageCoefficient = 3f;

        public ChronoTrackerBomb componentFromSkillDef { get; set; }

        public HurtBox trackingTarget;

        public override void OnEnter()
        {
            projectilePrefab = ChronoAssets.chronoBombProjectile;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            attackSoundString = "HenryBombThrow";

            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = BaseDelayDuration;

            damageCoefficient = DamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            force = 80f;

            //base.projectilePitchBonus = 0;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            recoilAmplitude = 0.1f;
            bloom = 10;

            if (componentFromSkillDef)
            {
                trackingTarget = componentFromSkillDef.GetTrackingTarget();
            }

            trackingTarget.healthComponent.body.AddTimedBuff(ChronoBuffs.ivand, 3);

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override Ray ModifyProjectileAimRay(Ray aimRay)
        {
            return new Ray(trackingTarget.transform.position, Vector3.up);
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