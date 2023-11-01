using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;

namespace JoeModForReal.Components {
    public class ConditionalSkillOverride2 : MonoBehaviour {
		// Token: 0x06004587 RID: 17799 RVA: 0x000026ED File Offset: 0x000008ED
		private void Start() {
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x0012150C File Offset: 0x0011F70C
		private void FixedUpdate() {
			bool flag = false;
			bool flag2 = false;
			if (this.characterBody) {
				flag = this.characterBody.isSprinting;
				if (this.characterBody.characterMotor) {
					flag2 = this.characterBody.characterMotor.isGrounded;
				}
			}
			bool flag3 = this.wasSprinting || this.wasAirborne;
			bool flag4 = flag || flag2;
			foreach (ConditionalSkillOverride2.ConditionalSkillInfo conditionalSkillInfo in this.conditionalSkillInfos) {
				if (flag3) {
					if (this.wasSprinting && !flag) {
						conditionalSkillInfo.skillSlot.UnsetSkillOverride(this, conditionalSkillInfo.sprintSkillDef, GenericSkill.SkillOverridePriority.Replacement);
					}
				}
				if (flag4) {
					if (flag && !this.wasSprinting) {
						conditionalSkillInfo.skillSlot.SetSkillOverride(this, conditionalSkillInfo.sprintSkillDef, GenericSkill.SkillOverridePriority.Replacement);
					}
				}
			}
			this.wasAirborne = flag2;
			this.wasSprinting = flag;
		}

		// Token: 0x040043B7 RID: 17335
		public CharacterBody characterBody;

		// Token: 0x040043B8 RID: 17336
		public ConditionalSkillOverride2.ConditionalSkillInfo[] conditionalSkillInfos;

		// Token: 0x040043B9 RID: 17337
		private bool wasSprinting;

		// Token: 0x040043BA RID: 17338
		private bool wasAirborne;

		// Token: 0x02000BF8 RID: 3064
		[Serializable]
		public struct ConditionalSkillInfo {
			// Token: 0x040043BB RID: 17339
			public GenericSkill skillSlot;

			// Token: 0x040043BC RID: 17340
			public SkillDef sprintSkillDef;
		}
	}
}
