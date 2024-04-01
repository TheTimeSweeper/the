using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.Components
{
    public abstract class PlagueSkillGrid : MonoBehaviour
    {
        [SerializeField]
        protected PlagueBombSelectUI bombSelectUI;

        [SerializeField]
        private Transform anchor;

        [SerializeField]
        public List<BombSelectSkillIcon> skillIcons;

        public abstract void OnPointerDown(GenericSkill targetSkill);

        public void UpdateGrid(List<GenericSkill> genericSkills)
        {
            if(genericSkills.Count != skillIcons.Count)
            {
                SyncSkillIcons(genericSkills);
            }

            for (int i = 0; i < skillIcons.Count; i++)
            {
                skillIcons[i].SkillIcon.targetSkill = genericSkills[i];
            }
        }

        private void SyncSkillIcons(List<GenericSkill> genericSkills)
        {
            int i = 0;
            for (; i < genericSkills.Count; i++)
            {
                if (skillIcons.Count <= i)
                {
                    skillIcons.Add(Instantiate(PlagueAssets.skillIconPrefab, anchor));
                }
                skillIcons[i].gameObject.SetActive(true);
                skillIcons[i].plagueSkillGrid = this;
            }
            for (; i < skillIcons.Count; i++)
            {
                skillIcons[i].gameObject.SetActive(false);
            }
        }
    }
}
