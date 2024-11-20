using EntityStates;
using MatcherMod.Modules.SkillDefs;
using MatcherMod.Survivors.Matcher.Components;
using MatcherMod.Survivors.Matcher.Content;
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

            if (characterBody.master.playerCharacterMasterController == null)
            {
                outer.SetNextState(new MatchMenuAI { componentFromSkillDef1 = componentFromSkillDef1, activatorSkillSlot = activatorSkillSlot });
                return;
            }

            PlayAnimation("Gesture, Override", "HoldGrid");

            if (isAuthority)
            {                                  //bit ugly but I just want to release this guy
                componentFromSkillDef1.ShowUI(TimeStopAction);
            }
        }

        public void TimeStopAction(float duration)
        {
            fixedAge -= duration;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (isAuthority && (fixedAge > CharacterConfig.M5_MatchGrid_Duration || Inputted()))
            {
                outer.SetNextStateToMain();
            }
        }

        private bool Inputted()
        {
            return inputBank.skill4.justPressed && fixedAge > 0.1f;
        }

        public override void OnExit()
        {
            base.OnExit();

            if (isAuthority)
            {
                componentFromSkillDef1.QueueGridClose();
            }

            PlayAnimation("Gesture, Override", "HoldGridReturn");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}