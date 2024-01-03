using RoR2.Skills;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.SkillDefs
{
    public class PlagueBombPowderSkillDef : SkillDef
    {
        public EntityStates.SerializableEntityStateType flyingState;
        public GameObject impactProjectilePrefab;
        public Material projectileMaterial;
    }
}
