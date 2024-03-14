using RoR2;
using RoR2.Skills;

namespace Modules.Achievements {

    //scrapped because boring
    public class TeslaTrooperZapCloseRangeAchievement : GenericModdedUnlockable {

		public override string AchievementTokenPrefix => Survivors.TeslaTrooperSurvivor.TESLA_PREFIX + "ZAPCLOSERANGE";
		public override string AchievementSpriteName => "texTeslaSkillSecondaryAlt";
		public override string PrerequisiteUnlockableIdentifier => "";//todo tesla unlock

		private int closeZaps;
		public static int requirement = 20;

		public override BodyIndex LookUpRequiredBodyIndex() {
			return BodyCatalog.FindBodyIndex("TeslaTrooperBody");
		}

		public override void OnBodyRequirementMet() {
			ModdedEntityStates.TeslaTrooper.Zap.onZapAuthority += OnZapAuthority;
		}
		
		public override void OnBodyRequirementBroken() {
			ModdedEntityStates.TeslaTrooper.Zap.onZapAuthority -= OnZapAuthority;
		}

		private void OnZapAuthority(bool close, bool teammate) {
			if (!teammate) {
				if (close) {
					closeZaps++;
					if (closeZaps >= requirement)
						base.Grant();
					//Helpers.LogWarning(closeZaps);
				} else {
					closeZaps = 0;
				}
			}
		}
	}
}