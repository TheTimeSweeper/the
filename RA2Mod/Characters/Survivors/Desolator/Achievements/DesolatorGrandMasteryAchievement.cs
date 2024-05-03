using RA2Mod.Modules.Achievements;
using RoR2;

namespace RA2Mod.Survivors.Desolator.Achievements
{
    //[RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class DesolatorGrandMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = DesolatorSurvivor.TOKEN_PREFIX + "GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = DesolatorSurvivor.TOKEN_PREFIX + "GRANDMASTERYUNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => DesolatorSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3.5f;
    }
}