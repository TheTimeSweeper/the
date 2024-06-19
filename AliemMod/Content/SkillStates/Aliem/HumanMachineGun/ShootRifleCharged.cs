using AliemMod.Content;
using AliemMod.Modules;
using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ShootRifleCharged : BaseShootBullet
    {
        public override float damageCoefficient => AliemConfig.M1_MachineGunCharged_Damage.Value;
        public override float baseDuration => baseInterval * bullets;
        public override float procCoefficient => 0.7f;
        public override float force => 200f;
        public override float recoil => 1f;
        public override float bloom => 0;
        public override float range => 256f;
        public override float radius => 1.6f;
        public override float minSpread => 0;
        public override float spread => AliemConfig.M1_MachineGunCharged_Spread.Value * characterBody.spreadBloomAngle;
        public override LayerMask stopperMask => LayerIndex.world.mask;
        public override GameObject tracerEffectPrefab => Assets.rifleTracerBig;
        public override string muzzleString => isOffHanded ? "BlasterMuzzle.R" : "BlasterMuzzle";

        public virtual float baseInterval => AliemConfig.M1_MachineGunCharged_Interval.Value;
        public virtual int bullets => _chargedBullets;

        private float _interval;
        private float _shootTimer;
        private int _shotsFired;
        private int _chargedBullets;
        private float _currentBloom;
        private float _bloomIncrement => 0.5f;

        public ShootRifleCharged()
        {
            _chargedBullets = AliemConfig.M1_MachineGunCharged_Bullets_Max.Value;
        }

        public ShootRifleCharged(int bullets)
        {
            _chargedBullets = bullets;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _interval = baseInterval / attackSpeedStat;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _shootTimer -= Time.fixedDeltaTime;

            while (_shootTimer <= 0 && _shotsFired < bullets)
            {
                characterBody.SetSpreadBloom(_currentBloom, false);
                _currentBloom += _bloomIncrement;
                Fire();
                _shootTimer += _interval;
                _shotsFired++;
            }
        }

        protected override void ModifyBullet(BulletAttack bulletAttack)
        {
            R2API.DamageAPI.AddModdedDamageType(bulletAttack, DamageTypes.FuckinChargedKillAchievementTracking);
        }

        protected override void playShootAnimation()
        {
            Util.PlaySound("Play_AliemRifleCharged", gameObject);
            base.PlayAnimation("Gesture, Override", "ShootGunBig", "ShootGun.playbackRate", _interval * 5);
            base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootGunBig");
            base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootGunBig");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}