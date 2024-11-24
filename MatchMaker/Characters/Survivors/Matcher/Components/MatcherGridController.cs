using MatcherMod.Modules.UI;
using MatcherMod.Survivors.Matcher.Components.UI;
using MatcherMod.Survivors.Matcher.Content;
using MatcherMod.Survivors.Matcher.SkillDefs;
using Matchmaker.MatchGrid;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace MatcherMod.Survivors.Matcher.Components
{
    public class MatcherGridController : NetworkBehaviour, IHasCompanionUI<MatcherUI>
    {
        public bool allowUIUpdate { get; set; } = true;
        public MatcherUI CompanionUI { get; set; }

        public delegate void MatchGainedEvent(GameObject matcher, int skillIndex, int matchesGained, int tilesMatched);
        public event MatchGainedEvent OnMatchGainedServer;

        [SerializeField]
        public List<GenericSkill> genericSkills = new List<GenericSkill>();

        public Dictionary<SkillDef, int> matchesGainedMap = new Dictionary<SkillDef, int>();

        private SkillDef[] _skillDefs = new SkillDef[4];
        private MatchTileType[] _tileTypes;
        public MatchTileType[] TileTypes => _tileTypes;

        private Queue<Action> _awaitingGridActions = new Queue<Action>();

        private int GetSkillIndex(SkillDef skillDef)
        {
            for (int i = 0; i < _skillDefs.Length; i++)
            {
                if (_skillDefs[i] == skillDef)
                    return i;
            }
            return -1;
        }

        public CharacterBody CharacterBody;

        void Awake()
        {
            CharacterBody = GetComponent<CharacterBody>();
            InstanceTracker.Add(this);

            for (int i = 0; i < genericSkills.Count; i++)
            {
                genericSkills[i].onSkillChanged += genericSkill_onSkillChanged;
            }
        }

        void Start()
        {
            InitSkillDefsAndTileTypes();
        }

        void OnDestroy()
        {
            InstanceTracker.Remove(this);
        }

        void FixedUpdate()
        {
            if (_awaitingGridActions.Count > 0 && CompanionUI != null && CompanionUI.MatchGrid.CanInteract)
            {
                _awaitingGridActions.Dequeue().Invoke();
            }
        }

        private void genericSkill_onSkillChanged(GenericSkill genericSkill)
        {
            if (CompanionUI == null) 
                return;

            if (!matchesGainedMap.ContainsKey(genericSkill.skillDef))
            {
                //ResetMatches();
                InitSkillDefsAndTileTypes(); 
                ReGenerateGrid();
            }
        }

        private void GenerateGrid()
        {
            CompanionUI.Init(_tileTypes, GetSpecialTiles(), GetGridSize());
        }
        private void ReGenerateGrid()
        {
            if (!CompanionUI.Created)
            {
                GenerateGrid();
                return;
            }
            CompanionUI.InitGrid(_tileTypes, GetSpecialTiles(), GetGridSize());
        }

        private SpecialTileInfo[] GetSpecialTiles()
        {
            List<SpecialTileInfo> specialTiles = new List<SpecialTileInfo>();

            for (int i = 0; i < CharacterAssets.SpecialTiles.Count; i++)
            {
                SpecialTileInfo tile = CharacterAssets.SpecialTiles[i];

                if (CharacterBody.inventory.GetItemCount(tile.itemDef) > 0)
                {
                    specialTiles.Add(tile);
                }
            }

            return specialTiles.ToArray();
        }

        private Vector2Int GetGridSize()
        {
            int count = CharacterBody.inventory.GetItemCount(CharacterItems.ExpandTileGrid);

            //start at (5, 4). increase x every other stack, starting at 2, increase y every other stack, starting at 1
            return new Vector2Int(5 + Mathf.FloorToInt((count) / 2), 4 + Mathf.FloorToInt((count + 1) / 2));
        }

        public bool ToggleUI()
        {
            if (CompanionUI == null)
            {
                Log.Error($"No companion ui for {CharacterBody.name}" + Environment.StackTrace);
                return false;
            }

            if (!CompanionUI.Created)
            {
                if (_tileTypes == null)
                {
                    InitSkillDefsAndTileTypes();
                }
                GenerateGrid();
            }
            CompanionUI.Show();
            return CompanionUI.Showing;
        }

        //ew copypaste
        public void ShowUI(Action<float> timeStopAction)
        {
            if (CompanionUI == null)
            {
                Log.Error($"No companion ui for {CharacterBody.name}" + Environment.StackTrace);
                return;
            }

            if (!CompanionUI.Created)
            {
                if (_tileTypes == null)
                {
                    InitSkillDefsAndTileTypes();
                }
                GenerateGrid();
            }
            CompanionUI.Show(true, timeStopAction);
        }

        public void HideUI()
        {
            if (CompanionUI == null || !CompanionUI.Created)
            {
                Log.Error($"No companion ui for {CharacterBody.name}" + Environment.StackTrace);
                return;
            }

            CompanionUI.Show(false);
        }

        public void InitSkillDefsAndTileTypes()
        {
            matchesGainedMap = new Dictionary<SkillDef, int>();

            _skillDefs = new SkillDef[genericSkills.Count];
            for (int i = 0; i < _skillDefs.Length; i++)
            {
                _skillDefs[i] = genericSkills[i].skillDef;
                matchesGainedMap[_skillDefs[i]] = 0;
            }

            _tileTypes = new MatchTileType[_skillDefs.Length];
            for (int i = 0; i < _skillDefs.Length; i++)
            {
                _tileTypes[i] = new MatchTileType(_skillDefs[i]);
            }
        }

        //private void ResetMatches()
        //{
        //    for (int i = 0; i < _skillDefs.Length; i++)
        //    {
        //        matchesGainedMap[_skillDefs[i]] = 0;
        //    }

        //    SyncBuffCounts();
        //}

        //public void SyncBuffCounts()
        //{
        //    for (int i = 0; i < genericSkills.Count; i++)
        //    {
        //        MatchBoostedSkillDef matchSkillDef;
        //        if ((matchSkillDef = genericSkills[0].skillDef as MatchBoostedSkillDef) != null)
        //        {
        //            SyncBuffCount(matchSkillDef);
        //        }
        //    }
        //}

        private void SyncBuffCount(MatchBoostedSkillDef matchSkillDef)
        {
            if (matchSkillDef.associatedBuff == null)
                return;

            if (matchesGainedMap.ContainsKey(matchSkillDef))
            {
                int buffCount = CharacterBody.GetBuffCount(matchSkillDef.associatedBuff);

                int matchCount = matchesGainedMap[matchSkillDef];

                BuffIndex buffIndex = matchSkillDef.associatedBuff.buffIndex;

                SetBuffAmount((int)buffIndex, buffCount, matchCount);
            }
        }

        private void SetBuffAmount(int buffIndex, int buffCount, int matchCount)
        {
            if (NetworkServer.active)
            {
                SetBuffAmountInternal(buffIndex, buffCount, matchCount);
            } else
            {
                CmdSetBuffAmount(buffIndex, buffCount, matchCount);
            }
        }

        [Command]
        private void CmdSetBuffAmount(int buffIndex, int buffCount, int matchCount)
        {
            SetBuffAmountInternal(buffIndex, buffCount, matchCount);
        }

        private void SetBuffAmountInternal(int buffIndex, int buffCount, int matchCount)
        {
            while (buffCount > matchCount)
            {
                buffCount--;
                CharacterBody.RemoveBuff((BuffIndex)buffIndex);
            }
            while (buffCount < matchCount)
            {
                buffCount++;
                CharacterBody.AddBuff((BuffIndex)buffIndex);
            }
        }

        public void OnMatchAwarded(MatchTileType matchType, int matchCount, int tilesMatched)
        {
            MatchBoostedSkillDef matchSkillDef;

            CallMatchGained(gameObject, matchCount, tilesMatched, GetSkillIndex(matchType.skillDef));

            GameObject orbTarget = gameObject;
            if ((matchSkillDef = matchType.skillDef as MatchBoostedSkillDef) != null)
            {
                if (matchSkillDef.respectChangedBuffCount)
                {
                    matchesGainedMap[matchSkillDef] = CharacterBody.GetBuffCount(matchSkillDef.associatedBuff);
                    SyncBuffCount(matchSkillDef);
                }

                orbTarget = matchSkillDef.OnMatchAwarded(this, genericSkills[GetSkillIndex(matchSkillDef)], matchCount);
                if (matchSkillDef.associatedBuff != null)
                {
                    if (!matchesGainedMap.ContainsKey(matchSkillDef))
                    {
                        matchesGainedMap.Add(matchSkillDef, 0);
                    }

                    matchesGainedMap[matchSkillDef] += matchCount;

                    SyncBuffCount(matchSkillDef);
                }
            }
            else
            {
                for (int i = 0; i < genericSkills.Count; i++)
                {
                    if (genericSkills[i].skillDef == matchType.skillDef)
                    {
                        if (i > 0)
                        {
                            matchCount = Mathf.FloorToInt(matchCount / 2);
                        }
                        for (int j = 0; j < matchCount; j++)
                        {
                            genericSkills[i].AddOneStock();
                        }
                    }
                }
            }

            if (orbTarget != null)
            {
                EffectData effectData = new EffectData
                {
                    origin = transform.position,
                    genericFloat = 0.5f,
                    genericUInt = Util.IntToUintPlusOne(SkillCatalog.FindSkillIndexByName(matchType.skillDef.skillName))
                };
                effectData.SetNetworkedObjectReference(orbTarget);
                for (int i = 0; i < matchCount; i++)
                {
                    EffectManager.SpawnEffect(Content.CharacterAssets.SkillTakenOrbEffect, effectData, true);
                }
            }
        }

        private void CallMatchGained(GameObject gameObject, int matchCount, int tilesMatched, int skillIndex)
        {
            if (NetworkServer.active)
            {
                OnMatchGainedServer?.Invoke(gameObject, skillIndex, matchCount, tilesMatched);
            } 
            else
            {
                CmdCallMatchGained(gameObject, matchCount, tilesMatched, skillIndex);
            }
        }

        [Command]
        private void CmdCallMatchGained(GameObject gameObject, int matchCount, int tilesMatched, int skillIndex)
        {
            OnMatchGainedServer?.Invoke(gameObject, skillIndex, matchCount, tilesMatched);
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
            if (matchesGainedMap.ContainsKey(skillDef))
            {
                if (skillDef.respectChangedBuffCount)
                {
                    matchesGainedMap[skillDef] = CharacterBody.GetBuffCount(skillDef.associatedBuff);
                    SyncBuffCount(skillDef);
                }

                if (matchesGainedMap[skillDef] < consumptionMinimum)
                    return 0;

                int consumptions = 0;
                while (matchesGainedMap[skillDef] >= consumptionCost && maxConsumptions > 0)
                {
                    consumptions++;
                    matchesGainedMap[skillDef] -= consumptionCost;
                    maxConsumptions--;
                }

                SyncBuffCount(skillDef);

                return consumptions;
            }
            return 0;
        }

        public void AwardRandomMatchForAI(int matches)
        {                                                                                  //just first 3 skilldefs
            OnMatchAwarded(new MatchTileType(_skillDefs[UnityEngine.Random.Range(0, 4)]), matches, matches * 3);
        }

        [Command]
        public void CmdKeySetInteractableCost(GameObject gameObject, int matches)
        {
            if (gameObject.TryGetComponent(out PurchaseInteraction purchaseInteraction))
            {
                int flatCost = Run.instance.GetDifficultyScaledCost(Mathf.FloorToInt(CharacterConfig.M4_Key_UnlockBaseValue));

                int cost = purchaseInteraction.Networkcost;
                for (int i = 0; i < matches; i++)
                {
                    cost = Mathf.RoundToInt(cost * (100 - 0.99f) * 0.01f - flatCost);
                }
                

                purchaseInteraction.Networkcost = Mathf.Max(0, cost);
            }
        }

        public void OnBoxCompleted()
        {
            RpcQueueRegenerateGrid();
        }
        [ClientRpc]
        private void RpcQueueRegenerateGrid()
        {
            if (CompanionUI != null)
            {
                _awaitingGridActions.Enqueue(ReGenerateGrid);
            }
        }

        public void QueueGridClose()
        {
            if (CompanionUI != null)
            {
                _awaitingGridActions.Enqueue(HideUI);
            }
        }
    }
}