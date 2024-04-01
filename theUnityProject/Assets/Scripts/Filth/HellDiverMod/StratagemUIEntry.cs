using RoR2.UI;
using UnityEngine;

namespace HellDiverMod.Survivors.HellDiver.Components.UI {
    public class StratagemUIEntry: MonoBehaviour {
        [SerializeField]
        public Transform skillAnchor;
        [SerializeField]
        private StratagemUIEntryInput[] inputs;
    }
}
