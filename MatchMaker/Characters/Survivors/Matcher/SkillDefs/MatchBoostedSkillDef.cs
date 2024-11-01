using EntityStates;
using JetBrains.Annotations;
using MatcherMod.Modules.SkillDefs;
using MatcherMod.Survivors.Matcher.Components;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Matchmaker.Survivors.Matcher.SkillDefs
{
    public interface IMatchBoostedState 
    {
        int consumedMatches { get; set; }
    }

    public class MatchBoostedSkillDef : HasComponentSkillDef<MatcherGridController>
    {
        /// <summary>
        /// the amount of matches to consume on skill cast
        /// </summary>
        public int matchConsumptionCost = 1;
        /// <summary>
        /// minimum amount needed to consume. <para>for example, you can consume all charges at 1 consumption cost, but you need a minimum of 3.</para>
        /// </summary>
        public int matchConsumptionMinimum = 1;
        /// <summary>
        /// the amount of times consumptionCost will be used
        /// </summary>
        public int matchMaxConsumptions = 1;

        public BuffDef associatedBuff;
        /// <summary>
        /// Any custom functionality when matches are added. 
        /// </summary>
        public Func<MatcherGridController, int, bool> CustomMatchAction;

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return base.OnAssigned(skillSlot);
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState entityState = base.InstantiateNextState(skillSlot);

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            IMatchBoostedState matchBoostedState;
            if ((matchBoostedState = entityState as IMatchBoostedState) != null)
            {
                matchBoostedState.consumedMatches = (instanceData.componentFromSkillDef1.ConsumeMatches(this, matchConsumptionCost, matchConsumptionMinimum, matchMaxConsumptions));
            }

            return entityState;
        }

        public bool OnMatchAwarded(MatcherGridController matcherGridController, int matches)
        {
            if(CustomMatchAction != null)
            {
                return CustomMatchAction(matcherGridController, matches);
            }
            return true;
        }
    }
}
