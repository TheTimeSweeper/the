using AliemMod.Content;
using EntityStates;
using Modules;
using RoR2;
using UnityEngine;

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

		private float minBurrowTime = 0.1f;

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

			if (fixedAge > minBurrowTime) {
				if ((base.isAuthority && !inputButtonState.down) || !isGrounded || fixedAge > MaxBurrowTime) {

					base.outer.SetState(new AliemCharacterMain { wasRiding = true });
				}
			}
		}

        public override void OnExit() {
            base.OnExit();

            GameObject burrwoObject = GetModelChildLocator().FindChildGameObject("Burrow");
            burrwoObject?.SetActive(false);
			base.PlayCrossfade("FullBody, Override", "UnBurrow", 0.1f);
			Util.PlaySound("Play_DigPopOut_AHI", gameObject);

            EffectManager.SpawnEffect(Assets.burrowPopOutEffect, new EffectData
            {
                origin = burrwoObject.transform.position,
                rotation = Util.QuaternionSafeLookRotation(Vector3.up),
            }, false);

            if (isAuthority) {
                new BlastAttack
                {
                    attacker = base.gameObject,
                    baseDamage = this.damageStat * AliemConfig.M3_Burrow_PopOutDamage.Value,
                    baseForce = 0,
                    bonusForce = Vector3.up * AliemConfig.M3_Burrow_PopOutForce.Value,
                    crit = base.RollCrit(),
                    damageType = DamageType.Stun1s,
                    falloffModel = BlastAttack.FalloffModel.None,
                    procCoefficient = 1,
                    radius = 4,
                    position = transform.position,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    //impactEffect = EffectCatalog.FindEffectIndexFromPrefab(this.blastImpactEffectPrefab),
                    teamIndex = base.teamComponent.teamIndex
                }.Fire();
            }
		}
    }
}
