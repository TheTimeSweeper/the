using RA2Mod.Survivors.Tesla;
using RA2Mod.Survivors.Tesla.States;
using RoR2;
using RoR2.Achievements;
using RoR2.Skills;

namespace Modules.Achievements {

    //scrapped because boring
    //[RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class TeslaTrooperZapCloseRangeAchievement : BaseAchievement {

        public const string identifier = TeslaTrooperSurvivor.TESLA_PREFIX + "ZAPCLOSERANGEUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = TeslaTrooperSurvivor.TESLA_PREFIX + "ZAPCLOSERANGEUNLOCKABLE_REWARD_ID";

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
                        base.Grant();
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