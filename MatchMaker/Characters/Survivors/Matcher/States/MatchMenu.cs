using EntityStates;
using MatcherMod.Modules.SkillDefs;
using MatcherMod.Survivors.Matcher.Components;
using MatcherMod.Survivors.Matcher.Components.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatcherMod.Survivors.Matcher.SkillStates
{
    internal class MatchMenu : BaseSkillState, IHasSkillDefComponent<MatcherGridController>
    {
        public MatcherGridController componentFromSkillDef1 { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();

            if(characterBody.master.playerCharacterMasterController == null)
            {
                outer.SetNextState(new MatchMenuAI { componentFromSkillDef1 = componentFromSkillDef1, activatorSkillSlot = activatorSkillSlot});
                return;
            }

            if (isAuthority)
            {
                MatcherUI companionUI = componentFromSkillDef1.CompanionUI;
                if (!componentFromSkillDef1.ToggleUI())
                {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}