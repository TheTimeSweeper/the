using Matchmaker.MatchGrid;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

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

        public void Init(MatchTileType[] tileTypes, int[] matchAmounts)
        {
            tileTypesToShow = tileTypes;

            _tileMatchAmountsRemaining = new int[tileTypes.Length];
            int i = 0;
            for (; i < matchAmounts.Length; i++)
            {
                _tileMatchAmountsRemaining[i] = matchAmounts[i];
            }
            for (; i < tileTypesToShow.Length; i++)
            {
                _tileMatchAmountsRemaining[i] = 0;
            }
            _amountsDirty = true;
            
            CheckMatchers();
        }

        void OnDestroy()
        {
            for (int i = 0; i < subscribedMatchers.Count; i++)
            {
                subscribedMatchers[i].OnMatchGained -= ModifyAmount;
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
        }

        public void ModifyAmount(MatchTileType type, int skillIndex, int matchesGained, int tilesMatched)
        {
            _tileMatchAmountsRemaining[skillIndex] -= tilesMatched;
            _amountsDirty = true;
            CheckCompleted();
        }

        private bool CheckCompleted()
        {
            for (int i = 0; i < _tileMatchAmountsRemaining.Length; i++)
            {
                if (_tileMatchAmountsRemaining[i] > 0)
                    return false;
            }
            _matchesCompleted = true;
            return true;
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