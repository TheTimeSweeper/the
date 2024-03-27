using RoR2;
using System;
using UnityEngine;
using KatamariMod.Survivors.Plague;
using KatamariMod.Modules;

namespace KatamariMod.Survivors.Plague.Achievements
{
    //automatically crates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class KatamariMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = KatamariSurvivor.PLAGUE_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = KatamariSurvivor.PLAGUE_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => KatamariSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}