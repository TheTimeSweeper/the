using MatcherMod.Modules.UI;
using Matchmaker.MatchGrid;
using RoR2.Skills;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Components.UI
{
    public class MatcherUI : MonoBehaviour, ICompanionUI<MatcherGridController>
    {
        [SerializeField]
        private MatchGrid matchGrid;

        void ICompanionUI<MatcherGridController>.OnInitialize(MatcherGridController hasUIComponent)
        {
            GenerateGrid(hasUIComponent);

            matchGrid.OnMatchAwarded += hasUIComponent.OnMatchAwarded;

            Show(false);
        }

        private void GenerateGrid(MatcherGridController hasUIComponent)
        {
            SkillDef[] skillDefs = hasUIComponent.GetSkillDefs();

            MatchTileType[] tileTypes = new MatchTileType[skillDefs.Length];
            for (int i = 0; i < skillDefs.Length; i++)
            {
                tileTypes[i] = new MatchTileType(skillDefs[i]);
            }

            matchGrid.Init(tileTypes);
        }

        public void OnUIUpdate()
        {

        }

        public void Show() => Show(!gameObject.activeSelf);
        public void Show(bool shouldShow)
        {
            gameObject.SetActive(shouldShow);
        }
    }
}
