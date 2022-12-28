using EntityStates;

namespace ModdedEntityStates {

    public class WindDownState : BaseSkillState {

        public float windDownTime = 0.5f;
        private float _windDownTime;

        public InterruptPriority interruptPriority = InterruptPriority.Skill;

        public override void OnEnter() {
            base.OnEnter();
            _windDownTime = windDownTime / attackSpeedStat;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (fixedAge > _windDownTime) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return interruptPriority;
        }
    }
}