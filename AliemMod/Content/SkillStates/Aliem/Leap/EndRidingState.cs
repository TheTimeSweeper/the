using EntityStates;

namespace ModdedEntityStates.Aliem {
    public class EndRidingState : BaseState {

        public override void OnEnter() {
            base.OnEnter();

			PlayAnimation("FullBody, Underride", "BufferEmpty");

			base.outer.SetState(new AliemCharacterMain { wasRiding = true });
		}
    }
}
