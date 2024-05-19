using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AliemMod.Components.Bundled
{
    public class WeaponSkinController : MonoBehaviour
    {
        [SerializeField]
        private SkinDef[] weaponSkins;

        private static Dictionary<SkillDef, SkinDef> _weaponSkillsSkins = new Dictionary<SkillDef, SkinDef>();

        private GenericSkill _primaryGenericSkill;

        public void AddWeaponSkin(SkillDef skillDef, int skin)
        {
            if (!_weaponSkillsSkins.ContainsKey(skillDef))
            {
                _weaponSkillsSkins[skillDef] = weaponSkins[skin];
            }
        }

        void Start()
        {
            if (TryGetComponent(out CharacterModel model))
            {
                _primaryGenericSkill = model.body.GetComponent<SkillLocator>().primary;
                _primaryGenericSkill.onSkillChanged += OnSkillChanged;
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

        private void OnSkillChanged(GenericSkill genericSkill)
        {
            if (_weaponSkillsSkins.ContainsKey(genericSkill.skillDef))
            {
                ApplyWeaponSkin(genericSkill.skillDef);
            }
        }

        public void ApplyWeaponSkin(SkillDef skillDef)
        {
            ApplyWeaponSkin(_weaponSkillsSkins[skillDef]);
        }

        public void ApplyWeaponSkin(SkinDef skin)
        {
            Helpers.LogWarning("applying " + skin.name);
            skin.Apply(gameObject);
        }
    }
}
