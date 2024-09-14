using RoR2;
using RoR2.Skills;
using UnityEngine;
using JetBrains.Annotations;
using EntityStates;
using System;
using System.Linq;
using System.Collections.Generic;
using JoeModForReal.Components;

namespace JoeModForReal.Content {

    //copypaste RoR2.Skills.SteppedSkillDef with few changes
    //hang me at the stake, but if you do, also explain a better way pls
    //really wish you could mix skilldefs
    public class CombinedSteppedSkillDef : SkillDef {

        /// <summary>
        /// set -1 for infinite
        /// </summary>
        public int maxCombinedStepCount = 4;
        /// <summary>
        /// does this SkillDef reset all CombinedSteppedSkillDefs on the body when it completes.
        /// </summary>
        public bool resetOthersOnReset;

        // Token: 0x0400440F RID: 17423
        public int stepCount = 2;
        // Token: 0x04004410 RID: 17424
        public float stepGraceDuration = 0.1f;
        // Token: 0x04004411 RID: 17425
        protected float stepResetTimer;

        // Token: 0x02000C1C RID: 3100
        public class InstanceData : SkillDef.BaseSkillInstanceData {

            public List<GenericSkill> otherSkills;
            public int step;

            public InstanceData () {
                otherSkills = HG.CollectionPool<GenericSkill, List<GenericSkill>>.RentCollection();
            }

            public void Dispose() {
                otherSkills = HG.CollectionPool<GenericSkill, List<GenericSkill>>.ReturnCollection(otherSkills);
            }
        }

        // Token: 0x0600461E RID: 17950 RVA: 0x00122873 File Offset: 0x00120A73
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot) {

            InstanceData data = new InstanceData();

            GenericSkill[] allSkills = skillSlot.GetComponents<GenericSkill>();
            for (int i = 0; i < allSkills.Length; i++) {
                if (allSkills[i] != skillSlot) {
                    data.otherSkills.Add(allSkills[i]);
                }
            }

            return data;
        }

        public override void OnUnassigned([NotNull] GenericSkill skillSlot) {
            base.OnUnassigned(skillSlot);
            (skillSlot.skillInstanceData as InstanceData).Dispose();
        }

        public interface ICombinedStepSetter {
            void SetCombinedStep(int i);
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot) {

            EntityState entityState = base.InstantiateNextState(skillSlot);
            InstanceData skillInstanceData = skillSlot.skillInstanceData as InstanceData;

            ICombinedStepSetter combinedStepState = (entityState as ICombinedStepSetter);
            if (combinedStepState != null) {
                combinedStepState.SetCombinedStep(GetTotalCombinedUses(skillSlot));
            }

            SteppedSkillDef.IStepSetter stepState = (entityState as SteppedSkillDef.IStepSetter);

            if (stepState != null && skillInstanceData != null) {
                stepState.SetStep(skillInstanceData.step);
            }
            
            return entityState;
        }

        private int GetTotalCombinedUses([NotNull] GenericSkill thisSkillSlot) {

            int uses = (thisSkillSlot.skillInstanceData as InstanceData).step;

            List<GenericSkill> otherSkills = (thisSkillSlot.skillInstanceData as InstanceData).otherSkills;

            for (int i = 0; i < otherSkills.Count; i++) {
                InstanceData skillInstanceData = otherSkills[i].skillInstanceData as InstanceData;
                if (skillInstanceData != null) {
                    uses += skillInstanceData.step;
                }
            }
            return uses;
        }
        // Token: 0x06004620 RID: 17952 RVA: 0x001228B4 File Offset: 0x00120AB4
        public override void OnExecute([NotNull] GenericSkill skillSlot) {
            //unlike SteppedSkillDef, reset step before cast so you can hold on to the last step after the last cast (to be counted by other skilldefs combined counts)
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step >= this.stepCount) {
                instanceData.step = 0;
            }

            base.OnExecute(skillSlot);

            ResetAllCombinedSteppedSkillDefTimers(skillSlot);
            
            instanceData.step++;
            if(maxCombinedStepCount >= 0 && GetTotalCombinedUses(skillSlot) >= maxCombinedStepCount) {

                ResetAllCombinedSteppedSkillDefSteps(skillSlot);
            }
        }

        private void ResetAllCombinedSteppedSkillDefTimers([NotNull] GenericSkill thisSkillSlot) {

            stepResetTimer = 0;

            List<GenericSkill> otherSkills = (thisSkillSlot.skillInstanceData as InstanceData).otherSkills;

            for (int i = 0; i < otherSkills.Count; i++) {

                GenericSkill otherSkillSLot = otherSkills[i];
                CombinedSteppedSkillDef def = otherSkillSLot.skillDef as CombinedSteppedSkillDef;
                if (def != null) {
                    def.stepResetTimer = 0;
                }
            }
        }

        private void ResetAllCombinedSteppedSkillDefSteps([NotNull]GenericSkill thisSkillSlot) {

            (thisSkillSlot.skillInstanceData as InstanceData).step = 0;
            ResetOtherCombinedSteppedSkillDefSteps(thisSkillSlot);
        }

        private void ResetOtherCombinedSteppedSkillDefSteps([NotNull] GenericSkill thisSkillSlot) {

            List<GenericSkill> otherSkills = (thisSkillSlot.skillInstanceData as InstanceData).otherSkills;

            for (int i = 0; i < otherSkills.Count; i++) {

                GenericSkill otherSkillSLot = otherSkills[i];
                CombinedSteppedSkillDef combinedDef = otherSkillSLot.skillDef as CombinedSteppedSkillDef;
                if (combinedDef != null) {
                    InstanceData instanceData = otherSkillSLot.skillInstanceData as InstanceData;
                    if (instanceData != null) {
                        instanceData.step = 0;
                    }
                }
            }
        }

        // Token: 0x06004621 RID: 17953 RVA: 0x001228F8 File Offset: 0x00120AF8
        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime) {
            base.OnFixedUpdate(skillSlot, deltaTime);
            if (skillSlot.CanExecute()) {
                this.stepResetTimer += Time.fixedDeltaTime;
            } else {
                this.stepResetTimer = 0f;
            }
            if (this.stepResetTimer > this.stepGraceDuration) {

                InstanceData skillInstanceData = ((InstanceData)skillSlot.skillInstanceData);
                if (skillInstanceData.step != 0) {
                    OnTimeoutResetSkill(skillSlot);
                }
                skillInstanceData.step = 0;
            }
        }

        protected virtual void OnTimeoutResetSkill([NotNull] GenericSkill skillSlot) {
            if (resetOthersOnReset) {
                ResetOtherCombinedSteppedSkillDefSteps(skillSlot);
            }
        }
    }
}
