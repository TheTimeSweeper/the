using RoR2.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrower : MonoBehaviour
{

    [System.Serializable]
    public struct ProjectileThrow {
        public KeyCode throwKey;
        public GameObject prefab;
    }


    [SerializeField]
    private ProjectileThrow[] projectiles;

    void Update()
    {
        for (int i = 0; i < projectiles.Length; i++) {

            if (Input.GetKeyDown(projectiles[i].throwKey)) {

                GameObject proj = Instantiate(projectiles[i].prefab, transform.position, transform.rotation, transform);
                if(proj.TryGetComponent(out ProjectileSimple projectileSimple)) {
                    if(proj.TryGetComponent(out Rigidbody rib)) {
                        rib.velocity = transform.forward * projectileSimple.desiredForwardSpeed;
                    }
                }
            }
        }
    }
}
