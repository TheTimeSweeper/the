using EntityStates;
using HellDiverMod.General.SkillDefs;
using HellDiverMod.Survivors.HellDiver.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace HellDiverMod.Survivors.HellDiver.SkillStates
{

    public class InputStratagem : BaseSkillState, IHasSkillDefComponent<StratagemInputController>
    {
        public StratagemInputController componentFromSkillDef1 { get; set; }

        private bool _stratagemReady;
        private bool _success;

        public override void OnEnter()
        {
            base.OnEnter();
            componentFromSkillDef1.ShowUI(true);
        }

        public override void Update()
        {
            base.Update();

            if (_stratagemReady)
                return;

            if(GetStratagemInput(out StratagemInput input))
            {
                _stratagemReady = componentFromSkillDef1.TryStratagemInput(input);
                if (_stratagemReady)
                {
                    skillLocator.primary.SetSkillOverride(gameObject, HellDiverSurvivor.throwStratagemSkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_stratagemReady && isAuthority && inputBank.skill1.down)
            {
                _success = true;
                outer.SetNextState(new ThrowStratagem());
            }
        }

        public bool GetStratagemInput(out StratagemInput input)
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.UpArrow))
            {
                input = StratagemInput.UP;
                return true;
            }
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.DownArrow))
            {
                input = StratagemInput.DOWN;
                return true;
            }
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.RightArrow))
            {
                input = StratagemInput.RIGHT;
                return true;
            }
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.LeftArrow))
            {
                input = StratagemInput.LEFT;
                return true;
            }
            input = default(StratagemInput);
            return false;
        }

        public override void OnExit()
        {
            base.OnExit();

            componentFromSkillDef1.ShowUI(false);

            if (!_success)
            {
                componentFromSkillDef1.Reset();

                skillLocator.primary.UnsetSkillOverride(gameObject, HellDiverSurvivor.throwStratagemSkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return _stratagemReady ? InterruptPriority.Skill : InterruptPriority.Any;
        }
    }
}
