using EntityStates;
using RA2Mod.Survivors.GI.SkillDefs;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class BaseTransform : BaseSkillState
    {
        private CrosshairUtils.OverrideRequest crosshairRequest = null;
        protected UpgradableSkillDef mainUpgrade;

        public override void OnEnter()
        {
            base.OnEnter();

            OverrideSkills(true);
        }

        private void OverrideSkills(bool setOverride)
        {
            for (int i = 0; i < skillLocator.allSkills.Length; i++)
            {
                TryUpgradeSkill(skillLocator.allSkills[i], setOverride);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.inputBank.moveVector = Vector3.zero;
        }

        private void TryUpgradeSkill(GenericSkill genericSkill, bool setOverride)
        {
            UpgradableSkillDef skillDef = genericSkill.baseSkill as UpgradableSkillDef;
            if (skillDef)
            {
                if (setOverride)
                {
                    genericSkill.SetSkillOverride(this, skillDef.upgradedSkillDef, GenericSkill.SkillOverridePriority.Upgrade);

                    if (skillDef.crosshairOverride != null)
                    {
                        mainUpgrade = skillDef;
                        crosshairRequest = CrosshairUtils.RequestOverrideForBody(characterBody, skillDef.crosshairOverride, CrosshairUtils.OverridePriority.Skill);
                    }
                }
                else
                {
                    genericSkill.UnsetSkillOverride(this, skillDef.upgradedSkillDef, GenericSkill.SkillOverridePriority.Upgrade);

                    if (crosshairRequest != null)
                    {
                        crosshairRequest.Dispose();
                    }
                }
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            OverrideSkills(false);
        }
        //public override InterruptPriority GetMinimumInterruptPriority()
        //{
        //    return InterruptPriority.PrioritySkill;
        //}
    }
}