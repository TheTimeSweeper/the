using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Text;
using UnityEngine;

namespace RA2Mod.General.SkillDefs
{

    public abstract class PlayAnimationWhenReadySkillDef : SkillDef
    {
        public string animatorBoolParameterName;

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData { animator = skillSlot.characterBody.modelLocator.modelTransform.GetComponent<Animator>()};
        }

        public class InstanceData : BaseSkillInstanceData
        {
            public Animator animator;
        }

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime)
        {
            if (!skillSlot.CanExecute())
            {
                ((InstanceData)skillSlot.skillInstanceData).animator.SetBool(animatorBoolParameterName, false);
            }
        }
    }

    public interface IHasSkillDefComponent<T>
    {
        T componentFromSkillDef1 { get; set; }
    }

    public abstract class HasComponentSkillDef<T> : SkillDef
    {
        public class InstanceData : BaseSkillInstanceData
        {
            public T componentFromSkillDef1;
        }

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                componentFromSkillDef1 = skillSlot.GetComponent<T>(),
            };
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState entityState = base.InstantiateNextState(skillSlot);

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            IHasSkillDefComponent<T> hasComopnentState;
            if ((hasComopnentState = entityState as IHasSkillDefComponent<T>) != null)
            {
                hasComopnentState.componentFromSkillDef1 = instanceData.componentFromSkillDef1;

            }
            return entityState;
        }
    }

    //the first time it was neat. this time it's kinda silly
    //idk if there's something in c# that I'm completely missing here that could generalize this for any number of components, kinda how action and func do
    public interface IHasSkillDefComponent<T, U>
    {
        T componentFromSkillDef1 { get; set; }
        U componentFromSkillDef2 { get; set; }
    }

    public abstract class HasComponentSkillDef<T, U> : SkillDef
    {
        public class InstanceData : BaseSkillInstanceData
        {
            public T componentFromSkillDef1;
            public U componentFromSkillDef2;
        }

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                componentFromSkillDef1 = skillSlot.GetComponent<T>(),
                componentFromSkillDef2 = skillSlot.GetComponent<U>()
            };
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState entityState = base.InstantiateNextState(skillSlot);

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            IHasSkillDefComponent<T, U> hasComponentState;
            if ((hasComponentState = entityState as IHasSkillDefComponent<T, U>) != null)
            {
                hasComponentState.componentFromSkillDef1 = instanceData.componentFromSkillDef1;
                hasComponentState.componentFromSkillDef2 = instanceData.componentFromSkillDef2;
            }
            return entityState;
        }
    }

    public interface IHasSkillDefComponent<T, U, V>
    {
        T componentFromSkillDef1 { get; set; }
        U componentFromSkillDef2 { get; set; }
        V componentFromSkillDef3 { get; set; }
    }

    public abstract class HasComponentSkillDef<T, U, V> : SkillDef
    {
        public class InstanceData : BaseSkillInstanceData
        {
            public T componentFromSkillDef1;
            public U componentFromSkillDef2;
            public V componentFromSkillDef3;
        }

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                componentFromSkillDef1 = skillSlot.GetComponent<T>(),
                componentFromSkillDef2 = skillSlot.GetComponent<U>(),
                componentFromSkillDef3 = skillSlot.GetComponent<V>()
            };
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState entityState = base.InstantiateNextState(skillSlot);

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            IHasSkillDefComponent<T, U, V> hasComponentState;
            if ((hasComponentState = entityState as IHasSkillDefComponent<T, U, V>) != null)
            {
                hasComponentState.componentFromSkillDef1 = instanceData.componentFromSkillDef1;
                hasComponentState.componentFromSkillDef2 = instanceData.componentFromSkillDef2;
                hasComponentState.componentFromSkillDef3 = instanceData.componentFromSkillDef3;
            }
            return entityState;
        }
    }
}