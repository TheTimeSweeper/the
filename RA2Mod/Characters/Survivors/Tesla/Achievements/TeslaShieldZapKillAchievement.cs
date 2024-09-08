using RoR2;
using RoR2.Achievements;

namespace RA2Mod.Survivors.Tesla.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, null, 3, typeof(TeslaShieldZapKillAchievementServer))]
    public class TeslaShieldZapKillAchievement : BaseAchievement
    {
        public const string identifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "SHIELDZAPUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "SHIELDZAPUNLOCKABLE_REWARD_ID";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("TeslaTrooperBody");
        }

        // Token: 0x06005681 RID: 22145 RVA: 0x0015D621 File Offset: 0x0015B821
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            base.SetServerTracked(true);
        }

        // Token: 0x06005682 RID: 22146 RVA: 0x0015D630 File Offset: 0x0015B830
        public override void OnBodyRequirementBroken()
        {
            base.SetServerTracked(false);
            base.OnBodyRequirementBroken();
        }

        // Token: 0x02000ED8 RID: 3800
        public class TeslaShieldZapKillAchievementServer : BaseServerAchievement
        {
            // Token: 0x04005096 RID: 20630
            private bool hasFiredShieldZap;
            //private SkillDef requiredSkillDef;

            // Token: 0x06005685 RID: 22149 RVA: 0x001600BC File Offset: 0x0015E2BC
            public override void OnInstall()
            {
                base.OnInstall();

                RoR2Application.onFixedUpdate += this.OnFixedUpdate;
                GlobalEventManager.onCharacterDeathGlobal += this.OnCharacterDeath;
                //ModdedEntityStates.TeslaTrooper.ShieldZapReleaseDamage.onShieldZap += OnShieldZap;
            }

            private void OnShieldZap()
            {
                hasFiredShieldZap = true;
            }

            // Token: 0x06005687 RID: 22151 RVA: 0x00160108 File Offset: 0x0015E308
            public override void OnUninstall()
            {
                GlobalEventManager.onCharacterDeathGlobal -= this.OnCharacterDeath;
                RoR2Application.onFixedUpdate -= this.OnFixedUpdate;
                //ModdedEntityStates.TeslaTrooper.ShieldZapReleaseDamage.onShieldZap -= OnShieldZap;
                base.OnUninstall();
            }

            // Token: 0x06005689 RID: 22153 RVA: 0x00160172 File Offset: 0x0015E372
            private void OnFixedUpdate()
            {
                this.hasFiredShieldZap = false;
            }

            // Token: 0x0600568A RID: 22154 RVA: 0x00160184 File Offset: 0x0015E384
            private void OnCharacterDeath(DamageReport damageReport)
            {
                if (damageReport.attackerMaster == base.networkUser.master && damageReport.attackerMaster != null && this.hasFiredShieldZap)
                {
                    if (damageReport.victimBody.isChampion)
                    {
                        base.Grant();
                    }
                }
            }
        }
    }
}