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

        protected List<GenericSkill> _otherSkills;
        protected ComboRecipeCooker _comboRecipeCooker;
        protected int _thisSkillComboMoveIndex;

        // Token: 0x02000C1C RID: 3100
        public class InstanceData : SkillDef.BaseSkillInstanceData {

            public int step;
            public List<int> comboHistory = new List<int>();
        }

        // Token: 0x0600461E RID: 17950 RVA: 0x00122873 File Offset: 0x00120A73
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot) {

            _otherSkills = skillSlot.GetComponents<GenericSkill>().ToList();

            for (int i = 0; i < _otherSkills.Count; i++) {
                if (_otherSkills[i] == skillSlot) {
                    _thisSkillComboMoveIndex = i;
                    _otherSkills.RemoveAt(i);
                    break;
                }
            }

            _comboRecipeCooker = skillSlot.GetComponent<ComboRecipeCooker>();

            return new InstanceData();
        }

        public override void OnUnassigned([NotNull] GenericSkill skillSlot) {
            base.OnUnassigned(skillSlot);
            _otherSkills.Clear();
            _comboRecipeCooker = null;
        }

        public interface ICombinedStepSetter {
            void SetCombinedStep(int i);
        }

        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot) {

            InstanceData skillInstanceData = skillSlot.skillInstanceData as InstanceData;
            ComboRecipeCooker.ComboRecipe potentialCombo = _comboRecipeCooker.GetCombo(skillInstanceData.comboHistory, _thisSkillComboMoveIndex);
            if(potentialCombo!= null && potentialCombo.sprite != null) {
                return potentialCombo.sprite;
            }

            return base.GetCurrentIcon(skillSlot);
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot) {

            AddComboIndexToAll(skillSlot);

            EntityState entityState = base.InstantiateNextState(skillSlot);
            InstanceData skillInstanceData = skillSlot.skillInstanceData as InstanceData;

            ComboRecipeCooker.ComboRecipe comboRecipe = _comboRecipeCooker.GetCombo(skillInstanceData.comboHistory);
            if(comboRecipe != null) {
                entityState = EntityStateCatalog.InstantiateState(comboRecipe.resultState);

                if (comboRecipe.resetComboHistory) {

                    ResetAllComboHistory(skillSlot);
                    ResetAllCombinedSteppedSkillDefSteps(skillSlot);
                }
            }

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

            for (int i = 0; i < _otherSkills.Count; i++) {
                InstanceData skillInstanceData = _otherSkills[i].skillInstanceData as InstanceData;
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
                if (resetOthersOnReset) {
                    ResetAllComboHistory(skillSlot);
                }
            }

            base.OnExecute(skillSlot);

            ResetAllCombinedSteppedSkillDefTimers();
            
            instanceData.step++;
            if(maxCombinedStepCount >= 0 && GetTotalCombinedUses(skillSlot) >= maxCombinedStepCount) {

                ResetAllCombinedSteppedSkillDefSteps(skillSlot);
                ResetAllComboHistory(skillSlot);
            }
        }

        private void AddComboIndexToAll([NotNull] GenericSkill skillSlot) {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            instanceData.comboHistory.Add(_thisSkillComboMoveIndex);

            for (int i = 0; i < _otherSkills.Count; i++) {
                instanceData = (InstanceData)_otherSkills[i].skillInstanceData;
                if (instanceData != null) {
                    instanceData.comboHistory.Add(_thisSkillComboMoveIndex);
                }
            }
        }

        private void ResetAllCombinedSteppedSkillDefTimers() {

            stepResetTimer = 0;

            for (int i = 0; i < _otherSkills.Count; i++) {

                GenericSkill otherSkillSLot = _otherSkills[i];
                CombinedSteppedSkillDef def = otherSkillSLot.skillDef as CombinedSteppedSkillDef;
                if (def != null) {
                    def.stepResetTimer = 0;
                }
            }
        }

        private void ResetAllCombinedSteppedSkillDefSteps([NotNull]GenericSkill thisSkillSlot) {

            (thisSkillSlot.skillInstanceData as InstanceData).step = 0;
            ResetOtherCombinedSteppedSkillDefSteps();
        }

        private void ResetOtherCombinedSteppedSkillDefSteps() {
            for (int i = 0; i < this._otherSkills.Count; i++) {

                GenericSkill otherSkillSLot = _otherSkills[i];
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
        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot) {
            base.OnFixedUpdate(skillSlot);
            if (skillSlot.CanExecute()) {
                this.stepResetTimer += Time.fixedDeltaTime;
            } else {
                this.stepResetTimer = 0f;
            }
            if (this.stepResetTimer > this.stepGraceDuration) {

                InstanceData skillInstanceData = ((InstanceData)skillSlot.skillInstanceData);
                if (skillInstanceData.step != 0)
                    OnTimeoutResetSkill(skillSlot);
                skillInstanceData.step = 0;
            }
        }

        protected virtual void OnTimeoutResetSkill([NotNull] GenericSkill skillSlot) {
            if (resetOthersOnReset)
                ResetOtherCombinedSteppedSkillDefSteps();

            ResetAllComboHistory(skillSlot);
        }

        private void ResetAllComboHistory([NotNull] GenericSkill skillSlot) {

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            instanceData.comboHistory.Clear();

            for (int i = 0; i < _otherSkills.Count; i++) {
                instanceData = (InstanceData)_otherSkills[i].skillInstanceData;
                if (instanceData != null) {
                    instanceData.comboHistory.Clear();
                }
            }
        }
    }
}
