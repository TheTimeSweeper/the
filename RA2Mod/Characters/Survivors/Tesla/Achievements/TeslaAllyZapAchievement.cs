using RoR2;
using RoR2.Achievements;

namespace RA2Mod.Survivors.Tesla.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, null, 3, null)]
    public class TeslaAllyZapAchievement : BaseAchievement
    {
        public const string identifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "ZAPALLYUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "ZAPALLYUNLOCKABLE_REWARD_ID";

        private int allyZaps;
        public static int requirement = 3;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("TeslaTrooperBody");
        }

        public override void OnBodyRequirementMet()
        {
            States.Zap.onZapAllyAuthority += Zap_onZapAllyAuthority;
        }

        private void Zap_onZapAllyAuthority(HurtBox allyHurtbox)
        {

            //check if they're not already charged
            if (!allyHurtbox.healthComponent.body.HasBuff(TeslaBuffs.conductiveBuffTeam))
            {
                allyZaps++;
                if (allyZaps >= requirement)
                    base.Grant();
                //Log.Warning(allyZaps);
            }
        }

        public override void OnBodyRequirementBroken()
        {
            States.Zap.onZapAllyAuthority -= Zap_onZapAllyAuthority;
        }
    }
}