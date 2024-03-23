using UnityEngine;

namespace RA2Mod.Survivors.Desolator.Components {
    public class DesolatorMenu : MonoBehaviour {

        [SerializeField]
        private GameObject lightPrefab;

        [SerializeField]
        private Transform deployPoint;

        private void Start() {
            GameObject thing = Instantiate(lightPrefab, deployPoint, true);
            thing.transform.parent = null;
        }
    }
}