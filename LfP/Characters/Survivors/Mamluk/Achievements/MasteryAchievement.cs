using RoR2;
using LfPMod.Modules.Achievements;

namespace LfPMod.Survivors.Mamluk.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class MasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = MamlukSurvivor.TOKEN_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = MamlukSurvivor.TOKEN_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => MamlukSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}