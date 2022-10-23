using EntityStates;

namespace ModdedEntityStates.Aliem {
    public class AliemCharacterMain : GenericCharacterMain {

		public bool wasRiding = false;

        public override void OnEnter() {
            base.OnEnter();

			if (wasRiding && base.isAuthority) {

				GenericCharacterMain.ApplyJumpVelocity(base.characterMotor, base.characterBody, 1, 1, false);
			}
        }
    }
}
