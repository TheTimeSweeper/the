using MatcherMod.Modules.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Components.UI
{
    public class MatcherHUDManager : CompanionHUDManager<MatcherGridController, MatcherUI>
    {
        public override GameObject UIPrefab => MatcherContent.Assets.matchGrid;

        public override bool LocalUserOnly => true;

        protected override string transformPath => "MainContainer/MainUIArea/SpringCanvas/BottomCenterCluster";

    }
}
