using AliemMod.Content;
using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public abstract class BaseShootRifle : BaseSkillState
    {
        public abstract float damageCoefficient { get; }
        public virtual float procCoefficient => 1f;
        public virtual float baseDuration => 0.2f;
        public virtual float force => 800f;
        public virtual float recoil => 0.1f;
        public virtual float bloom => AliemConfig.bloom1.Value;
        public virtual float range => 256f;
        public virtual float radius => 0.5f;
        public virtual float minSpread => 0;
        public virtual float spread => 0;
        public virtual LayerMask stopperMask => LayerIndex.CommonMasks.bullet;
        public virtual string muzzleString => "BlasterMuzzle";

        public virtual GameObject tracerEffectPrefab => Modules.Assets.rifleTracer;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(2f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        protected virtual void Fire()
        {
            characterBody.AddSpreadBloom(bloom);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);

            playShootAnimation();

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
                    minSpread = minSpread,
                    maxSpread = spread,
                    isCrit = RollCrit(),
                    owner = gameObject,
                    muzzleName = muzzleString,
                    smartCollision = true,
                    procChainMask = default,
                    procCoefficient = procCoefficient,
                    radius = radius,
                    sniper = false,
                    stopperMask = stopperMask,
                    weapon = null,
                    tracerEffectPrefab = tracerEffectPrefab,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                }.Fire();
            }
        }

        protected virtual void playShootAnimation()
        {
            Util.PlaySound("Play_AliemRifle", gameObject);
            base.PlayAnimation("Gesture, Override", "ShootGun", "ShootGun.playbackRate", duration * 2);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}