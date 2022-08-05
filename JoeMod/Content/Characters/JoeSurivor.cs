using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using ModdedEntityStates.Joe;
using UnityEngine;
using ModdedEntityStates.TeslaTrooper;
using Modules.Characters;

namespace Modules.Survivors
{
    internal class JoeSurivor : SurvivorBase
    {
        public override string bodyName => "Joe";

        public const string JOE_PREFIX = FacelessJoePlugin.DEV_PREFIX + "_JOE_BODY_";

        public override string survivorTokenPrefix => JOE_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            armor = 20f,
            armorGrowth = 0f,
            bodyName = "JoeBody",
            bodyNameToken = JOE_PREFIX + "NAME",
            bodyColor = Color.magenta,
            characterPortrait = Modules.Assets.LoadCharacterIcon("joe_icon"),
            sortPosition = 69f,

            crosshair = Modules.Assets.LoadCrosshair("SimpleDot"),
            damage = 12f,
            healthGrowth = 33f,
            healthRegen = 1.5f,
            jumpCount = 2,
            maxHealth = 110f,
            subtitleNameToken = FacelessJoePlugin.DEV_PREFIX + "_JOE_BODY_SUBTITLE",
            podPrefab = Assets.LoadAsset<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };

        public static Material joeMat => Modules.Materials.CreateHotpooMaterial("0fdsa - Default");
        
        public override CustomRendererInfo[] customRendererInfos { get; set; }

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => null;// new JoeItemDisplays();

        public override UnlockableDef characterUnlockableDef { get; }

        public override ConfigEntry<bool> characterEnabledConfig => Modules.Config.CharacterEnableConfig(bodyName, false, "Very in-dev test character that was used as a basis for tesla trooper. Enable for fun");

        private static UnlockableDef masterySkinUnlockableDef;

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();

