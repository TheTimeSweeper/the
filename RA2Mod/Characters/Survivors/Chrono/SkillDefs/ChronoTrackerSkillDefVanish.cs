using RA2Mod.General.SkillDefs;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using System.Diagnostics.CodeAnalysis;

namespace RA2Mod.Survivors.Chrono.SkillDefs
{
    public class ChronoTrackerSkillDefVanish : GenericTrackerSkillDef<ChronoTrackerVanish>
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                componentFromSkillDef = skillSlot.GetComponent<ChronoTrackerVanish>()
            };
        }
    }
}
