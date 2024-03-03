using RoR2;
using System.Linq;
using UnityEngine;

namespace RA2Mod.General.Components
{
    public abstract class GenericTracker : MonoBehaviour, ITracker
    {
        public abstract float maxTrackingDistance { get; }
        public abstract float maxTrackingAngle { get; }
        public abstract BullseyeSearch.SortMode bullseyeSortMode { get; }
        public abstract bool filterByLoS { get; }

        protected TeamMask mask;

        protected float trackerUpdateInterval = 0.01f;
        protected Indicator indicator;

        private HurtBox trackingTarget;

        // Token: 0x04002A78 RID: 10872
        private InputBankTest inputBank;
        // Token: 0x04002A79 RID: 10873
        private float trackerUpdateStopwatch;
        private TeamComponent teamComponent;

        // Token: 0x04002A7B RID: 10875
        private readonly BullseyeSearch search = new BullseyeSearch();

        // Token: 0x06002686 RID: 9862 RVA: 0x000A83C1 File Offset: 0x000A65C1
        protected virtual void Awake()
        {
            SetIndicator();
        }

        protected virtual void SetIndicator()
        {
            //LegacyResourcesAPI runtime
            indicator = new Indicator(gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
        }

        // Token: 0x06002687 RID: 9863 RVA: 0x000A83DE File Offset: 0x000A65DE
        protected virtual void Start()
        {
            inputBank = GetComponent<InputBankTest>();
            teamComponent = GetComponent<TeamComponent>();

            mask = GetTeamMask();
        }

        protected virtual TeamMask GetTeamMask()
        {
            return TeamMask.GetUnprotectedTeams(teamComponent.teamIndex);
        }

        public HurtBox GetTrackingTarget()
        {
            return trackingTarget;
        }
        public bool GetIsAlly()
        {
            if (trackingTarget == null)
                return false;

            return !FriendlyFireManager.ShouldDirectHitProceed(trackingTarget.healthComponent, teamComponent.teamIndex);
        }

        // Token: 0x06002689 RID: 9865 RVA: 0x000A840C File Offset: 0x000A660C
        protected virtual void OnEnable()
        {
            indicator.active = true;
        }

        // Token: 0x0600268A RID: 9866 RVA: 0x000A841A File Offset: 0x000A661A
        protected virtual void OnDisable()
        {
            indicator.active = false;
        }

        // Token: 0x0600268B RID: 9867 RVA: 0x000A8428 File Offset: 0x000A6628
        private void FixedUpdate()
        {
            trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (trackerUpdateStopwatch >= trackerUpdateInterval)
            {
                trackerUpdateStopwatch -= trackerUpdateInterval;

                Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);

                trackingTarget = SearchForTarget(aimRay);

                indicator.targetTransform = trackingTarget?.transform;
            }
        }

        protected virtual HurtBox SearchForTarget(Ray aimRay)
        {
            if (this is IDependentTracker tracker)
            {
                if (tracker.dependentTracker != null && tracker.dependentTracker.GetTrackingTarget())
                {
                    return tracker.dependentTracker.GetTrackingTarget();
                }
            }

            search.teamMaskFilter = mask;
            search.filterByLoS = filterByLoS;
            search.searchOrigin = aimRay.origin;
            search.searchDirection = aimRay.direction;
            search.sortMode = bullseyeSortMode;
            search.maxDistanceFilter = maxTrackingDistance;
            search.maxAngleFilter = maxTrackingAngle;
            search.RefreshCandidates();
            search.FilterOutGameObject(gameObject);
            return search.GetResults().FirstOrDefault();
        }
    }
}