using RA2Mod.Survivors.Tesla;
using RoR2;
using System;
using UnityEngine;

[RequireComponent(typeof(CharacterBody))]
[RequireComponent(typeof(TeslaTrackerComponent))]
[RequireComponent(typeof(TeamComponent))]
public class TeslaTrackerComponentDash : MonoBehaviour {

    private TeslaDashIndicator indicator;

    private TeslaTrackerComponent teslaTrackerComponent;
    private CharacterBody characterBody;
    private TeamComponent teamComponent;

    private HurtBox _trackingTarget;
    private bool _targetingAlly;

    private bool _isDashing;
    private bool _isReady;

    void Awake() {
        indicator = new TeslaDashIndicator(base.gameObject, TeslaAssets.TeslaIndicatorPrefabDash);
    }

    void Start() {
        teslaTrackerComponent = GetComponent<TeslaTrackerComponent>();
        characterBody = base.GetComponent<CharacterBody>();
        //inputBank = base.GetComponent<InputBankTest>();
        teamComponent = base.GetComponent<TeamComponent>();

        teslaTrackerComponent.SearchEvent += OnSearch;

        characterBody.skillLocator.utility.onSkillChanged += Utility_onSkillChanged;

        Utility_onSkillChanged(characterBody.skillLocator.utility);
    }

    void OnDestroy() {

        teslaTrackerComponent.SearchEvent -= OnSearch;
    }

    private void Utility_onSkillChanged(GenericSkill genericSkill) {

        _isDashing = genericSkill.skillDef.skillNameToken == TeslaTrooperSurvivor.TOKEN_PREFIX + "UTILITY_BLINK_NAME";
    }

    private void FixedUpdate() {

        indicator.active = _isDashing && _isReady;
    }

    #region access

    public HurtBox GetTrackingTarget() {
        return _trackingTarget;
    }

    public bool GetIsTargetingTeammate() {
        return _targetingAlly;
    }

    #endregion access

    public void SetIsReady(bool ready) {
        _isReady = ready;
    }

    private void OnSearch() {

        _trackingTarget = teslaTrackerComponent.trackingTargetDash;

        setIsTargetingTeammate();

        indicator.targetTransform = (_trackingTarget ? _trackingTarget.transform : null);
    }

    private void setIsTargetingTeammate() {
        bool targetingFriendlyFire = false;
        if (_trackingTarget) {
            targetingFriendlyFire = !FriendlyFireManager.ShouldDirectHitProceed(_trackingTarget.healthComponent, teamComponent.teamIndex);// _trackingTarget.teamIndex == teamComponent.teamIndex;
        }

        _targetingAlly = targetingFriendlyFire;
    }


    public class TeslaDashIndicator : Indicator {

        public TeslaDashIndicator(GameObject owner, GameObject visualizerPrefab) : base(owner, visualizerPrefab) { }

        //public override void UpdateVisualizer() {
        //    base.UpdateVisualizer();

        //    if (visualizerTransform) {

        //        TeslaIndicatorView indicatorView = visualizerTransform.GetComponent<TeslaIndicatorView>();

        //        //color
        //        TargetType currentTarget = TargetType.DEFAULT;

        //        if (empowered) {
        //            currentTarget = TargetType.EMPOWERED;
        //        } else if (targetingAlly) {
        //            currentTarget = TargetType.ALLY;
        //        }

        //        indicatorView.SetColor((int)currentTarget);

        //        //sprite
        //        switch (currentTarget) {

        //            default:
        //            case TargetType.DEFAULT:
        //                indicatorView.SetSpriteRange((int)currentRange);
        //                break;
        //            case TargetType.EMPOWERED:
        //                indicatorView.SetSpriteTower();
        //                break;
        //            case TargetType.ALLY:
        //                indicatorView.SetSpriteAlly();
        //                break;
        //        }

        //        //tower indicator
        //        indicatorView.SetTowerSprite(!targetingAlly && towerIsTargeting);
        //    }
        //}
    }
}
