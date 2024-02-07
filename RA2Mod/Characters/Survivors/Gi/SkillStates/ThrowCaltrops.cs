using EntityStates;
using RA2Mod.General.SkillDefs;
using RA2Mod.General.Components;
using RA2Mod.Survivors.GI;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using RA2Mod.Survivors.GI.Components;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class ThrowCaltrops : GenericProjectileBaseState
    {
        public static float BaseDuration => GIConfig.M2CaltropsDuration.Value;

        public static float DamageCoefficient => GIConfig.M2CaltropsDamage.Value;

        public override void OnEnter()
        {
            projectilePrefab = GIAssets.caltropsPrefab;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            attackSoundString = "Play_PatriotMissile";
            
            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = 0;

            damageCoefficient = DamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            force = 80f;

            base.projectilePitchBonus = GIConfig.M2CaltropsPitch.Value;
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