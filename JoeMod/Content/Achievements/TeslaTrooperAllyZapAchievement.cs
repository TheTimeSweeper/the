using RoR2;

namespace Modules.Achievements {
    public class TeslaTrooperAllyZapAchievement : GenericModdedUnlockable {

		public override string AchievementTokenPrefix => Survivors.TeslaTrooperSurvivor.TESLA_PREFIX + "ZAPALLY";
		public override string AchievementSpriteName => "texTeslaSkillSecondaryAlt";
		public override string PrerequisiteUnlockableIdentifier => "";//todo tesla unlock

		private int allyZaps;
		public static int requirement = 3;

		public override BodyIndex LookUpRequiredBodyIndex() {
			return BodyCatalog.FindBodyIndex("TeslaTrooperBody");
		}

		public override void OnBodyRequirementMet() {
            ModdedEntityStates.TeslaTrooper.Zap.onZapAllyAuthority += Zap_onZapAllyAuthority;
		}

        private void Zap_onZapAllyAuthority(HurtBox allyHurtbox) {

			//check if they're not already charged
			if (!allyHurtbox.healthComponent.body.HasBuff(Modules.Buffs.conductiveBuffTeam)) {
				allyZaps++;
				if (allyZaps >= requirement)
					base.Grant();
				Helpers.LogWarning(allyZaps);
			}
		}

        public override void OnBodyRequirementBroken() {
			ModdedEntityStates.TeslaTrooper.Zap.onZapAllyAuthority -= Zap_onZapAllyAuthority;
		}
	}
}