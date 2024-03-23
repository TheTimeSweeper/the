using EntityStates;

namespace RA2Mod.General.States
{
    public class WindDownState : BaseSkillState
    {

        public float windDownTime = 0.5f;
        public bool ignoreAttackSpeed;

        public InterruptPriority minimumInterruptPriority = InterruptPriority.Skill;

        private float _windDownTime;

        public override void OnEnter()
        {
            base.OnEnter();
            _windDownTime = ignoreAttackSpeed ? windDownTime : windDownTime / attackSpeedStat;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge > _windDownTime)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return minimumInterruptPriority;
        }
    }
}