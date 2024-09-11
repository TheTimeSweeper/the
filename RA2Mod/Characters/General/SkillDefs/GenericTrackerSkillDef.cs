using RA2Mod.General.Components;
using RoR2;
using JetBrains.Annotations;

namespace RA2Mod.General.SkillDefs
{

    public abstract class GenericTrackerSkillDef<T> : HasComponentSkillDef<T> where T : ITracker
    {
        // Token: 0x060045B8 RID: 17848 RVA: 0x00122449 File Offset: 0x00120649
        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            T huntressTracker = ((InstanceData)skillSlot.skillInstanceData).componentFromSkillDef1;
            return huntressTracker != null ? huntressTracker.GetTrackingTarget() : null;
        }
        // Token: 0x060045B9 RID: 17849 RVA: 0x00122471 File Offset: 0x00120671
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }
        // Token: 0x060045BA RID: 17850 RVA: 0x00122484 File Offset: 0x00120684
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HasTarget(skillSlot);
        }
    }
}
