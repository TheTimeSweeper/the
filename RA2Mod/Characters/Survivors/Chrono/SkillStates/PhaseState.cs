using EntityStates;

namespace RA2Mod.Survivors.Chrono.SkillStates {
    public class PhaseState : BaseSkillState {

        public float windDownTime = 0.5f;

        private float _windDownTime;

        public override void OnEnter() {
            base.OnEnter();
            //apply phase effect
            GetModelAnimator().enabled = false;
        }

        public override void OnExit() {
            base.OnExit();
            //unapply phase effect
            GetModelAnimator().enabled = true;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (fixedAge > windDownTime) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }
}