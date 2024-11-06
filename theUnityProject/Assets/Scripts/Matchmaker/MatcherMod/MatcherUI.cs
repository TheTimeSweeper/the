using MatcherMod.Modules.UI;
using Matchmaker.MatchGrid;
using RoR2.UI;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Components.UI
{
    public class MatcherUI : MonoBehaviour, ICompanionUI<MatcherGridController>
    {
        [SerializeField]
        private MatchGrid matchGrid;

        [SerializeField]
        private MPEventSystemLocator eventSystemLocator;

        void ICompanionUI<MatcherGridController>.OnInitialize(MatcherGridController hasUIComponent)
        {
            Show(false);
        }

        public void OnUIUpdate()
        {
        }

        public void Show(bool shouldShow)
        {
            gameObject.SetActive(shouldShow);
        }
    }
}
