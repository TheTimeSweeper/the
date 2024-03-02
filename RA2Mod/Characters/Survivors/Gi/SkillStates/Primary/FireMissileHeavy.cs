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
    public class FireMissileHeavy : BurstFireDuration, IHasSkillDefComponent<GIMissileTracker>
    {
        private GameObject projectilePrefab;

        public override float baseDuration => GIConfig.M1_HeavyMissile_Interval.Value * 2;
        public override float baseInterval => GIConfig.M1_HeavyMissile_Interval.Value;
        public override float baseFinalInterval => GIConfig.M1_HeavyMissile_FinalInterval.Value;

        public static float BaseDuration => GIConfig.M1_HeavyMissile_Interval.Value;

        public static float DamageCoefficient => GIConfig.M1_HeavyMissile_Damage.Value;

        public GIMissileTracker componentFromSkillDef1 { get; set; }

        public override void OnEnter()
        {
            projectilePrefab = GIAssets.heavyMissilePrefab;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            //recoilAmplitude = 0.1f;
            //bloom = 10;

            base.OnEnter();
        }

        protected override void Fire()
        {

            PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", interval);

            Util.PlaySound("Play_PatriotMissile", gameObject);

            if (isAuthority)
            {
                FireProjectileAuthority();
            }
        }

        public Ray ModifyProjectileAimRay(Ray aimRay)
        {
            aimRay.direction = Vector3.up + aimRay.direction * 0.1f;
            return aimRay;
        }
        
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public void FireProjectileAuthority()
        {
            Ray aimRay = base.GetAimRay();
            aimRay.direction = Vector3.up + aimRay.direction * 0.1f;
            ProjectileManager.instance.FireProjectile(
                this.projectilePrefab,
                aimRay.origin,
                Util.QuaternionSafeLookRotation(aimRay.direction),
                base.gameObject,
                this.damageStat * DamageCoefficient,
                0,
                base.RollCrit(),
                DamageColorIndex.Default,
                GetTarget(),
                -1f);
        }

        private GameObject GetTarget()
        {
            if (!componentFromSkillDef1)
                return null;
            if (!componentFromSkillDef1.GetTrackingTarget())
                return null;
            return componentFromSkillDef1.GetTrackingTarget().gameObject;
        }
    }
}