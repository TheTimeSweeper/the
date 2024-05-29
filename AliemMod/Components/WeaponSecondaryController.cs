﻿using AliemMod.Content;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace AliemMod.Components
{
    public class WeaponSecondaryController : MonoBehaviour
    {
        public static Dictionary<SkillDef, SkillDef> skillPairs = new Dictionary<SkillDef, SkillDef>();

        private GenericSkill _primaryGenericSkill;
        private GenericSkill _secondaryGenericSkill;

        void Start()
        {
            if (TryGetComponent(out SkillLocator skillLocator))
            {
                _primaryGenericSkill = skillLocator.primary;
                _primaryGenericSkill.onSkillChanged += OnSkillChanged;
                _secondaryGenericSkill = skillLocator.secondary;
                OnSkillChanged(_primaryGenericSkill);
            }
        }

        void OnDestroy()
        {
            if (_primaryGenericSkill != null)
            {
                _primaryGenericSkill.onSkillChanged -= OnSkillChanged;
            }
        }

        private void OnSkillChanged(GenericSkill primaryGenericSkill)
        {
            Helpers.LogWarning(primaryGenericSkill.skillDef.skillName);

            if (skillPairs.ContainsKey(primaryGenericSkill.skillDef))
            {
                Helpers.LogWarning(skillPairs[primaryGenericSkill.skillDef].skillName);
                _secondaryGenericSkill.SetBaseSkill(skillPairs[primaryGenericSkill.skillDef]);
            }
        }
    }
}
