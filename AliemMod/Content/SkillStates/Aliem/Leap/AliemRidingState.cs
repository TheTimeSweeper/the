using AliemMod.Content;
using AliemMod.Content.Survivors;
using AliemMod.Modules;
using RoR2;
using UnityEngine.Networking;

namespace ModdedEntityStates.Aliem
{
    public class AliemRidingState : BaseRidingState {

        public int inputButton;

        private GenericSkill _originalSkill;

        private GenericSkill inputGenericSkill
        {
            get
            {
                switch (inputButton)
                {
                    case 1:
                        return skillLocator.primary;
                    default:
                    case 2:
                        return skillLocator.secondary;
                    case 3:
                        return skillLocator.utility;
                    case 4:
                        return skillLocator.special;
                }
            }
            set
            {
                switch (inputButton)
                {
                    case 1:
                        skillLocator.primary = value;
                        break;
                    default:
                    case 2:
                        skillLocator.secondary = value;
                        break;
                    case 3:
                        skillLocator.utility = value;
                        break;
                    case 4:
                        skillLocator.special = value;
                        break;
                }
            }
        }

        public override void OnEnter() {
            base.OnEnter();

			PlayAnimation("FullBody, Underride", "RidingClimb", "RidincClimb.playbackRate", 0.4f);

            //genericskill swapping
            _originalSkill = inputGenericSkill;
            inputGenericSkill = skillLocator.FindSkill("LOADOUT_SKILL_RIDING");

            if (NetworkServer.active)
            {
                characterBody.AddBuff(Buffs.ridingBuff);
            }

            ////skilldef replacement
            //inputGenericSkill.SetSkillOverride(this, skillLocator.FindSkill("Riding").skillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        public override void FixedUpdate() {
			base.FixedUpdate();

			if (isAuthority && inputBank.jump.justPressed) {

				base.outer.SetState(new EndRidingState());
				
				return;
			}
		}

        public override void OnExit()
        {
            base.OnExit();

            if (NetworkServer.active)
            {
                characterBody.RemoveBuff(Buffs.ridingBuff);
            }

            //genericskill swapping
            inputGenericSkill = _originalSkill;

            ////skilldef replacement
            //inputGenericSkill.UnsetSkillOverride(this, skillLocator.FindSkill("Riding").skillDef, GenericSkill.SkillOverridePriority.Contextual);
        }
    }
}
