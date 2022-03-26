using EntityStates;
using ModdedEntityStates.TeslaTrooper.Tower;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeslaCoilControllerController : MonoBehaviour {

    private List<GameObject> teslaTowers = new List<GameObject>();

    private float nearTowerRange = 60f;

    public GameObject GetNearestTower () { 

            float nearest = nearTowerRange * 2;
            GameObject nearestTower = null;

            for (int i = 0; i < teslaTowers.Count; i++) {
                GameObject coil = teslaTowers[i];
                if (coil == null) {
                    continue;
                }

                float dist = Vector3.Distance(coil.transform.position, transform.position);
                if (dist < nearTowerRange && dist < nearest) {
                    nearest = dist;
                    nearestTower = coil;
                }
            }

            //Helpers.LogWarning($"{nearestCoil != null} | {teslaCoils.Count}");

            return nearestTower;
    }

    public bool coilReady {
        get {
            List<GameObject> towers = GetNearbyTowers();
            for (int i = 0; i < towers.Count; i++) {

                bool skillReady = towers[i].GetComponent<SkillLocator>().FindSkill("Secondary").IsReady();
                if (skillReady)
                    return true;
            }
            return false;
        }
    }

    public void commandPrismTowers(HurtBox target) {

    }

    public void commandTowers(HurtBox target) {

        List<GameObject> nearbyTowers = GetNearbyTowers();

        for (int i = 0; i < nearbyTowers.Count; i++) {
            GameObject coil = nearbyTowers[i];
            if (coil == null) {
                continue;
            }

            //coil.GetComponent<SkillLocator>().secondary.DeductStock(1);
            coil.GetComponent<EntityStateMachine>().SetInterruptState(new TowerBigZap() {
                lightningTarget = target,
            }, InterruptPriority.PrioritySkill);
        }
    }


    public List<GameObject> GetNearbyTowers(GameObject nearObject = null) {

        if (nearObject == null)
            nearObject = gameObject;

        List<GameObject> coils = new List<GameObject>();

        for (int i = 0; i < teslaTowers.Count; i++) {
            GameObject coil = teslaTowers[i];
            if (coil == null) {
                continue;
            }

            float dist = Vector3.Distance(coil.transform.position, nearObject.transform.position);
            if (dist < nearTowerRange) {
                coils.Add(coil);
            }
        }

        return coils;
    }

    public void addTower(GameObject towerObject) {
        teslaTowers.Add(towerObject);

        SkinRecolorController trooperRecolor = this.gameObject.GetComponent<CharacterBody>().modelLocator.modelTransform.GetComponent<SkinRecolorController>();
        if (trooperRecolor) {
                                                            //pass in characterbody instead of gameobject?
            SkinRecolorController towerRecolor = towerObject.GetComponent<CharacterBody>().modelLocator.modelTransform.GetComponent<SkinRecolorController>();
            if (towerRecolor) {
                towerRecolor.SetRecolor(trooperRecolor.currentColor);
            }
        }
    }

    public void removeTower(GameObject towerObject) {
        teslaTowers.Remove(towerObject);
    }

    public void destroyTower() {

        if(teslaTowers[0] != null)
            Destroy(teslaTowers[0]);

        teslaTowers.RemoveAt(0);
    }
}
