
using UnityEngine;

    [RequireComponent(typeof(Light))]
    public class LightRadiusScale : MonoBehaviour {
        [SerializeField]
        private Light light;

        [SerializeField]
        public float sizeMultiplier;

        void LateUpdate() {
            light.range = transform.lossyScale.x * 2 * sizeMultiplier;
        }
    }
