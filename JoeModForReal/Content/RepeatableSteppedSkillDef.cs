using RoR2;
using UnityEngine;
using JetBrains.Annotations;

namespace JoeModForReal.Content {
    public class RepeatableSteppedSkillDef : CombinedSteppedSkillDef {

        public Sprite[] extraUseIcons = new Sprite[0];

        public int stocksToConsumeAfterAllUses;

        public override void OnExecute([NotNull] GenericSkill skillSlot) {

            base.OnExecute(skillSlot);

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step == stepCount) { 
                //on base.OnExecute, step is about to be incremented past max and reset
                skillSlot.stock -= stocksToConsumeAfterAllUses;
            }
        }

        protected override void OnResetSkillInstance([NotNull] GenericSkill skillSlot) {
            skillSlot.stock -= stocksToConsumeAfterAllUses;
        }

        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot) {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            int index = instanceData.step;
            if (extraUseIcons.Length <= index)
                return base.GetCurrentIcon(skillSlot);

            return HG.ArrayUtils.GetSafe<Sprite>(this.extraUseIcons, index);
        }
    }
}
