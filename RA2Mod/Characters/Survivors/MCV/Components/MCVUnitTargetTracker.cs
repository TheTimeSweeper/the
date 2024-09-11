using RA2Mod.General.Components;
using RA2Mod.Survivors.GI.SkillDefs;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.MCV.Components
{
    public class MCVUnitComponent : MonoBehaviour
    {
        public CharacterBody selectedUnit;
    }

    public class MCVUnitTargetTracker : GenericTracker
    {
        public override float maxTrackingDistance => 369;

        public override float maxTrackingAngle => 30;

        public override BullseyeSearch.SortMode bullseyeSortMode => BullseyeSearch.SortMode.Angle;

        public override bool filterByLoS => true;

        protected override TeamMask GetTeamMask()
        {
            return TeamMask.all;
        }
    }
}