using EntityStates;
using JetBrains.Annotations;
using MatcherMod;
using MatcherMod.Modules.SkillDefs;
using MatcherMod.Survivors.Matcher.Components;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.SkillDefs
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
        /// <summary>
        /// set true if buffcount changes as not simply a display of match count 
        /// </summary>
        public bool respectChangedBuffCount = false;

        public BuffDef associatedBuff;
        /// <summary>
        /// Any custom functionality when matches are added. 
        /// </summary>
        public Func<MatcherGridController, GenericSkill, int, GameObject> CustomMatchAction = DefaultMatchAction;

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState entityState = base.InstantiateNextState(skillSlot);

            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            IMatchBoostedState matchBoostedState;
            if ((matchBoostedState = entityState as IMatchBoostedState) != null)
            {
                matchBoostedState.consumedMatches = instanceData.componentFromSkillDef1.ConsumeMatches(this, matchConsumptionCost, matchConsumptionMinimum, matchMaxConsumptions);
            }

            return entityState;
        }

        public GameObject OnMatchAwarded(MatcherGridController matcherGridController, GenericSkill genericSkill, int matches)
        {
            if (CustomMatchAction != null)
            {
                return CustomMatchAction(matcherGridController, genericSkill, matches);
            }
            return matcherGridController.gameObject;
        }

        public static GameObject DefaultMatchAction(MatcherGridController controller, GenericSkill genericSkill, int matches)
        {
            return controller.gameObject;
        }
    }
}