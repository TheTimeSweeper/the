using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.Components {
    public class ChronosphereProjection : NetworkBehaviour {

        [SerializeField]
        private ObjectScaleCurve sphere;

        [SerializeField]
        public Renderer[] sphereRenderers;

        [SerializeField]
        private LoopSoundDef loopSoundDef;

        public void SetRadiusAndEnable(float radius) {
            sphere.transform.localScale = Vector3.one * radius * 2;
            sphere.enabled = true;
        }
    }
}