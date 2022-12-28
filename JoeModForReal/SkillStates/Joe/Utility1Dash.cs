using EntityStates;
using RoR2;

namespace ModdedEntityStates.Joe {
    public class Utility1Dash : UtilityBaseDash {
        public override void OnEnter() {

			if (inputBank.skill1.down) {

				EntityStateMachine.FindByCustomName(gameObject, "Body").SetNextState(new Utility1ChargeMeleeDash());

				base.outer.SetNextStateToMain();
				return;
			}

            base.OnEnter();
        }

        protected override void SetNextState() {

            WindDownState state = new WindDownState();
            state.windDownTime = 0.2f;//todo testvaluemanager
            state.interruptPriority = InterruptPriority.PrioritySkill;

            base.outer.SetNextState(state);
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}