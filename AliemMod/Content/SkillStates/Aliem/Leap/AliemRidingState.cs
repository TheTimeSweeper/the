namespace ModdedEntityStates.Aliem {

    public class AliemRidingState : BaseRidingState {

        public override void OnEnter() {
            base.OnEnter();

			PlayAnimation("Fullbody, Override", "Riding");
        }

        public override void FixedUpdate() {
			base.FixedUpdate();

			if (isAuthority && inputBank.jump.justPressed) {

				base.outer.SetState(new EndRidingState());
				
				return;
			}

			if (isAuthority && inputBank.skill1.justPressed) {

				AliemRidingChomp chompState = new AliemRidingChomp();
				chompState.riddenBody = this.riddenBody;

				base.outer.SetState(chompState);
				return;
			}
		}
    }
}
