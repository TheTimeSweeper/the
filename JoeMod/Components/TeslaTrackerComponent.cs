using Modules.Survivors;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterBody))]
[RequireComponent(typeof(InputBankTest))]
[RequireComponent(typeof(TeamComponent))]
public class TeslaTrackerComponent : MonoBehaviour {

    public enum RangeTier {
        CLOSEST,
        MIDDLE,
        FURTHEST
    }

    public enum TargetType {
        DEFAULT,
        EMPOWERED,
        ALLY
    }

    public static float maxTrackingDistance = 40f;

    public static float nearDist1 = 0.4f;
    public static float nearDist2 = 0.7f;

    //public float maxTrackingAngle = 15f;
    public float trackingRadius = 1f;
    public float trackerUpdateFrequency = 16f;

    private float trackerUpdateStopwatch;
    private HurtBox _trackingTarget;
    private CharacterBody characterBody;
    private TeamComponent teamComponent;
    private InputBankTest inputBank;
    private TeslaIndicator indicator;

    private bool _targetingAlly;

    void Awake() {
        indicator = new TeslaIndicator(base.gameObject, Modules.Assets.TeslaIndicatorPrefab);
        teamComponent = GetComponent<TeamComponent>();
    }

    void Start() {
        characterBody = base.GetComponent<CharacterBody>();
        inputBank = base.GetComponent<InputBankTest>();
        teamComponent = base.GetComponent<TeamComponent>();
    }

    #region access
    public HurtBox GetTrackingTarget() {
        return _trackingTarget;
    }

    public bool GetIsTargetingTeammate() {
        bool team = false;
        if (_trackingTarget) {
            team = _trackingTarget.teamIndex == teamComponent.teamIndex;
        }

        _targetingAlly = team;
        setIndicatorAlly();

        return _targetingAlly;
    }
    #endregion access

    public RangeTier GetTrackingTargetDistance() {

        RangeTier range = RangeTier.FURTHEST;

        float dist = Vector3.Distance(_trackingTarget.transform.position, transform.position);

        if (dist > maxTrackingDistance * nearDist2) {
            range = RangeTier.FURTHEST;
        }
        if (dist < maxTrackingDistance * nearDist2) {
            range = RangeTier.MIDDLE;
        }
        if (dist < maxTrackingDistance * nearDist1) {
            range = RangeTier.CLOSEST;
        }

        return range;
    }

    #region indicator
    public void SetIndicatorTower(bool hasTower) {
        indicator.hasTower = hasTower;
    }

    public void SetIndicatorEmpowered(bool empowered) {
        indicator.empowered = empowered;
    }

    private void setIndicatorRange(RangeTier tier) {
        indicator.currentRange = tier;
    }
    private void setIndicatorAlly() {
        indicator.targetingAlly = _targetingAlly;
    }

    private void OnEnable() {
        indicator.active = true;
    }
    private void OnDisable() {
        indicator.active = false;
    }
    #endregion indicator

    #region search

    private void FixedUpdate() {

        trackerUpdateStopwatch += Time.fixedDeltaTime;
        if (trackerUpdateStopwatch >= 1f / trackerUpdateFrequency) {
            OnSearch();
        }
    }

    private void OnSearch() {

        trackerUpdateStopwatch -= 1f / trackerUpdateFrequency;
        Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);

        FindTrackingTarget(aimRay);
        GetIsTargetingTeammate();

        if (_trackingTarget) {
            setIndicatorRange(GetTrackingTargetDistance());
        }

        ZappableTower zappableTower;
        if (_trackingTarget && _trackingTarget.hurtBoxGroup.TryGetComponent<ZappableTower>(out zappableTower)) {
            _trackingTarget = zappableTower.MainHurtbox;
        }

        indicator.targetTransform = (_trackingTarget ? _trackingTarget.transform : null);
    }

    private bool FindTrackingTarget(Ray aimRay) {

        bool found = SearchForTargetPoint(aimRay);
        if (!found)
            found = SearchForTargetSphere(aimRay);
        //if(!found) searchfortargetbiggersphereinthedistance(aimray)
        return found;
    }

    private bool SearchForTargetPoint(Ray aimRay) {

        RaycastHit hitinfo;
        Util.CharacterRaycast(gameObject, aimRay, out hitinfo, maxTrackingDistance, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.UseGlobal);

        _trackingTarget = hitinfo.collider?.GetComponent<HurtBox>();

        return _trackingTarget;
    }

    private bool SearchForTargetSphere(Ray aimRay) {

        RaycastHit hitinfo;
        Util.CharacterSpherecast(gameObject, aimRay, trackingRadius, out hitinfo, maxTrackingDistance, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.UseGlobal);

        _trackingTarget = hitinfo.collider?.GetComponent<HurtBox>();
        return _trackingTarget;
    }
#endregion search

    public class TeslaIndicator : Indicator {

        public RangeTier currentRange = RangeTier.FURTHEST;

        public bool empowered;
        public bool targetingAlly;
        public bool hasTower;

        public TeslaIndicator(GameObject owner, GameObject visualizerPrefab) : base(owner, visualizerPrefab) { }

        public override void UpdateVisualizer() {
            base.UpdateVisualizer();

            if (visualizerTransform) {

                TeslaIndicatorView indicatorView = visualizerTransform.GetComponent<TeslaIndicatorView>();

                //color
                TargetType currentTarget = TargetType.DEFAULT;

                if (empowered) {
                    currentTarget = TargetType.EMPOWERED;
                } else if (targetingAlly) {
                    currentTarget = TargetType.ALLY;
                }

                indicatorView.UpdateColor((int)currentTarget);

                //sprite
                switch (currentTarget) {

                    default:
                    case TargetType.DEFAULT:
                        indicatorView.setSprite((int)currentRange);
                        break;
                    case TargetType.EMPOWERED:
                        indicatorView.setSprite((int)RangeTier.CLOSEST);
                        break;
                    case TargetType.ALLY:
                        indicatorView.setSpriteAlly();
                        break;
                }

                //tower
                indicatorView.setTowerSprite(hasTower);
            }
        }
    }
}