using RA2Mod.General.Components;
using RA2Mod.Survivors.Chrono.SkillDefs;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoTrackerBomb : GenericTracker
    {
        public override float maxTrackingDistance => 12;

        public override float maxTrackingAngle => 180;

        public override BullseyeSearch.SortMode bullseyeSortMode => BullseyeSearch.SortMode.Angle;

        public override bool filterByLoS => false;

        private CameraTargetParams cameraTargetParams;

        private SkillLocator skillLocator;

        protected override void Awake()
        {
            cameraTargetParams = GetComponent<CameraTargetParams>();
            this.indicator = new Indicator(base.gameObject, ChronoAssets.chronoIndicatorIvan);

            skillLocator = GetComponent<SkillLocator>();
        }

        protected override HurtBox SearchForTarget(Ray aimRay)
        {
            //if (cameraTargetParams)
            //{
            //    aimRay.origin = cameraTargetParams.cameraPivotTransform.position;
            //}
            
            return base.SearchForTarget(aimRay);
        }

        protected override TeamMask GetTeamMask()
        {
            return TeamMask.all;
        }
    }
}