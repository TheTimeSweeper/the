using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.Components {
    public class ChronosphereProjection : NetworkBehaviour {

        [SerializeField]
        private ObjectScaleCurve sphere;

        [SerializeField]
        public Renderer[] sphereRenderer;

        public void SetRadiusAndEnable(float radius) {
            sphere.transform.localScale = Vector3.one * radius * 2;
            sphere.enabled = true;
        }
    }
}