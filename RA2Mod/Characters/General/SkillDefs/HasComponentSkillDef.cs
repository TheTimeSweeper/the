using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using UnityEngine;

namespace RA2Mod.General.SkillDefs
{
    public interface IHasSkillDefComponent<T>
    {
        T componentFromSkillDef { get; set; }
    }

    public abstract class HasComponentSkillDef<T> : SkillDef where T : MonoBehaviour
    {
        public abstract override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot);

        public class InstanceData : BaseSkillInstanceData 
        {
            public T componentFromSkillDef;
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState entityState = base.InstantiateNextState(skillSlot);

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            IHasSkillDefComponent<T> somethingComponentSkill;
            if ((somethingComponentSkill = entityState as IHasSkillDefComponent<T>) != null)
            {
                somethingComponentSkill.componentFromSkillDef = instanceData.componentFromSkillDef;

            }
            return entityState;
        }
    }
}