using RoR2;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.Components {
    
    public abstract class PlagueSkillGrid : MonoBehaviour {

        [SerializeField]
        protected PlagueBombSelectUI bombSelectUI;

        [SerializeField]
        private Transform anchor;

        [SerializeField]
        public List<BombSelectSkillIcon> skillIcons;

        public abstract void OnPointerDown(GenericSkill targetSkill);
    }
}
