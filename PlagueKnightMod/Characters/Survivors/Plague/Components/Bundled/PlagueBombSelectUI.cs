using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.Components
{
    public class PlagueBombSelectUI : MonoBehaviour
    {
        [HideInInspector]
        public PlagueBombSelectorController plagueBombSelectorController;
        
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private CursorOpener cursorOpener;

        [SerializeField]
        private PlagueSkillGrid powderSkills;
        [SerializeField]
        private PlagueSkillGrid casingSkills;

        public void UpdateGrids(List<GenericSkill> powderGenericSkills, List<GenericSkill> casingGenericSkills)
        {
            powderSkills.UpdateGrid(powderGenericSkills);
            casingSkills.UpdateGrid(casingGenericSkills);
        }

        public void Show(bool shouldShow = true)
        {
            canvas.enabled = shouldShow;
            cursorOpener.enabled = shouldShow;
        }
    }
}
