using UnityEngine;

//DRY:
//the best way to do this is have one unattacheanimator that can handle multiple characters via fields
//but time
public class UnattachedAnimatorTower : MonoBehaviour {

    [SerializeField]
    private Animator[] towinators;

    void Update() {
        if (towinators.Length == 0)
            return;

        Shooting();
    }

    private void Shooting() {
        if (Input.GetMouseButtonDown(0)) {
            for (int i = 0; i < towinators.Length; i++) {

                towinators[i].Play("PrepZap");
            }
        }
    }
}
