using RoR2.Skills;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.SkillDefs
{
    public class PlagueBombPowderSkillDef : PlagueBombSelectionSkillDef
    {
        public EntityStates.SerializableEntityStateType flyingState;
        public GameObject impactProjectilePrefab;
        public Material projectileMaterial;
    }
}
