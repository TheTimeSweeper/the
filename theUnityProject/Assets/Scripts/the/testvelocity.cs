using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testvelocity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.down * 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
