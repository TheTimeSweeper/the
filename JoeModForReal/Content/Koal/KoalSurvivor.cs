using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using ModdedEntityStates.Joe;
using UnityEngine;
using Modules.Characters;
using Modules.Survivors;
using Modules;
using JoeModForReal.Components;
using System.Runtime.CompilerServices;
using ModdedEntityStates.Koal;

namespace JoeModForReal.Content.Survivors {

    internal class KoalSurvivor : SurvivorBase {

        public override string characterName => "Koal";

        public const string KOAL_PREFIX = FacelessJoePlugin.DEV_PREFIX + "_KOAL_";

        public override string survivorTokenPrefix => KOAL_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            bodyName = "KoalBody",
            bodyNameToken = KOAL_PREFIX + "NAME",
            subtitleNameToken = KOAL_PREFIX + "SUBTITLE",
            sortPosition = 69,

            characterPortrait = null, //Modules.Assets.LoadCharacterIcon("joe_icon"),
            bodyColor = Color.cyan/2,

            crosshair = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 120f,
            healthRegen = 2f,
            armor = 20f,

            jumpCount = 1,

            aimOriginPosition = new Vector3(0, 1.3f, 0),
            cameraParamsDepth = -13,

            cameraParamsVerticalOffset = 1.0f,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; }

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => new JoeItemDisplays();

        public override ConfigEntry<bool> characterEnabledConfig => null;

        public override UnlockableDef characterUnlockableDef { get; }
        private static UnlockableDef masterySkinUnlockableDef;

        public override void Initialize() {
            base.Initialize();
        }

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();
            
            ComboRecipeCooker comboComponent = bodyPrefab.AddComponent<ComboRecipeCooker>();

            //in reverse order of priority because combo checker goes through the list and finds the first match
            #region tier3 combos
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalPrimary33),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 1, 1, 0, 0, 0 },
                resetComboHistory = true
            });
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalPrimary32),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 1, 1, 0, 0 },
            });
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalPrimary31),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 1, 1, 0 },
            });

            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalSecondary33),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 0, 0, 1, 1, 1 },
                resetComboHistory = true
            });
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalSecondary32),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 0, 0, 1, 1 }
            });
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalSecondary31),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 0, 0, 1 }
            });
            #endregion

            #region tier1 combos
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalPrimary23),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 1, 0, 0, 0 },
                resetComboHistory = true
            });
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalPrimary22),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 1, 0, 0 },
            });
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalPrimary21),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 1, 0 },
            });

            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalSecondary23),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 0, 1, 1, 1 },
                resetComboHistory = true
            });
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalSecondary22),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 0, 1, 1 }
            });
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalSecondary21),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 0, 1 }
            });
            #endregion

            #region tier0 combos
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalPrimary13),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 0, 0, 0 },
                resetComboHistory = true,
            });
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalPrimary12),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 0, 0 },
            });

            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalSecondary13),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 1, 1, 1 },
                resetComboHistory = true
            });
            comboComponent.comboRecipes.Add(new ComboRecipeCooker.ComboRecipe {
                resultSkillDef = CreateKoalComboSkillDef(
                    typeof(KoalSecondary12),
                    "name",
                    "nametoken",
                    "descriptiontoken",
                    null
                ),
                combo = new List<int> { 1, 1 }
            });
            #endregion
        }

        private SkillDef CreateKoalComboSkillDef(Type stateType, string skillName, string nameToken, string descriptionToken, Sprite sprite) {
            return Skills.CreateSkillDef(new SkillDefInfo {
                activationState = new EntityStates.SerializableEntityStateType(stateType),
                skillName = skillName,
                skillNameToken = nameToken,
                skillDescriptionToken = descriptionToken,
                skillIcon = sprite
            });
        }

        protected override void InitializeCharacterModel() {
            base.InitializeCharacterModel();
        }

        public override void InitializeDoppelganger(string clone) {
            base.InitializeDoppelganger("Loader");
        }
        
        public override void InitializeUnlockables() {
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
        }

        public override void InitializeHitboxes() {

            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Modules.Prefabs.SetupHitbox(model, "primary1Group", "primary1");
        }

        public override void InitializeSkills() {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            InitializePrimarySkills();

            InitializeSecondarySkills();

            InitializeUtilitySkills();

            InitializeSpecialSkills();
        }

        private void InitializePrimarySkills() {

            ComboSkillDef primarySkillDefKoal = Modules.Skills.CreateSkillDef<ComboSkillDef>(
                new SkillDefInfo(KOAL_PREFIX + "koalswingPrimary",
                                 KOAL_PREFIX + "PRIMARY_KOAL_NAME",
                                 KOAL_PREFIX + "PRIMARY_KOAL_DESCRIPTION",
                                 Modules.Asset.LoadAsset<Sprite>("texIconPrimaryJumpSwing"),
                                 new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Koal.KoalPrimary11)),
                                 "Weapon",
                                 true));
            primarySkillDefKoal.mustKeyPress = true;
            primarySkillDefKoal.comboGraceDuration = 2f;

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefKoal);
        }

        private void InitializeSecondarySkills() {
            RepeatableComboSkillDef secondarySkillDefKoal = Modules.Skills.CreateSkillDef<RepeatableComboSkillDef>(new SkillDefInfo {
                skillName = KOAL_PREFIX + "koalSwingSecondary",
                skillNameToken = KOAL_PREFIX + "SECONDARY_KOAL_NAME",
                skillDescriptionToken = KOAL_PREFIX + "SECONDARY_KOAL_DESCRIPTION",
                skillIcon = Modules.Asset.LoadAsset<Sprite>("texIconPrimaryJumpSwing"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Koal.KoalSecondary11)),
                activationStateMachineName = "Slide",
                baseMaxStock = 2,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 0,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });
            secondarySkillDefKoal.comboGraceDuration = 2f;
            secondarySkillDefKoal.stocksToConsumeAfterAllUses = 1;
            secondarySkillDefKoal.maxUsesPerStock = 3;



            Modules.Skills.AddSecondarySkills(bodyPrefab, secondarySkillDefKoal);
        }

        private void InitializeUtilitySkills() {

            SkillDef dashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = KOAL_PREFIX + "UTILITY_DASH_NAME",
                skillNameToken = KOAL_PREFIX + "UTILITY_DASH_NAME",
                skillDescriptionToken = KOAL_PREFIX + "UTILITY_DASH_DESCRIPTION",
                skillIcon = Modules.Asset.LoadAsset<Sprite>("texIconUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Utility1Dash)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, dashSkillDef);
        }

        private void InitializeSpecialSkills() {

            SkillDef tenticlesSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = KOAL_PREFIX + "SPECIAL_TENTICLES_NAME",
                skillNameToken = KOAL_PREFIX + "SPECIAL_TENTICLES_NAME",
                skillDescriptionToken = KOAL_PREFIX + "SPECIAL_TENTICLES_DESCRIPTION",
                skillIcon = Modules.Asset.LoadAsset<Sprite>("texIconSpecial"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Special1Tenticles)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
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
                stockToConsume = 1,
                keywordTokens = new string[] { KOAL_PREFIX + "KEYWORD_TENTICLES" }
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, tenticlesSkillDef);
        }

        public override void InitializeSkins() {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef("DEFAULT_SKIN",
                Asset.LoadAsset<Sprite>("texiconSkinDefault"),
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