using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoTrackerBomb : DependentChronoHuntressTracker
    {
        public override float maxTrackingDistance => 10;

        public override float maxTrackingAngle => 180;

        public override BullseyeSearch.SortMode bullseyeSortMode => BullseyeSearch.SortMode.DistanceAndAngle;

        public override bool filterByLoS => false;

        public override DependentChronoHuntressTracker dependentTracker { get; set; }

        private CameraTargetParams cameraTargetParams;

        protected override void Awake()
        {
            dependentTracker = null;
            cameraTargetParams = GetComponent<CameraTargetParams>();
            this.indicator = new Indicator(base.gameObject, ChronoAssets.chronoIndicatorIvan);
        }

        protected override HurtBox SearchForTarget(Ray aimRay)
        {
            if (cameraTargetParams)
            {
                aimRay.origin = cameraTargetParams.cameraPivotTransform.position;
            }

            return base.SearchForTarget(aimRay);
        }
    }
}