using UnityEngine;

public class QuaternionAngleTest : MonoBehaviour {


    [SerializeField]
    private int turns;

    [SerializeField]
    private float angle = 10;

    private void Start() {
        setDiraction();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.G)) {

            turns++;
            setDiraction();
        }
    }

    private void setDiraction() {
        Vector3 dir = Vector3.forward;
        dir = Quaternion.Euler(0, turns * angle, 0) * dir;

        transform.rotation = RoR2.Util.QuaternionSafeLookRotation(dir);
    }
}
