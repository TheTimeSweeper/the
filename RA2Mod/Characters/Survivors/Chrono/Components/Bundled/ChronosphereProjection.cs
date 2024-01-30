using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronosphereProjection : NetworkBehaviour
    {
        [SerializeField]
        public Renderer sphereRenderer;

        [SerializeField]
        private ObjectScaleCurve sphere;

        public void SetRadiusAndEnable(float radius)
        {
            sphere.transform.localScale = Vector3.one * radius;
            sphere.enabled = true;
        }
    }
}