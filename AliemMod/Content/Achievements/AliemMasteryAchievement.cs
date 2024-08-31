using AliemMod.Content.Survivors;
using Modules.Achievements;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliemMod.Content.Achievements
{

    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, AliemUnlockables.AliemPrerequisiteAchievementIdentifier, 10, null)]
    public class AliemMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = AliemSurvivor.ALIEM_PREFIX + "MASTERYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = AliemSurvivor.ALIEM_PREFIX + "MASTERYUNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => AliemSurvivor.instance.bodyInfo.bodyPrefabName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}
