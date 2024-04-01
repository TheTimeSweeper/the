using UnityEngine;
using UnityEngine.UI;

namespace HellDiverMod.Survivors.HellDiver.Components.UI {
    public class StratagemUIEntryInput : MonoBehaviour {
        [SerializeField]
        private Image image;
        [SerializeField]
        private Sprite[] inputSprites;

        private void Reset() {
            image = GetComponent<Image>();
        }
    }
}
