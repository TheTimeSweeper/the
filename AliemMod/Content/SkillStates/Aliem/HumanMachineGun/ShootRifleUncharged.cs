using AliemMod.Content;
using AliemMod.Modules;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ShootRifleUncharged : BaseShootBullet
    {
        public override float damageCoefficient => AliemConfig.M1_MachineGun_Damage.Value;
        public override float baseDuration => AliemConfig.M1_MachineGun_Duration.Value;
        public override float force => 100;
        public override float bloom => 0.5f;
        public override float range => 256f;
        public override float radius => 0.3f;
        public override float spread => AliemConfig.M1_MachineGun_Spread.Value * characterBody.spreadBloomAngle;
        public override BulletAttack.FalloffModel falloff => AliemConfig.M1_MachineGun_Falloff.Value ? BulletAttack.FalloffModel.DefaultBullet : BulletAttack.FalloffModel.None;

        public override GameObject tracerEffectPrefab => Assets.rifleTracer;

        public override void OnEnter()
        {
            base.OnEnter();
            Fire();
        }
    }
}