using KinematicCharacterController;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KatamariMod.Characters.Survivors.Katamari.Components
{
    class RollUp : MonoBehaviour
    {
        private Vector3 lastposition;
        private float radius;

        void Awake()
        {
            lastposition = transform.position;
            radius = (transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) / 3;
        }

        void Update()
        {
            if(transform.position != lastposition)
            {
                float distance = (lastposition - transform.position).magnitude;
                float theta = Mathf.Atan(distance / radius)* 57.2958f;
                transform.Rotate(Vector3.Cross(Vector3.up, transform.position - lastposition), theta, Space.World);

                lastposition = transform.position;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            //if (other.gameObject.layer == RoR2.LayerIndex.defaultLayer.intVal)
            //{
            
            if (other.GetComponent<Renderer>())
            {
                Parent(other.transform);
                return;
            }

            Renderer renderer = other.GetComponentInParent<Renderer>();
            if (renderer != null)
            {
                Parent(renderer.transform);
                return;
            }

            if (other.transform.parent != null)
            {
                if (!other.transform.parent.name.Contains("HOLDER"))
                {
                    if (other.transform.parent.GetComponentInChildren<Renderer>() != null)
                    {
                        Parent(other.transform.parent);
                        return;
                    }
                }
            }

            Parent(other.transform);
            
            //}
        }

        void Parent(Transform tran)
        {
            TryDisableAll<CharacterMotor>(tran);
            TryDisableAll<KinematicCharacterMotor>(tran);
            TryDisableAll<Collider>(tran);
            TryDisableAll<Rigidbody>(tran);
            TryDisableAll<CharacterDirection>(tran);
            //TryDisable(tran.GetComponentInParent<Animator>());

            tran.parent = transform;
        }

        void TryDisableAll<T>(Transform tran) where T : Component
        {
            TryDisable(tran.GetComponentInParent<T>());
            TryDisable(tran.GetComponentsInChildren<T>());
        }

        void TryDisable<T>(T[] componentsinchildren) where T : Component
        {
            for (int i = 0; i < componentsinchildren.Length; i++)
            {
                TryDisable(componentsinchildren[i]);
            }
        }

        void TryDisable(Component thing)
        {
            if (thing != null)
            {
                if (thing is Behaviour) (thing as Behaviour).enabled = false;
                if (thing is Collider) (thing as Collider).enabled = false;
                if (thing is Rigidbody) (thing as Rigidbody).isKinematic = true;
            }
        }
    }
}
