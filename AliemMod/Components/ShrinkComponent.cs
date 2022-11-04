using UnityEngine;

namespace AliemMod.Components {
    internal class ShrinkComponent : MonoBehaviour {

        void LateUpdate() {
            transform.localScale = Vector3.zero;
        }
    }
}