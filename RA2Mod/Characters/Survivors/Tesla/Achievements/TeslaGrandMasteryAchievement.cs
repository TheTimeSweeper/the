using RoR2;
using RA2Mod.Modules.Achievements;

namespace RA2Mod.Survivors.Tesla.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class TeslaGrandMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = TeslaTrooperSurvivor.TESLA_PREFIX + "GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = TeslaTrooperSurvivor.TESLA_PREFIX + "GRANDMASTERYUNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => TeslaTrooperSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3.5f;
    }
}