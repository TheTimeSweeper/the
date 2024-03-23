using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.General.Components {
    [RequireComponent(typeof(Light))]
    public class LightRadiusScale : MonoBehaviour {
        [SerializeField]
        private Light light;
        private void Reset() {
            light = GetComponent<Light>();
        }

        [SerializeField]
        public float sizeMultiplier;

        void LateUpdate() {
            light.range = transform.lossyScale.x * 2 * sizeMultiplier;
        }
    }
}