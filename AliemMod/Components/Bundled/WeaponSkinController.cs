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
        [SerializeField]
        private SkinDef[] weaponSkinsSecondaries;

        private static Dictionary<SkillDef, SkinDef> _weaponSkillsSkins = new Dictionary<SkillDef, SkinDef>();

        private GenericSkill _primaryGenericSkill;
        private GenericSkill _secondaryGenericSkill;

        private SkinDef _currentWeaponSkinPrimary;
        private SkinDef _currentWeaponSkinSecondary;

        public void AddWeaponSkin(SkillDef skillDef, int skin)
        {
            if (!_weaponSkillsSkins.ContainsKey(skillDef))
            {
                _weaponSkillsSkins[skillDef] = weaponSkins[skin];
            }
        }
        public void AddWeaponSkinSecondary(SkillDef skillDef, int skin)
        {
            if (!_weaponSkillsSkins.ContainsKey(skillDef))
            {
                _weaponSkillsSkins[skillDef] = weaponSkinsSecondaries[skin];
            }
        }

        void Start()
        {
            if (TryGetComponent(out CharacterModel model))
            {
                if (model.body == null)
                    return;

                _primaryGenericSkill = model.body.GetComponent<SkillLocator>().primary;
                _primaryGenericSkill.onSkillChanged += OnSkillChanged;
                OnSkillChanged(_primaryGenericSkill);

                _secondaryGenericSkill = model.body.GetComponent<SkillLocator>().secondary;
                _secondaryGenericSkill.onSkillChanged += OnSkillChangedSecondary;
                OnSkillChangedSecondary(_secondaryGenericSkill);
            }
        }

        void OnDestroy()
        {
            if (_primaryGenericSkill != null)
            {
                _primaryGenericSkill.onSkillChanged -= OnSkillChanged;
            }
            if (_secondaryGenericSkill != null)
            {
                _secondaryGenericSkill.onSkillChanged -= OnSkillChangedSecondary;
            }
        }

        private void OnSkillChanged(GenericSkill genericSkill)
        {
            if (_weaponSkillsSkins.ContainsKey(genericSkill.skillDef))
            {
                ApplyWeaponSkin(genericSkill.skillDef);
            }
        }
        private void OnSkillChangedSecondary(GenericSkill genericSkill)
        {
            if (_weaponSkillsSkins.ContainsKey(genericSkill.skillDef))
            {
                ApplyWeaponSkinSecondary(genericSkill.skillDef);
            } else
            {
                ApplyWeaponSkinSecondary(weaponSkinsSecondaries[0]);
            }
        }

        public void ApplyWeaponSkin(SkillDef skillDef)
        {
            ApplyWeaponSkin(_weaponSkillsSkins[skillDef]);
        }
        public void ApplyWeaponSkin(SkinDef skin)
        {
            if (_currentWeaponSkinPrimary == null)
            {
                _currentWeaponSkinPrimary = skin;
                return;
            }
            (skin as PartialSkinDef).Apply(gameObject);
            _currentWeaponSkinPrimary = skin;
        }

        public void ApplyWeaponSkinSecondary(SkillDef skillDef)
        {
            ApplyWeaponSkinSecondary(_weaponSkillsSkins[skillDef]);
        }
        public void ApplyWeaponSkinSecondary(SkinDef skin)
        {
            //if (_currentWeaponSkinSecondary == null)
            //{
            //    _currentWeaponSkinSecondary = skin;
            //    return;
            //}
            (skin as PartialSkinDef).Apply(gameObject);
            _currentWeaponSkinSecondary = skin;
        }

        public void ApplyCurrentWeaponSkin()
        {
            if (_currentWeaponSkinPrimary != null)
            {
                ApplyWeaponSkin(_currentWeaponSkinPrimary);
            }
            //if(_currentWeaponSkinSecondary != null)
            //{
            //    ApplyWeaponSkinSecondary(_currentWeaponSkinSecondary);
            //}
        }
    }
}
