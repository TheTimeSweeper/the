using AliemMod.Content;
using AliemMod.Modules;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Aliem
{
    public abstract class BaseShootBullet : BaseSkillState, IOffHandable
    {
        public abstract float damageCoefficient { get; }
        public abstract float baseDuration { get; }
        public virtual float procCoefficient => 1f;
        public virtual uint bullets => 1;
        public virtual float force => 100;
        public virtual float recoil => 0.1f;
        public virtual float bloom => AliemConfig.bloomRifle.Value;
        public virtual float range => 256f;
        public virtual float radius => 0.5f;
        public virtual float minSpread => 0;
        public virtual float spread => 0;
        public virtual float spreadPitchScale => 1;
        public virtual LayerMask stopperMask => LayerIndex.CommonMasks.bullet;
        public virtual string muzzleString => isOffHanded ? "BlasterMuzzle.R" : "BlasterMuzzle";
        public virtual GameObject muzzleEffectPrefab => EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab;

        public virtual GameObject tracerEffectPrefab => Assets.rifleTracer;

        public bool isOffHanded { get; set; }

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

        protected virtual void Fire(bool playEffects = true)
        {
            if (playEffects)
            {
                characterBody.AddSpreadBloom(bloom);
                EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, false);

                playShootAnimation();

                AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);
            }
            if (isAuthority)
            {
                Ray aimRay = GetAimRay();

                new BulletAttack
                {
                    bulletCount = bullets,
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
                    spreadPitchScale = spreadPitchScale,
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
            base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootGun");
            base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootGun");//stupid sync didn't work

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(isOffHanded);
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            isOffHanded = reader.ReadBoolean();
        }
    }
}