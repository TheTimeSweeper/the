using BepInEx.Configuration;
using KatamariMod.Characters.Survivors.Katamari.Components;
using KatamariMod.Modules;
using KatamariMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KatamariMod.Survivors.Plague
{
    public class KatamariSurvivor : SurvivorBase<KatamariSurvivor>
    {
        //todo guide
        //used to load the assetbundle for this character. make sure to rename this
        public override string assetBundleName => "katamaribundle";

        //the name of the prefab we will create. conventionally ending in "Body"
        public override string bodyName => "KatamariBody";

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabName => "mdlKatamari";
        public override string displayPrefabName => "KatamariDisplay";

        public const string PLAGUE_PREFIX = KatamariPlugin.DEVELOPER_PREFIX + "_PLAGUE_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => PLAGUE_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = PLAGUE_PREFIX + "NAME",
            subtitleNameToken = PLAGUE_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texIconPlague"),
            bodyColor = Color.green,
            sortPosition = 69.3f,

            crosshair = Assets.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 2,
        };

        public override UnlockableDef characterUnlockableDef => KatamariUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => new KatamariItemDisplays();

        public override void InitializeCharacter()
        {
            //uncomment if you have multiple characters
            //ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "Henry");

            //if (!characterEnabled.Value)
            //    return;

            //need the character unlockable before you initialize the survivordef
            //KatamariUnlockables.Init();

            base.InitializeCharacter();

            //KatamariConfig.Init();
            //KatamariStates.Init();
            //KatamariTokens.Init();
            
            //KatamariAssets.Init(assetBundle);
            //KatamariBuffs.Init(assetBundle);
     
            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();
            
            AdditionalBodySetup();

            AddHooks();
        }

        protected override void InitializeCharacterBodyPrefab()
        {
            //characterModelObject = Prefabs.LoadCharacterModel(assetBundle, modelPrefabName);
            GameObject bundleBodyPrefab = Prefabs.LoadCharacterBody(assetBundle, "KatamariBody", false);

            bodyPrefab = Modules.Prefabs.CreateBodyPrefab(bundleBodyPrefab, bundleBodyPrefab.GetComponentInChildren<CharacterModel>().gameObject, bodyInfo);

            prefabCharacterBody = bodyPrefab.GetComponent<CharacterBody>();

            prefabCharacterModel = Modules.Prefabs.SetupCharacterModel(bodyPrefab);
        }
        
        private void AdditionalBodySetup()
        {
            AddHitboxes();

            prefabCharacterModel.transform.Find("Katamari").gameObject.AddComponent<RollUp>();

            //todo fail
            bodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Commando.DeathState));
        }

        public void AddHitboxes()
        {
           //ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
           //
           ////example of how to create a hitbox
           //Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
           //Prefabs.SetupHitbox(characterModelObject, hitboxTransform, "Sword");
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
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }
        
        #region skills
        public override void InitializeSkills()
        {
            Skills.ClearGenericSkills(bodyPrefab);

            //Skills.CreateSkillFamilies(bodyPrefab);

            //AddPrmarySkills();
            //AddSecondarySkills();
            //AddUtiitySkills();
            //AddSpecialSkills();
        }
        
        //private void AddPrmarySkills()
        //{
        //    SkillDef slashSkillDef = Skills.CreateSkillDef(new SkillDefInfo
        //        (
        //            "plagueThrow",
        //            PLAGUE_PREFIX + "PRIMARY_THROW_NAME",
        //            PLAGUE_PREFIX + "PRIMARY_THROW_DESCRIPTION",
        //            assetBundle.LoadAsset<Sprite>("texIconSkillPlagueBomb"),
        //            new EntityStates.SerializableEntityStateType(typeof(EntityStates.Commando.CommandoWeapon.FirePistol2)),
        //            "Weapon",
        //            true
        //        ));

        //    Skills.AddPrimarySkills(bodyPrefab, slashSkillDef);
        //}

        //private void AddSecondarySkills()
        //{
        //    SkillDef gunSkillDef = Skills.CreateSkillDef(new SkillDefInfo
        //    {
        //        skillName = "HenryGun",
        //        skillNameToken = PLAGUE_PREFIX + "SECONDARY_GUN_NAME",
        //        skillDescriptionToken = PLAGUE_PREFIX + "SECONDARY_GUN_DESCRIPTION",
        //        keywordTokens = new string[] { "KEYWORD_AGILE" },
        //        skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

        //        activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PaintShotgun)),
        //        activationStateMachineName = "Weapon",
        //        interruptPriority = EntityStates.InterruptPriority.Skill,

        //        baseRechargeInterval = 1f,
        //        baseMaxStock = 1,

        //        rechargeStock = 1,
        //        requiredStock = 1,
        //        stockToConsume = 1,

        //        resetCooldownTimerOnUse = false,
        //        fullRestockOnAssign = true,
        //        dontAllowPastMaxStocks = false,
        //        beginSkillCooldownOnSkillEnd = false,
        //        mustKeyPress = false,

        //        isCombatSkill = true,
        //        canceledFromSprinting = false,
        //        cancelSprintingOnActivation = false,
        //        forceSprintDuringState = false,

        //    });

        //    Skills.AddSecondarySkills(bodyPrefab, gunSkillDef);
        //}

        //private void AddUtiitySkills()
        //{
        //    //here's a skilldef of a typical movement skill. some fields are omitted and will just have default values
        //    SkillDef rollSkillDef = Skills.CreateSkillDef(new SkillDefInfo
        //    {
        //        skillName = "PlagueBlastJump",
        //        skillNameToken = PLAGUE_PREFIX + "UTILITY_ROLL_NAME",
        //        skillDescriptionToken = PLAGUE_PREFIX + "UTILITY_ROLL_DESCRIPTION",
        //        skillIcon = assetBundle.LoadAsset<Sprite>("texIconSkillPlagueBlastJump"),

        //        activationState = new EntityStates.SerializableEntityStateType(typeof(SimpleBlastJump)),
        //        activationStateMachineName = "Body",
        //        interruptPriority = EntityStates.InterruptPriority.Skill,
                
        //        baseMaxStock = 2,
        //        baseRechargeInterval = 8f,

        //        isCombatSkill = false,
        //        mustKeyPress = true,
        //        forceSprintDuringState = true,
        //        cancelSprintingOnActivation = false,
        //    });

        //    Skills.AddUtilitySkills(bodyPrefab, rollSkillDef);
        //}

        //private void AddSpecialSkills()
        //{
        //    PlagueBombSelectionSkillDef bombSkillDef = Skills.CreateSkillDef<PlagueBombSelectionSkillDef>(new SkillDefInfo
        //    {
        //        skillName = "PlagueSelectBomb",
        //        skillNameToken = PLAGUE_PREFIX + "SPECIAL_SELECT_NAME",
        //        skillDescriptionToken = PLAGUE_PREFIX + "SPECIAL_SELECT_DESCRIPTION",
        //        keywordTokens = new string[0],
        //        skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

        //        activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.SelectBomb)),
        //        activationStateMachineName = "Weapon",
        //        interruptPriority = EntityStates.InterruptPriority.Skill,

        //        baseRechargeInterval = 0f,
        //        baseMaxStock = 1,

        //        rechargeStock = 1,
        //        requiredStock = 0,
        //        stockToConsume = 0,

        //        resetCooldownTimerOnUse = false,
        //        fullRestockOnAssign = true,
        //        dontAllowPastMaxStocks = false,
        //        beginSkillCooldownOnSkillEnd = false,
        //        mustKeyPress = true,

        //        isCombatSkill = false,
        //        canceledFromSprinting = false,
        //        cancelSprintingOnActivation = false,
        //        forceSprintDuringState = false,
        //    });


        //    Skills.AddSpecialSkills(bodyPrefab, bombSkillDef);
        //}
        #endregion skills

        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
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
            /*
            //creating a new skindef as we did before
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(PLAGUE_PREFIX + "MASTERY_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                PlagueUnlockables.masterySkinUnlockableDef);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "meshHenrySwordAlt",
                null,//no gun mesh replacement. use same gun mesh
                "meshHenryAlt");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GunModel"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);
            */
            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, "KatamariMonsterMaster", "Merc");

            //how to set up AI in code
            //HenryAI.Init(bodyPrefab);
            
            //how to load a master set up in unity
            //assetBundle.LoadMaster("HenryMonsterMaster", bodyPrefab);
        }

        private void AddHooks()
        {
        }
    }
}