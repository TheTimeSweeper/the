using AliemMod.Content;
using AliemMod.Modules;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ShootSawedOffUncharged : BaseShootBullet
    {
        public override float damageCoefficient => AliemConfig.M1_SawedOff_Damage.Value;
        public override float baseDuration => AliemConfig.M1_SawedOff_Duration.Value;
        public override float force => 100;
        public override float bloom => 0.5f;
        public override float range => 50;
        public override float radius => 1;
        public override float spread => _spread;
        public override uint bullets => _bullets;
        public override float spreadPitchScale => 0.6f;
        public override float procCoefficient => 0.5f;
        public override float recoil => AliemConfig.M1_SawedOff_Recoil.Value;
        public override string muzzleString => isOffHanded ? "BlasterMuzzleFar.R" : "BlasterMuzzleFar";

        public override GameObject muzzleEffectPrefab => AliemAssets.sawedOffMuzzleFlash;
        public override GameObject tracerEffectPrefab => AliemAssets.sawedOffTracer;

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
                characterMotor.velocity = characterMotor.velocity - GetAimRay().direction * AliemConfig.M1_SawedOff_SelfKnockback.Value / Mathf.Sqrt(attackSpeedStat);
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