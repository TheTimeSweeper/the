using RoR2;
using RoR2.Skills;
using System.Collections.Generic;

namespace RA2Mod.Survivors.Chrono.Components
{
    public abstract class TrackerSkillDefRequired<T> : Tracker where T: SkillDef
    {
        private Dictionary<GenericSkill, bool> skillMap = new Dictionary<GenericSkill, bool>();

        protected override void Awake()
        {
            base.Awake();

            GenericSkill[] skills = GetComponents<GenericSkill>();
            for (int i = 0; i < skills.Length; i++)
            {
                skillMap[skills[i]] = false;
                skills[i].onSkillChanged += OnSkillChanged;
            }
        }

        void OnDestroy()
        {
            foreach (GenericSkill skill in skillMap.Keys)
            {
                skill.onSkillChanged -= OnSkillChanged;
            }
        }

        protected virtual void OnSkillChanged(GenericSkill genericSKill)
        {
            skillMap[genericSKill] = genericSKill.skillDef is T;

            foreach (bool isSkill in skillMap.Values)
            {
                if (isSkill)
                {
                    indicator.active = true;
                    return;
                }
            }
            indicator.active = false;
        }
    }
}