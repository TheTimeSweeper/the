using RoR2;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterBody))]
[RequireComponent(typeof(InputBankTest))]
[RequireComponent(typeof(TeamComponent))]
public class TotallyOriginalTrackerComponent : MonoBehaviour
{
    public float maxTrackingDistance = 35f;
    //public float maxTrackingAngle = 15f;
    public float trackingRadius = 1.5f;
    public float trackerUpdateFrequency = 16f;

    private float trackerUpdateStopwatch;
    private HurtBox trackingTarget;
    private CharacterBody characterBody;
    private TeamComponent teamComponent;
    private InputBankTest inputBank;
    private TeslaIndicator indicator;

    // Token: 0x04000FAB RID: 4011
    //private readonly BullseyeSearch search = new BullseyeSearch();

    public void setIndicatorEmpowered(bool empowered) {
        indicator.empowered = true;
    }

    void Awake()
    {
        indicator = new TeslaIndicator(base.gameObject, RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/LightningIndicator"));
    }

    void Start()
    {
        characterBody = base.GetComponent<CharacterBody>();
        inputBank = base.GetComponent<InputBankTest>();
        teamComponent = base.GetComponent<TeamComponent>();
    }

    public HurtBox GetTrackingTarget()
    {
        return trackingTarget;
    }

    public float GetTrackingTargetDistance() {
        return Vector3.Distance(trackingTarget.transform.position, transform.position);
    }

    private void OnEnable()
    {
        indicator.active = true;
    }
    private void OnDisable()
    {
        indicator.active = false;
    }

    private void FixedUpdate()
    {
        trackerUpdateStopwatch += Time.fixedDeltaTime;
        if (trackerUpdateStopwatch >= 1f / trackerUpdateFrequency)
        {
            HurtBox hurtBox = trackingTarget;

            trackerUpdateStopwatch -= 1f / trackerUpdateFrequency;
            Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);

            SearchForTarget(aimRay);

            indicator.targetTransform = (trackingTarget ? trackingTarget.transform : null);
        }
    }

    private bool SearchForTarget(Ray aimRay)
    {
        bool found = SearchForTargetPoint(aimRay);
        if (!found) 
            found = SearchForTargetSphere(aimRay);
        //if(!found) searchfortargetbiggersphereinthedistance(aimray)
        return found;
    }

    private bool SearchForTargetPoint(Ray aimRay)
    {
        RaycastHit hitinfo;
        Util.CharacterRaycast(gameObject, aimRay, out hitinfo, maxTrackingDistance, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.UseGlobal);

        trackingTarget = hitinfo.collider?.GetComponent<HurtBox>();

        return trackingTarget;
    }

    private bool  SearchForTargetSphere(Ray aimRay)
    {
        RaycastHit hitinfo;
        Util.CharacterSpherecast(gameObject, aimRay, trackingRadius, out hitinfo, maxTrackingDistance, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.UseGlobal);

        trackingTarget = hitinfo.collider?.GetComponent<HurtBox>();
        return trackingTarget;
    }

    //wasn't being used anyway
    //private void SearchForTargetBullet(Ray aimRay)
    //{
    //    //well I already reinvented the wheel a bit so I guess I don't need this anymore
    //        //also didn't work for some reason :c
    //    BulletAttack bulletAttack = new BulletAttack
    //    {
    //        aimVector = aimRay.direction,
    //        origin = aimRay.origin,
    //        owner = base.gameObject,
    //        weapon = null,
    //        bulletCount = 1,
    //        damage = 0,
    //        damageColorIndex = DamageColorIndex.Default,
    //        damageType = DamageType.Generic,
    //        //falloffModel = BulletAttack.FalloffModel.Buckshot,
    //        force = 0,
    //        HitEffectNormal = false,
    //        procChainMask = default(ProcChainMask),
    //        procCoefficient = 0,
    //        maxDistance = maxTrackingDistance,
    //        radius = trackingRadius,
    //        isCrit = false,
    //        //muzzleName = muzzleName,
    //        //minSpread = minSpread,
    //        //maxSpread = maxSpread + num,
    //        hitEffectPrefab = null,// hitEffectPrefab,
    //        smartCollision = true,
    //        sniper = false,
    //        //spreadPitchScale = spreadPitchScale,
    //        //spreadYawScale = spreadYawScale,
    //        //tracerEffectPrefab = tracerEffectPrefab
    //    };

    //    bulletAttack.hitCallback = delegate (ref BulletAttack.BulletHit info)
    //    {
    //        bool hitBullet = bulletAttack.DefaultHitCallback(ref info);
    //        if (hitBullet)
    //        {
    //            trackingTarget = info.hitHurtBox;
    //            //HealthComponent healthComponent = info.hitHurtBox ? info.hitHurtBox.healthComponent : null;
    //            //if (healthComponent && healthComponent.alive && info.hitHurtBox.teamIndex != teamIndex)
    //            //{
    //            //    hitBullet = false;
    //            //}
    //        }
    //        return hitBullet;
    //    };
    //    bulletAttack.filterCallback = delegate (ref BulletAttack.BulletHit info)
    //    {
    //        return (!info.entityObject || info.entityObject != bulletAttack.owner) && bulletAttack.DefaultFilterCallback(ref info);
    //    };

    //    bulletAttack.Fire();
    //}

    public class TeslaIndicator : Indicator {
        public bool empowered;

        public TeslaIndicator(GameObject owner, GameObject visualizerPrefab) : base(owner, visualizerPrefab) { }

        //public override void UpdateVisualizer() {
        //    base.UpdateVisualizer();

        //    if (visualizerTransform) {
        //        visualizerTransform.GetComponentInChildren<Image>().color = empowered ? Color.red : Color.white;
        //    }
        //}
    }
}
