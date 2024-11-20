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
        public const int MAX_UPGRADES = 8;

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

        private bool _initialized;

        public void InitServer(GameObject matcher, int[] matchAmounts)
        {
            RpcInit(matcher, matchAmounts);
        }

        [ClientRpc]
        private void RpcInit(GameObject matcher, int[] matchAmounts)
        {
            MatchTileType[] tileTypes = matcher.GetComponent<MatcherGridController>().TileTypes;

            tileTypesToShow = tileTypes;

            _tileMatchAmountsRemaining = matchAmounts;

            _amountsDirty = true;
            _initialized = true;

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
                    matchers[i].OnMatchGainedServer += ModifyAmountServer;
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
                    subscribedMatchers[i].OnMatchGainedServer -= ModifyAmountServer;
                }
            }
        }
        
        public void ModifyAmountServer(GameObject matcher, int skillIndex, int matchesGained, int tilesMatched)
        {
            if ((matcher.transform.position - transform.position).magnitude > 15)
                return;

            RpcModifyAmount(skillIndex, tilesMatched);
        }

        [ClientRpc]
        private void RpcModifyAmount(int skillIndex, int tilesMatched)
        {
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
            if (NetworkServer.active)
            {
                CompleteBoxServer();

                //todo cool breaking animation
                NetworkServer.Destroy(gameObject);
            }

            return true;
        }

        private void CompleteBoxServer()
        {
            for (int i = 0; i < subscribedMatchers.Count; i++)
            {
                CharacterMaster master = subscribedMatchers[i].CharacterBody.master;

                int completedCount = master.inventory.GetItemCount(CharacterItems.GridUpgradedCount);

                if (completedCount >= MAX_UPGRADES)
                    continue;

                ItemDef itemDef = null;
                switch (completedCount)
                {
                    case 0: //enter stage 2
                        itemDef = GetRandomSpecialTile(master.inventory, CharacterItems.AddTileWild, CharacterItems.AddTileBomb);
                        break;
                    case 1: //enter stage 3
                        itemDef = CharacterItems.AddTile2X;
                        break;
                    case 2: //enter stage 4
                        itemDef = GetRandomSpecialTile(master.inventory, CharacterItems.AddTileScroll, CharacterItems.AddTileTimeStop);
                        break;
                    case 3: //enter stage 5
                        itemDef = CharacterItems.ExpandTileGrid;
                        break;
                    case 4: //enter stage 6
                        itemDef = GetRandomSpecialTile(master.inventory, CharacterItems.AddTileWild, CharacterItems.AddTileBomb);
                        break;
                    case 5: //enter stage 7
                        itemDef = CharacterItems.AddTile3X;
                        break;
                    case 6: //enter stage 8
                        itemDef = GetRandomSpecialTile(master.inventory, CharacterItems.AddTileScroll, CharacterItems.AddTileTimeStop);
                        break;
                    case 7: //enter stage 9
                        itemDef = CharacterItems.ExpandTileGrid;
                        break;
                }

                master.inventory.GiveItem(itemDef);
                GenericPickupController.SendPickupMessage(master, PickupCatalog.FindPickupIndex(itemDef.itemIndex));

                master.inventory.GiveItem(CharacterItems.GridUpgradedCount);

                subscribedMatchers[i].OnBoxCompleted();
            }
        }

        //currently bomb, scroll, wild
        private ItemDef GetRandomSpecialTile(Inventory inventory, params ItemDef[] itemsToRandomize)
        {
            List<ItemDef> tiles = new List<ItemDef>();

            for (int i = 0; i < itemsToRandomize.Length; i++)
            {
                AddIfMissing(inventory, tiles, itemsToRandomize[i]);
            }

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
            if (_initialized && (cachedHologram == null || hologramContentObject != cachedObject))
            {
                cachedObject = hologramContentObject;
                cachedHologram = hologramContentObject.GetComponent<BoxToOpenHologramContent>();

                cachedHologram.Init(tileTypesToShow, _tileMatchAmountsRemaining);
            }

            if (_amountsDirty && cachedHologram != null)
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