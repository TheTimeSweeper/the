using AliemMod.Content;

namespace ModdedEntityStates.Aliem
{
    public class SwordFireChargedDash : EntityStates.Huntress.BlinkState
    {
        private float _speedCoefficient;

        public SwordFireChargedDash()
        {
            _speedCoefficient = AliemConfig.M1_SwordCharged_Speed_Max.Value;
        }

        public SwordFireChargedDash(float speedCoefficient)
        {
            _speedCoefficient = speedCoefficient;
        }

        public override void OnEnter()
        {
            beginSoundString = "";
            endSoundString = "";
            speedCoefficient = _speedCoefficient;

            base.OnEnter();

            duration = 0.2f;

            characterMotor.Motor.ForceUnground();
        }
    }
}