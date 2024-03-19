using EntityStates;
using RA2Mod.General.Components;
using RA2Mod.Minions.TeslaTower.States;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeslaTowerControllerController : MonoBehaviour {

    public static float NearTowerRange = 60f;

    private List<GameObject> teslaTowers = new List<GameObject>();
    private List<GameObject> otherCommandables = new List<GameObject>();

    public GameObject GetNearestTower () { 

            float nearest = NearTowerRange * 2;
            GameObject nearestTower = null;

            for (int i = 0; i < teslaTowers.Count; i++) {
                GameObject coil = teslaTowers[i];
                if (coil == null) {
                    continue;
                }

                float dist = Vector3.Distance(coil.transform.position, transform.position);
                if (dist < NearTowerRange && dist < nearest) {
                    nearest = dist;
                    nearestTower = coil;
                }
            }

            //Helpers.LogWarning($"{nearestCoil != null} | {teslaCoils.Count}");

            return nearestTower;
    }

    //scrapped
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

        if (!NetworkServer.active)
            return;

        List<GameObject> nearbyTowers = GetNearbyTowers();

        for (int i = 0; i < nearbyTowers.Count; i++) {
            GameObject coil = nearbyTowers[i];
            if (coil == null) {
                continue;
            }

            coil.GetComponent<EntityStateMachine>().SetInterruptState(new TowerBigZap() {
                lightningTarget = target,
            }, InterruptPriority.PrioritySkill);
        }
        
        for (int i = 0; i < otherCommandables.Count; i++) {
            otherCommandables[i].GetComponent<EntityStateMachine>().SetInterruptState(new TowerZap() {
                lightningTarget = target,
                zaps = 2
            }, InterruptPriority.PrioritySkill);
        }
    }

    //so DRY it's dehydrated
    public void commandTowersGauntlet(HurtBox target) {

        if (!NetworkServer.active)
            return;

        List<GameObject> nearbyTowers = GetNearbyTowers();

        for (int i = 0; i < nearbyTowers.Count; i++) {
            GameObject coil = nearbyTowers[i];
            if (coil == null)
                continue;

            EntityStateMachine machine = EntityStateMachine.FindByCustomName(coil, "Weapon");
            if (machine == null)
                continue;

            machine.SetInterruptState(new TowerBigZapGauntlet() {
                lightningTarget = target,
            }, InterruptPriority.PrioritySkill);
        }
    }

    public int NearbyTowers() {
        return GetNearbyTowers().Count;
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
            if (dist < NearTowerRange) {
                coils.Add(coil);
            }
        }

        return coils;
    }

    public void addTower(GameObject towerBodyObject) {
        teslaTowers.Add(towerBodyObject);

        towerBodyObject.GetComponent<TowerOwnerTrackerComponent>().OwnerTrackerComponent = GetComponent<TeslaTrackerComponentZap>();

        SkinRecolorController trooperRecolor = this.gameObject.GetComponent<CharacterBody>().modelLocator.modelTransform.GetComponent<SkinRecolorController>();
        if (trooperRecolor && trooperRecolor.Recolors != null) {
                                                            //pass in characterbody instead of gameobject?
            SkinRecolorController towerRecolor = towerBodyObject.GetComponent<CharacterBody>().modelLocator.modelTransform.GetComponent<SkinRecolorController>();
            if (towerRecolor) {
                towerRecolor.SetRecolor(trooperRecolor.currentColor);
            }
        }
    }

    public void removeTower(GameObject towerObject) {
        teslaTowers.Remove(towerObject);
    }

    public void addNotTower(GameObject notTowerBodyObject) {
        otherCommandables.Add(notTowerBodyObject);
    }
    public void removeNotTower(GameObject notTowerBodyObject) {
        otherCommandables.Remove(notTowerBodyObject);
    }
}
