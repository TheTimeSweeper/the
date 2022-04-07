using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsoluteFilth : MonoBehaviour
{
    [ContextMenu("rotten disgusting crimes")]
    private void NeverInMyLife () {
        foreach(var item in FindObjectsOfType<SkinnedMeshRenderer>()) {
            item.enabled = true;
        }
        Debug.Log("How dare you");
    }
}
