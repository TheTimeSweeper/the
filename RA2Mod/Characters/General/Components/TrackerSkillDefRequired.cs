using RoR2;
using RoR2.Skills;
using System.Collections;
using System.Collections.Generic;

namespace RA2Mod.General.Components
{
    public abstract class TrackerSkillDefRequired<T> : GenericTracker where T : SkillDef
    {
        private Dictionary<GenericSkill, bool> skillMap = new Dictionary<GenericSkill, bool>();

        protected override void Start()
        {
            base.Start();
            GenericSkill[] skills = GetComponents<GenericSkill>();
            for (int i = 0; i < skills.Length; i++)
            {
                skillMap[skills[i]] = false;
                skills[i].onSkillChanged += OnSkillChanged;
                OnSkillChanged(skills[i]);
            }
        }

        void OnDestroy()
        {
            foreach (GenericSkill skill in skillMap.Keys)
            {
                skill.onSkillChanged -= OnSkillChanged;
            }
        }

        protected virtual void OnSkillChanged(GenericSkill genericSkill)
        {
            skillMap[genericSkill] = genericSkill.skillDef is T;

            CheckSkills();
        }

        private void CheckSkills()
        {
            foreach (bool isSkill in skillMap.Values)
            {
                if (isSkill)
                {
                    enabled = true;
                    return;
                }
            }

            enabled = false;
        }
    }
}