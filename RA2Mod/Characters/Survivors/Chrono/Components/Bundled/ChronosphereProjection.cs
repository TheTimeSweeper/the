using RoR2;
using UnityEngine;
namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronosphereProjection : MonoBehaviour
    {
        [SerializeField]
        public Renderer sphereRenderer;

        [SerializeField]
        private ObjectScaleCurve sphere;

        public void SetRadiusAndEnable(float radius)
        {
            sphere.transform.localScale = Vector3.one * radius * 2;
            sphere.enabled = true;
        }
    }
}