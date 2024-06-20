using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem {
    public class AliemCharacterMain : GenericCharacterMain {

		public bool wasRiding = false;

        public override void OnEnter() {
            base.OnEnter();

            if (Input.GetKey(KeyCode.G))
            {
                Animator animator = GetModelAnimator();
                Helpers.LogWarning(animator.GetLayerWeight(animator.GetLayerIndex("Flinch")));
            }

            if (wasRiding && base.isAuthority) {

				GenericCharacterMain.ApplyJumpVelocity(base.characterMotor, base.characterBody, 1, 1, false);
			}
        }
    }
}
