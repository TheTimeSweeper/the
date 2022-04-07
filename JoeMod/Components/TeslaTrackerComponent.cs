using Modules.Survivors;
using RoR2;
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

    private bool _empowered;
    private bool _targetingAlly;

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

    // Token: 0x04000FAB RID: 4011
    //private readonly BullseyeSearch search = new BullseyeSearch();

    void Awake() {
        indicator = new TeslaIndicator(base.gameObject, Modules.Assets.TeslaIndicatorPrefab);// RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/LightningIndicator"));
        teamComponent = GetComponent<TeamComponent>();
    }

    void Start() {
        characterBody = base.GetComponent<CharacterBody>();
        inputBank = base.GetComponent<InputBankTest>();
        teamComponent = base.GetComponent<TeamComponent>();
    }

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

    public void SetIndicatorEmpowered(bool empowered) {
        _empowered = empowered;
        indicator.empowered = _empowered;
    }

    private void setIndicatorRamge(RangeTier tier) {
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
            setIndicatorRamge(GetTrackingTargetDistance());
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

    public class TeslaIndicator : Indicator {

        public static Sprite allySprite = Modules.Assets.LoadAsset<Sprite>("texIndicatorAlly");
        public static Sprite[] rangeSprites = new Sprite[] { Modules.Assets.LoadAsset<Sprite>("texIndicator1Close"),
                                                             Modules.Assets.LoadAsset<Sprite>("texIndicator2Med"),
                                                             Modules.Assets.LoadAsset<Sprite>("texIndicator3Far")
        };

        public static Color[] targetcolors = new Color[] { Color.cyan,
                                                           Color.red,
                                                           Color.green
        };

        public RangeTier currentRange = RangeTier.FURTHEST;

        public bool empowered;
        public bool targetingAlly;

        public TeslaIndicator(GameObject owner, GameObject visualizerPrefab) : base(owner, visualizerPrefab) { }

        public override void UpdateVisualizer() {
            base.UpdateVisualizer();

            if (visualizerTransform) {

                SpriteRenderer rend = visualizerTransform.GetComponentInChildren<SpriteRenderer>();

                //color
                TargetType currentTarget = TargetType.DEFAULT;

                if (empowered) {
                    currentTarget = TargetType.EMPOWERED;
                } else if(targetingAlly) {
                    currentTarget = TargetType.ALLY;
                }

                rend.color = targetcolors[(int)currentTarget];

                //sprite
                switch (currentTarget) {

                    default:
                    case TargetType.DEFAULT:
                        rend.sprite = rangeSprites[(int)currentRange];
                        break;
                    case TargetType.EMPOWERED:
                        rend.sprite = rangeSprites[(int)RangeTier.CLOSEST];
                        break;
                    case TargetType.ALLY:
                        rend.sprite = allySprite;
                        break;
                }
            }
        }
    }
}
