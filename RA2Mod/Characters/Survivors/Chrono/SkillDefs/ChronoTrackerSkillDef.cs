﻿using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using System.Diagnostics.CodeAnalysis;

namespace RA2Mod.Survivors.Chrono.SkillDefs
{
    public abstract class ChronoTrackerSkillDef<T> : HasComponentSkillDef<T> where T : DependentChronoHuntressTracker
    {
        // Token: 0x060045B8 RID: 17848 RVA: 0x00122449 File Offset: 0x00120649
        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            T huntressTracker = ((InstanceData<T>)skillSlot.skillInstanceData).componentFromSkillDef;
            return (huntressTracker != null) ? huntressTracker.GetTrackingTarget() : null;
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