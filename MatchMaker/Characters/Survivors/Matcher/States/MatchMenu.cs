using EntityStates;
using MatcherMod.Survivors.Matcher.Components;
using MatcherMod.Survivors.Matcher.Components.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatcherMod.Survivors.Matcher.SkillStates
{
    internal class MatchMenu : BaseSkillState 
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Log.CheckNullAndWarn("nip", GetComponent<MatcherGridController>().CompanionUI);
            GetComponent<MatcherGridController>().CompanionUI.Show();
        }
    }
}