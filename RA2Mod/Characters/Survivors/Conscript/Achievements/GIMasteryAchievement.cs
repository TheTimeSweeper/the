using RoR2;
using RA2Mod.Modules.Achievements;

namespace RA2Mod.Survivors.Conscript.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    //[RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class ConscriptMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = ConscriptSurvivor.TOKEN_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = ConscriptSurvivor.TOKEN_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => ConscriptSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}