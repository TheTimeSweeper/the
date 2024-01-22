using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components {
    public class ChronosphereProjection : MonoBehaviour {

        [SerializeField]
        private ObjectScaleCurve sphere;

        [SerializeField]
        public Renderer sphereRenderer;

        public void SetRadiusAndEnable(float radius) {
            sphere.transform.localScale = Vector3.one * radius * 2;
            sphere.enabled = true;
        }
    }
}