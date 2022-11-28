using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;

namespace Modules.Achievements {
    public class TeslaTrooperTowerBigZapAchievement : GenericModdedUnlockable {

        public override string AchievementTokenPrefix => Survivors.TeslaTrooperSurvivor.TESLA_PREFIX + "BIGZAP";
        public override string AchievementSpriteName => "texTeslaSkillUtilityAlt";
        public override string PrerequisiteUnlockableIdentifier => "";//todo tesla unlock

		public override BodyIndex LookUpRequiredBodyIndex() {
			return BodyCatalog.FindBodyIndex("TeslaTrooperBody");
		}

		// Token: 0x060056FD RID: 22269 RVA: 0x0015D621 File Offset: 0x0015B821
		public override void OnBodyRequirementMet() {
			base.OnBodyRequirementMet();
			base.SetServerTracked(true);
		}

		// Token: 0x060056FE RID: 22270 RVA: 0x0015D630 File Offset: 0x0015B830
		public override void OnBodyRequirementBroken() {
			base.SetServerTracked(false);
			base.OnBodyRequirementBroken();
		}

		// Token: 0x02000EED RID: 3821
		public class TeslaTrooperTowerBigZapServerAchievement : BaseServerAchievement {
			// Token: 0x06005700 RID: 22272 RVA: 0x00160BE8 File Offset: 0x0015EDE8
			public override void OnInstall() {
				base.OnInstall();
				ModdedEntityStates.TeslaTrooper.Tower.TowerBigZap.onTowerBigZapMultiHit += OnTowerBigZapHit;
			}

			// Token: 0x06005701 RID: 22273 RVA: 0x00160C04 File Offset: 0x0015EE04
			private void OnTowerBigZapHit(GameObject tower) {

				if (tower.GetComponent<TowerOwnerTrackerComponent>().OwnerTrackerComponent.gameObject == base.GetCurrentBody().gameObject) {
					base.Grant();
                }
			}

			// Token: 0x06005702 RID: 22274 RVA: 0x00160C48 File Offset: 0x0015EE48
			public override void OnUninstall() {
				ModdedEntityStates.TeslaTrooper.Tower.TowerBigZap.onTowerBigZapMultiHit -= OnTowerBigZapHit;
				base.OnUninstall();
			}
		}
	}
}