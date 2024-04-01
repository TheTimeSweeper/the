using EntityStates;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class FireGunHeavy : BurstFireDuration
    {
        public override float baseDuration => GIConfig.M1_HeavyFire_Interval.Value * 3f;
        public override float baseInterval => GIConfig.M1_HeavyFire_Interval.Value;
        public override float baseFinalInterval => GIConfig.M1_HeavyFire_FinalInterval.Value;

        public static float damageCoefficient => GIConfig.M1_HeavyFire_Damage.Value;
        public static float procCoefficient = 1f;
        public static float force => GIConfig.M1_HeavyFire_Force.Value;
        public static float recoil => GIConfig.M1_HeavyFire_Recoil.Value;
        public static float range = 200f;
        public static GameObject tracerEffectPrefab = GIAssets.heavyGunTracer;

        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.SetAimTimer(2f);
            muzzleString = "JoeSword";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        protected override void Fire()
        {
            characterBody.AddSpreadBloom(1.5f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
            Util.PlaySound("Play_GIShoot2Single", gameObject);

            if (isAuthority)
            {
                Ray aimRay = GetAimRay();
                AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = damageCoefficient * damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = RollCrit(),
                    owner = gameObject,
                    muzzleName = muzzleString,
                    smartCollision = true,
                    procChainMask = default,
                    procCoefficient = procCoefficient,
                    radius = GIConfig.M1_HeavyFire_Radius.Value,
                    sniper = false,
                    stopperMask = LayerIndex.world.mask,
                    weapon = null,
                    tracerEffectPrefab = tracerEffectPrefab,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Toolbot.FireNailgun.hitEffectPrefab,
                }.Fire();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}