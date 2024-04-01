using BepInEx.Configuration;
using EntityStates;
using EntityStates.Engi.Mine;
using HellDiverMod.General.Components;
using HellDiverMod.Modules;
using HellDiverMod.Modules.Characters;
using HellDiverMod.Survivors.HellDiver.Components;
using HellDiverMod.Survivors.HellDiver.Components.UI;
using HellDiverMod.Survivors.HellDiver.SkillDefs;
using HellDiverMod.Survivors.HellDiver.SkillStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HellDiverMod.Survivors.HellDiver
{
    public class HellDiverSurvivor : SurvivorBase<HellDiverSurvivor>
    {
        public override string assetBundleName => "helldiverbundle";

        public override string bodyName => "HellDiverBody";
        
        public override string masterName => "HellDiverMonsterMaster";

        public override string modelPrefabName => "mdlJoe";
        public override string displayPrefabName => "JoeDisplay";

        public const string HELLDIVER_PREFIX = HellDiverPlugin.DEVELOPER_PREFIX + "_HELLDIVER_";

        public override string survivorTokenPrefix => HELLDIVER_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = HELLDIVER_PREFIX + "NAME",
            subtitleNameToken = HELLDIVER_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texIconGI"),
            bodyColor = Color.blue,
            sortPosition = 69.6f,

            //crosshairBundlePath = "GICrosshair",
            crosshairAddressablePath = "RoR2/Base/UI/StandardCrosshair.prefab",
            podPrefabAddressablePath = "RoR2/Base/SurvivorPod/SurvivorPod.prefab",

            maxHealth = 140f,
            healthRegen = 2.0f,
            armor = 10f,

            jumpCount = 1,
        };

        public override UnlockableDef characterUnlockableDef => null;// GIUnlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays { get; } = new HellDiverMod.General.JoeItemDisplays();

        public static SkillDef throwStratagemSkillDef;

        public override void Initialize()
        {
            //if (!General.GeneralConfig.GIEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            //GIUnlockables.Init();

            base.InitializeCharacter();
        }

        public override void OnCharacterInitialized()
        {
            //Config.ConfigureBody(prefabCharacterBody, GIConfig.SectionBody);

            //GIConfig.Init();

            //GIStates.Init();
            //GITokens.Init();

            HellDiverAssets.Init(assetBundle);
            //GIBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }
        
        private void AdditionalBodySetup()
        {
            //AddHitboxes();
            bodyPrefab.AddComponent<StratagemInputController>();
        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //if you set up a custom main characterstate, set it up here
                //don't forget to register custom entitystates in your HenryStates.cs
            //the main "body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Dive");
        }

        #region skills
        public override void InitializeSkills()
        {
            Skills.ClearGenericSkills(bodyPrefab);

            Skills.CreateSkillFamilies(bodyPrefab);
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();
            AddStratagemSkills();

            throwStratagemSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HellDiverThrowStratagem",
                skillNameToken = HELLDIVER_PREFIX + "OVERRIDE_THROW_NAME",
                skillDescriptionToken = HELLDIVER_PREFIX + "OVERRIDE_THROW_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_STUNNING" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texIconChronoPrimary"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(ThrowStratagem)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Any,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });
        }

        private void AddPrimarySkills()
        {
            SkillDef primarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
                (
                    "HellDiverGun1",
                    HELLDIVER_PREFIX + "PRIMARY_GUN_NAME",
                    HELLDIVER_PREFIX + "PRIMARY_GUN_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FirePistol2)),
                    "Weapon",
                    false
                ));

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private void AddSecondarySkills()
        {
            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HellDiverGrenade1",
                skillNameToken = HELLDIVER_PREFIX + "SECONDARY_CALTROPS_NAME",
                skillDescriptionToken = HELLDIVER_PREFIX + "SECONDARY_CALTROPS_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_STUNNING" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                
                activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.ThrowGrenade)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,

            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);
        }

        private void AddUtiitySkills()
        {
            //here's a skilldef of a typical movement skill.
            SkillDefInfo skillDefInfo = new SkillDefInfo
            {
                skillName = "HellDive",
                skillNameToken = HELLDIVER_PREFIX + "UTILITY_SLIDE_NAME",
                skillDescriptionToken = HELLDIVER_PREFIX + "UTILITY_SLIDE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Commando.DodgeState)),
                activationStateMachineName = "Dive",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 6f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            };
            var utilitySkillDef1 = Skills.CreateSkillDef(skillDefInfo);

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
        }

        private void AddSpecialSkills()
        {
            StratagemComponentSkillDef specialSkillDef1 = Skills.CreateSkillDef<StratagemComponentSkillDef>(new SkillDefInfo
            {
                skillName = "HellDiverStratagem",
                skillNameToken = HELLDIVER_PREFIX + "SPECIAL_DEPLOY_NAME",
                skillDescriptionToken = HELLDIVER_PREFIX + "SPECIAL_DEPLOY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(InputStratagem)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                 
                baseMaxStock = 1,
                baseRechargeInterval = 0f,

                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0,

                isCombatSkill = false,
                mustKeyPress = true,
                fullRestockOnAssign = false,
                beginSkillCooldownOnSkillEnd = true,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
        }

        private void AddStratagemSkills()
        {
            StratagemSkillDef OrbitalPrecisionStrike = Skills.CreateSkillDef<StratagemSkillDef>(new SkillDefInfo
            {
                skillName = "HellDiverOrbital1",
                skillNameToken = HELLDIVER_PREFIX + "STRATAGEM_ORBITAL1_NAME",
                skillDescriptionToken = HELLDIVER_PREFIX + "STRATAGEM_ORBITAL1_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_STUNNING" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(BootlegOrbitalPrecisionStrike)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });
            OrbitalPrecisionStrike.sequence = new StratagemInput[] { 
                StratagemInput.UP, 
                StratagemInput.RIGHT, 
                StratagemInput.RIGHT 
            };

            StratagemSkillDef OrbitalPrecisionStrike2 = Skills.CreateSkillDef<StratagemSkillDef>(new SkillDefInfo
            {
                skillName = "HellDiverOrbital2",
                skillNameToken = HELLDIVER_PREFIX + "STRATAGEM_ORBITAL2_NAME",
                skillDescriptionToken = HELLDIVER_PREFIX + "STRATAGEM_ORBITAL2_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_STUNNING" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(BootlegOrbitalPrecisionStrike)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });
            OrbitalPrecisionStrike2.sequence = new StratagemInput[] { 
                StratagemInput.UP, 
                StratagemInput.RIGHT, 
                StratagemInput.LEFT 
            };

            StratagemSkillDef OrbitalPrecisionStrike3 = Skills.CreateSkillDef<StratagemSkillDef>(new SkillDefInfo
            {
                skillName = "HellDiverOrbital3",
                skillNameToken = HELLDIVER_PREFIX + "STRATAGEM_ORBITAL3_NAME",
                skillDescriptionToken = HELLDIVER_PREFIX + "STRATAGEM_ORBITAL3_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_STUNNING" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(BootlegOrbitalPrecisionStrike)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });
            OrbitalPrecisionStrike3.sequence = new StratagemInput[] { 
                StratagemInput.UP, 
                StratagemInput.RIGHT, 
                StratagemInput.DOWN, 
                StratagemInput.RIGHT 
            };

            StratagemSkillDef OrbitalPrecisionStrike4 = Skills.CreateSkillDef<StratagemSkillDef>(new SkillDefInfo
            {
                skillName = "HellDiverOrbital4",
                skillNameToken = HELLDIVER_PREFIX + "STRATAGEM_ORBITAL4_NAME",
                skillDescriptionToken = HELLDIVER_PREFIX + "STRATAGEM_ORBITAL4_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_STUNNING" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(BootlegOrbitalPrecisionStrike)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });
            OrbitalPrecisionStrike4.sequence = new StratagemInput[] { 
                StratagemInput.UP, 
                StratagemInput.RIGHT, 
                StratagemInput.UP, 
                StratagemInput.LEFT 
            };
            
            for (int i = 0; i < 4; i++)
            {
                SkillFamily family = Skills.CreateSkillFamily(bodyPrefab + "StratagemFamily" + i);

                Skills.AddSkillsToFamily(family, OrbitalPrecisionStrike, OrbitalPrecisionStrike2, OrbitalPrecisionStrike3, OrbitalPrecisionStrike4);

                Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "Stratagem" + i, family, true);
            }
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
            Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            //GIAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            On.RoR2.UI.HUD.Awake += HUD_Awake;
        }

        private void HUD_Awake(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            orig(self);
            self.gameObject.AddComponent<HellDiverHUDManager>();
        }
    }
}