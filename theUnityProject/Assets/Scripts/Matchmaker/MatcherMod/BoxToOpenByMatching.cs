using Matchmaker.MatchGrid;
using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Components
{
    public class BoxToOpenByMatching : MonoBehaviour, IHologramContentProvider
    {
        [SerializeField]
        public GameObject hologramPrefab;

        private GameObject cachedObject;
        private BoxToOpenHologramContent cachedHologram;

        private bool _matchesCompleted;

        public void Init(MatcherGridController matcherGridController)
        {
        }

        public bool ShouldDisplayHologram(GameObject viewer)
        {
            return true /*!_matchesCompleted*/;
        }

        public GameObject GetHologramContentPrefab()
        {
            return null;
        }

        public void UpdateHologramContent(GameObject hologramContentObject, Transform viewerBody)
        {
        }
    }
}