using AliemMod.Content;
using AliemMod.Modules;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ShootRifleUncharged : BaseShootBullet
    {
        public override float damageCoefficient => AliemConfig.M1_MachineGun_Damage.Value;
        public override float baseDuration => AliemConfig.M1_MachineGun_Duration.Value;
        public override float force => 100;
        public override float bloom => AliemConfig.bloomRifle.Value;
        public override float range => 256f;
        public override float radius => 0.3f;

        public override GameObject tracerEffectPrefab => Assets.rifleTracer;

        public override void OnEnter()
        {
            base.OnEnter();
            Fire();
        }
    }
}