using AliemMod.Content.Survivors;
using AliemMod.Modules;
using R2API;
using RoR2;
using RoR2.Achievements;
using System;

namespace AliemMod.Content.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, AliemUnlockables.AliemPrerequisiteAchievementIdentifier, typeof(AliemChargedKillAchievementServer))]
    public class AliemChargedKillAchievement : BaseAchievement
    {
        public const string identifier = AliemSurvivor.ALIEM_PREFIX + "ChargedKillAchievement" + AliemUnlockables.DevResetString;
        public const string unlockableIdentifier = AliemSurvivor.ALIEM_PREFIX + "ChargedKillUnlockable" + AliemUnlockables.DevResetString;

        public static float Requirement = 6;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(AliemSurvivor.instance.bodyInfo.bodyPrefabName);
        }

        // Token: 0x0600581E RID: 22558 RVA: 0x001608E1 File Offset: 0x0015EAE1
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            base.SetServerTracked(true);
        }

        // Token: 0x0600581F RID: 22559 RVA: 0x001608F0 File Offset: 0x0015EAF0
        public override void OnBodyRequirementBroken()
        {
            base.SetServerTracked(false);
            base.OnBodyRequirementBroken();
        }

        private class AliemChargedKillAchievementServer : BaseServerAchievement
        {
            private int progress;
            private float graceTimer;

            // Token: 0x06005825 RID: 22565 RVA: 0x00164AA4 File Offset: 0x00162CA4
            public override void OnInstall()
            {
                base.OnInstall();
                GlobalEventManager.onCharacterDeathGlobal += this.OnCharacterDeathGlobal;
                RoR2Application.onFixedUpdate += this.FixedUpdate;

            }

            // Token: 0x06005826 RID: 22566 RVA: 0x00164AE3 File Offset: 0x00162CE3
            public override void OnUninstall()
            {
                base.OnUninstall();
                GlobalEventManager.onCharacterDeathGlobal -= this.OnCharacterDeathGlobal;
                RoR2Application.onFixedUpdate -= this.FixedUpdate;
            }

            private void OnCharacterDeathGlobal(DamageReport damageReport)
            {
                if (damageReport.attackerBody == null || damageReport.attackerBody != GetCurrentBody())
                {
                    return;
                }

                if (damageReport.damageInfo.HasModdedDamageType(DamageTypes.FuckinChargedKillAchievementTracking))
                {
                    this.progress++;
                    graceTimer = 0.3f;
                    if (this.progress >= AliemChargedKillAchievement.Requirement)
                    {
                        base.Grant();
                    }
                }
            }

            private void FixedUpdate()
            {
                graceTimer -= UnityEngine.Time.fixedDeltaTime;
                if (graceTimer < 0)
                {
                    progress = 0;
                }
            }
        }
    }
}
