using EntityStates;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class PhaseState : BaseSkillState {

        public float windDownTime = 0.5f;
        public PhaseIndicatorController controller;

        public override void OnEnter() {
            base.OnEnter();
            Util.PlaySound("Play_ChronoMove", gameObject);
            characterBody.isSprinting = false;

            //apply phase effect
            controller?.UpdateIndicatorActive(true);
            GetModelAnimator().enabled = false;
        }

        public override void OnExit() {
            base.OnExit();
            //unapply phase effect
            controller?.UpdateIndicatorActive(false);
            GetModelAnimator().enabled = true;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            controller?.UpdateIndicatorFill((windDownTime - fixedAge) / windDownTime);

            if (fixedAge > windDownTime) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }
}