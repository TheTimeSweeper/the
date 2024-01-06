using RoR2;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlagueMod.Survivors.Plague.Components {
    
    public class PlagueSkillGrid : MonoBehaviour {

        [SerializeField]
        private Transform anchor;

        [SerializeField]
        private List<BombSelectSkillIcon> skillIcons;

        public void Init(List<GenericSkill> genericSkills) {

            int i = 0;
            for (; i < genericSkills.Count; i++) {

                if (skillIcons.Count <= i) {
                    skillIcons.Add(Instantiate(skillIcons[0], anchor));
                    
                }
                skillIcons[i].gameObject.SetActive(true);
                skillIcons[i].SkillIcon.targetSkill = genericSkills[i];
                skillIcons[i].BombSelectUI = skillIcons[0].BombSelectUI;
            }
            for (; i < skillIcons.Count; i++) {
                skillIcons[i].gameObject.SetActive(false);
            }
        }
    }
}
