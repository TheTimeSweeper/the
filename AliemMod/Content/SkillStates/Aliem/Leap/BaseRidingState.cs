using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Aliem {
    public class BaseRidingState : BaseCharacterMain {

		public CharacterBody riddenBody;

		public CapsuleCollider motorCollider;

        public override void OnEnter() {
            base.OnEnter();
			riddenBody.AddBuff(Modules.Buffs.riddenBuff);

            if (riddenBody.characterMotor) {
				motorCollider = riddenBody.characterMotor.capsuleCollider;
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

			if (riddenBody.healthComponent.alive) {

				if (base.isAuthority && base.characterMotor) {

					characterMotor.moveDirection = Vector3.zero;
					characterMotor.velocity = Vector3.zero;
					characterMotor.rootMotion = Vector3.zero;

					Vector3 pos = riddenBody.corePosition;
                    if (motorCollider) {
						pos = motorCollider.transform.position + Vector3.up * motorCollider.height * 0.6f;
                    }

					characterMotor.Motor.SetPosition(pos);
				}
			}else {
				base.outer.SetNextStateToMain();
				return;
            }
		}
    }
}
