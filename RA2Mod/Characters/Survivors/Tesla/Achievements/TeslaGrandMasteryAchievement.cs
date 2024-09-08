using RoR2;
using RA2Mod.Modules.Achievements;

namespace RA2Mod.Survivors.Tesla.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, null, 15, null)]
    public class TeslaGrandMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "GRANDMASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "GRANDMASTERYUNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => TeslaTrooperSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3.5f;
    }
}