using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class testo: MonoBehaviour {

    void Awake() {
        var nip = new TimeSpan(0, 0, 0, 10, 40);
        Debug.LogWarning(nip.TotalSeconds.ToString("0.000"));

    }
}
