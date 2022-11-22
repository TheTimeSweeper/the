namespace Modules.Achievements {
    public class DesolatorMastery : BaseMasteryUnlockable {
        public override string RequiredCharacterBody => "DesolatorBody";

        public override float RequiredDifficultyCoefficient => 3;

        public override string AchievementTokenPrefix => Modules.Survivors.DesolatorSurvivor.DESOLATOR_PREFIX + "MASTERY";

        public override string AchievementSpriteName => "texIconSkinDesolatorDefault";

        public override string PrerequisiteUnlockableIdentifier => Modules.Survivors.DesolatorSurvivor.DESOLATOR_PREFIX + "CHARACTERUNLOCKABLE_ACHIEVEMENT_ID"; //todo oh shit unlockable
    }

    public class DesolatorGrandMastery : BaseMasteryUnlockable {
        public override string RequiredCharacterBody => "DesolatorBody";

        public override float RequiredDifficultyCoefficient => 3.5f;

        public override string AchievementTokenPrefix => Modules.Survivors.DesolatorSurvivor.DESOLATOR_PREFIX + "GRANDMASTERY";

        public override string AchievementSpriteName => "texTeslaSkinNod";

        public override string PrerequisiteUnlockableIdentifier => Modules.Survivors.DesolatorSurvivor.DESOLATOR_PREFIX + "CHARACTERUNLOCKABLE_ACHIEVEMENT_ID"; //todo oh shit unlockable
    }
}