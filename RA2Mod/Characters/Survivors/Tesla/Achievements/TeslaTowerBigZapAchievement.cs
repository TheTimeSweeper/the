using RA2Mod.Minions.TeslaTower.States;
using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, null, 3, typeof(TeslaTrooperTowerBigZapServerAchievement))]
    public class TeslaTowerBigZapAchievement : BaseAchievement
    {
        public const string identifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "BIGZAPUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = TeslaTrooperSurvivor.TOKEN_PREFIX + "BIGZAPUNLOCKABLE_REWARD_ID";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("TeslaTrooperBody");
        }

        // Token: 0x060056FD RID: 22269 RVA: 0x0015D621 File Offset: 0x0015B821
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            base.SetServerTracked(true);
        }

        // Token: 0x060056FE RID: 22270 RVA: 0x0015D630 File Offset: 0x0015B830
        public override void OnBodyRequirementBroken()
        {
            base.SetServerTracked(false);
            base.OnBodyRequirementBroken();
        }

        // Token: 0x02000EED RID: 3821
        public class TeslaTrooperTowerBigZapServerAchievement : BaseServerAchievement
        {
            // Token: 0x06005700 RID: 22272 RVA: 0x00160BE8 File Offset: 0x0015EDE8
            public override void OnInstall()
            {
                base.OnInstall();
                TowerBigZap.onTowerBigZapMultiHit += OnTowerBigZapHit;
            }

            // Token: 0x06005701 RID: 22273 RVA: 0x00160C04 File Offset: 0x0015EE04
            private void OnTowerBigZapHit(GameObject tower)
            {
                //todo teslamove tower
                if (tower.GetComponent<TowerOwnerTrackerComponent>().OwnerTrackerComponent.gameObject == GetCurrentBody().gameObject)
                {
                    Grant();
                }
            }

            // Token: 0x06005702 RID: 22274 RVA: 0x00160C48 File Offset: 0x0015EE48
            public override void OnUninstall()
            {
                TowerBigZap.onTowerBigZapMultiHit -= OnTowerBigZapHit;
                base.OnUninstall();
            }
        }
    }
}