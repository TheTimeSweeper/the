
using UnityEngine;

public class KeepHeadInPositionComponent : MonoBehaviour {
    private void LateUpdate() {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}
