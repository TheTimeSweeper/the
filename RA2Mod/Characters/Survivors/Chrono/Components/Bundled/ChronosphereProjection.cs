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

        [ClientRpc]
        public void RpcSetRadiusAndEnable(float radius)
        {
            sphere.transform.localScale = Vector3.one * radius * 2;
            sphere.enabled = true;
        }
    }
}