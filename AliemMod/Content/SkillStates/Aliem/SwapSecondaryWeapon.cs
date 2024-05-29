using AliemMod.Content;
using AliemMod.Content.SkillDefs;
using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModdedEntityStates.Aliem
{
    public class SwapSecondaryWeapon : BaseTimedSkillState
    {
        private WeaponSwapSkillDef _swapSkillDef;
        private float _duration = AliemConfig.M4_WeaponSwap_Duration.Value;

        public override float TimedBaseDuration => _duration;

        public override float TimedBaseCastStartPercentTime => 1;

        public override void OnEnter()
        {
            base.OnEnter();
            _swapSkillDef = activatorSkillSlot.skillDef as WeaponSwapSkillDef;
            if(_swapSkillDef == null)
            {
                AliemPlugin.Log.LogError(activatorSkillSlot.skillDef.skillName + " is not a valid WeaponSwapSkillDef");
                return;
            }

            PlayAnimation("RightHandClosed", "Closed");

            skillLocator.secondary.SetSkillOverride(this, _swapSkillDef.swapSkillDef, RoR2.GenericSkill.SkillOverridePriority.Replacement);

            //play weapon get animation
        }

        public override void OnExit()
        {
            base.OnExit();

            PlayAnimation("RightHandClosed", "BufferEmpty");

            RoR2.EntityStateMachine.FindByCustomName(gameObject, "Inputs2").SetNextStateToMain();

            skillLocator.secondary.UnsetSkillOverride(this, _swapSkillDef.swapSkillDef, RoR2.GenericSkill.SkillOverridePriority.Replacement);
        }
    }
}
