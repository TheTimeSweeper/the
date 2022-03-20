using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;

namespace Modules {
    public class TeslaTrackingSkillDef : SkillDef {

        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot) {
            return new TeslaTrackingSkillDef.InstanceData {
                teslaTracker = skillSlot.GetComponent<TotallyOriginalTrackerComponent>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot) {
            TotallyOriginalTrackerComponent teslaTracker = ((TeslaTrackingSkillDef.InstanceData)skillSlot.skillInstanceData).teslaTracker;
            return (teslaTracker != null) ? teslaTracker.GetTrackingTarget() : null;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot) {
            return TeslaTrackingSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot) {
            return base.IsReady(skillSlot) && TeslaTrackingSkillDef.HasTarget(skillSlot);
        }

        protected class InstanceData : SkillDef.BaseSkillInstanceData {

            public TotallyOriginalTrackerComponent teslaTracker;
        }
    }
}
