using UnityEngine;
using UnityEngine.Events;

namespace AliemMod.Components.Bundled
{
    public class ParticleEmissionScaler : MonoBehaviour {

        [SerializeField]
        private float time;

        [SerializeField]
        private float maxEmission;
        [SerializeField]
        private float minEmission;

        [SerializeField]
        private ParticleSystem particleSystem;

        private float _tim;

        private ParticleSystem.EmissionModule _emission;

        void Awake() {
            _emission = particleSystem.emission;
        }

        void Update() {

            _tim += Time.deltaTime;
            _emission.rateOverTime = Mathf.Lerp(minEmission, maxEmission, time / _tim);
        }
    }
}
