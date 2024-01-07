using RoR2.UI;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.Components
{
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
}
