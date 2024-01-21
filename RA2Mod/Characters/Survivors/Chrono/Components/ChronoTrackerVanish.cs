using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoTrackerVanish : DependentChronoHuntressTracker
    {
        public override float maxTrackingDistance => 40;

        public override float maxTrackingAngle => 30;

        public override BullseyeSearch.SortMode bullseyeSortMode => BullseyeSearch.SortMode.Angle;

        public override bool filterByLoS => false;

        public override DependentChronoHuntressTracker dependentTracker { get; set; }

        protected override void Awake()
        {
            //dependentTracker = GetComponent<ChronoTrackerBomb>();
            this.indicator = new Indicator(base.gameObject, ChronoAssets.chronoIndicatorVanish);
        }
    }
}