using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsoluteFilth : MonoBehaviour
{

    [SerializeField]
    private Transform anchor;

    void Start() {
        Invoke("FullofShameAndDisappointment", 1f);
    }


    [ContextMenu("rotten disgusting crimes")]
    private void FullofShameAndDisappointment () {

        if (anchor == null) {

            foreach (var item in FindObjectsOfType<Renderer>()) {
                item.enabled = true;
            }
            Debug.Log("How dare you");
        } else {
            foreach(var item in anchor.GetComponentsInChildren<Renderer>()) {
                item.enabled = true;
            }

            Debug.Log("slightly less but still how dare you");
        }
    }
}
