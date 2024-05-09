namespace ModdedEntityStates.Aliem
{
    public class SwordFireChargedDash : EntityStates.Huntress.BlinkState
    {
        public override void OnEnter()
        {
            beginSoundString = "";
            endSoundString = "";
            speedCoefficient = 18f;

            base.OnEnter();

            duration = 0.2f;

            characterMotor.Motor.ForceUnground();
        }
    }
}