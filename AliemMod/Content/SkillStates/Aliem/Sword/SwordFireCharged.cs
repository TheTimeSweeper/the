using AliemMod.Content;
using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class SwordFireCharged : SwordFire
    {
        public override GameObject projectile => Modules.Projectiles.SwordProjectilePrefabBig;
        public override string soundString => "Play_AliemEnergySwordCharged";
        public float SwordDamageCoefficient;
        public override float BaseDamageCoefficient => SwordDamageCoefficient;

        public SwordFireCharged()
        {
            SwordDamageCoefficient = AliemConfig.M2_SwordCharged_Damage.Value;
        }

        public SwordFireCharged(float dam)
        {
            SwordDamageCoefficient = dam;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var machine = RoR2.EntityStateMachine.FindByCustomName(gameObject, "Body");
            machine.SetNextState(new SwordFireChargedDash());
        }
    }
}