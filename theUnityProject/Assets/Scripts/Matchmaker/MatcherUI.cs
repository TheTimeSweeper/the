using MatcherMod.Modules.UI;
using Matchmaker.MatchGrid;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Components.UI
{
    public class MatcherUI : MonoBehaviour, ICompanionUI<MatcherGridController>
    {
        [SerializeField]
        private MatchGrid matchGrid;

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
