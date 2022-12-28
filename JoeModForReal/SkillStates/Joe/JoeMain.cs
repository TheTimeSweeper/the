using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Joe {
    public class JoeMain : GenericCharacterMain {

        public override void OnEnter() {
            base.OnEnter();

        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            this.modelAnimator.SetFloat("sprinting", characterBody.isSprinting ? 1 : 0, 0.1f, Time.fixedDeltaTime);
        }
    }
}