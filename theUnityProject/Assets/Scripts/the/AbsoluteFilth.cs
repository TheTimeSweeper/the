using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsoluteFilth : MonoBehaviour
{
    void Start() {
        Invoke("FullofShameAndDisappointment", 1f);
    }


    [ContextMenu("rotten disgusting crimes")]
    private void FullofShameAndDisappointment () {
        foreach(var item in FindObjectsOfType<Renderer>()) {
            item.enabled = true;
        }
        Debug.Log("How dare you");
    }
}
