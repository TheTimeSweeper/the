using AliemMod.Content.Survivors;
using RoR2;
using RoR2.Achievements;
using System;

namespace AliemMod.Content.Achievements
{

    [RegisterAchievement(identifier, unlockableIdentifier, AliemUnlockables.AliemPrerequisiteAchievementIdentifier, null)]
    public class AliemBurrowPopOutAchievement : BaseAchievement
    {
        public const string identifier = AliemSurvivor.ALIEM_PREFIX + "BurrowPopOutAchievement" + AliemUnlockables.DevResetString;
        public const string unlockableIdentifier = AliemSurvivor.ALIEM_PREFIX + "BurrowPopOutUnlockable" + AliemUnlockables.DevResetString;

        public static int Requirement = 10;

        // Token: 0x0600573C RID: 22332 RVA: 0x0016336C File Offset: 0x0016156C
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            Helpers.LogWarning("burrwopopoupt bodyindex " + BodyCatalog.FindBodyIndex("AliemBody"));
            return BodyCatalog.FindBodyIndex("AliemBody");
        }

        // Token: 0x0600573D RID: 22333 RVA: 0x001634A1 File Offset: 0x001616A1
        public override void OnBodyRequirementMet()
        {
            Helpers.LogWarning("burrwopopout requirement met");
            ModdedEntityStates.Aliem.AliemBurrow.onBurrowPopOutMultiHit += AliemBurrow_onBurrowPopOutMultiHit;
        }

        // Token: 0x0600573E RID: 22334 RVA: 0x001634C5 File Offset: 0x001616C5
        public override void OnBodyRequirementBroken()
        {
            Helpers.LogWarning("burrwopopout requirement broken");
            ModdedEntityStates.Aliem.AliemBurrow.onBurrowPopOutMultiHit -= AliemBurrow_onBurrowPopOutMultiHit;
        }

        private void AliemBurrow_onBurrowPopOutMultiHit(int enemies)
        {
            Helpers.LogWarning("burrwopopoupt enemies " + enemies);
            if (enemies >= Requirement)
            {
                Helpers.LogWarning("uh just grant. should be authority");
                base.Grant();
            }
        }
    }
}
