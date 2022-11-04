using EntityStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Aliem {
    public class BaseRidingState : BaseCharacterMain {

		public CharacterBody riddenBody;

		public Collider riddenCollider;

        public override void OnEnter() {
            base.OnEnter();

			if (riddenBody && NetworkServer.active) {
				riddenBody.AddBuff(Modules.Buffs.riddenBuff);
			}
			riddenCollider = findHighestHurtbox();

			gameObject.layer = RoR2.LayerIndex.fakeActor.intVal;
		}

        private Collider findHighestHurtbox() {

            HurtBox[] hurtboxes = riddenBody.hurtBoxGroup.hurtBoxes;
			HurtBox highestHurtbox = hurtboxes[0];
            for (int i = 0; i < hurtboxes.Length; i++) {

                HurtBox hurtBox = hurtboxes[i];

				if(hurtBox.transform.position.y > highestHurtbox.transform.position.y) {
					highestHurtbox = hurtBox;
                }
            }
			return highestHurtbox.collider;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

			if (riddenBody.healthComponent.alive) {

				if (base.isAuthority && base.characterMotor) {
					
					characterMotor.moveDirection = Vector3.zero;
					characterMotor.velocity = Vector3.zero;
					characterMotor.rootMotion = Vector3.zero;

                    if (riddenCollider) {
						Vector3 pos = riddenCollider.bounds.center + Vector3.up * (riddenCollider.bounds.max.y - riddenCollider.bounds.center.y);
						characterMotor.Motor.SetPosition(pos);
					} else {

						base.outer.SetNextStateToMain();
						PlayAnimation("FullBody, Override", "BufferEmpty");
						return;
					}
				}
			}else {
				base.outer.SetNextStateToMain();
				PlayAnimation("FullBody, Override", "BufferEmpty");
				return;
            }
		}


		public override void OnExit() {
			base.OnExit();
			if (riddenBody && NetworkServer.active) {
				riddenBody.RemoveBuff(Modules.Buffs.riddenBuff);
			}
			gameObject.layer = RoR2.LayerIndex.defaultLayer.intVal;
		}

        public override void OnSerialize(NetworkWriter writer) {
            base.OnSerialize(writer);
			writer.Write(riddenBody.gameObject);
        }
        public override void OnDeserialize(NetworkReader reader) {
            base.OnDeserialize(reader);
			riddenBody = reader.ReadGameObject().GetComponent<CharacterBody>();
        }
    }
}
