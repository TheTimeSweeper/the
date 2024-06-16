using AliemMod.Content.Survivors;
using AliemMod.Modules;
using R2API;
using RoR2;
using RoR2.Achievements;

namespace AliemMod.Content.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, AliemUnlockables.AliemPrerequisiteAchievementIdentifier, typeof(AliemChompEnemiesAchievementServer))]
    public class AliemChompEnemiesAchievement : BaseAchievement
    {
        public const string identifier = AliemSurvivor.ALIEM_PREFIX + "ChompEnemiesAchievement" + AliemUnlockables.DevResetString;
        public const string unlockableIdentifier = AliemSurvivor.ALIEM_PREFIX + "ChompEnemiesUnlockable" + AliemUnlockables.DevResetString;

        public static float Requirement = 20;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            Helpers.LogWarning("chompenemies bodyindex " + BodyCatalog.FindBodyIndex(AliemSurvivor.instance.bodyInfo.bodyPrefabName));

            return BodyCatalog.FindBodyIndex(AliemSurvivor.instance.bodyInfo.bodyPrefabName);
        }

        // Token: 0x0600581E RID: 22558 RVA: 0x001608E1 File Offset: 0x0015EAE1
        public override void OnBodyRequirementMet()
        {
            Helpers.LogWarning("chompenemies met");
            base.OnBodyRequirementMet();
            base.SetServerTracked(true);
        }

        // Token: 0x0600581F RID: 22559 RVA: 0x001608F0 File Offset: 0x0015EAF0
        public override void OnBodyRequirementBroken()
        {
            Helpers.LogWarning("chompenemies broken");
            base.SetServerTracked(false);
            base.OnBodyRequirementBroken();
        }

        private class AliemChompEnemiesAchievementServer : BaseServerAchievement
        {
            private CharacterBody trackedBody
            {
                get
                {
                    return this._trackedBody;
                }
                set
                {
                    if (this._trackedBody == value)
                    {
                        return;
                    }
                    if (this._trackedBody != null)
                    {
                        this._trackedBody.characterMotor.onHitGroundServer -= CharacterMotor_onHitGroundServer;
                    }
                    this._trackedBody = value;
                    if (this._trackedBody != null)
                    {
                        this._trackedBody.characterMotor.onHitGroundServer += CharacterMotor_onHitGroundServer;
                        this.progress = 0;
                    }
                }
            }
            private CharacterBody _trackedBody;
            private int progress;

            // Token: 0x06005825 RID: 22565 RVA: 0x00164AA4 File Offset: 0x00162CA4
            public override void OnInstall()
            {
                base.OnInstall();
                Helpers.LogWarning("chompenemies install");
                RoR2Application.onFixedUpdate += this.FixedUpdate;
                GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            }

            // Token: 0x06005826 RID: 22566 RVA: 0x00164AE3 File Offset: 0x00162CE3
            public override void OnUninstall()
            {
                Helpers.LogWarning("chompenemies uninstall");
                this.trackedBody = null;
                RoR2Application.onFixedUpdate -= this.FixedUpdate;
                GlobalEventManager.onServerDamageDealt -= GlobalEventManager_onServerDamageDealt;
                base.OnUninstall();
            }

            // Token: 0x06005827 RID: 22567 RVA: 0x00164B14 File Offset: 0x00162D14
            private void FixedUpdate()
            {
                this.trackedBody = base.GetCurrentBody();
            }

            private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
            {
                Helpers.LogWarning("decapitating" + DamageTypes.Decapitating);
                if (!damageReport.damageInfo.HasModdedDamageType(DamageTypes.Decapitating))
                    return;

                if (damageReport.attackerBody == null || damageReport.attackerBody != this.trackedBody)
                    return;
                
                progress++;
                Helpers.LogWarning("CHompEnemies progress " + progress);
                if (progress >= AliemChompEnemiesAchievement.Requirement)
                {
                    base.Grant();
                }
                
            }

            private void CharacterMotor_onHitGroundServer(ref CharacterMotor.HitGroundInfo hitGroundInfo)
            {
                if (EntityStateMachine.FindByCustomName(_trackedBody.gameObject, "Body").state is ModdedEntityStates.Aliem.AliemRidingState)
                {
                    Helpers.LogWarning("no reset, riding");
                    return;
                }
                progress = 0;
                Helpers.LogWarning("CHompEnemies progress reset " + progress);
            }
        }
    }
}
