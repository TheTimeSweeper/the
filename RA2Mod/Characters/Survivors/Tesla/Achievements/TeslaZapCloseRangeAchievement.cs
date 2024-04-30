using RA2Mod.Survivors.Tesla;
using RA2Mod.Survivors.Tesla.States;
using RoR2;
using RoR2.Achievements;
using RoR2.Skills;

namespace RA2Mod.Survivors.Tesla.Achievements
{
    //not implemented, if you couldn't tell
    //[RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class TeslaRepairTowerAchievement : BaseAchievement
    {
        public const string identifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "CHARACTERUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "CHARACTERUNLOCKABLE_REWARD_ID";

    }

    //scrapped because boring
    //[RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class TeslaZapCloseRangeAchievement : BaseAchievement
    {
        public const string identifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "ZAPCLOSERANGEUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "ZAPCLOSERANGEUNLOCKABLE_REWARD_ID";

        //public override string AchievementSpriteName => "texTeslaSkillSecondaryAlt";

        private int closeZaps;
        public static int requirement = 20;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("TeslaTrooperBody");
        }

        public override void OnBodyRequirementMet()
        {
            Zap.onZapAuthority += OnZapAuthority;
        }

        public override void OnBodyRequirementBroken()
        {
            Zap.onZapAuthority -= OnZapAuthority;
        }

        private void OnZapAuthority(bool close, bool teammate)
        {
            if (!teammate)
            {
                if (close)
                {
                    closeZaps++;
                    if (closeZaps >= requirement)
                        Grant();
                    //Helpers.LogWarning(closeZaps);
                }
                else
                {
                    closeZaps = 0;
                }
            }
        }
    }
}