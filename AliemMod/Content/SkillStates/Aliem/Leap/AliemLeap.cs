using EntityStates;
using EntityStates.Croco;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RoR2.LayerIndex;

namespace ModdedEntityStates.Aliem {

	public class AliemLeapM2 : AliemLeap {
        //is this jank?
        protected override int inputButton => 2;
	}

	public class AliemLeapM3 : AliemLeap {
		//is this jank?
		protected override int inputButton => 3;
	}

	public class AliemLeap : BaseCharacterMain {

		protected virtual int inputButton => 2;
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
		public static float DamageCoefficient = 2;

		private OverlapAttack overlapAttack;
		//private List<HurtBox> overlapAttackHits = new List<HurtBox>();

		private bool hitSomething;
		private int hitSomethingFrames;

		private float previousAirControl;

		private BullseyeSearch bullseyeSearch;
        private List<HurtBox> overlapAttackHits = new List<HurtBox>();

        public override void OnEnter() {
			base.OnEnter();
			bullseyeSearch = new BullseyeSearch();
			HitBoxGroup hitBoxGroup = null;
			Transform modelTransform = base.GetModelTransform();
			if (modelTransform) {
				hitBoxGroup = Array.Find(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup hitboxGroup) => hitboxGroup.groupName == "Leap");
			}

			overlapAttack = new OverlapAttack {
				attacker = base.gameObject,
				inflictor = gameObject,
				teamIndex = base.GetTeam(),
				damage = DamageCoefficient * damageStat,
				isCrit = RollCrit(),
				hitBoxGroup = hitBoxGroup,
				hitEffectPrefab = Modules.Assets.nemforcerImpactEffect,
				impactSound = Modules.Assets.nemforcerImpactSound.index,
				//damageType = DamageType.Stun1s,
			};

			//handled in riding state
			//R2API.DamageAPI.AddModdedDamageType(overlapAttack, Modules.DamageTypes.ApplyAliemRiddenBuff);

			GetModelAnimator().SetFloat("aimYawCycle", 0.5f);
			GetModelAnimator().SetFloat("aimPitchCycle", 0.5f);

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
			base.PlayCrossfade("FullBody, Override", "Dive", 0.1f);
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

				CharacterBody foundBody = FireOverlap();
				if (!foundBody) {
					foundBody = FindBodyToRide();
				}
				
				if(foundBody != null && (inputButtonState.down || Modules.Config.AlwaysRide.Value)) {
					base.outer.SetNextState(new AliemRidingState {
						riddenBody = foundBody
					});
					return;
                }

				//hit ground
				if ((base.characterMotor.Motor.GroundingStatus.IsStableOnGround && !base.characterMotor.Motor.LastGroundingStatus.IsStableOnGround)) {
										
					if (inputButtonState.down) {
						outer.SetState(new AliemBurrow(inputButton));

					} else {
						this.outer.SetNextStateToMain();
					}
					return;
				}

				if (hitSomething)
					hitSomethingFrames++;

				//hit wall or somethin
				if (base.fixedAge >= BaseLeap.minimumDuration && this.hitSomethingFrames > 1) {

					PlayAnimation("FullBody, Override", "BufferEmpty");
					this.outer.SetNextStateToMain();
					return;
				}
			}
		}

        private CharacterBody FindBodyToRide() {

            Ray mond = new Ray(characterBody.corePosition, Vector3.forward);

			if (Util.CharacterSpherecast(gameObject, mond, 1.51f, out RaycastHit HitInfo, 0, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal)) {

				if(HitInfo.collider.GetComponent<HurtBox>().healthComponent.body.teamComponent.teamIndex == teamComponent.teamIndex)
					return HitInfo.collider.GetComponent<HurtBox>().healthComponent.body;
			}
			return null;
        }

		private CharacterBody FireOverlap() {

			overlapAttackHits.Clear();
			if (overlapAttack.Fire(overlapAttackHits)) {
				if (overlapAttackHits.Count > 0) {
					return overlapAttackHits[0].healthComponent?.body;
				}
			}

			return null;
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
