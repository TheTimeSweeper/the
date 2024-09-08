using EntityStates;
using RA2Mod.Survivors.Chrono;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using RA2Mod.Survivors.Chrono.Components;
using UnityEngine.Networking;
using RA2Mod.General.SkillDefs;

namespace RA2Mod.Survivors.Chrono.States
{
    public class ChronoBomb : GenericProjectileBaseState, IHasSkillDefComponent<ChronoTrackerBomb>
    {
        public static float BaseDuration = 0.65f;

        public static float DamageCoefficient = ChronoConfig.M2_Bomb_Damage.Value;

        public ChronoTrackerBomb componentFromSkillDef1 { get; set; }

        public HurtBox trackingTarget;

        public override void OnEnter()
        {
            projectilePrefab = ChronoAssets.chronoBombProjectile;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            attackSoundString = "";

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

            if (componentFromSkillDef1 && isAuthority)
            {
                trackingTarget = componentFromSkillDef1.GetTrackingTarget();
            }
            //if (NetworkServer.active)
            //{
            //    trackingTarget.healthComponent.body.AddTimedBuff(ChronoBuffs.ivand, 3);
            //}

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override Ray ModifyProjectileAimRay(Ray aimRay)
        {
            return new Ray(trackingTarget.transform.position, Vector3.forward);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void FireProjectile()
        {
            if (NetworkServer.active)
            {
                Ray aimRay = base.GetAimRay();
                aimRay = ModifyProjectileAimRay(aimRay);
                ProjectileManager.instance.FireProjectile(
                    this.projectilePrefab,
                    aimRay.origin,
                    Util.QuaternionSafeLookRotation(aimRay.direction),
                    base.gameObject,
                    this.damageStat * DamageCoefficient,
                    0,
                    base.RollCrit(),
                    DamageColorIndex.Default,
                    null,
                    -1f);
            }
        }

        public override void PlayAnimation(float duration)
        {

            if (GetModelAnimator())
            {
                PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
            }
        }

        // Token: 0x06000E64 RID: 3684 RVA: 0x0003E1A0 File Offset: 0x0003C3A0
        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.trackingTarget));
        }

        // Token: 0x06000E65 RID: 3685 RVA: 0x0003E1B4 File Offset: 0x0003C3B4
        public override void OnDeserialize(NetworkReader reader)
        {
            this.trackingTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}