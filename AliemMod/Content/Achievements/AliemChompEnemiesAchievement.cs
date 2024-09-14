using AliemMod.Content.Survivors;
using AliemMod.Modules;
using R2API;
using RoR2;
using RoR2.Achievements;

namespace AliemMod.Content.Achievements
{
                                                                                                                    //todo find out what good coin rewards are
    [RegisterAchievement(identifier, unlockableIdentifier, AliemUnlockables.AliemPrerequisiteAchievementIdentifier, 3, typeof(AliemChompEnemiesAchievementServer))]
    public class AliemChompEnemiesAchievement : BaseAchievement
    {
        public const string identifier = AliemSurvivor.ALIEM_PREFIX + "ChompEnemiesAchievement" + AliemUnlockables.DevResetString;
        public const string unlockableIdentifier = AliemSurvivor.ALIEM_PREFIX + "ChompEnemiesUnlockable" + AliemUnlockables.DevResetString;

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
                        this._trackedBody.characterMotor.onHitGroundAuthority -= CharacterMotor_onHitGroundAuthority;
                    }
                    this._trackedBody = value;
                    if (this._trackedBody != null)
                    {
                        this._trackedBody.characterMotor.onHitGroundAuthority += CharacterMotor_onHitGroundAuthority;
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
                RoR2Application.onFixedUpdate += this.FixedUpdate;
                GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            }

            // Token: 0x06005826 RID: 22566 RVA: 0x00164AE3 File Offset: 0x00162CE3
            public override void OnUninstall()
            {
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
                if (damageReport.attackerBody == null || damageReport.attackerBody != this.trackedBody)
                    return;

                if (damageReport.damageInfo.HasModdedDamageType(DamageTypes.Decapitating))
                {
                    progress++;
                    if (progress >= AliemChompEnemiesAchievement.Requirement)
                    {
                        base.Grant();
                    }
                }
            }

            private void CharacterMotor_onHitGroundAuthority(ref CharacterMotor.HitGroundInfo hitGroundInfo)
            {
                if (EntityStateMachine.FindByCustomName(_trackedBody.gameObject, "Body").state is ModdedEntityStates.Aliem.AliemRidingState)
                {
                    return;
                }
                progress = 0;
            }
        }
    }
}
