using BepInEx.Configuration;
using EntityStates;
using EntityStates.Engi.Mine;
using MatcherMod.Modules;
using MatcherMod.Modules.Characters;
using MatcherMod.Survivors.Matcher.Components;
using MatcherMod.Survivors.Matcher.Components.UI;
using MatcherMod.Survivors.Matcher.MatcherContent;
using MatcherMod.Survivors.Matcher.SkillStates;
using Matchmaker.Survivors.Matcher.SkillDefs;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher
{
    public class MatcherSurvivor : SurvivorBase<MatcherSurvivor>
    {
        public const string CHARACTER_NAME = "Matcher";

        public override string assetBundleName => "joe";

        public override string bodyName => CHARACTER_NAME + "Body";

        public override string masterName => CHARACTER_NAME + "MonsterMaster";

        public override string modelPrefabName => "mdlJoe";
        public override string displayPrefabName => "JoeDisplay";

        public const string TOKEN_PREFIX = MatcherPlugin.DEVELOPER_PREFIX + "_MATCHER_";

        public override string survivorTokenPrefix => TOKEN_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = TOKEN_PREFIX + "NAME",
            subtitleNameToken = TOKEN_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texIconConscript"),
            bodyColor = Color.red,
            sortPosition = 69.5f,
            
            crosshairBundlePath = "GICrosshair",
            podPrefabAddressablePath = "RoR2/Base/SurvivorPod/SurvivorPod.prefab",

            maxHealth = 140f,
            healthRegen = 2.0f,
            armor = 10f,

            jumpCount = 1,
        };

        public override UnlockableDef characterUnlockableDef => null;// GIUnlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays { get; } = new MatcherContent.JoeItemDisplays();

        public MatcherGridController matcherGridController;

        public override void Initialize()
        {
            //if (!MatcherContent.Config.MatcherEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void OnCharacterInitialized()
        {
            Modules.Config.ConfigureBody(prefabCharacterBody, MatcherContent.Config.SectionBody);

            MatcherContent.Config.Init();

            //GIUnlockables.Init();

            MatcherContent.States.Init();
            MatcherContent.Tokens.Init();
            Modules.Language.PrintOutput("concsript.txt");

            MatcherContent.Buffs.Init(assetBundle);
            MatcherContent.Assets.Init(assetBundle);

            AdditionalBodySetup();

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();


            AddHooks();
        }
        
        private void AdditionalBodySetup()
        {
            matcherGridController = bodyPrefab.AddComponent<Components.MatcherGridController>();
        }

        public override void InitializeEntityStateMachines() 
        {
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            Skills.ClearGenericSkills(bodyPrefab);

            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddFourthSymbolSkills();
            AddSpecialSkills();
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            var genericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);
            matcherGridController.genericSkills.Add(genericSkill);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            MatchBoostedSkillDef primarySkillDef1 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
                (
                    "MatcherSword",
                    TOKEN_PREFIX + "PRIMARY_SLASH_NAME",
                    TOKEN_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texTileSword"),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)),
                    "Weapon",
                    true
                ));
            ////custom Skilldefs can have additional fields that you can set manually
            //primarySkillDef1.stepCount = 2;
            //primarySkillDef1.stepGraceDuration = 0.5f;
            primarySkillDef1.matchConsumptionCost = 1;
            primarySkillDef1.matchConsumptionMinimum = 1;
            primarySkillDef1.matchMaxConsumptions = 1;
            primarySkillDef1.associatedBuff = CreateMatchBuff(primarySkillDef1);


            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private BuffDef CreateMatchBuff(MatchBoostedSkillDef matchBoostedSkillDef)
        {
            return Content.CreateAndAddBuff(matchBoostedSkillDef.skillName, matchBoostedSkillDef.icon, Color.white, true, false);
        }

        private void AddSecondarySkills()
        {
            var genericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);
            matcherGridController.genericSkills.Add(genericSkill);

            //here is a basic skill def with all fields accounted for
            MatchBoostedSkillDef secondarySkillDef1 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherStaff",
                skillNameToken = TOKEN_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texTileStaff"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Secondary1Fireball)),
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
            secondarySkillDef1.matchConsumptionCost = 1;
            secondarySkillDef1.matchConsumptionMinimum = 1;
            secondarySkillDef1.matchMaxConsumptions = int.MaxValue;
            secondarySkillDef1.associatedBuff = CreateMatchBuff(secondarySkillDef1);

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);
        }
        
        private void AddUtiitySkills()
        {
            var genericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);
            matcherGridController.genericSkills.Add(genericSkill);
            //here's a skilldef of a typical movement skill.
            MatchBoostedSkillDef utilitySkillDef1 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherShield",
                skillNameToken = TOKEN_PREFIX + "UTILITY_ROLL_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTileShield"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Roll)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });
            utilitySkillDef1.matchConsumptionCost = 3;
            utilitySkillDef1.matchConsumptionMinimum = 3;
            utilitySkillDef1.matchMaxConsumptions = 1;
            utilitySkillDef1.associatedBuff = CreateMatchBuff(utilitySkillDef1);

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
        }

        private void AddFourthSymbolSkills()
        {
            GenericSkill genericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "FourthSymbol");
            matcherGridController.genericSkills.Add(genericSkill);

            MatchBoostedSkillDef passiveSkillDef1 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherKey",
                skillNameToken = TOKEN_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "PASSIVE_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texTileKey"),

                //unless you're somehow activating your passive like a skill, none of the following is needed.
                //but that's just me saying things. the tools are here at your disposal to do whatever you like with

                //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                //activationStateMachineName = "Weapon1",
                //interruptPriority = EntityStates.InterruptPriority.Skill,

                //baseRechargeInterval = 1f,
                //baseMaxStock = 1,

                //rechargeStock = 1,
                //requiredStock = 1,
                //stockToConsume = 1,

                //resetCooldownTimerOnUse = false,
                //fullRestockOnAssign = true,
                //dontAllowPastMaxStocks = false,
                //mustKeyPress = false,
                //beginSkillCooldownOnSkillEnd = false,

                //isCombatSkill = true,
                //canceledFromSprinting = false,
                //cancelSprintingOnActivation = false,
                //forceSprintDuringState = false,

            });
            passiveSkillDef1.matchConsumptionCost = 1;
            passiveSkillDef1.matchMaxConsumptions = 1;
            passiveSkillDef1.matchConsumptionMinimum = 1;
            passiveSkillDef1.associatedBuff = CreateMatchBuff(passiveSkillDef1);
            passiveSkillDef1.CustomMatchAction = (matcherGridController, matches) =>
            {
                GameObject currentInteractable = matcherGridController.GetComponent<InteractionDriver>().currentInteractable;
                if (currentInteractable != null && currentInteractable.TryGetComponent(out PurchaseInteraction purchaseInteraction))
                {
                    if (purchaseInteraction.costType == CostTypeIndex.Money)
                    {
                        purchaseInteraction.Networkcost = Mathf.Max(0, purchaseInteraction.Networkcost - matches * Run.instance.GetDifficultyScaledCost(5));
                    }
                }
                return false;
            };

            //MatchBoostedSkillDef passiveSkillDef2 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            //{
            //    skillName = "MatcherMuscle",
            //    skillNameToken = TOKEN_PREFIX + "PASSIVE_NAME",
            //    skillDescriptionToken = TOKEN_PREFIX + "PASSIVE_DESCRIPTION",
            //    keywordTokens = new string[] { "KEYWORD_AGILE" },
            //    skillIcon = assetBundle.LoadAsset<Sprite>("texTileMuscle"),

            //    //unless you're somehow activating your passive like a skill, none of the following is needed.
            //    //but that's just me saying things. the tools are here at your disposal to do whatever you like with

            //    //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
            //    //activationStateMachineName = "Weapon1",
            //    //interruptPriority = EntityStates.InterruptPriority.Skill,

            //    //baseRechargeInterval = 1f,
            //    //baseMaxStock = 1,

            //    //rechargeStock = 1,
            //    //requiredStock = 1,
            //    //stockToConsume = 1,

            //    //resetCooldownTimerOnUse = false,
            //    //fullRestockOnAssign = true,
            //    //dontAllowPastMaxStocks = false,
            //    //mustKeyPress = false,
            //    //beginSkillCooldownOnSkillEnd = false,

            //    //isCombatSkill = true,
            //    //canceledFromSprinting = false,
            //    //cancelSprintingOnActivation = false,
            //    //forceSprintDuringState = false,

            //});
            //passiveSkillDef2.matchConsumptionCost = 1;
            //passiveSkillDef2.matchMaxConsumptions = 1;
            //passiveSkillDef2.matchConsumpationMinimum = 1;
            //passiveSkillDef2.associatedBuff = CreateMatchBuff(passiveSkillDef2);

            Skills.AddSkillsToFamily(genericSkill.skillFamily, passiveSkillDef1/*, passiveSkillDef2*/);
        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "MatcherGrid",
                skillNameToken = TOKEN_PREFIX + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.MatchMenu)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                requiredStock = 0,
                stockToConsume = 0,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = true,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
        }
        #endregion skills

        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.GetComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin

            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(HENRY_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);

            #endregion
            
            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            MatcherContent.AI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;

            On.RoR2.UI.HUD.Awake += HUD_Awake;
        }

        private void HUD_Awake(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            orig(self);
            self.gameObject.AddComponent<MatcherHUDManager>();
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(MatcherContent.Buffs.armorBuff))
            {
                args.armorAdd += 100;
                args.moveSpeedMultAdd += 0.5f;
            }
        }
    }
}