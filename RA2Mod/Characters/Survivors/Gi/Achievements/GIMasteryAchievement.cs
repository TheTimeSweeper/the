using RoR2;
using RA2Mod.Modules.Achievements;

namespace RA2Mod.Survivors.GI.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    //[RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class GIMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = GISurvivor.GI_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = GISurvivor.GI_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => GISurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}