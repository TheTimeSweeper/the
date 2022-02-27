using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HenryMod.Modules {

    internal static class Skills
    {
        internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
        internal static List<SkillDef> skillDefs = new List<SkillDef>();

        internal static void CreateSkillFamilies(GameObject targetPrefab, int families = 15) {
            foreach (GenericSkill obj in targetPrefab.GetComponentsInChildren<GenericSkill>()) {
                FacelessJoePlugin.DestroyImmediate(obj);
            }

            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            if ((families & (1 << 0)) != 0) {
                skillLocator.primary = CreateGenericSkillWithSkillFamily(targetPrefab, "Primary");
            }
            if ((families & (1 << 1)) != 0) {
                skillLocator.secondary = CreateGenericSkillWithSkillFamily(targetPrefab, "Secondary");
            }
            if ((families & (1 << 2)) != 0) {
                skillLocator.utility = CreateGenericSkillWithSkillFamily(targetPrefab, "Utility");
            }
            if ((families & (1 << 3)) != 0) {
                skillLocator.special = CreateGenericSkillWithSkillFamily(targetPrefab, "Special");
            }
        }

        public static GenericSkill CreateGenericSkillWithSkillFamily(GameObject targetPrefab, string familyName, bool hidden = false) {

            GenericSkill skill = targetPrefab.AddComponent<GenericSkill>();
            skill.skillName = familyName;
            skill.hideInCharacterSelect = hidden;

            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (newFamily as ScriptableObject).name = targetPrefab.name + familyName + "Family";
            newFamily.variants = new SkillFamily.Variant[0];

            skill._skillFamily = newFamily;

            skillFamilies.Add(newFamily);
            return skill;
        }

        // this could all be a lot cleaner but at least it's simple and easy to work with
        // todo: this could all be a lot cleaner but at least it's simple and easy to work with
        internal static void AddPrimarySkill(GameObject targetPrefab, SkillDef skillDef) {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            AddSkillToFamily(skillFamily, skillDef);
        }

        internal static void AddPrimarySkills(GameObject targetPrefab, params SkillDef[] skillDefs) {
            foreach (SkillDef i in skillDefs) {
                AddPrimarySkill(targetPrefab, i);
            }
        }

        internal static void AddSecondarySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

            AddSkillToFamily(skillFamily, skillDef);
        }

        internal static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddSecondarySkill(targetPrefab, i);
            }
        }

        internal static void AddUtilitySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.utility.skillFamily;

            AddSkillToFamily(skillFamily, skillDef);
        }

        internal static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddUtilitySkill(targetPrefab, i);
            }
        }

        internal static void AddSpecialSkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.special.skillFamily;

            AddSkillToFamily(skillFamily, skillDef);
        }

        internal static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddSpecialSkill(targetPrefab, i);
            }
        }

        internal static void AddSkillToFamily(SkillFamily skillFamily, SkillDef skillDef, UnlockableDef unlockableDef = null) {
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant {
                skillDef = skillDef,
                unlockableDef = unlockableDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static SkillDef CreatePrimarySkillDef(SerializableEntityStateType state, string stateMachine, string skillName, string skillNameToken, string skillDescriptionToken, Sprite skillIcon, bool agile) {
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            
            populatePrimarySKillDef(state, stateMachine, skillName, skillNameToken, skillDescriptionToken, skillIcon, agile, skillDef);

            skillDefs.Add(skillDef);

            return skillDef;
        }

        internal static T CreatePrimarySkillDef<T>(SerializableEntityStateType state, 
                                                string stateMachine, 
                                                string skillName, 
                                                string skillNameToken, 
                                                string skillDescriptionToken, 
                                                Sprite skillIcon, 
                                                bool agile) where T : SkillDef{
            T skillDef = ScriptableObject.CreateInstance<T>();

            populatePrimarySKillDef(state, stateMachine, skillName, skillNameToken, skillDescriptionToken, skillIcon, agile, skillDef);

            skillDefs.Add(skillDef);

            return skillDef;
        }
        //todo eugh
        private static void populatePrimarySKillDef(SerializableEntityStateType state, string stateMachine, string skillName, string skillNameToken, string skillDescriptionToken, Sprite skillIcon, bool agile, SkillDef skillDef) {
            skillDef.skillName = skillName;
            (skillDef as ScriptableObject).name = skillName;
            skillDef.skillNameToken = skillNameToken;
            skillDef.skillDescriptionToken = skillDescriptionToken;
            skillDef.icon = skillIcon;

            skillDef.activationState = state;
            skillDef.activationStateMachineName = stateMachine;
            skillDef.baseMaxStock = 1;
            skillDef.baseRechargeInterval = 0;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.forceSprintDuringState = false;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.Any;
            skillDef.resetCooldownTimerOnUse = false;
            skillDef.isCombatSkill = true;
            skillDef.mustKeyPress = false;
            skillDef.cancelSprintingOnActivation = !agile;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 0;
            skillDef.stockToConsume = 0;

            if (agile) skillDef.keywordTokens = new string[] { "KEYWORD_AGILE" };
        }

        internal static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo) {
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

            popuplateSKillDef(skillDefInfo, skillDef);

            skillDefs.Add(skillDef);

            return skillDef;
        }

        internal static T CreateSkillDef<T>(SkillDefInfo skillDefInfo) where T: SkillDef {
            T skillDef = ScriptableObject.CreateInstance<T>();

            popuplateSKillDef(skillDefInfo, skillDef);

            skillDefs.Add(skillDef);

            return skillDef;
        }
        //todo eugh but also add unlockable
        private static void popuplateSKillDef(SkillDefInfo skillDefInfo, SkillDef skillDef) {
            skillDef.skillName = skillDefInfo.skillName;
            (skillDef as ScriptableObject).name = skillDefInfo.skillName;
            skillDef.skillNameToken = skillDefInfo.skillNameToken;
            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
            skillDef.icon = skillDefInfo.skillIcon;

            skillDef.activationState = skillDefInfo.activationState;
            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
            skillDef.interruptPriority = skillDefInfo.interruptPriority;
            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
            skillDef.rechargeStock = skillDefInfo.rechargeStock;
            skillDef.requiredStock = skillDefInfo.requiredStock;
            skillDef.stockToConsume = skillDefInfo.stockToConsume;

            skillDef.keywordTokens = skillDefInfo.keywordTokens;
        }
    }
}

internal class SkillDefInfo
{
    public string skillName;
    public string skillNameToken;
    public string skillDescriptionToken;
    public Sprite skillIcon;

    public SerializableEntityStateType activationState;
    public string activationStateMachineName;
    public int baseMaxStock;
    public float baseRechargeInterval;
    public bool beginSkillCooldownOnSkillEnd;
    public bool canceledFromSprinting;
    public bool forceSprintDuringState;
    public bool fullRestockOnAssign;
    public InterruptPriority interruptPriority;
    public bool resetCooldownTimerOnUse;
    public bool isCombatSkill;
    public bool mustKeyPress;
    public bool cancelSprintingOnActivation;
    public int rechargeStock;
    public int requiredStock;
    public int stockToConsume;

    public string[] keywordTokens;

    public UnlockableDef unlockableDef;
}