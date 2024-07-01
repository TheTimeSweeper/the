
using UnityEngine;

    public class KeepHeadInPositionComponent : MonoBehaviour {

        [SerializeField]
        private Transform reference;

        private void LateUpdate() {
            transform.localEulerAngles = new Vector3(0, reference.localEulerAngles.y, 0);
        }
    }
