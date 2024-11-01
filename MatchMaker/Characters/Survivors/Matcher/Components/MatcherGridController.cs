using MatcherMod.Modules.UI;
using MatcherMod.Survivors.Matcher.Components.UI;
using Matchmaker.MatchGrid;
using Matchmaker.Survivors.Matcher.SkillDefs;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Components
{
    public class MatcherGridController : MonoBehaviour, IHasCompanionUI<MatcherUI>
    {
        public bool allowUIUpdate { get; set; } = true;
        public MatcherUI CompanionUI { get; set; }

        [SerializeField]
        public List<GenericSkill> genericSkills = new List<GenericSkill>();

        public Dictionary<SkillDef, int> matchesGained = new Dictionary<SkillDef, int>();
        private CharacterBody characterBody;

        void Awake()
        {
            characterBody = GetComponent<CharacterBody>();
        }

        public SkillDef[] GetSkillDefs()
        {
            SkillDef[] defs = new SkillDef[genericSkills.Count];
            for (int i = 0; i < defs.Length; i++)
            {
                defs[i] = genericSkills[i].skillDef;
            }
            return defs;
        }

        public void SyncBuffCounts()
        {
            for (int i = 0; i < genericSkills.Count; i++)
            {
                MatchBoostedSkillDef matchSkillDef;
                if ((matchSkillDef = genericSkills[0].skillDef as MatchBoostedSkillDef) != null)
                {
                    SyncBuffCount(matchSkillDef);
                }
            }
        }

        private void SyncBuffCount(MatchBoostedSkillDef matchSkillDef)
        {
            if (matchesGained.ContainsKey(matchSkillDef))
            {
                int buffCount = characterBody.GetBuffCount(matchSkillDef.associatedBuff);
                while(buffCount > matchesGained[matchSkillDef])
                {
                    buffCount--;
                    characterBody.RemoveBuff(matchSkillDef.associatedBuff);
                }
                while (buffCount < matchesGained[matchSkillDef])
                {
                    buffCount++;
                    characterBody.AddBuff(matchSkillDef.associatedBuff);
                }
            }
        }

        public void OnMatchAwarded(int matchCount, MatchTileType matchType)
        {
            MatchBoostedSkillDef matchSkillDef;
            if((matchSkillDef = matchType.skillDef as MatchBoostedSkillDef) != null)
            {
                bool addToMatches = matchSkillDef.OnMatchAwarded(this, matchCount);
                if (addToMatches)
                {
                    if (!matchesGained.ContainsKey(matchSkillDef))
                    {
                        matchesGained.Add(matchSkillDef, 0);
                    }
                    matchesGained[matchSkillDef] += matchCount;
                    SyncBuffCount(matchSkillDef);
                }
            }
            else
            {
                for (int i = 0; i < genericSkills.Count; i++)
                {
                    if (genericSkills[i].skillDef == matchType.skillDef)
                    {
                        for (int j = 0; j < matchCount; j++)
                        {
                            genericSkills[i].AddOneStock();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// consume the amount of matches for this skilldef.
        /// </summary>
        /// <param name="skillDef">the skilldef matches to consume</param>
        /// <param name="consumptionCost">the amount to consume.</param>
        /// <param name="consumptionMinimum">minimum amount needed to consume. <para>for example, you can consume all charges at 1 consumption cost, but you need a minimum of 3.</para></param>
        /// <param name="maxConsumptions">the amount of times consumptionCost will be used.</param>
        /// <returns>the amount of successful consumptions. for example, if you had 6 matches and consumption cost was 3, result would be 2 consumptions</returns>
        public int ConsumeMatches(MatchBoostedSkillDef skillDef, int consumptionCost = 1, int consumptionMinimum = 1, int maxConsumptions = 1)
        {
            if (matchesGained.ContainsKey(skillDef))
            {
                if (matchesGained[skillDef] < consumptionMinimum)
                    return 0;

                int consumptions = 0;
                while (matchesGained[skillDef] >= consumptionCost && maxConsumptions > 0)
                {
                    consumptions++;
                    matchesGained[skillDef] -= consumptionCost;
                    maxConsumptions--;
                }

                SyncBuffCount(skillDef);

                return consumptions;
            }
            return 0;
        }
    }
}