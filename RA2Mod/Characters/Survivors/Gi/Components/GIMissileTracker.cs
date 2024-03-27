using RA2Mod.General.Components;
using RA2Mod.Survivors.GI.SkillDefs;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.GI.Components
{
    public class GIMissileTracker : TrackerSkillDefRequired<GIMissileTrackerSkillDef>
    {
        public override float maxTrackingDistance => 69;

        public override float maxTrackingAngle => 30;

        public override BullseyeSearch.SortMode bullseyeSortMode => BullseyeSearch.SortMode.Angle;

        public override bool filterByLoS => true;
    }
}