using MatcherMod.Survivors.Matcher.Content;
using Matchmaker.MatchGrid;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.LowLevel;

namespace MatcherMod.Survivors.Matcher.Components
{
    public class BoxToOpenByMatching : NetworkBehaviour, IHologramContentProvider
    {
        private GameObject cachedObject;
        private BoxToOpenHologramContent cachedHologram;

        [SerializeField]
        public GameObject hologramPrefab;

        private int[] _tileMatchAmountsRemaining;

        private bool _amountsDirty;
        private bool _matchesCompleted;

        private MatchTileType[] tileTypesToShow;

        List<MatcherGridController> subscribedMatchers = new List<MatcherGridController>();

        private float _checkMatchersInterval = 0.2f;

        public void Init(MatchTileType[] tileTypes, int[] matchAmounts)
        {
            tileTypesToShow = tileTypes;

            _tileMatchAmountsRemaining = new int[tileTypes.Length];
            int i = 0;
            for (; i < matchAmounts.Length; i++)
            {
                _tileMatchAmountsRemaining[i] = matchAmounts[i];
            }
            _amountsDirty = true;
            
            CheckMatchers();
        }

        void FixedUpdate()
        {
            _checkMatchersInterval -= Time.deltaTime;

            if(_checkMatchersInterval < 0)
            {
                CheckMatchers();
            }
        }

        private void CheckMatchers()
        {
            List<MatcherGridController> matchers = InstanceTracker.GetInstancesList<MatcherGridController>();
            for (int i = 0; i < matchers.Count; i++)
            {
                if (!subscribedMatchers.Contains(matchers[i]))
                {
                    subscribedMatchers.Add(matchers[i]);
                    matchers[i].OnMatchGained += ModifyAmount;
                }
            }

            for (int i = subscribedMatchers.Count- 1; i >= 0; i--)
            {
                if(subscribedMatchers[i] == null)
                {
                    subscribedMatchers.RemoveAt(i);
                }
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < subscribedMatchers.Count; i++)
            {
                if (subscribedMatchers[i] != null)
                {
                    subscribedMatchers[i].OnMatchGained -= ModifyAmount;
                }
            }
        }

        public void ModifyAmount(MatcherGridController matcher, MatchTileType type, int skillIndex, int matchesGained, int tilesMatched)
        {
            if((matcher.transform.position - transform.position).magnitude > 15)
                return;

            _tileMatchAmountsRemaining[skillIndex] -= tilesMatched;
            _amountsDirty = true;
            CheckCompleted();
        }

        private bool CheckCompleted()
        {
            if (_matchesCompleted)
                return true;

            for (int i = 0; i < _tileMatchAmountsRemaining.Length; i++)
            {
                if (_tileMatchAmountsRemaining[i] > 0)
                    return false;
            }
            _matchesCompleted = true;
            OnComplete();
            return true;
        }

        private void OnComplete()
        {
            for (int i = 0; i < subscribedMatchers.Count; i++)
            {
                CharacterMaster master = subscribedMatchers[i].CharacterBody.master;
                int completedCount = master.inventory.GetItemCount(CharacterItems.GridUpgradedCount);

                if (completedCount > 6)
                    continue;

                ItemDef itemDef = null;
                switch (completedCount)
                {
                    case 0:
                        itemDef = GetRandomSpecialTile(master.inventory);
                        break;
                    case 1:
                        itemDef = CharacterItems.AddTile2X;
                        break;
                    case 2:
                        itemDef = CharacterItems.ExpandTileGrid;
                        break;
                    case 3:
                        itemDef = GetRandomSpecialTile(master.inventory);
                        break;
                    case 4:
                        itemDef = CharacterItems.AddTile3X;
                        break;
                    case 5:
                        itemDef = CharacterItems.ExpandTileGrid;
                        break;
                    case 6:
                        itemDef = GetRandomSpecialTile(master.inventory);
                        break;
                }

                master.inventory.GiveItem(itemDef);
                GenericPickupController.SendPickupMessage(master, PickupCatalog.FindPickupIndex(itemDef.itemIndex));

                master.inventory.GiveItem(CharacterItems.GridUpgradedCount);

                subscribedMatchers[i].OnBoxCompleted();
            }

            //todo cool breaking animation
            Destroy(gameObject);
        }

        //currently bomb, scroll, wild
        private ItemDef GetRandomSpecialTile(Inventory inventory)
        {
            List<ItemDef> tiles = new List<ItemDef>();

            AddIfMissing(inventory, tiles, CharacterItems.AddTileWild);
            AddIfMissing(inventory, tiles, CharacterItems.AddTileBomb);
            AddIfMissing(inventory, tiles, CharacterItems.AddTileScroll);

            if (tiles.Count == 0)
                return null;

            return tiles[UnityEngine.Random.Range(0, tiles.Count)];
        }

        private static void AddIfMissing(Inventory inventory, List<ItemDef> tiles, ItemDef tileToAdd)
        {
            if (inventory.GetItemCount(tileToAdd) <= 0)
            {
                tiles.Add(tileToAdd);
            }
        }

        public bool ShouldDisplayHologram(GameObject viewer)
        {
            return true /*!_matchesCompleted*/;
        }

        public GameObject GetHologramContentPrefab()
        {
            return hologramPrefab;
        }

        public void UpdateHologramContent(GameObject hologramContentObject, Transform viewerBody)
        {
            if (cachedHologram == null || hologramContentObject != cachedObject)
            {
                cachedObject = hologramContentObject;
                cachedHologram = hologramContentObject.GetComponent<BoxToOpenHologramContent>();

                cachedHologram.Init(tileTypesToShow, _tileMatchAmountsRemaining);
            }

            if (cachedHologram != null && _amountsDirty)
            {
                _amountsDirty = false;

                for (int i = 0; i < _tileMatchAmountsRemaining.Length; i++)
                {
                    cachedHologram.UpdateAmount(i, _tileMatchAmountsRemaining[i]);
                }
            }
        }
    }
}