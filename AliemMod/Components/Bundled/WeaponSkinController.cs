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

        private SkinDef _currentWeaponSkin;

        public void AddWeaponSkin(SkillDef skillDef, int skin)
        {
            if (!_weaponSkillsSkins.ContainsKey(skillDef))
            {
                _weaponSkillsSkins[skillDef] = weaponSkins[skin];
            }
        }
        public void AddWeaponSkin(SkillDef skillDef, SkinDef skin)
        {
            if (!_weaponSkillsSkins.ContainsKey(skillDef))
            {
                _weaponSkillsSkins[skillDef] = skin;
            }
            else
            {
                Helpers.LogWarning("already contains key for " + skillDef.skillName);
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
            if (_currentWeaponSkin == null)
            {
                _currentWeaponSkin = skin;
                return;
            }
            (skin as PartialSkinDef).Apply(gameObject);
            _currentWeaponSkin = skin;
        }

        public void ApplyCurrentWeaponSkin()
        {
            if (_currentWeaponSkin == null)
                return;

            ApplyWeaponSkin(_currentWeaponSkin);
        }
    }
}
