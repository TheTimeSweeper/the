using RoR2;
using UnityEngine;
using JetBrains.Annotations;

namespace JoeModForReal.Content {
    public class RepeatableCombinedSteppedSkillDef : CombinedSteppedSkillDef {

        public Sprite[] extraUseIcons = new Sprite[0];

        public int stocksToConsumeAfterAllUses;

        public override void OnExecute([NotNull] GenericSkill skillSlot) {

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            if (instanceData.step >= stepCount -1) {
                skillSlot.stock -= stocksToConsumeAfterAllUses;
            }

            base.OnExecute(skillSlot);
        }

        protected override void OnTimeoutResetSkill([NotNull] GenericSkill skillSlot) {
            base.OnTimeoutResetSkill(skillSlot);
            skillSlot.stock -= stocksToConsumeAfterAllUses;
        }

        //public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot) {
        //    InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

        //    int index = instanceData.step;
        //    if (extraUseIcons.Length <= index)
        //        return base.GetCurrentIcon(skillSlot);

        //    return HG.ArrayUtils.GetSafe<Sprite>(this.extraUseIcons, index);
        //}
    }
}
