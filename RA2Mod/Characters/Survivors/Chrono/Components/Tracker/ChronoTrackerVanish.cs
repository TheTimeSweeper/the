using RA2Mod.General.Components;
using RA2Mod.Survivors.Chrono.SkillDefs;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoTrackerVanish : TrackerSkillDefRequired<ChronoTrackerSkillDefVanish>//, IDependentTracker
    {
        public override float maxTrackingDistance => 30;
        
        public override float maxTrackingAngle => 30;

        public override BullseyeSearch.SortMode bullseyeSortMode => BullseyeSearch.SortMode.Angle;

        public override bool filterByLoS => false;

        //public Tracker dependentTracker { get; set; }

        protected override void SetIndicator()
        {
            //dependentTracker = GetComponent<ChronoTrackerBomb>();
            this.indicator = new Indicator(base.gameObject, ChronoAssets.chronoIndicatorVanish);
        }

        //protected override TeamMask GetTeamMask()
        //{
        //    return TeamMask.all;
        //}
    }
}