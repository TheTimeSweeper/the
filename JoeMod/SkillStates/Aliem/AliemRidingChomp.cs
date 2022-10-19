using EntityStates;
using ModdedEntityStates.BaseStates;
using RoR2;

namespace ModdedEntityStates.Aliem {

    internal class AliemRidingChomp : BaseRidingState {

		public float ChompDamageCoefficient = 6;

		private bool hasCasted;

        public override void OnEnter() {
            base.OnEnter();

			//todo play animation chomp
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

			if(base.fixedAge > 0.4f / base.attackSpeedStat && !hasCasted) {
				hasCasted = true;
				OnCastEnter();

				base.outer.SetNextState(ChooseNextState());
            }
        }

        protected void OnCastEnter() {

			Util.PlayAttackSpeedSound(EntityStates.LemurianMonster.Bite.attackString, base.gameObject, this.attackSpeedStat);

			new BlastAttack {
				attacker = base.gameObject,
				baseDamage = this.damageStat * this.ChompDamageCoefficient,
				//baseForce = this.blastForce,
				//bonusForce = this.blastBonusForce,
				crit = this.RollCrit(),
				//damageType = this.GetBlastDamageType(),
				falloffModel = BlastAttack.FalloffModel.None,
				procCoefficient = 1,
				radius = 3,
				position = transform.position,
				attackerFiltering = AttackerFiltering.NeverHitSelf,
				//impactEffect = EffectCatalog.FindEffectIndexFromPrefab(this.blastImpactEffectPrefab),
				teamIndex = base.teamComponent.teamIndex
			}.Fire();
		}

        protected EntityState ChooseNextState() {
            return new AliemCharacterMain { wasRiding = true };
        }
    }
}