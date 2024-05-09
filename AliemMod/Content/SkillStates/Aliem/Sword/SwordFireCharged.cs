using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class SwordFireCharged : SwordFire
    {
        public override GameObject projectile => Modules.Projectiles.SwordProjectilePrefabBig;
        public override string soundString => "Play_AliemEnergySwordCharged";

        public SwordFireCharged(float dam)
        {
            base.damageCoefficient = dam;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var machine = RoR2.EntityStateMachine.FindByCustomName(gameObject, "Body");
            machine.SetNextState(new SwordFireChargedDash());
        }
    }
}