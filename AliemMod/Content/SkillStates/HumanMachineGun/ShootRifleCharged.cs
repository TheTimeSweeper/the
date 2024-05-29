using AliemMod.Content;
using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ShootRifleCharged : BaseShootRifle
    {
        public override float damageCoefficient => AliemConfig.M2_MachineGunCharged_Damage.Value;
        public override float procCoefficient => 0.7f;
        public override float baseDuration => baseInterval * bullets;
        public override float force => 200f;
        public override float recoil => 1f;
        public override float bloom => AliemConfig.bloom2.Value;
        public override float range => 256f;
        public override float radius => AliemConfig.radius.Value;
        public override float minSpread => spread;
        public override float spread => AliemConfig.M2_MachineGunCharged_Spread.Value * characterBody.spreadBloomAngle;
        public override LayerMask stopperMask => LayerIndex.world.mask;
        public override GameObject tracerEffectPrefab => Modules.Assets.rifleTracerBig;
        public override string muzzleString => "BlasterMuzzle";

        public virtual float baseInterval => AliemConfig.M2_MachineGunCharged_Interval.Value;
        public virtual int bullets => AliemConfig.M2_MachineGunCharged_Bullets.Value;

        private float _interval;
        private float _shootTimer;
        private int _shotsFired;

        public override void OnEnter()
        {
            base.OnEnter();
            _interval = baseInterval / attackSpeedStat;
            characterBody.SetSpreadBloom(0, false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _shootTimer -= Time.fixedDeltaTime;

            while (_shootTimer <= 0 && _shotsFired < bullets)
            {
                Fire();
                _shootTimer += _interval;
                _shotsFired++;
            }
        }

        protected override void playShootAnimation()
        {
            Util.PlaySound("Play_AliemRifleCharged", gameObject);
            base.PlayAnimation("Gesture, Override", "ShootGunBig", "ShootGun.playbackRate", _interval * 5);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}