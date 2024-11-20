using RoR2;
using MatcherMod.Modules.Achievements;

namespace MatcherMod.Survivors.Matcher.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class MasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = MatcherSurvivor.TOKEN_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = MatcherSurvivor.TOKEN_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => MatcherSurvivor.GetBodyNameSafe();

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}