using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(InputBankTest))]
public class TeslaTrackerComponent : MonoBehaviour {

    public delegate void OnSearchEvent();
    public OnSearchEvent SearchEvent;

    public static float maxTrackingDistance = 50f;
    //public float maxTrackingAngle = 15f;
    public float trackingRadiusZap = 1f;
    public float trackingRadiusDash = 3f;
    public float trackerUpdateFrequency = 16f;

    public HurtBox trackingTargetZap;
    public HurtBox trackingTargetDash;

    private InputBankTest inputBank;

    private float trackerUpdateStopwatch;
    
    void Start() {
        inputBank = base.GetComponent<InputBankTest>();
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

        ZappableTower zappableTower;
        if (trackingTargetZap && trackingTargetZap.hurtBoxGroup.TryGetComponent<ZappableTower>(out zappableTower)) {
            trackingTargetZap = zappableTower.MainHurtbox;
        }

        SearchEvent?.Invoke();
    }

    #region search

    private bool FindTrackingTarget(Ray aimRay) {

        linecasts.Clear();

        bool found = SearchForTargetPoint(aimRay);
        if (!found)
            found = SearchForTargetSphere(aimRay, trackingRadiusZap);

        if (!trackingTargetDash) {
            SearchForDashTarget(aimRay, trackingRadiusDash);
        }

        ShowLineCasts();
        //if(!found) searchfortargetbiggersphereinthedistance(aimray)
        return found;
    }

    private bool SearchForTargetPoint(Ray aimRay) {

        return CharacterRaycast(gameObject, aimRay, out trackingTargetZap, out trackingTargetDash, maxTrackingDistance, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.Ignore);
    }

    private bool SearchForTargetSphere(Ray aimRay, float radius) {

        return CharacterSpherecast(gameObject, aimRay, radius, out trackingTargetZap, out trackingTargetDash, maxTrackingDistance, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.Ignore);
    }

    private bool SearchForDashTarget(Ray aimRay, float radius) {

        return CharacterSpherecast(gameObject, aimRay, radius, out _, out trackingTargetDash, maxTrackingDistance, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.Ignore);
    }

    #endregion search

    #region hurtbox raycast

    // Token: 0x06003E56 RID: 15958 RVA: 0x00101B5C File Offset: 0x000FFD5C
    public static bool CharacterRaycast(GameObject bodyObject, Ray ray, out HurtBox zapHit, out HurtBox dashHit, float maxDistance, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction) {
        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
        return HandleCharacterPhysicsCastResults(bodyObject, ray, queryTriggerInteraction, hits, out zapHit, out dashHit);
    }

    // Token: 0x06003E57 RID: 15959 RVA: 0x00101B84 File Offset: 0x000FFD84
    public static bool CharacterSpherecast(GameObject bodyObject, Ray ray, float radius, out HurtBox zapHit, out HurtBox dashHit, float maxDistance, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction) {
        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, maxDistance, layerMask, queryTriggerInteraction);
        return HandleCharacterPhysicsCastResults(bodyObject, ray, queryTriggerInteraction, hits, out zapHit, out dashHit);
    }

    // Token: 0x06003E55 RID: 15957 RVA: 0x00101AA8 File Offset: 0x000FFCA8
    private static bool HandleCharacterPhysicsCastResults(GameObject bodyObject, Ray ray, QueryTriggerInteraction queryTriggerInteraction, RaycastHit[] hits, out HurtBox zapHit, out HurtBox dashHit) {

        zapHit = null;
        dashHit = null;

        float shortestDashDistance = float.PositiveInfinity;
        float shortestZapDistance = float.PositiveInfinity;

        for (int i = 0; i < hits.Length; i++) {

            HurtBox hurtBox = hits[i].collider.GetComponent<HurtBox>();
            if (hurtBox) {

                bool isTower = hurtBox.hurtBoxGroup.GetComponent<ZappableTower>();

                //cast a line to see if it is interrupted by world
                //however the tesla tower is also world so exclude that
                if (!isTower) {
                    //RaycastHit losHit;
                    bool lineOfSightBlocked = Physics.Linecast(hits[i].point, ray.origin, LayerIndex.world.mask, queryTriggerInteraction);

                    linecasts.Add(new linecast(hits[i].point, ray.origin));

                    if (lineOfSightBlocked) {

                        //linecasts.Add(new linecast(losHit.point, losHit.point + Vector3.up));
                        continue;
                    }
                }
                
                float distance = hits[i].distance;
                if (distance < shortestDashDistance || distance < shortestZapDistance) {
                    
                    HealthComponent healthComponent = hurtBox.healthComponent;
                    if (healthComponent && healthComponent.gameObject == bodyObject) {
                        continue;
                    }

                    if (distance < shortestZapDistance) {
                        zapHit = hurtBox;
                        shortestZapDistance = distance;
                    }

                    if (distance < shortestDashDistance) {
                        if (!hurtBox.healthComponent.body.HasBuff(Modules.Buffs.blinkCooldownBuff)) {
                            dashHit = hurtBox;
                            shortestDashDistance = distance;
                        }
                    }

                } else {
                    continue;
                }
            }
        }

        if (zapHit == null && dashHit == null) {
            return false;
        } else {
            return true;
        }
    }

    static List<linecast> linecasts = new List<linecast>();
    static List<GameObject> linecastShowers = new List<GameObject>();
    private static void ShowLineCasts() {

        for (int i = linecastShowers.Count - 1; i >= 0; i--) {
            if (linecastShowers[i] == null)
                linecastShowers.RemoveAt(i);
        }

        for (int i = 0; i < linecastShowers.Count || i < linecasts.Count; i++) {

            if (i >= linecasts.Count) {
                linecastShowers[i].SetActive(false);
                //Helpers.LogWarning($"hiding linecastShower {i}");
                continue;
            }

            while(i >= linecastShowers.Count) {
                linecastShowers.Add(Instantiate(Modules.Assets.LoadAsset<GameObject>("LineCastShower")));
            }

            linecastShowers[i].SetActive(true);
            linecasts[i].positionShower(linecastShowers[i]);
        }
    }

    #endregion hurtbox raycast
    class linecast {
        public Vector3 start;
        public Vector3 end;

        public linecast(Vector3 start, Vector3 end) {
            this.start = start;
            this.end = end;
        }
        public void positionShower(GameObject shower) {
            shower.transform.position = start;
            shower.transform.LookAt(end);
            shower.transform.localScale = new Vector3(1, 1, (end - start).magnitude);
        }
    }

}
