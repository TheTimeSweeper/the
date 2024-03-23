using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components {
    [ExecuteAlways]
    public class MidPointBetweenManyTransforms : MonoBehaviour {
        [SerializeField]
        private Transform[] transforms;

        private float inverseLength;

        void Awake() {
        }

        void Update() {

            inverseLength = 1f / (transforms.Length - 1);

            for (int i = 1; i < transforms.Length - 1; i++) {
                transforms[i].position = Vector3.Lerp(this.transforms[0].position, this.transforms[transforms.Length - 1].position, i * inverseLength);
            }
        }
    }
}