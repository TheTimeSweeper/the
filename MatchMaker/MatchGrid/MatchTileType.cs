using RoR2.Skills;
using UnityEngine;

namespace Matchmaker.MatchGrid
{
    public class MatchTileType
    {
        public SkillDef skillDef;

        public MatchTileType(SkillDef skillDef)
        {
            this.skillDef = skillDef;
        }

        internal Color GetColor()
        {
            return Color.white;
        }

        internal Sprite GetIcon()
        {
            return skillDef.icon;
        }
    }
}