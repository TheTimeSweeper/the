using RoR2;
using JetBrains.Annotations;

namespace JoeModForReal.Content {
    public class RepeatableComboSkillDef : ComboSkillDef {
        public int stocksToConsumeAfterAllUses;
        public int maxUsesPerStock;
        private bool _startedUses;

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot) {

            BaseSkillInstanceData instanceData = base.OnAssigned(skillSlot);
            (instanceData as InstanceData).onComboHistoryCleared += OnResetCombo;
            return instanceData;
        }

        public override void OnUnassigned([NotNull] GenericSkill skillSlot) {
            (skillSlot.skillInstanceData as InstanceData).onComboHistoryCleared -= OnResetCombo;
        }

        private void OnResetCombo(GenericSkill skillSlot) {
            if (_startedUses) {
                skillSlot.stock -= stocksToConsumeAfterAllUses;
                (skillSlot.skillInstanceData as InstanceData).comboStep = 0;
                _startedUses = false;
            }
        }

        public override void OnExecute([NotNull] GenericSkill skillSlot) {

            _startedUses = true;

            base.OnExecute(skillSlot);

            InstanceData instanceData = (skillSlot.skillInstanceData as InstanceData);
            if (instanceData.comboStep == maxUsesPerStock && _startedUses) {
                _startedUses = false;
                skillSlot.stock -= stocksToConsumeAfterAllUses;
                instanceData.comboStep = 0;
            }

        }
    }
}
