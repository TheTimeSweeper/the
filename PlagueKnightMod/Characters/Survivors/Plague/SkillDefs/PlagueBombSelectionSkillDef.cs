using EntityStates;
using JetBrains.Annotations;
using PlagueMod.Survivors.Plague.Components;
using RoR2;
using RoR2.Skills;
using System;

namespace PlagueMod.Survivors.Plague.SkillDefs
{
    public class PlagueBombSelectionSkillDef : SkillDef
    {
        public interface IPlagueBombSelector
        {
            PlagueBombSelectorController selectorComponent { get; set; }
        }

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                selectorComponent = skillSlot.GetComponent<PlagueBombSelectorController>()
            };
        }

        public class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public PlagueBombSelectorController selectorComponent;
        }

        //damn anyone that hooks skilldef.InstantiateNextState is gonna be forked
        //well if you're hooking that there's probably a problem no?
        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            SerializableEntityStateType overrideState = GetOverrideState(skillSlot);
            EntityState entityState = EntityStateCatalog.InstantiateState(ref overrideState);
            ISkillState skillState;
            if ((skillState = (entityState as ISkillState)) != null)
            {
                skillState.activatorSkillSlot = skillSlot;
            }
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            IPlagueBombSelector throwSkill;
            if ((throwSkill = (entityState as IPlagueBombSelector)) != null)
            {
                throwSkill.selectorComponent = instanceData.selectorComponent;
            }
            return entityState;
        }

        public virtual SerializableEntityStateType GetOverrideState(GenericSkill skillSlot)
        {
            return this.activationState;
        }
    }
}
