using BepInEx.Configuration;
using Modules;
using Modules.Characters;
using Modules.Survivors;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AliemMod.Content.Survivors {

    internal class AliemSurvivor : SurvivorBase {
        public override string bodyName => "Aliem";

        public const string ALIEM_PREFIX = AliemPlugin.DEV_PREFIX + "_ALIEM_BODY_";
        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => ALIEM_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            bodyPrefabName = "AliemBody",
            bodyNameToken = ALIEM_PREFIX + "NAME",
            subtitleNameToken = ALIEM_PREFIX + "SUBTITLE",

            characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("texHenryIcon"),
            bodyColor = Color.yellow,

            crosshair = Assets.LoadCrosshair("Standard"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 1,

            aimOriginPosition = new Vector3(0, 1.3f, 0)
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                },
                new CustomRendererInfo
                {
                    childName = "GunModel",
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                }
        };

        public override UnlockableDef characterUnlockableDef => null;

        public override Type characterMainState => typeof(ModdedEntityStates.Aliem.AliemCharacterMain);

        public override ItemDisplaysBase itemDisplays => null;// new AliemItemDisplays();

        //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef masterySkinUnlockableDef;

        public override void InitializeCharacter() {
            base.InitializeCharacter();
        }

        public override void InitializeUnlockables() {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>();
        }

        public override void InitializeHitboxes() {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            //example of how to create a hitbox
            Transform swordHitbox = childLocator.FindChild("SwordHitbox");
            Prefabs.SetupHitbox(model, swordHitbox, "Sword");

            Transform leapHitbox = childLocator.FindChild("LeapHitbox");
            Prefabs.SetupHitbox(model, swordHitbox, "Leap");
        }
    

        public override void InitializeSkills() {
            Skills.CreateSkillFamilies(bodyPrefab, 3);

            #region Primary
            //Creates a skilldef for a typical primary 
            SkillDef primarySkillDef = Skills.CreateSkillDef(new SkillDefInfo("aliem_primary_gun",
                                                                              ALIEM_PREFIX + "PRIMARY_GUN_NAME",
                                                                              ALIEM_PREFIX + "PRIMARY_GUN_DESCRIPTION",
                                                                              Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                                                                              new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunInputs)),
                                                                              "Slide",
                                                                              true));
            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);
            #endregion

            #region Secondary
            
            SkillDef leapSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_secondary_leap",
                skillNameToken = ALIEM_PREFIX + "SECONDARY_LEAP_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SECONDARY_LEAP_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.AliemLeap)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Skills.AddSecondarySkills(bodyPrefab, leapSkillDef);
            #endregion

            #region Utility
            //SkillDef rollSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
            //    skillName = prefix + "_HENRY_BODY_UTILITY_ROLL_NAME",
            //    skillNameToken = prefix + "_HENRY_BODY_UTILITY_ROLL_NAME",
            //    skillDescriptionToken = prefix + "_HENRY_BODY_UTILITY_ROLL_DESCRIPTION",
            //    skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
            //    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Roll)),
            //    activationStateMachineName = "Body",
            //    baseMaxStock = 1,
            //    baseRechargeInterval = 4f,
            //    beginSkillCooldownOnSkillEnd = false,
            //    canceledFromSprinting = false,
            //    forceSprintDuringState = true,
            //    fullRestockOnAssign = true,
            //    interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
            //    resetCooldownTimerOnUse = false,
            //    isCombatSkill = false,
            //    mustKeyPress = false,
            //    cancelSprintingOnActivation = false,
            //    rechargeStock = 1,
            //    requiredStock = 1,
            //    stockToConsume = 1
            //});

            //Skills.AddUtilitySkills(bodyPrefab, rollSkillDef);
            #endregion

            #region Special
            //SkillDef bombSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
            //    skillName = prefix + "_HENRY_BODY_SPECIAL_BOMB_NAME",
            //    skillNameToken = prefix + "_HENRY_BODY_SPECIAL_BOMB_NAME",
            //    skillDescriptionToken = prefix + "_HENRY_BODY_SPECIAL_BOMB_DESCRIPTION",
            //    skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
            //    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowBomb)),
            //    activationStateMachineName = "Slide",
            //    baseMaxStock = 1,
            //    baseRechargeInterval = 10f,
            //    beginSkillCooldownOnSkillEnd = false,
            //    canceledFromSprinting = false,
            //    forceSprintDuringState = false,
            //    fullRestockOnAssign = true,
            //    interruptPriority = EntityStates.InterruptPriority.Skill,
            //    resetCooldownTimerOnUse = false,
            //    isCombatSkill = true,
            //    mustKeyPress = false,
            //    cancelSprintingOnActivation = true,
            //    rechargeStock = 1,
            //    requiredStock = 1,
            //    stockToConsume = 1
            //});

            //Skills.AddSpecialSkills(bodyPrefab, bombSkillDef);
            #endregion
        }

        public override void InitializeSkins() {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef(ALIEM_PREFIX + "DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                model);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
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
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(AliemPlugin.DEV_PREFIX + "_HENRY_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                defaultRendererinfos,
                model,
                masterySkinUnlockableDef);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "meshHenrySwordAlt",
                null,//no gun mesh replacement. use same gun mesh
                "meshHenryAlt");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            masterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            masterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            masterySkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");

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
    }
}