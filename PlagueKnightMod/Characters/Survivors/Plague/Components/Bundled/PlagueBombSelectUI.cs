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
        private PlagueSkillGrid powderSkills;
        [SerializeField]
        private PlagueSkillGrid casingSkills;

        internal void UpdateGrids(List<GenericSkill> powderGenericSkills, List<GenericSkill> casingGenericSkills)
        {
            powderSkills.UpdateGrid(powderGenericSkills);
            casingSkills.UpdateGrid(casingGenericSkills);
        }
    }
}
