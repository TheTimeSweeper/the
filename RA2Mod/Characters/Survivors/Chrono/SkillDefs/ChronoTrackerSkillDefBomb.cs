using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using System.Diagnostics.CodeAnalysis;

namespace RA2Mod.Survivors.Chrono.SkillDefs
{
    public class ChronoTrackerSkillDefBomb : ChronoTrackerSkillDef<ChronoTrackerBomb>
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData<ChronoTrackerBomb>
            {
                componentFromSkillDef = skillSlot.GetComponent<ChronoTrackerBomb>()
            };
        }
    }
}
