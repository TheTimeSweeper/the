using EntityStates;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.SkillDefs
{
    public class PlagueBombPowderSkillDef : PlagueBombSelectionSkillDef
    {
        public EntityStates.SerializableEntityStateType flyingState;
        public GameObject impactProjectilePrefab;
        public Material projectileMaterial;
        public float projectileFuseTime;
        public bool projectileImpactEnemy;
        public bool projectileImpactWorld;

        public override SerializableEntityStateType GetOverrideState([NotNull] GenericSkill skillSlot)
        {
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            
            return instanceData.selectorComponent.casingLoadoutSkill.activationState;
        }
    }
}
