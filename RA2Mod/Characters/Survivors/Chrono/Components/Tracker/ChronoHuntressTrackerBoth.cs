using RoR2;
using System.Linq;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoHuntressTrackerBoth : MonoBehaviour
    {
        public float maxCloseTrackingDistance = 6;
        // Token: 0x04002A72 RID: 10866
        public float maxTrackingDistance = 20f;

        // Token: 0x04002A73 RID: 10867
        public float maxTrackingAngle = 20f;

        // Token: 0x04002A74 RID: 10868
        public float trackerUpdateInterval = 0.1f;

        private HurtBox bombTrackingTarget;

        private HurtBox vanishTrackingTarget;

        // Token: 0x04002A76 RID: 10870
        private CharacterBody characterBody;

        // Token: 0x04002A77 RID: 10871
        private TeamComponent teamComponent;

        // Token: 0x04002A78 RID: 10872
        private InputBankTest inputBank;

        // Token: 0x04002A79 RID: 10873
        private float trackerUpdateStopwatch;

        private Indicator bombIndicator;
        private Indicator vanishIndicator;

        // Token: 0x04002A7B RID: 10875
        private readonly BullseyeSearch search = new BullseyeSearch();
        // Token: 0x06002686 RID: 9862 RVA: 0x000A83C1 File Offset: 0x000A65C1
        private void Awake()
        {
            //LegacyResourcesAPI runtime
            this.bombIndicator = new Indicator(base.gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
            this.vanishIndicator = new Indicator(base.gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
        }

        // Token: 0x06002687 RID: 9863 RVA: 0x000A83DE File Offset: 0x000A65DE
        private void Start()
        {
            this.characterBody = base.GetComponent<CharacterBody>();
            this.inputBank = base.GetComponent<InputBankTest>();
            this.teamComponent = base.GetComponent<TeamComponent>();
        }

        public HurtBox GetBombTrackingTarget()
        {
            return this.bombTrackingTarget;
        }

        public HurtBox GetVanishTrackingTarget()
        {
            return this.vanishTrackingTarget;
        }

        // Token: 0x06002689 RID: 9865 RVA: 0x000A840C File Offset: 0x000A660C
        private void OnEnable()
        {
            this.bombIndicator.active = true;
            vanishIndicator.active = true;
        }

        // Token: 0x0600268A RID: 9866 RVA: 0x000A841A File Offset: 0x000A661A
        private void OnDisable()
        {
            this.bombIndicator.active = false;
            vanishIndicator.active = false;
        }

        // Token: 0x0600268B RID: 9867 RVA: 0x000A8428 File Offset: 0x000A6628
        private void FixedUpdate()
        {
            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (this.trackerUpdateStopwatch >= this.trackerUpdateInterval)
            {
                this.trackerUpdateStopwatch -= this.trackerUpdateInterval;

                Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);

                HurtBox hurtBox = this.SearchForTarget(aimRay, maxCloseTrackingDistance, BullseyeSearch.SortMode.DistanceAndAngle);
                if (hurtBox)
                {
                    bombTrackingTarget = hurtBox;
                    vanishTrackingTarget = hurtBox;
                } 
                else
                {
                    hurtBox = this.SearchForTarget(aimRay, maxTrackingDistance, BullseyeSearch.SortMode.Angle);
                    if (hurtBox)
                    {
                        vanishTrackingTarget = hurtBox;
                    }
                }

                bombIndicator.targetTransform = bombTrackingTarget?.transform;
                vanishIndicator.targetTransform = vanishTrackingTarget?.transform;
            }
        }

        private HurtBox SearchForTarget(Ray aimRay, float trackingDistance, BullseyeSearch.SortMode bullseyeSortMode)
        {
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(this.teamComponent.teamIndex);
            this.search.filterByLoS = true;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = bullseyeSortMode;
            this.search.maxDistanceFilter = trackingDistance;
            this.search.maxAngleFilter = this.maxTrackingAngle;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(base.gameObject);
            return this.search.GetResults().FirstOrDefault<HurtBox>();
        }
    }
}