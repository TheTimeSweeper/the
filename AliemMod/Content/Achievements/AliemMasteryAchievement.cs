using AliemMod.Content.Survivors;
using Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliemMod.Content.Achievements {
    public class AliemMasteryAchievement : BaseMasteryUnlockable {

        public override string RequiredCharacterBody => "AliemBody";

        public override float RequiredDifficultyCoefficient => 3;

        public override string AchievementTokenPrefix => AliemSurvivor.ALIEM_PREFIX + "MASTERY";

        public override string AchievementSpriteName => "texIconMasteryAchievement";

        public override string PrerequisiteUnlockableIdentifier => "";
    }
}
