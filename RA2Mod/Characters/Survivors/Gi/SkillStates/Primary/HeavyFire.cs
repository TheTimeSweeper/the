using EntityStates;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class HeavyFire : BaseSkillState
    {
        public static float damageCoefficient = GIConfig.M1HeavyFireDamage.Value;
        public static float procCoefficient = 1f;
        public static float baseDuration => GIConfig.M1HeavyFireDuration.Value;
        public static float force => GIConfig.M1HeavyFireForce.Value;
        public static float recoil => GIConfig.M1HeavyFireRecoil.Value;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/TracerToolbotRebar.prefab").WaitForCompletion();

        private float duration;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(2f);
            muzzleString = "JoeSword";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            Fire();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration)
                base.outer.SetNextStateToMain();
        }

        protected void Fire()
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
                    radius = GIConfig.M1HeavyFireRadius.Value,
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