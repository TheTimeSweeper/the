using AliemMod.Content;
using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ChargeRayGun : BaseChargingState {

        public override float BaseMaxChargeDuration => 3;
        public virtual float MinDamageCoefficient => AliemConfig.M1_RayGunCharged_Damage_Min.Value;
        public virtual float MaxDamageCoefficient => AliemConfig.M1_RayGunCharged_Damage_Max.Value;

        protected override void StartNextState(float chargeCoefficient)
        {
            string shootSound = chargeCoefficient >= 1 ? "Play_RayGunBigClassic" : "Play_RayGunBigClassic"/* "Play_RayGun"*/;
            outer.SetNextState(new RayGunChargedFire(GetChargedValue(MinDamageCoefficient, MaxDamageCoefficient), shootSound) { isOffHanded = isOffHanded });
        }
    }
}
