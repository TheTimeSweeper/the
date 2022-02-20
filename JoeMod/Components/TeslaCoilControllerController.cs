using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeslaCoilControllerController : MonoBehaviour {

    private List<GameObject> teslaCoils = new List<GameObject>();

    private float nearCoilRange = 60f;

    //todo coil character deployable
    private int maxCoils = 1;

    public GameObject nearestCoil {

        get {
            float nearest = nearCoilRange * 2;
            GameObject nearestCoil = null;

            for (int i = 0; i < teslaCoils.Count; i++) {
                GameObject coil = teslaCoils[i];

                float dist = Vector3.Distance(coil.transform.position, transform.position);
                if (dist < nearCoilRange && dist < nearest) {
                    nearest = dist;
                    nearestCoil = coil;
                }
            }

            return nearestCoil;
        }
    }

    public void addCoil(GameObject gameObject) {
        teslaCoils.Add(gameObject);

        while (teslaCoils.Count > maxCoils) {
            destroyCoil();
        }
    }

    public void destroyCoil() {

        if(teslaCoils[0] != null)
            Destroy(teslaCoils[0]);

        teslaCoils.RemoveAt(0);
    }
}
