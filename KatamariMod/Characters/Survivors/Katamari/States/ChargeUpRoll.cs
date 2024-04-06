using EntityStates;

namespace KatamariMod.Survivors.Katamari.States
{
    public class ChargeUpRoll : BaseSkillState
    {
        private float speed;

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            speed += KatamariConfig.ChargeRoll_Multiplier.Value;

            if (!inputBank.skill1.down)
            {
                characterMotor.velocity = GetAimRay().direction.normalized * speed;
                Log.Warning(speed);
                outer.SetNextStateToMain();
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
