using RoR2.UI;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.Components {

    public class PlagueBombSelectUI : MonoBehaviour {

        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private CursorOpener cursorOpener;

        [SerializeField]
        private PlagueSkillGrid powderSkills;
        [SerializeField]
        private PlagueSkillGrid casingSkills;
    }
}
