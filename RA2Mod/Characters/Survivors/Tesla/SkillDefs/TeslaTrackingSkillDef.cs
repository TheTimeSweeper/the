using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;

namespace RA2Mod.Survivors.Tesla.SkillDefs
{
    public class TeslaTrackingSkillDef : SkillDef
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                teslaTracker = skillSlot.GetComponent<TeslaTrackerComponentZap>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            TeslaTrackerComponentZap teslaTracker = ((InstanceData)skillSlot.skillInstanceData).teslaTracker;
            return teslaTracker != null && teslaTracker.GetTrackingTarget();
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HasTarget(skillSlot);
        }

        protected class InstanceData : BaseSkillInstanceData
        {

            public TeslaTrackerComponentZap teslaTracker;
        }
    }
}
