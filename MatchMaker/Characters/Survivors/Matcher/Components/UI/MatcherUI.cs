using RoR2.UI;
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

        [SerializeField]
        private MPEventSystemLocator eventSystemLocator;

        public MatcherGridController controller;

        public bool Created { get; private set; }
        public bool Showing { get; private set; } = true;

        void ICompanionUI<MatcherGridController>.OnInitialize(MatcherGridController hasUIComponent)
        {
            controller = hasUIComponent;

            Show(false);
        }

        public void GenerateGrid(SkillDef[] skillDefs)
        {
            Created = true;

            matchGrid.OnMatchAwarded = controller.OnMatchAwarded;

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

        public void Show() => Show(!Showing);
        public void Show(bool shouldShow)
        {
            if (shouldShow == Showing)
                return;
            eventSystemLocator.eventSystem.SetSelectedGameObject(shouldShow ? gameObject : null);
            Showing = shouldShow;
            gameObject.SetActive(shouldShow);
        }

        public void OnBodyLost()
        {
            Destroy(gameObject);
        }

        public void OnBodyUnFocused()
        {
            Show(false);
        }
    }
}
