using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootlegCopyLocation : MonoBehaviour
{
    [SerializeField]
    private Transform boneToFollow;

    [SerializeField]
    private Vector3 offset;
    private void LateUpdate() {
        transform.position = boneToFollow.position + offset;
    }

    private void OnValidate() {
        if(boneToFollow != null) {
            offset = transform.position - boneToFollow.position;
        }
    }
}
