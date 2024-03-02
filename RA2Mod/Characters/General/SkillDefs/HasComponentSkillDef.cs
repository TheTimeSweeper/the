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
            Log.Warning("nip1");
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
            Log.Warning("nip2");
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
            Log.Warning($"skillslot onassigned spawner " + (skillSlot.GetComponent<RA2Mod.Survivors.Chrono.Components.ChronoSprintProjectionSpawner>() != null));
            /*return*/var instanceData = new InstanceData
            {
                componentFromSkillDef1 = skillSlot.GetComponent<T>(),
                componentFromSkillDef2 = skillSlot.GetComponent<U>(),
                componentFromSkillDef3 = skillSlot.GetComponent<V>()
            };
            Log.Warning($"skillslot onassigned instanceData spawner " + (instanceData.componentFromSkillDef2 != null));
            return instanceData;
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState entityState = base.InstantiateNextState(skillSlot);

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            IHasSkillDefComponent<T, U, V> hasComponentState;
            Log.Warning("nip3 this is only running on authority?");
            if ((hasComponentState = entityState as IHasSkillDefComponent<T, U, V>) != null)
            {
                hasComponentState.componentFromSkillDef1 = instanceData.componentFromSkillDef1;
                hasComponentState.componentFromSkillDef2 = instanceData.componentFromSkillDef2;
                Log.Warning($"fucking has component2 instanceData {instanceData.componentFromSkillDef2 != null} hasComponentState {hasComponentState.componentFromSkillDef2 != null}");
                hasComponentState.componentFromSkillDef3 = instanceData.componentFromSkillDef3;
            }
            return entityState;
        }
    }
}