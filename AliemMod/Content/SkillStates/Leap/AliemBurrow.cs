using EntityStates;
using RoR2;

namespace ModdedEntityStates.Aliem {
    public class AliemBurrow : BaseCharacterMain {

		private int inputButton = 2;
		private InputBankTest.ButtonState inputButtonState {
			get {
				switch (inputButton) {
					case 1:
						return inputBank.skill1;
					default:
					case 2:
						return inputBank.skill2;
					case 3:
						return inputBank.skill3;
					case 4:
						return inputBank.skill4;
				}
			}
        }

		public static float MaxBurrowTime = 1.4f;
		public SprintEffectController sprintEffectController;

		public AliemBurrow() {}
		public AliemBurrow(int inputButtonState) {
            this.inputButton = inputButtonState;
        }

        public override void OnEnter() {
            base.OnEnter();

			base.PlayCrossfade("FullBody, Override", "Burrow", 0.1f);
			Util.PlaySound(EntityStates.Treebot.BurrowIn.burrowInSoundString, gameObject);
			GetModelChildLocator().FindChildGameObject("Burrow")?.SetActive(true);
		}

        public override void FixedUpdate() {
            base.FixedUpdate();

			characterBody.isSprinting = true;

			StartAimMode();

			if (base.isAuthority && base.characterMotor) {
				base.characterMotor.moveDirection = base.inputBank.moveVector * 2.6f;
			}
			
			if((base.isAuthority && !inputButtonState.down) || !isGrounded || fixedAge > MaxBurrowTime) {

				base.outer.SetState(new AliemCharacterMain { wasRiding = true });
			}
		}

        public override void OnExit() {
            base.OnExit();

			GetModelChildLocator().FindChildGameObject("Burrow")?.SetActive(false);
			base.PlayCrossfade("FullBody, Override", "UnBurrow", 0.1f);
			Util.PlaySound("Play_INV_DigPopOut", gameObject);
		}
    }
}
