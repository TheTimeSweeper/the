using RoR2;
using RoR2.UI;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.Components
{
    public class BombSelectSkillIcon : MonoBehaviour
    {
        [SerializeField]
        private SkillIcon skillIcon;
        public SkillIcon SkillIcon { get => skillIcon; set => skillIcon = value; }

        [HideInInspector]
        public PlagueSkillGrid plagueSkillGrid;

        public void HandlePointerDown()
        {
            plagueSkillGrid.OnPointerDown(SkillIcon.targetSkill);
        }
    }
}
