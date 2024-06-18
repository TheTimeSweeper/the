using AliemMod.Content;

namespace ModdedEntityStates.Aliem
{
    public class ChargeSword : ChargeRayGun
    {
        public override float BaseMaxChargeDuration => 3;
        public override float MinDamageCoefficient => AliemConfig.M1_SwordCharged_Damage_Min.Value;
        public override float MaxDamageCoefficient => AliemConfig.M1_SwordCharged_Damage_Max.Value;
        public virtual float MinSpeedCoefficient => AliemConfig.M1_SwordCharged_Speed_Min.Value;
        public virtual float MaxSpeedCoefficient => AliemConfig.M1_SwordCharged_Speed_Max.Value;

        protected override void StartNextState(float dam)
        {
            outer.SetNextState(new SwordFireCharged(
                GetChargedValue(MinDamageCoefficient,MaxDamageCoefficient), 
                GetChargedValue(MinSpeedCoefficient,MaxSpeedCoefficient))
            { 
                isOffHanded = isOffHanded 
            });
        }
    }
}
