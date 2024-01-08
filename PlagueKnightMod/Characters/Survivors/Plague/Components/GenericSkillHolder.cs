using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Skills;

namespace PlagueMod.Survivors.Plague.Components
{
    public class GenericSkillHolder : MonoBehaviour
    {
        public List<GenericSkill> genericSkills;

        public void Init(SkillFamily skillFamily)
        {
            if(skillFamily.variants.Length != genericSkills.Count)
            {
                SyncGenericSkills(skillFamily);
            }
            
            for (int i = 0; i < genericSkills.Count; i++)
            {
                genericSkills[i].SetBaseSkill(skillFamily.variants[i].skillDef);
            }
        }

        private void SyncGenericSkills(SkillFamily skillFamily)
        {
            //need to hide GenericSkill before their awake runs and explodes the universe
            gameObject.SetActive(false);
            
            for (int i = 0; i < skillFamily.variants.Length; i++)
            {
                if (genericSkills.Count <= i)
                {
                    GenericSkill genericSkill = gameObject.AddComponent<GenericSkill>();
                    genericSkill._skillFamily = skillFamily;
                    genericSkills.Add(genericSkill);
                }
            }
            for (int i = genericSkills.Count - 1; i >= skillFamily.variants.Length; i--)
            {
                UnityEngine.Object.DestroyImmediate(genericSkills[i]);
                genericSkills.RemoveAt(i);
            }
            gameObject.SetActive(true);

            GetComponent<SkillLocator>().Awake();
        }
    }
}