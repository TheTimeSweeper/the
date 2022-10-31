﻿
using UnityEngine;

public class DesolatorMenu : MonoBehaviour {

    [SerializeField]
    private GameObject lightPrefab;

    [SerializeField]
    private Transform deployPoint;

    private void Start() {
        GameObject thing = Instantiate(lightPrefab, deployPoint, true);
        thing.transform.parent = null;

        RoR2.Util.PlaySound("Play_Desolator_Deploy", gameObject);
    }
}