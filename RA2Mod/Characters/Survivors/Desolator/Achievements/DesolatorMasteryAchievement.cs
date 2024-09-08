using RA2Mod.Modules.Achievements;
using RoR2;

namespace RA2Mod.Survivors.Desolator.Achievements
{
    //automatically creates language tokens $"ACHIEVMENT_{identifier.ToUpper()}_NAME" and $"ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class DesolatorMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = DesolatorSurvivor.TOKEN_PREFIX + "MASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = DesolatorSurvivor.TOKEN_PREFIX + "MASTERYUNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => DesolatorSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}