using RA2Mod.Survivors.Tesla;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterBody))]
[RequireComponent(typeof(TeslaTrackerComponent))]
[RequireComponent(typeof(TeamComponent))]
public class TeslaTrackerComponentZap : MonoBehaviour {
    
    public static float nearTrackingDistance = 16;
    public static float mediumTrackingDistance = 28f;

    //public static float maxTrackingDistance = 50f;
    ////public float maxTrackingAngle = 15f;
    //public float trackingRadius = 1f;
    //public float trackerUpdateFrequency = 16f;

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

    private TeslaTrackerComponent teslaTrackerComponent;
    private CharacterBody characterBody;
    private TeamComponent teamComponent;
    //private InputBankTest inputBank;
    private TeslaTowerControllerController towerControllerComponent;

    private TeslaZapIndicator indicator;
    
    //private float trackerUpdateStopwatch;
    private HurtBox _trackingTarget;

    private HealthComponent _towerTargetHealthComponent;
    private bool _targetingAlly;
    private bool _hasTowerNear;
    private bool _empowered;
    private bool _isMelee;

    void Awake() {
        indicator = new TeslaZapIndicator(base.gameObject, TeslaAssets.TeslaIndicatorPrefab);
        towerControllerComponent = GetComponent<TeslaTowerControllerController>();
    }
    
    void Start() {
        teslaTrackerComponent = GetComponent<TeslaTrackerComponent>();
        characterBody = base.GetComponent<CharacterBody>();
        //inputBank = base.GetComponent<InputBankTest>();
        teamComponent = base.GetComponent<TeamComponent>();

        teslaTrackerComponent.SearchEvent += OnSearch;

        characterBody.skillLocator.primary.onSkillChanged += Primary_onSkillChanged;

        Primary_onSkillChanged(characterBody.skillLocator.primary);
    }

    void OnDestroy() {

        teslaTrackerComponent.SearchEvent -= OnSearch;
    }
        
    private void Primary_onSkillChanged(GenericSkill genericSkill) {

        _isMelee = genericSkill.skillDef.skillNameToken == TeslaTrooperSurvivor.TOKEN_PREFIX + "PRIMARY_PUNCH_NAME";
    }

    #region access

    public HurtBox GetTowerTrackingTarget() {
        if (_targetingAlly)
            return null;
        if (!_hasTowerNear)
            return null;
        if (_trackingTarget == null)
            return null;
        if (_trackingTarget.hurtBoxGroup == null)
            return null;

        return _trackingTarget.hurtBoxGroup.mainHurtBox;
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

        //trackerUpdateStopwatch += Time.fixedDeltaTime;
        //if (trackerUpdateStopwatch >= 1f / trackerUpdateFrequency) {
        //    OnSearch();
        //}
    }

    private void OnSearch() {

        //trackerUpdateStopwatch -= 1f / trackerUpdateFrequency;
        //Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);

        //FindTrackingTarget(aimRay);
        _trackingTarget = teslaTrackerComponent.trackingTargetZap;

        setIsTargetingTeammate();
        
        if (_trackingTarget) {
            setIndicatorRange(GetTrackingTargetDistance());
        }

        //ZappableTower zappableTower;
        //if (_trackingTarget && _trackingTarget.hurtBoxGroup.TryGetComponent<ZappableTower>(out zappableTower)) {
        //    _trackingTarget = zappableTower.MainHurtbox;
        //}

        indicator.targetTransform = (_trackingTarget ? _trackingTarget.transform : null);
        
        if(TeslaConfig.M4_Tower_Targeting.Value)
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

    public class TeslaZapIndicator : Indicator {

        public RangeTier currentRange = RangeTier.FURTHEST;

        public bool empowered;
        public bool targetingAlly;
        public bool towerIsTargeting;

        public TeslaZapIndicator(GameObject owner, GameObject visualizerPrefab) : base(owner, visualizerPrefab) { }

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
                indicatorView.SetTowerIndicator(!targetingAlly && towerIsTargeting);
            }
        }
    }
}