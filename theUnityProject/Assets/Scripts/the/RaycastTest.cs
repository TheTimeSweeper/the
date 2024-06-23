using System.Collections;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    [SerializeField]
    private int numbo = 1000;

    [SerializeField]
    private float waitforseconds = 1;

    [SerializeField]
    private bool tesuto;

    void Update()
    {
        if (tesuto)
        {
            tesuto = false;
            StartCoroutine(DoThing());
        }
    }

    private IEnumerator DoThing()
    {
        for (int i = 0; i < numbo; i++) {
            RaycastHit[] nip = Physics.RaycastAll(transform.position, transform.forward, 10);
        }
        Debug.LogWarning("did a thingo");
        yield return new WaitForSeconds(2);

        for (int i = 0; i < numbo; i++)
        {
            RaycastHit[] nip = Physics.SphereCastAll(transform.position, 0.3f, transform.forward, 10);
        }
        Debug.LogWarning("did a thingo2");
    }
}