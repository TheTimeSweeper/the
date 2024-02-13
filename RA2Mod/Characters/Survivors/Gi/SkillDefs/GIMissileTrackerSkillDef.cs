using RA2Mod.General.SkillDefs;
using RoR2;
using JetBrains.Annotations;
using RA2Mod.Survivors.GI.Components;

namespace RA2Mod.Survivors.GI.SkillDefs
{
    public class GIMissileTrackerSkillDef : GenericTrackerSkillDef<GIMissileTracker>
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                componentFromSkillDef = skillSlot.GetComponent<GIMissileTracker>()
            };
        }
    }
}