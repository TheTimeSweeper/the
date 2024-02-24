using EntityStates;
using RoR2.Skills;
using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillDefs
{
    public class UpgradableSkillDef : SkillDef
    {
        public SkillDef upgradedSkillDef;
        public GameObject crosshairOverride;
        public SerializableEntityStateType enterBarricadeState;
        //public SerializableEntityStateType exitBarricadeState;
    }
}
