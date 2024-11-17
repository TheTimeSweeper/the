using EntityStates;
using MatcherMod.Modules.SkillDefs;
using MatcherMod.Survivors.Matcher.Components;
using MatcherMod.Survivors.Matcher.Components.UI;
using MatcherMod.Survivors.Matcher.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatcherMod.Survivors.Matcher.SkillStates
{
    internal class MatchMenu: BaseSkillState, IHasSkillDefComponent<MatcherGridController>
    {
        public MatcherGridController componentFromSkillDef1 { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();

            if (characterBody.master.playerCharacterMasterController == null)
            {
                outer.SetNextState(new MatchMenuAI { componentFromSkillDef1 = componentFromSkillDef1, activatorSkillSlot = activatorSkillSlot });
                return;
            }

            if (isAuthority)
            {
                if (!componentFromSkillDef1.ToggleUI())
                {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (isAuthority && fixedAge > CharacterConfig.M5_MatchGrid_Duration)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }

    internal class MatchMenuInfinite : BaseSkillState, IHasSkillDefComponent<MatcherGridController>
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