using AliemMod.Content;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ShootSawedOffUncharged : BaseShootBullet
    {
        public override float damageCoefficient => AliemConfig.M1_SawedOff_Damage.Value;
        public override float baseDuration => 1f;
        public override float force => 100;
        public override float bloom => AliemConfig.bloomRifle.Value;
        public override float range => 50f;
        public override float radius => 1f;

        public override GameObject tracerEffectPrefab => Modules.Assets.rifleTracer;

        public override void OnEnter()
        {
            base.OnEnter();
            Fire();
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