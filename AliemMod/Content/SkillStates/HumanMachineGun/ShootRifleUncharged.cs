using AliemMod.Content;

namespace ModdedEntityStates.Aliem
{
    public class ShootRifleUncharged : BaseShootRifle
    {
        public override float damageCoefficient => AliemConfig.M1_MachineGun_Damage.Value;
        public override float force => 100;

        public override void OnEnter()
        {
            base.OnEnter();
            Fire();
        }
    }
}