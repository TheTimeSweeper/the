using AliemMod.Content;
using AliemMod.Modules;
using EntityStates;
using RoR2;
using RoR2.Orbs;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Aliem
{
    public class AliemRidingChomp : BaseSkillState {

        public static float ChompDamageCoefficient => AliemConfig.M3_Chomp_Damage.Value;

        public Collider riddenCollider;

		private float baseDuration = 0.3f;
		private float chompPercentTime = 1f;

		private float duration;

		private bool hasCasted;

        public override void OnEnter() {
            base.OnEnter();

			duration = baseDuration / attackSpeedStat;
            PlayCrossfade("FullBody, Override", "RidingChomp", "Chomp.playbackRate", duration*2, 0.1f);
		}

        public override void FixedUpdate() {
            base.FixedUpdate();

			if(base.fixedAge > duration * chompPercentTime && !hasCasted) {
				hasCasted = true;
				OnCastEnter();
            }

			if (base.fixedAge > duration && isAuthority) {
                base.outer.SetNextStateToMain();
			}
		}

        protected void OnCastEnter() {

			Util.PlayAttackSpeedSound("Play_Chomp", base.gameObject, this.attackSpeedStat);

            Transform ridePoint = GetComponent<AliemMod.Components.AliemRidingColliderHolderThatsIt>().riddenCollider.transform;
            if (ridePoint == null)
                ridePoint = transform;

            if (base.isAuthority) {
				BlastAttack blast = new BlastAttack {
					attacker = base.gameObject,
					baseDamage = this.damageStat * ChompDamageCoefficient,
					damageType = AliemConfig.M3_Chomp_Slayer.Value? DamageType.BonusToLowHealth : DamageType.Generic,
					//baseForce = this.blastForce,
					//bonusForce = this.blastBonusForce,
					crit = this.RollCrit(),
					//damageType = this.GetBlastDamageType(),
					falloffModel = BlastAttack.FalloffModel.None,
					procCoefficient = 1,
					radius = 2,
					position = ridePoint.position,
					attackerFiltering = AttackerFiltering.NeverHitSelf,
					//impactEffect = EffectCatalog.FindEffectIndexFromPrefab(this.blastImpactEffectPrefab),
					teamIndex = base.teamComponent.teamIndex
				};

				R2API.DamageAPI.AddModdedDamageType(blast, DamageTypes.Decapitating);

				blast.Fire();
			}
            
            if (NetworkServer.active) {

                float healAmount = characterBody.maxHealth - (AliemConfig.M3_Chomp_HealMissing.Value ? healthComponent.health : 0);
                healAmount *= AliemConfig.M3_Chomp_Healing.Value;

                Helpers.LogWarning($"\ncharacterBody.maxHealth {characterBody.maxHealth}\nhealthComponent.health{healthComponent.health}\ndiff {characterBody.maxHealth - healthComponent.health}\nfinal {healAmount}");

                HealOrb healOrb = new HealOrb();
                healOrb.origin = ridePoint.position;
                healOrb.target = characterBody.mainHurtBox;
                healOrb.healValue = healAmount;
                healOrb.overrideDuration = 0.1f;
                OrbManager.instance.AddOrb(healOrb);

                //healthComponent.Heal(healAmount, default(ProcChainMask));
            }
		}

        public override void OnExit()
        {
            base.OnExit();
            EntityStateMachine.FindByCustomName(gameObject, "Body").SetInterruptState(new EndRidingState(), InterruptPriority.PrioritySkill);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}