using EntityStates;
using RA2Mod.Survivors.GI.SkillDefs;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class BaseTransform : BaseSkillState
    {
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
            UpgradableSkillDef def = genericSkill.baseSkill as UpgradableSkillDef;
            if (def)
            {
                if (setOverride)
                {
                    genericSkill.SetSkillOverride(this, def.upgradedSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                }
                else
                {
                    genericSkill.UnsetSkillOverride(this, def.upgradedSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                }
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            OverrideSkills(false);
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}