namespace ModdedEntityStates.Aliem
{
    public class SwordCharging : RayGunCharging
    {
        public override float BaseMaxChargeDuration => 1;
        public override float MinDamageCoefficient => 3;
        public override float MaxDamageCoefficient => 6;

        protected override void StartNextState(float dam)
        {
            outer.SetNextState(new SwordFireCharged(dam));
        }
    }
}
