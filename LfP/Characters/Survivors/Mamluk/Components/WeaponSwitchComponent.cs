using RoR2.Skills;
using RoR2;
using System;
using UnityEngine;
using LfPMod.Survivors.Mamluk.MamlukContent;

namespace LfPMod.Survivors.Mamluk.Components
{
    internal class WeaponSwitchComponent : MonoBehaviour
    {
        private CharacterBody _characterBody;
        private SkillLocator _skillLocator;

        public class MamlukWeaponSkillSet
        {
            public SkillDef weapon_M1;
            public SkillDef weapon_Skill_Enter;
            public SkillDef weapon_Skill_During;
            public SkillSlot skillSlot;

            public void SetSKillOverrides(SkillLocator skillLocator)
            {
                skillLocator.primary.SetSkillOverride(skillLocator, weapon_M1, GenericSkill.SkillOverridePriority.Replacement);
                if (weapon_Skill_During)
                {
                    skillLocator.GetSkill(skillSlot).SetSkillOverride(skillLocator, weapon_Skill_During, GenericSkill.SkillOverridePriority.Replacement);
                }
            }
            public void UnsetSkillOverrides(SkillLocator skillLocator)
            {
                skillLocator.primary.UnsetSkillOverride(skillLocator, weapon_M1, GenericSkill.SkillOverridePriority.Replacement);
                if (weapon_Skill_During)
                {
                    skillLocator.GetSkill(skillSlot).UnsetSkillOverride(skillLocator, weapon_Skill_During, GenericSkill.SkillOverridePriority.Replacement);
                }
            }
        }

        public static ListWithEvents<MamlukWeaponSkillSet> WeaponSkillSets;

        private MamlukWeaponSkillSet lastWeaponSkillSet;

        //todo add a callsite for other mods to hook if they want to add alt skills. 
        public static void InitializeStaticWeapons()
        {
            WeaponSkillSets = new ListWithEvents<MamlukWeaponSkillSet>();
            WeaponSkillSets.Add(new MamlukWeaponSkillSet
            {
                weapon_M1 = CharacterSkillDefs.Axe_M1,
                weapon_Skill_Enter = CharacterSkillDefs.Axe_M2Enter,
                weapon_Skill_During = CharacterSkillDefs.Axe_M2During
            });
            WeaponSkillSets.Add(new MamlukWeaponSkillSet
            {
                weapon_M1 = CharacterSkillDefs.Shield_M1,
                weapon_Skill_Enter = CharacterSkillDefs.Shield_M3Enter,
                weapon_Skill_During = CharacterSkillDefs.Shield_M3During
            });
            WeaponSkillSets.Add(new MamlukWeaponSkillSet
            {
                weapon_M1 = CharacterSkillDefs.Spear_M1,
                weapon_Skill_Enter = CharacterSkillDefs.Spear_M4Enter,
                weapon_Skill_During = CharacterSkillDefs.Spear_M4During
            });
        }

        private void Awake()
        {
            _characterBody = GetComponent<CharacterBody>();
            _skillLocator = GetComponent<SkillLocator>();
            _characterBody.onSkillActivatedAuthority += WeaponSwitchComponent_onSkillActivatedAuthority;

            //todo assign starting weapon based on chosen primary on spawn
        }

        private void WeaponSwitchComponent_onSkillActivatedAuthority(GenericSkill skillSlot)
        {
            for (int i = 0; i < WeaponSkillSets.Count; i++)
            {
                var skillSet = WeaponSkillSets[i];
                if(skillSlot.skillDef == skillSet.weapon_Skill_Enter)
                {
                    lastWeaponSkillSet?.UnsetSkillOverrides(_skillLocator);
                    skillSet.SetSKillOverrides(_skillLocator);
                    lastWeaponSkillSet = skillSet;
                }
            }
        }
    }
}