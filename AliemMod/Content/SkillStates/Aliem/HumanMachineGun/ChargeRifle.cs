using AliemMod.Content;
using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    internal class ChargeRifle : BaseChargingState
    {
        public override float BaseMaxChargeDuration => 3;
        public virtual int MinBullets => AliemConfig.M1_MachineGunCharged_Bullets_Min.Value;
        public virtual int MaxBullets => AliemConfig.M1_MachineGunCharged_Bullets_Max.Value;

        protected override void StartNextState(float chargeCoefficient)
        {
            outer.SetNextState(new ShootRifleCharged(Mathf.RoundToInt(GetChargedValue(MinBullets, MaxBullets))) { isOffHanded = isOffHanded });
        }
    }
}