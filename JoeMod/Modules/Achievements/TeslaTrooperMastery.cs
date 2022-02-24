using HenryMod.Modules;
using HenryMod;

namespace HenryMod.Modules.Achievements {
    public class TeslaTrooperMastery : BaseMasteryUnlockable {
        public override string RequiredCharacterBody => "TeslaTrooperBody";

        public override float RequiredDifficultyCoefficient => 3;

        public override string AchievementTokenPrefix => Modules.Survivors.TeslaTrooperSurvivor.teslaPrefix + "MASTERY";

        public override string AchievementSpriteName => "texIconTeslaSkinDefault";

        public override string PrerequisiteUnlockableIdentifier => Modules.Survivors.TeslaTrooperSurvivor.teslaPrefix + "CHARACTERUNLOCKABLE_ACHIEVEMENT_ID"; //todo oh shit unlockable
    }
}