            bodyPrefab.AddComponent<TeslaTrackerComponent>();
        }

        protected override void InitializeCharacterModel() {
            setRetardedRendererInfosAndChildTransforms();
            base.InitializeCharacterModel();
        }

        //you ready for some stupid shit?
        private void setRetardedRendererInfosAndChildTransforms() {

            List<CustomRendererInfo> customInfos = new List<CustomRendererInfo>();

            FacelessJoe.CharacterRenderers retardedRenderersComponent = bodyPrefab.GetComponentInChildren<FacelessJoe.CharacterRenderers>();
            retardedRenderersComponent.setHoopooShaders();

            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            List<ChildLocator.NameTransformPair> pairs = new List<ChildLocator.NameTransformPair>(childLocator.transformPairs);

            
            for (int i = 0; i < retardedRenderersComponent.Renderers.Count; i++) {
                Renderer rend = retardedRenderersComponent.Renderers[i];

                pairs.Add(new ChildLocator.NameTransformPair {
                    name = rend.name,
                    transform = rend.transform
                });


                customInfos.Add(new CustomRendererInfo {
                    childName = rend.name,
                    material = rend.material,
                });
            }

            pairs.Add(new ChildLocator.NameTransformPair { 
                name = retardedRenderersComponent.MainSkinnedMeshRenderer.name,
                transform = retardedRenderersComponent.MainSkinnedMeshRenderer.transform,
            });

            customInfos.Add(new CustomRendererInfo {
                childName = retardedRenderersComponent.MainSkinnedMeshRenderer.name,
                material = joeMat,
            });

            childLocator.transformPairs = pairs.ToArray();
            customRendererInfos = customInfos.ToArray();
        }

        public override void InitializeUnlockables()
        {
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
        }

        public override void InitializeHitboxes()
        {
            //hitboxes already set up baybee
            return;

            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            Modules.Prefabs.SetupHitbox(model, "Sword", hitboxTransform);
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            string prefix = FacelessJoePlugin.DEV_PREFIX + "_JOE_BODY_";


            #region Primary

            States.entityStates.Add(typeof(Primary1Swing));
            States.entityStates.Add(typeof(Primary1JumpSwingFall));
            States.entityStates.Add(typeof(Primary1JumpSwingLand));
            
            SkillDef primarySkillDef = Modules.Skills.CreateSkillDef( new SkillDefInfo("JoeSwing",
                                                                prefix + "PRIMARY_SWING_NAME",
                                                                prefix + "PRIMARY_SWING_DESCRIPTION",
                                                                Modules.Assets.LoadAsset<Sprite>("skill1_icon"),
                                                                new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.Primary1Swing)),
                                                                "Weapon",
                                                                true));

            States.entityStates.Add(typeof(PrimaryStupidSwing));
            SkillDef primarySkillDefSilly = Modules.Skills.CreateSkillDef(new SkillDefInfo("JoeSwingClassic",
                                                                     prefix + "PRIMARY_SWING_NAME_CLASSIC",
                                                                     prefix + "PRIMARY_SWING_DESCRIPTION",
                                                                     Modules.Assets.LoadAsset<Sprite>("skill1_icon"),
                                                                     new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.PrimaryStupidSwing)),
                                                                     "Weapon",
                                                                     true) { 
                mustKeyPress = true 
            });

            States.entityStates.Add(typeof(ThrowBoom));
            SkillDef primarySkillDefBomeb = Modules.Skills.CreateSkillDef(new SkillDefInfo("joeBomb",
                                                                     prefix + "PRIMARY_BOMB_NAME",
                                                                     prefix + "PRIMARY_BOMB_DESCRIPTION",
                                                                     null, //Modules.Assets.LoadAsset<Sprite>("skill1_icon"),
                                                                     new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.ThrowBoom)),
                                                                     "Weapon",
                                                                     false));

            States.entityStates.Add(typeof(ThroBoomButCoolerQuestionMaark));
            SkillDef primarySkillDefBomebe = Modules.Skills.CreateSkillDef(new SkillDefInfo("joeBomb2",
                                                                      prefix + "PRIMARY_BOMB_NAME",
                                                                      prefix + "PRIMARY_BOMB_DESCRIPTION",
                                                                      null, //Modules.Assets.LoadAsset<Sprite>("skill1_icon"),
                                                                      new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.ThroBoomButCoolerQuestionMaark)),
                                                                      "Weapon",
                                                                      false));

            States.entityStates.Add(typeof(Zap));
            SkillDef primarySkillDefZap = Modules.Skills.CreateSkillDef(new SkillDefInfo("joeZap",
                                                                   prefix + "PRIMARY_ZAP_NAME",
                                                                   prefix + "PRIMARY_ZAP_DESCRIPTION",
                                                                   null, //Modules.Assets.LoadAsset<Sprite>("skill1_icon"),
                                                                   new EntityStates.SerializableEntityStateType(typeof(Zap)),
                                                                   "Weapon",
                                                                   false));

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefBomeb, primarySkillDefBomebe, primarySkillDef, primarySkillDefSilly, primarySkillDefZap);
            #endregion
            
            #region Secondary

            States.entityStates.Add(typeof(ThrowBoom));
            SkillDef fireballSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = prefix + "SECONDARY_FIREBALL_NAME",
                skillNameToken = prefix + "SECONDARY_FIREBALL_NAME",
                skillDescriptionToken = prefix + "SECONDARY_FIREBALL_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("skill2_icon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Joe.Secondary1Fireball)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
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


            States.entityStates.Add(typeof(AimBigZap));
            States.entityStates.Add(typeof(BigZap));
            SkillDef bigZapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo { 
                skillName = prefix + "SECONDARY_BIGZAP_NAME",
                skillNameToken = prefix + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = prefix + "SECONDARY_BIGZAP_DESCRIPTION",
                skillIcon = null,//Modules.Assets.LoadAsset<Sprite>("skill2_icon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(AimBigZap)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, fireballSkillDef, bigZapSkillDef);

            #endregion

            #region Utility

            SkillDef rollSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = prefix + "UTILITY_DASH_NAME",
                skillNameToken = prefix + "UTILITY_DASH_NAME",
                skillDescriptionToken = prefix + "UTILITY_DASH_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texUtilityIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Henry.Roll)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, rollSkillDef);

            #endregion

            #region Special

            States.entityStates.Add(typeof(DeployTeslaTower));

            SkillDef teslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SPECIAL_TOWER_NAME",
                skillNameToken = prefix + "SPECIAL_TOWER_NAME",
                skillDescriptionToken = prefix + "SPECIAL_TOWER_DESCRIPTION",
                skillIcon = null,//Modules.Assets.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployTeslaTower)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 0
            });

            SkillDef bombSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = prefix + "SPECIAL_BOMB_NAME",
                skillNameToken = prefix + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Henry.ThrowBomb)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, bombSkillDef, teslaCoilSkillDef);
            #endregion
        }

        public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(FacelessJoePlugin.DEV_PREFIX + "_JOE_BODY_DEFAULT_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texMainSkin"),
                defaultRenderers,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
            };

            skins.Add(defaultSkin);
            #endregion

            #region MasterySkin
            //Material masteryMat = Modules.Assets.CreateMaterial("matHenryAlt");
            //CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            //{
            //    masteryMat,
            //    masteryMat,
            //    masteryMat,
            //    masteryMat
            //});

            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(FacelessJoePlugin.developerPrefix + "_HENRY_BODY_MASTERY_SKIN_NAME",
            //    Assets.LoadAsset<Sprite>("texMasteryAchievement"),
            //    masteryRendererInfos,
            //    mainRenderer,
            //    model,
            //    masterySkinUnlockableDef);

            //masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            //{
            //    //new SkinDef.MeshReplacement
            //    //{
            //    //    mesh = Modules.Assets.LoadAsset<Mesh>("meshHenrySwordAlt"),
            //    //    renderer = defaultRenderers[0].renderer
            //    //},
            //    //new SkinDef.MeshReplacement
            //    //{
            //    //    mesh = Modules.Assets.LoadAsset<Mesh>("meshHenryAlt"),
            //    //    renderer = defaultRenderers[instance.mainRendererIndex].renderer
            //    //}
            //};

            //skins.Add(masterySkin);
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}