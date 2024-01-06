using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.Components
{

    public class PlagueBombSelectUI : MonoBehaviour
    {
        [SerializeField]
        private PlagueSkillGrid powderSkills;
        [SerializeField]
        private PlagueSkillGrid casingSkills;
    }

    public class PlagueSkillGrid : MonoBehaviour
    {
        [SerializeField]
        private Transform anchor;

        [SerializeField]
        private List<BombSelectSkillIcon> skillIcons;

        public void Init(List<GenericSkill> genericSkills)
        {
            int i = 0;
            for (; i < genericSkills.Count; i++)
            {
                if (skillIcons.Count <= i)
                {
                    skillIcons.Add(Instantiate(skillIcons[0], anchor));
                }
                skillIcons[i].gameObject.SetActive(true);
                skillIcons[i].SkillIcon.targetSkill = genericSkills[i];
                skillIcons[i].BombSelectUI = skillIcons[0].BombSelectUI;
            }
            for (; i < skillIcons.Count; i++)
            {
                skillIcons[i].gameObject.SetActive(false);
            }
        }
    }

    public class BombSelectSkillIcon : MonoBehaviour
    {
        [SerializeField]
        private SkillIcon skillIcon;
        public SkillIcon SkillIcon { get => skillIcon; set => skillIcon = value; }

        [SerializeField]
        private PlagueBombSelectUI bombSelectUI;
        public PlagueBombSelectUI BombSelectUI { get => bombSelectUI; set => bombSelectUI = value; }

        public void HandlePointerDown(SkillIcon skillIcon)
        {

        }
    }

    public class PlagueBombColorizer : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] renderers;

        internal void Colorize(Material material)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].sharedMaterial = material;
            }
        }
    }
}
