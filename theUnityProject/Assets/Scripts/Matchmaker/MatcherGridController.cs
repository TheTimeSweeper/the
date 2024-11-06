using MatcherMod.Modules.UI;
using MatcherMod.Survivors.Matcher.Components.UI;
using Matchmaker.MatchGrid;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Components
{
    public class MatcherGridController : MonoBehaviour, IHasCompanionUI<MatcherUI>
    {
        public bool allowUIUpdate { get; set; }
        public MatcherUI CompanionUI { get; set; }
    }
}
