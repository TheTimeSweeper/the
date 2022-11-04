using EntityStates;
using RoR2;
using UnityEngine.Networking;

namespace ModdedEntityStates.Aliem {

    internal class AliemRidingChomp : BaseRidingState {

		public static float ChompDamageCoefficient = 9;

		public float baseDuration = 0.3f;
		public float chompTime = 1f;

		private float duration;

		private bool hasCasted;

        public override void OnEnter() {
            base.OnEnter();

			duration = baseDuration / attackSpeedStat;
            PlayCrossfade("FullBody, Override", "RidingChomp", "Chomp.playbackRate", duration*2, 0.1f);
		}

        public override void FixedUpdate() {
            base.FixedUpdate();

			if(base.fixedAge > duration * chompTime && !hasCasted) {
				hasCasted = true;
				OnCastEnter();
            }

			if (base.fixedAge > duration) {
				base.outer.SetNextState(ChooseNextState());
			}
		}

        protected void OnCastEnter() {

			Util.PlayAttackSpeedSound("Play_Chomp", base.gameObject, this.attackSpeedStat);

			if (base.isAuthority) {
				BlastAttack blast = new BlastAttack {
					attacker = base.gameObject,
					baseDamage = this.damageStat * ChompDamageCoefficient,
					//baseForce = this.blastForce,
					//bonusForce = this.blastBonusForce,
					crit = this.RollCrit(),
					//damageType = this.GetBlastDamageType(),
					falloffModel = BlastAttack.FalloffModel.None,
					procCoefficient = 1,
					radius = 2,
					position = transform.position,
					attackerFiltering = AttackerFiltering.NeverHitSelf,
					//impactEffect = EffectCatalog.FindEffectIndexFromPrefab(this.blastImpactEffectPrefab),
					teamIndex = base.teamComponent.teamIndex
				};

				R2API.DamageAPI.AddModdedDamageType(blast, Modules.DamageTypes.Decapitating);

				blast.Fire();
			}

            if (NetworkServer.active) {
				healthComponent.Heal(characterBody.maxHealth * 0.1f, default(ProcChainMask));
            }
		}

        protected EntityState ChooseNextState() {
            return new AliemCharacterMain { wasRiding = true };
        }
    }
}