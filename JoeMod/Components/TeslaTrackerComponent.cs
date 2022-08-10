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

    public static float maxTrackingDistance = 50f;
    public static float nearTrackingDistance = 16;
    public static float mediumTrackingDistance = 28f;

    //public float maxTrackingAngle = 15f;
    public float trackingRadius = 1f;
    public float trackerUpdateFrequency = 16f;

    private CharacterBody characterBody;
    private TeamComponent teamComponent;
    private InputBankTest inputBank;
    private TeslaTowerControllerController towerControllerComponent;

    private TeslaIndicator indicator;
    
    private float trackerUpdateStopwatch;

    private HealthComponent _towerTargetHealthComponent;
    private HurtBox _trackingTarget;
    private bool _targetingAlly;
    private bool _hasTowerNear;
    private bool _empowered;
    private bool _isMelee;

    void Awake() {
        indicator = new TeslaIndicator(base.gameObject, Modules.Assets.TeslaIndicatorPrefab);
        teamComponent = GetComponent<TeamComponent>();
        towerControllerComponent = GetComponent<TeslaTowerControllerController>();
    }
    
    void Start() {
        characterBody = base.GetComponent<CharacterBody>();
        inputBank = base.GetComponent<InputBankTest>();
        teamComponent = base.GetComponent<TeamComponent>();

        characterBody.skillLocator.primary.onSkillChanged += Primary_onSkillChanged;

        Primary_onSkillChanged(characterBody.skillLocator.primary);
    }

    private void Primary_onSkillChanged(GenericSkill genericSkill) {

        _isMelee = genericSkill.skillDef.skillNameToken == TeslaTrooperSurvivor.TESLA_PREFIX + "PRIMARY_PUNCH_NAME";
    }

    #region access

    public HurtBox GetTowerTrackingTarget() {
        if (_targetingAlly)
            return null;
        if (!_hasTowerNear)
            return null;

        return _trackingTarget?.hurtBoxGroup.mainHurtBox;
    }

    public HurtBox GetTrackingTarget() {
        return _trackingTarget;
    }

    public bool GetIsTargetingTeammate() {
        return _targetingAlly;
    }

    public RangeTier GetTrackingTargetDistance() {

        RangeTier range = RangeTier.FURTHEST;

        float dist = Vector3.Distance(_trackingTarget.transform.position, transform.position);

        if (dist > mediumTrackingDistance) {
            range = RangeTier.FURTHEST;
        }
        if (dist < mediumTrackingDistance) {
            range = RangeTier.MIDDLE;
        }
        if (dist < nearTrackingDistance) {
            range = RangeTier.CLOSEST;
        }

        return range;
    }

    #endregion access

    #region indicator

    public void SetTowerLockedTarget(HealthComponent healthComponent) {
        _towerTargetHealthComponent = healthComponent;
    }

    public void SetIndicatorEmpowered(bool empowered) {
        indicator.empowered = empowered;
        _empowered = empowered;
    }

    private void setIndicatorRange(RangeTier tier) {
        indicator.currentRange = tier;
    }
    private void setIndicatorAlly() {
        indicator.targetingAlly = _targetingAlly;
    }

    private void setIndicatorTower(bool hasTower) {
        indicator.towerIsTargeting = hasTower;
    }

    private void OnEnable() {
        indicator.active = true;
    }
    private void OnDisable() {
        indicator.active = false;
    }

    #endregion indicator

    private void FixedUpdate() {

        indicator.active = !(_isMelee && !_empowered);

        trackerUpdateStopwatch += Time.fixedDeltaTime;
        if (trackerUpdateStopwatch >= 1f / trackerUpdateFrequency) {
            OnSearch();
        }
    }

    private void OnSearch() {

        trackerUpdateStopwatch -= 1f / trackerUpdateFrequency;
        Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);

        FindTrackingTarget(aimRay);
        setIsTargetingTeammate();
        

        if (_trackingTarget) {
            setIndicatorRange(GetTrackingTargetDistance());
        }

        ZappableTower zappableTower;
        if (_trackingTarget && _trackingTarget.hurtBoxGroup.TryGetComponent<ZappableTower>(out zappableTower)) {
            _trackingTarget = zappableTower.MainHurtbox;
        }

        indicator.targetTransform = (_trackingTarget ? _trackingTarget.transform : null);
        
        if(Modules.Config.TowerTargeting.Value)
            setIsTowerTargeting();
    }
    
    private void setIsTowerTargeting() {

        bool hasTarget = _towerTargetHealthComponent && _trackingTarget && _towerTargetHealthComponent == _trackingTarget.healthComponent;

        if (_towerTargetHealthComponent) {
            setIndicatorTower(hasTarget);
            return;
        }

        _hasTowerNear = towerControllerComponent.GetNearestTower();
        setIndicatorTower(_hasTowerNear);
    }

    private void setIsTargetingTeammate() {
        bool targetingFriendlyFire = false;
        if (_trackingTarget) {
            targetingFriendlyFire = !FriendlyFireManager.ShouldDirectHitProceed(_trackingTarget.healthComponent, teamComponent.teamIndex);// _trackingTarget.teamIndex == teamComponent.teamIndex;
        }
        
        _targetingAlly = targetingFriendlyFire;
        setIndicatorAlly();
    }

    #region search

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
        public bool towerIsTargeting;

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

                indicatorView.SetColor((int)currentTarget);

                //sprite
                switch (currentTarget) {

                    default:
                    case TargetType.DEFAULT:
                        indicatorView.SetSpriteRange((int)currentRange);
                        break;
                    case TargetType.EMPOWERED:
                        indicatorView.SetSpriteTower();
                        break;
                    case TargetType.ALLY:
                        indicatorView.SetSpriteAlly();
                        break;
                }

                //tower indicator
                indicatorView.SetTowerSprite(!targetingAlly && towerIsTargeting);
            }
        }
    }
}