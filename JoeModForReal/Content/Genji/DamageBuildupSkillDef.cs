using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using System;

namespace JoeModForReal.Content.Survivors {
    public class DamageBuildupSkillDef : SkillDef {
		
		protected class InstanceData : SkillDef.BaseSkillInstanceData {
			public UltimateBuildup ultimateBuildup;
			public GenericSkill skillSlot;
			
            public void buildDamage(float damage) {

				if (skillSlot.stateMachine.state.GetType() != skillSlot.skillDef.activationState.stateType &&
					skillSlot.stock < skillSlot.maxStock) {
					skillSlot.rechargeStopwatch += damage * GenjiConfig.dragonBladeChargeMultiplier.Value;
				}
            }
        }

        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot) {

			InstanceData data = new DamageBuildupSkillDef.InstanceData {
				ultimateBuildup = skillSlot.GetComponent<UltimateBuildup>(),
				skillSlot = skillSlot
			};

            data.ultimateBuildup.OnDamageForUltimate += data.buildDamage;

			skillSlot.stock = 0;

			return data;
		}

        public override void OnUnassigned([NotNull] GenericSkill skillSlot) {
            base.OnUnassigned(skillSlot);

            InstanceData data = ((DamageBuildupSkillDef.InstanceData)skillSlot.skillInstanceData);
			data.ultimateBuildup.OnDamageForUltimate -= data.buildDamage;
		}

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot) {
			RunRechargeWithoutTheRecharge(skillSlot);
			return;
        }

        private void RunRechargeWithoutTheRecharge([NotNull] GenericSkill skillSlot) {
			if (skillSlot.stock < skillSlot.maxStock) {
				if (skillSlot.rechargeStopwatch >= skillSlot.finalRechargeInterval) {
					skillSlot.RestockSteplike();
				}
			}
		}
    }
}