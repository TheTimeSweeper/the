using AliemMod.Content;
using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ChargeBBGun : BaseChargingState
    {
        public override float BaseMaxChargeDuration => 3;
        public virtual float MinDamageCoefficient => AliemConfig.M1_BBGunCharged_Damage_Min.Value;
        public virtual float MaxDamageCoefficient => AliemConfig.M1_BBGunCharged_Damage_Max.Value;

        protected override void StartNextState(float chargeCoefficient)
        {
            outer.SetNextState(new FireBBGunCharged(GetChargedValue(MinDamageCoefficient, MaxDamageCoefficient)) { isOffHanded = isOffHanded });
        }
    }
}