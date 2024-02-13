using EntityStates;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class ThrowMine : GenericProjectileBaseState
    {
        public static float BaseDuration => GIConfig.M2_Mine_ThrowDuration.Value;

        public static float DamageCoefficient => GIConfig.M2_Mine_Damage.Value;

        public static float BaseForce => GIConfig.M2_Mine_Force.Value;

        public override void OnEnter()
        {
            projectilePrefab = GIAssets.minePrefab;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            attackSoundString = "Play_engi_M2_throw";

            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = 0;

            damageCoefficient = DamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            force = BaseForce;

            base.projectilePitchBonus = GIConfig.M2_Mine_Pitch.Value;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            recoilAmplitude = 0.1f;
            bloom = 10;

            base.OnEnter();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
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