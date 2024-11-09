using Matchmaker;
using Matchmaker.MatchGrid;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private float[] costAmounts = new float[4];
        private float[] displayedCostAmounts = new float[4];

        public void Init(MatchTileType[] tileTypes, int[] costs)
        {
            int i = 0;
            for (; i < costs.Length; i++)
            {
                tileCostObjects[i].SetActive(true);
                tileCostSprites[i].sprite = tileTypes[i].GetIcon();
                tileCostSprites[i].color = tileTypes[i].GetColor();
                costAmounts[i] = costs[i];
            }
            for (; i < tileCostObjects.Length; i++)
            {
                tileCostObjects[i].SetActive(false);
            }
        }

        void Update()
        {
            for (int i = 0; i < tileCostTexts.Length; i++)
            {
                displayedCostAmounts[i] = Util.ExpDecayLerp(displayedCostAmounts[i], costAmounts[i], 10, Time.deltaTime);

                tileCostTexts[i].text = Mathf.RoundToInt(displayedCostAmounts[i]).ToString();
            }
        }
    }
}