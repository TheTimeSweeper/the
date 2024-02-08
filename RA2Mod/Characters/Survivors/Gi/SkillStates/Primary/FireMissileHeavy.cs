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
    public class FireMissileHeavy : GenericProjectileBaseState, IHasSkillDefComponent<GIMissileTracker>
    {
        public static float BaseDuration => GIConfig.M1HeavyMissileDuration.Value;

        public static float DamageCoefficient => GIConfig.M1HeavyMissileDamage.Value;

        public GIMissileTracker componentFromSkillDef { get; set; }

        public GameObject target
        {
            get
            {
                if (componentFromSkillDef == null)
                    return null;

                return componentFromSkillDef.GetTrackingTarget().gameObject;
            }
        }

        public override void OnEnter()
        {
            projectilePrefab = GIAssets.heavyMissilePrefab;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            attackSoundString = "Play_PatriotMissile";

            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = 0;

            damageCoefficient = DamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            force = 80f;

            //base.projectilePitchBonus = 0;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            recoilAmplitude = 0.1f;
            bloom = 10;

            base.OnEnter();
        }

        public override Ray ModifyProjectileAimRay(Ray aimRay)
        {
            aimRay.direction = Vector3.up + aimRay.direction * 0.1f;
            return aimRay;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void FireProjectile()
        {
            Ray aimRay = base.GetAimRay();
            aimRay = this.ModifyProjectileAimRay(aimRay);
            aimRay.direction = Util.ApplySpread(aimRay.direction, this.minSpread, this.maxSpread, 1f, 1f, 0f, this.projectilePitchBonus);
            ProjectileManager.instance.FireProjectile(
                this.projectilePrefab, 
                aimRay.origin, 
                Util.QuaternionSafeLookRotation(aimRay.direction),
                base.gameObject, 
                this.damageStat * this.damageCoefficient,
                this.force,
                Util.CheckRoll(this.critStat, base.characterBody.master), 
                DamageColorIndex.Default,
                componentFromSkillDef.GetTrackingTarget().gameObject, 
                -1f);
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