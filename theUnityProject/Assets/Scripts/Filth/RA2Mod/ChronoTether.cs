using UnityEngine;

namespace RA2Mod.Survivors.Chrono.Components {
    public class ChronoTether : MonoBehaviour {

        [SerializeField]
        private Transform tetherPoint;

        [SerializeField]
        private Renderer rend;

        public void SetTetherPoint(Vector3 position) {
            tetherPoint.position = position;
        }
    }
}