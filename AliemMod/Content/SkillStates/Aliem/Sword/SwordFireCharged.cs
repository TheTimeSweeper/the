using AliemMod.Content;
using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class SwordFireCharged : SwordFire
    {
        public override GameObject projectile => Modules.Projectiles.SwordProjectilePrefabBig;
        public override string soundString => "Play_AliemEnergySwordCharged";
        public override float BaseDamageCoefficient => _swordDamageCoefficient;

        private float _swordDamageCoefficient;
        private float _swordSpeedCoefficient;

        public SwordFireCharged()
        {
            _swordDamageCoefficient = AliemConfig.M1_SwordCharged_Damage_Max.Value;
            _swordSpeedCoefficient = AliemConfig.M1_SwordCharged_Speed_Max.Value;
        }

        public SwordFireCharged(float dam, float sped)
        {
            _swordDamageCoefficient = dam;
            _swordSpeedCoefficient = sped;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var machine = RoR2.EntityStateMachine.FindByCustomName(gameObject, "Body");
            machine.SetNextState(new SwordFireChargedDash(_swordSpeedCoefficient));
        }
    }
}