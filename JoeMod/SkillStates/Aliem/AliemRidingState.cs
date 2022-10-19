using EntityStates;
using EntityStates.Croco;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

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

	public class AliemRidingState : BaseRidingState {

		public override void FixedUpdate() {
			base.FixedUpdate();

			if (inputBank.jump.justPressed) {
				//todo jump off
				riddenBody.RemoveBuff(Modules.Buffs.riddenBuff);
				base.outer.SetState(new AliemCharacterMain { wasRiding = true });
				return;
			}

			if (inputBank.skill1.justPressed) {
				//todo chomp
				riddenBody.RemoveBuff(Modules.Buffs.riddenBuff);

				AliemRidingChomp chompState = new AliemRidingChomp();
				chompState.riddenBody = this.riddenBody;

				base.outer.SetState(chompState);
				return;
			}
		}
	}

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

    public class AliemLeap : BaseCharacterMain {

		public float DamageCoefficient = 2;


		private OverlapAttack overlapAttack;
		private List<HurtBox> overlapAttackHits = new List<HurtBox>();

		private bool hitSomething;

		private float previousAirControl;

		public override void OnEnter() {
			base.OnEnter();

			HitBoxGroup hitBoxGroup = null;
			Transform modelTransform = base.GetModelTransform();
			if (modelTransform) {
				hitBoxGroup = Array.Find(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "PunchHitbox");
			}

			overlapAttack = new OverlapAttack {
				attacker = base.gameObject,
				inflictor = gameObject,
				teamIndex = base.GetTeam(),
				damage = DamageCoefficient * damageStat,
				isCrit = RollCrit(),
				hitBoxGroup = hitBoxGroup
				//damageType = DamageType.Stun1s,
			};

			//R2API.DamageAPI.AddModdedDamageType(overlapAttack, Modules.DamageTypes.ApplyAliemRiddenBuff);


			this.previousAirControl = base.characterMotor.airControl;
			base.characterMotor.airControl = BaseLeap.airControl;
			Vector3 direction = base.GetAimRay().direction;
			if (base.isAuthority) {
				base.characterBody.isSprinting = true;
				direction.y = Mathf.Max(direction.y, BaseLeap.minimumY);
				Vector3 a = direction.normalized * BaseLeap.aimVelocity * this.moveSpeedStat;
				Vector3 b = Vector3.up * BaseLeap.upwardVelocity;
				Vector3 b2 = new Vector3(direction.x, 0f, direction.z).normalized * BaseLeap.forwardVelocity;
				base.characterMotor.Motor.ForceUnground();
				base.characterMotor.velocity = a + b + b2;
			}
			base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
			//base.GetModelTransform().GetComponent<AimAnimator>().enabled = true;
			//base.PlayCrossfade("Gesture, Override", "Leap", 0.1f);
			//base.PlayCrossfade("Gesture, AdditiveHigh", "Leap", 0.1f);
			//base.PlayCrossfade("Gesture, Override", "Leap", 0.1f);
			Util.PlaySound(BaseLeap.leapSoundString, base.gameObject);
			base.characterDirection.moveVector = direction;
			//this.leftFistEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.fistEffectPrefab, base.FindModelChild("MuzzleHandL"));
			//this.rightFistEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.fistEffectPrefab, base.FindModelChild("MuzzleHandR"));
			if (base.isAuthority) {
				base.characterMotor.onMovementHit += this.OnMovementHit;
			}
			Util.PlaySound(BaseLeap.soundLoopStartEvent, base.gameObject);
		}

		private void OnMovementHit(ref CharacterMotor.MovementHitInfo movementHitInfo) {
			this.hitSomething = true;
		}

		public override void FixedUpdate() {
			base.FixedUpdate();

			if (base.isAuthority && base.characterMotor) {
				base.characterMotor.moveDirection = base.inputBank.moveVector;

                CharacterBody foundEnemy = FireOverlap();
				if(foundEnemy != null) {
					base.outer.SetNextState(new AliemRidingState {
						riddenBody = foundEnemy
					});
					return;
                }

				//hit ground
				if (base.fixedAge >= BaseLeap.minimumDuration && (base.characterMotor.Motor.GroundingStatus.IsStableOnGround && !base.characterMotor.Motor.LastGroundingStatus.IsStableOnGround)) {
					//TODO: burrow state
					this.outer.SetNextStateToMain();
					return;
				}

				//hit wall or somethin
				if (base.fixedAge >= BaseLeap.minimumDuration && this.hitSomething) {
					this.outer.SetNextStateToMain();
					return;
				}
			}
		}

        private CharacterBody FireOverlap() {

			CharacterBody closestBody = null;

			if (overlapAttack.Fire(overlapAttackHits)) {

				float shortestDistance = 100;
				for (int i = 0; i < overlapAttackHits.Count; i++) {

					float hitDistance = Vector3.Distance(transform.position, overlapAttackHits[i].transform.position);
					if(hitDistance < shortestDistance) {
						closestBody = this.overlapAttackHits[i].healthComponent.body;
					}
				}
			}

			return closestBody;
        }

		// Token: 0x060011AE RID: 4526 RVA: 0x0004E124 File Offset: 0x0004C324
		public override void OnExit() {
			Util.PlaySound(BaseLeap.soundLoopStopEvent, base.gameObject);
			if (base.isAuthority) {
				base.characterMotor.onMovementHit -= this.OnMovementHit;
			}
			base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
			base.characterMotor.airControl = this.previousAirControl;
			base.characterBody.isSprinting = false;
			int layerIndex = base.modelAnimator.GetLayerIndex("Impact");
			if (layerIndex >= 0) {
				base.modelAnimator.SetLayerWeight(layerIndex, 2f);
				this.PlayAnimation("Impact", "LightImpact");
			}
			//base.PlayCrossfade("Gesture, Override", "BufferEmpty", 0.1f);
			//base.PlayCrossfade("Gesture, AdditiveHigh", "BufferEmpty", 0.1f);
			//EntityState.Destroy(this.leftFistEffectInstance);
			//EntityState.Destroy(this.rightFistEffectInstance);
			base.OnExit();
		}

		public override InterruptPriority GetMinimumInterruptPriority() {
			return InterruptPriority.PrioritySkill;
		}
	}
}
