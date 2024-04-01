using RoR2;
using System;
using UnityEngine;
using PlagueMod.Survivors.Plague;
using PlagueMod.Modules;

namespace PlagueMod.Survivors.Plague.Achievements
{
    //automatically crates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    //[RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class PlagueMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = PlagueSurvivor.PLAGUE_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = PlagueSurvivor.PLAGUE_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => PlagueSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}