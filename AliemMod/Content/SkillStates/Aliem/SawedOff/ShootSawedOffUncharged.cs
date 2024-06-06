using AliemMod.Content;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ShootSawedOffUncharged : BaseShootBullet
    {
        public override float damageCoefficient => AliemConfig.M1_SawedOff_Damage.Value;
        public override float baseDuration => AliemConfig.M1_SawedOff_Duration.Value;
        public override float force => AliemConfig.shotgunForce.Value;
        public override float bloom => AliemConfig.bloomRifle.Value;
        public override float range => AliemConfig.shotgunRange.Value;
        public override float radius => AliemConfig.shotgunBulletRadius.Value;
        public override float spread => _spread;
        public override uint bullets => _bullets;
        public override float spreadPitchScale => AliemConfig.shotgunSpreadPitchScale.Value;
        public override string muzzleString => isOffHanded ? "BlasterMuzzleFar.R" : "BlasterMuzzleFar";

        public override GameObject muzzleEffectPrefab => Modules.Assets.sawedOffMuzzleFlash;
        public override GameObject tracerEffectPrefab => Modules.Assets.sawedOffTracer;

        private uint _bullets = (uint)AliemConfig.M1_SawedOff_Bullets.Value - 1;
        private float _spread = AliemConfig.M1_SawedOff_Spread.Value;

        public override void OnEnter()
        {
            base.OnEnter();
            Fire();

            _bullets = 1;
            _spread = 0;
            Fire(false);

            if (!isGrounded)
            {
                characterMotor.velocity = characterMotor.velocity * (1 - AliemConfig.shotgunKnockbackSpeedOverride.Value) - GetAimRay().direction * AliemConfig.M1_SawedOff_SelfKnockback.Value;
            }
        }

        protected override void playShootAnimation()
        {
            Util.PlaySound("Play_AliemSawedoff", gameObject);
            base.PlayAnimation("Gesture, Override", "ShootGunBig", "ShootGun.playbackRate", baseDuration);
            base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootGunBig");
            base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootGunBig");
        }
    }
}