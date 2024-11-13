using Matchmaker.MatchGrid;
using RoR2;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MatcherMod.Survivors.Matcher.Components
{
    public class BoxToOpenHologramContent : MonoBehaviour
    {
        //let's try dropping a lot of OO principles to save on the amount of components

        [SerializeField]
        private GameObject[] tileCostObjects;
        [SerializeField]
        private SpriteRenderer[] tileCostSprites;
        [SerializeField]
        private TMP_Text[] tileCostTexts;
        public TMP_Text[] TileCostTexts => tileCostTexts;

        private float[] costAmounts = new float[4];
        private float[] displayedCostAmounts = new float[4];

        public void Init(MatchTileType[] tileTypes, int[] costs)
        {
            int i = 0;
            for (; i < costs.Length; i++)
            {
                if(costs[i] <= 0)
                {
                    tileCostObjects[i].SetActive(false);
                    costAmounts[i] = 0;
                    continue;
                }

                tileCostObjects[i].SetActive(true);
                tileCostSprites[i].sprite = tileTypes[i].GetIcon();
                tileCostSprites[i].color = tileTypes[i].GetColor();
                Matchmaker.Util.NormalizeSpriteScale(tileCostSprites[i], 0.9f);
                costAmounts[i] = costs[i];
            }
        }

        public void UpdateAmount(int skillIndex, int amount)
        {
            costAmounts[skillIndex] = Mathf.Max(amount, 0);
            if(amount <= 0)
            {
                tileCostTexts[skillIndex].color = Color.green;
            }
        }

        void Update()
        {
            for (int i = 0; i < tileCostTexts.Length; i++)
            {
                displayedCostAmounts[i] = Matchmaker.Util.ExpDecayLerp(displayedCostAmounts[i], costAmounts[i], 10, Time.deltaTime);

                tileCostTexts[i].text = Mathf.RoundToInt(displayedCostAmounts[i]).ToString();
            }
        }
    }
}