using AliemMod.Content;
using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ChargeSawedOff : BaseChargingState
    {
        public override float BaseMaxChargeDuration => 3;
        public virtual float MinDamageCoefficient => AliemConfig.M1_SawedOffCharged_Damage_Min.Value;
        public virtual float MaxDamageCoefficient => AliemConfig.M1_SawedOffCharged_Damage_Max.Value;

        protected override void StartNextState(float chargeCoefficient)
        {
            outer.SetNextState(new ShootSawedOffCharged(GetChargedValue(MinDamageCoefficient, MaxDamageCoefficient)) { isOffHanded = isOffHanded });
        }
    }
}