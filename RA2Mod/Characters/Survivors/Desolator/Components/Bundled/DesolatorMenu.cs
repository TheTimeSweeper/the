using UnityEngine;

namespace RA2Mod.Survivors.Desolator.Components {
    public class DesolatorMenu : RA2Mod.General.Components.MenuSoundComponent {

        [SerializeField]
        private GameObject lightPrefab;

        [SerializeField]
        private Transform deployPoint;

        private void Start() {
            GameObject thing = Instantiate(lightPrefab, deployPoint, false);
            thing.transform.localPosition = Vector3.zero;
            thing.transform.parent = null;
        }
    }
}
