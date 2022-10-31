using EntityStates;
using RoR2;

namespace ModdedEntityStates.Aliem {

    internal class AliemRidingChomp : BaseRidingState {

		public static float ChompDamageCoefficient = 8;

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
			
			new BlastAttack {
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
			}.Fire();
		}

        protected EntityState ChooseNextState() {
            return new AliemCharacterMain { wasRiding = true };
        }
    }
}