using Modules;

namespace Modules.Achievements {
    public class TeslaTrooperMastery : BaseMasteryUnlockable {
        public override string RequiredCharacterBody => "TeslaTrooperBody";

        public override float RequiredDifficultyCoefficient => 3;

        public override string AchievementTokenPrefix => Modules.Survivors.TeslaTrooperSurvivor.TESLA_PREFIX + "MASTERY";

        public override string AchievementSpriteName => "texIconTeslaSkinDefault";

        public override string PrerequisiteUnlockableIdentifier => Modules.Survivors.TeslaTrooperSurvivor.TESLA_PREFIX + "CHARACTERUNLOCKABLE_ACHIEVEMENT_ID"; //todo oh shit unlockable
    }
}