using RoR2;
using RoR2.Skills;
using UnityEngine;
using JetBrains.Annotations;
using System.Linq;
using System.Collections.Generic;
using JoeModForReal.Components;
using EntityStates;
using System;

namespace JoeModForReal.Content {

    public class ComboSkillDef : SkillDef {

        // Token: 0x04004410 RID: 17424
        public float comboGraceDuration = 0.1f;
        // Token: 0x04004411 RID: 17425
        protected float _comboResetTimer;

        protected List<GenericSkill> _otherSkills;
        protected ComboRecipeCooker _comboRecipeCooker;
        protected int _thisSkillComboMoveIndex;
        protected ComboRecipeCooker.ComboRecipe _nextPotentialCombo;
        protected bool _inCombo;

        public class InstanceData : BaseSkillInstanceData {

            public List<int> comboHistory = new List<int>();
            public int comboStep;
            public GenericSkill skillSlot;

            public event Action<GenericSkill> onComboHistoryCleared;

            public InstanceData(GenericSkill skillSlot_) {
                skillSlot = skillSlot_;
            }

            internal void ClearComboHistory() {
                comboHistory.Clear();

                onComboHistoryCleared?.Invoke(skillSlot);
            }
        }

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

            _nextPotentialCombo = null;

            return new InstanceData(skillSlot);
        }

        public override void OnUnassigned([NotNull] GenericSkill skillSlot) {
            base.OnUnassigned(skillSlot);
            _otherSkills.Clear();
            _comboRecipeCooker = null;

            _nextPotentialCombo = null;
        }

        #region Show Potential Skilldef
        public override string GetCurrentNameToken([NotNull] GenericSkill skillSlot) {
            if (_nextPotentialCombo != null) {
                return _nextPotentialCombo.resultSkillDef.skillNameToken;
            }

            return base.GetCurrentNameToken(skillSlot);
        }

        public override string GetCurrentDescriptionToken([NotNull] GenericSkill skillSlot) {
            if (_nextPotentialCombo != null) {
                return _nextPotentialCombo.resultSkillDef.skillDescriptionToken;
            }

            return base.GetCurrentDescriptionToken(skillSlot);
        }

        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot) {
            if (_nextPotentialCombo != null && _nextPotentialCombo.resultSkillDef.icon != null) {
                return _nextPotentialCombo.resultSkillDef.icon;
            }

            return base.GetCurrentIcon(skillSlot);
        }
        #endregion

        public override void OnExecute([NotNull] GenericSkill skillSlot) {

            _inCombo = true;

            AddComboIndexToAll(skillSlot);

            (skillSlot.skillInstanceData as InstanceData).comboStep++;

            base.OnExecute(skillSlot);

            SetNextPotentialCombo(skillSlot);

            ResetAllComboTimers();
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot) {

            EntityState entityState = base.InstantiateNextState(skillSlot);
            InstanceData instanceData = skillSlot.skillInstanceData as InstanceData;

            ComboRecipeCooker.ComboRecipe comboRecipe = _comboRecipeCooker.GetCombo(instanceData.comboHistory);
            if (comboRecipe != null) {
                entityState = EntityStateCatalog.InstantiateState(ref comboRecipe.resultSkillDef.activationState);

                if (comboRecipe.resetComboHistory) {

                    ResetAllComboHistory(skillSlot);
                }
            }

            return entityState;
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

        private void SetNextPotentialCombo(GenericSkill skillSlot) {
            InstanceData skillInstanceData = skillSlot.skillInstanceData as InstanceData;
            _nextPotentialCombo = _comboRecipeCooker.GetCombo(skillInstanceData.comboHistory, _thisSkillComboMoveIndex);
        }


        private void ResetAllComboTimers() {

            _comboResetTimer = 0;

            for (int i = 0; i < _otherSkills.Count; i++) {

                GenericSkill otherSkillSLot = _otherSkills[i];
                ComboSkillDef def = otherSkillSLot.skillDef as ComboSkillDef;
                if (def != null) {
                    def._comboResetTimer = 0;
                }
            }
        }

        protected virtual void ResetAllComboHistory([NotNull] GenericSkill skillSlot) {

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            instanceData.ClearComboHistory();
            instanceData.comboStep = 0;

            for (int i = 0; i < _otherSkills.Count; i++) {
                instanceData = (InstanceData)_otherSkills[i].skillInstanceData;
                if (instanceData != null) {
                    instanceData.ClearComboHistory();
                }
            }

            _nextPotentialCombo = null;
            _inCombo = false;
        }

        // Token: 0x06004621 RID: 17953 RVA: 0x001228F8 File Offset: 0x00120AF8
        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime) {
            base.OnFixedUpdate(skillSlot, deltaTime);
            if (skillSlot.CanExecute()) {
                this._comboResetTimer += Time.fixedDeltaTime;
            } else {
                this._comboResetTimer = 0f;
            }
            if (this._comboResetTimer > this.comboGraceDuration) {
                if (_inCombo) {
                    ResetAllComboHistory(skillSlot);
                }
            }
        }
    }
}
