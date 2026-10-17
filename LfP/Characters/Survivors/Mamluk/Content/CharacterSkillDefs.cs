using LfPMod.Modules;
using LfPMod.Survivors.Mamluk.SkillStates;
using RoR2.Skills;
using System;
using UnityEngine;

namespace LfPMod.Survivors.Mamluk.MamlukContent
{
    public class CharacterSkillDefs
    {
        public static SteppedSkillDef Axe_M1;
        public static SkillDef Axe_M2Enter;
        public static SkillDef Axe_M2During;

        public static SteppedSkillDef Shield_M1;
        public static SkillDef Shield_M3Enter;
        public static SkillDef Shield_M3During;

        public static SkillDef Spear_M1;
        public static SkillDef Spear_M4Enter;
        public static SkillDef Spear_M4During;

        public static string TOKEN_PREFIX => MamlukSurvivor.TOKEN_PREFIX;
        public static AssetBundle assetBundle => MamlukSurvivor.instance.assetBundle;

        public static void CreateSkillDefsAxe()
        {
            Axe_M1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "HenrySlash",
                    TOKEN_PREFIX + "PRIMARY_SLASH_NAME",
                    TOKEN_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texIconSecondary"),
                    new EntityStates.SerializableEntityStateType(typeof(Axe_M1)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            Axe_M1.stepCount = 2;
            Axe_M1.stepGraceDuration = 0.5f;

            Axe_M2Enter = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryGun",
                skillNameToken = TOKEN_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texIconSecondary"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Axe_M2Enter)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });
        }

        public static void CreateSkillDefsShield()
        {
            Shield_M1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "HenrySlash",
                    TOKEN_PREFIX + "PRIMARY_SLASH_NAME",
                    TOKEN_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texIconPrimary"),
                    new EntityStates.SerializableEntityStateType(typeof(Shield_M1)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            Shield_M1.stepCount = 2;
            Shield_M1.stepGraceDuration = 0.5f;

            Shield_M3Enter = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryGun",
                skillNameToken = TOKEN_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texIconPrimary"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Shield_M3Enter)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });
        }

        public static void CreateSkillDefsSpear()
        {
            Spear_M1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "HenrySlash",
                    TOKEN_PREFIX + "PRIMARY_SLASH_NAME",
                    TOKEN_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texIconUtility"),
                    new EntityStates.SerializableEntityStateType(typeof(Spear_M1)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            //Spear_M1.stepCount = 2;
            //Spear_M1.stepGraceDuration = 0.5f;

            Spear_M4Enter = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryGun",
                skillNameToken = TOKEN_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texIconUtility"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Spear_M4Enter)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });
        }

        internal static void Init()
        {
            CreateSkillDefsAxe();
            CreateSkillDefsShield();
            CreateSkillDefsSpear();
        }
    }
}
