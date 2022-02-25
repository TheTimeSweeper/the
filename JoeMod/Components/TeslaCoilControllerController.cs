using EntityStates;
using JoeMod.ModdedEntityStates.TeslaTrooper.Tower;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeslaCoilControllerController : MonoBehaviour {

    private List<GameObject> teslaTowers = new List<GameObject>();

    private float nearTowerRange = 60f;

    public List<GameObject> nearbyTowers {
        get {

            List<GameObject> coils = new List<GameObject>();

            for (int i = 0; i < teslaTowers.Count; i++) {
                GameObject coil = teslaTowers[i];
                if (coil == null) {
                    Helpers.LogWarning("uh");
                    continue;
                }

                float dist = Vector3.Distance(coil.transform.position, transform.position);
                if (dist < nearTowerRange) {
                    coils.Add(coil);
                }
            }

            return coils;
        }
    }

    public GameObject nearestCoil {

        get {
            float nearest = nearTowerRange * 2;
            GameObject nearestTower = null;

            for (int i = 0; i < teslaTowers.Count; i++) {
                GameObject coil = teslaTowers[i];
                if (coil == null) {
                    Helpers.LogWarning("uh command");
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
    }

    internal void commandTowers(HurtBox target) {
        for (int i = 0; i < nearbyTowers.Count; i++) {
            GameObject coil = nearbyTowers[i];
            if (coil == null) {
                Helpers.LogWarning("uh command");
                continue;
            }
            coil.GetComponent<EntityStateMachine>().SetInterruptState(new TowerBigZap() {
                lightningTarget = target,
            }, InterruptPriority.PrioritySkill);
        }
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
