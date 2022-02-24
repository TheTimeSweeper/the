using EntityStates;
using JoeMod.ModdedEntityStates.TeslaTrooper.Tower;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeslaCoilControllerController : MonoBehaviour {

    private List<GameObject> teslaCoils = new List<GameObject>();

    private float nearCoilRange = 60f;

    public List<GameObject> nearbyCoils {
        get {

            List<GameObject> coils = new List<GameObject>();

            for (int i = 0; i < teslaCoils.Count; i++) {
                GameObject coil = teslaCoils[i];
                if (coil == null) {
                    Helpers.LogWarning("uh");
                    continue;
                }

                float dist = Vector3.Distance(coil.transform.position, transform.position);
                if (dist < nearCoilRange) {
                    coils.Add(coil);
                }
            }

            return coils;
        }
    }

    public GameObject nearestCoil {

        get {
            float nearest = nearCoilRange * 2;
            GameObject nearestCoil = null;

            for (int i = 0; i < teslaCoils.Count; i++) {
                GameObject coil = teslaCoils[i];
                if (coil == null) {
                    Helpers.LogWarning("uh command");
                    continue;
                }

                float dist = Vector3.Distance(coil.transform.position, transform.position);
                if (dist < nearCoilRange && dist < nearest) {
                    nearest = dist;
                    nearestCoil = coil;
                }
            }

            //Helpers.LogWarning($"{nearestCoil != null} | {teslaCoils.Count}");

            return nearestCoil;
        }
    }

    internal void commandCoils(HurtBox target) {
        for (int i = 0; i < nearbyCoils.Count; i++) {
            GameObject coil = nearbyCoils[i];
            if (coil == null) {
                Helpers.LogWarning("uh command");
                continue;
            }
            coil.GetComponent<EntityStateMachine>().SetInterruptState(new TowerBigZap() {
                lightningTarget = target,
            }, InterruptPriority.PrioritySkill);
        }
    }

    public void addCoil(GameObject gameObject) {
        teslaCoils.Add(gameObject);
    }

    public void removeCoil(GameObject gameObject) {
        teslaCoils.Remove(gameObject);
    }

    public void destroyCoil() {

        if(teslaCoils[0] != null)
            Destroy(teslaCoils[0]);

        teslaCoils.RemoveAt(0);
    }
}
