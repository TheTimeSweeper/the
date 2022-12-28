using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerReparenter : MonoBehaviour
{
    [SerializeField]
    private Transform hammer;

    [SerializeField]
    private Transform hand;
    [SerializeField]
    private Transform hip;

    public void Reparent(int parent) {

        hammer.SetParent(parent == 0 ? hand : hip);
        hammer.localPosition = Vector3.zero;
        hammer.localRotation = Quaternion.identity;
    }
}
