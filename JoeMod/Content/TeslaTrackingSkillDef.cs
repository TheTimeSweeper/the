using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;

namespace Content {

    public class TeslaTrackingSkillDef : SkillDef {

        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot) {
            return new TeslaTrackingSkillDef.InstanceData {
                teslaTracker = skillSlot.GetComponent<TeslaTrackerComponent>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot) {
            TeslaTrackerComponent teslaTracker = ((TeslaTrackingSkillDef.InstanceData)skillSlot.skillInstanceData).teslaTracker;
            return (teslaTracker != null) ? teslaTracker.GetTrackingTarget() : null;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot) {
            return TeslaTrackingSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot) {
            return base.IsReady(skillSlot) && TeslaTrackingSkillDef.HasTarget(skillSlot);
        }

        protected class InstanceData : SkillDef.BaseSkillInstanceData {

            public TeslaTrackerComponent teslaTracker;
        }
    }

    public class TeslaConductiveTrackingSkillDef : SkillDef {

        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot) {
            return new TeslaConductiveTrackingSkillDef.InstanceData {
                teslaTracker = skillSlot.GetComponent<TeslaTrackerComponent>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot) {

            TeslaTrackerComponent teslaTracker = ((TeslaConductiveTrackingSkillDef.InstanceData)skillSlot.skillInstanceData).teslaTracker;
            HurtBox trackingTarget = teslaTracker?.GetTrackingTarget();

            return trackingTarget != null && trackingTarget.healthComponent.body.HasBuff(Modules.Buffs.conductiveBuff);
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot) {
            return TeslaConductiveTrackingSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot) {
            return base.IsReady(skillSlot) && TeslaConductiveTrackingSkillDef.HasTarget(skillSlot);
        }

        protected class InstanceData : SkillDef.BaseSkillInstanceData {

            public TeslaTrackerComponent teslaTracker;
        }
    }
}
