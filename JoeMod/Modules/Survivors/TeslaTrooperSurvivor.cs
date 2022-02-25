using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using JoeMod.ModdedEntityStates.TeslaTrooper;
using HenryMod.Modules.Characters;

namespace HenryMod.Modules.Survivors
{
    public class TeslaTrooperSurvivor : SurvivorBase {
        public override string bodyName => "TeslaTrooper";

        public override float sortPosition => 69f;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            armor = 10f,
            armorGrowth = 0f,
            bodyName = "TeslaTrooperBody",
            bodyNameToken = FacelessJoePlugin.developerPrefix + "_TESLA_BODY_NAME",
            bodyColor = new Color(0.8f, 2, 2),
            characterPortrait = Modules.Assets.LoadCharacterIcon("texIconTeslaTrooper"),
            crosshair = Modules.Assets.LoadCrosshair("TiltedBracket"),
            damage = 13f,
            healthGrowth = 33f,
            healthRegen = 1.5f,
            jumpCount = 1,
            maxHealth = 150f,
            subtitleNameToken = FacelessJoePlugin.developerPrefix + "_TESLA_BODY_SUBTITLE",
            podPrefab = Resources.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };

        public override ConfigEntry<bool> characterEnabledConfig => null;

        public const string teslaPrefix = FacelessJoePlugin.developerPrefix + "_TESLA_BODY_";
        public static Material matTeslaBody = Modules.Materials.CreateHotpooMaterial("matTeslaBody");
        public static Material matTeslaArmor = Modules.Materials.CreateHotpooMaterial("matTeslaArmor").SetCull(false);// Modules.Assets.CreateMaterial("matTeslaArmor", 1, new Color(0.28f, 0.70f, 1.0f));

        public override CustomRendererInfo[] customRendererInfos { get; set; }

            = new CustomRendererInfo[]
            {
                new CustomRendererInfo
                {
                    childName = "meshTeslaBody",
                    material = matTeslaBody,
                },
                new CustomRendererInfo
                {
                    childName = "meshTeslaArmor",
                    material = matTeslaArmor,
                },
                new CustomRendererInfo
                {
                    childName = "meshTeslaHammer",
                    material = Materials.CreateHotpooMaterial("MatHammer"),
                },
            };

        public override Type characterMainState => typeof(TeslaTrooperMain);

        public override UnlockableDef characterUnlockableDef => null;

        public override ItemDisplaysBase itemDisplays => new TeslaItemDisplays();

        private static UnlockableDef masterySkinUnlockableDef;

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();
            bodyPrefab.AddComponent<TotallyOriginalTrackerComponent>();
            bodyPrefab.AddComponent<TeslaCoilControllerController>();
        }

        protected override void InitializeEntityStateMachine() {
            base.InitializeEntityStateMachine();

            States.entityStates.Add(typeof(TeslaTrooperMain));
        }

        public override void InitializeUnlockables() {
            masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.TeslaTrooperMastery>();
        }

        public override void InitializeDoppelganger() {
            base.InitializeDoppelganger();
        }

        public override void InitializeHitboxes() {
            base.InitializeHitboxes();
        }

        public override void InitializeSkills() {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            InitializePrimarySkills();

            InitializeSecondarySkills();

            InitializeUtilitySkills();

            InitializeSpecialSkills();
        }

        protected override void InitializeSurvivor() {
            base.InitializeSurvivor();

            InitializeRecolorSkills();
        }

        private void InitializeRecolorSkills() {
            SkillFamily recolorFamily = Modules.Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "Color", true).skillFamily;

            SkillDef red = recolorSkillDef("Red", Color.red);

            SkillDef[] skilldefs = new SkillDef[] {
                recolorSkillDef("Blue", Color.blue),
                recolorSkillDef("Green", Color.green),
                recolorSkillDef("Yellow", Color.yellow),
                recolorSkillDef("Orange", new Color(255f/255f, 156f/255f, 0f)),
                recolorSkillDef("Cyan", Color.cyan),
                recolorSkillDef("Purple", new Color(145f/255f, 0, 200f/255f)),
                recolorSkillDef("Pink", new Color(255f/255f, 132f/255f, 235f/255f)),
            };

            Modules.Skills.AddSkillToFamily(recolorFamily, red);

            for (int i = 0; i < skilldefs.Length; i++) {
                Modules.Skills.AddSkillToFamily(recolorFamily, skilldefs[i], masterySkinUnlockableDef);
            }

            CharacterSelectSurvivorPreviewDisplayController CSSPreviewDisplayConroller = displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();
            CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse[] skillChangeResponses = CSSPreviewDisplayConroller.skillChangeResponses;

            for (int i = 0; i < skillChangeResponses.Length; i++) {

                CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse response = skillChangeResponses[i];
                response.triggerSkillFamily = recolorFamily;
                response.triggerSkill = i == 0 ? red : skilldefs[i-1];
            }
        }

        private SkillDef recolorSkillDef(string name, Color iconColor){

            var thing = characterBodyModel

            return Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = name,
                skillNameToken = $"{teslaPrefix}RECOLOR_{name.ToUpper()}_NAME",
                skillDescriptionToken = "",
                skillIcon = R2API.LoadoutAPI.CreateSkinIcon(iconColor, iconColor, iconColor, iconColor),
            });
        }
        
        private void InitializePrimarySkills()
        {
            States.entityStates.Add(typeof(Zap));
            SkillDef primarySkillDefZap = Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(Zap)),
                                                                            "Weapon",
                                                                            "Tesla_Primary_Zap",
                                                                            teslaPrefix + "PRIMARY_ZAP_NAME",
                                                                            teslaPrefix + "PRIMARY_ZAP_DESCRIPTION",
                                                                            Modules.Assets.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                                            false);

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefZap);
        }

        private void InitializeSecondarySkills()
        {
            States.entityStates.Add(typeof(AimBigZap));
            States.entityStates.Add(typeof(BigZap));
            SkillDef bigZapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Secondary_BigZap",
                skillNameToken = teslaPrefix + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = teslaPrefix + "SECONDARY_BIGZAP_DESCRIPTION",
                skillIcon = Resources.Load<Sprite>("textures/bufficons/texbuffteslaicon"), //Modules.Assets.LoadAsset<Sprite>("skill2_icon"),              //todo .TeslaTrooper
                activationState = new EntityStates.SerializableEntityStateType(typeof(AimBigZap)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 4.5f,
                beginSkillCooldownOnSkillEnd = true,
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

            Modules.Skills.AddSecondarySkills(bodyPrefab, bigZapSkillDef);
        }

        private void InitializeUtilitySkills()
        {

            States.entityStates.Add(typeof(ShieldZap));
            SkillDef rollSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Utility_ShieldZap",
                skillNameToken = teslaPrefix + "UTILITY_BARRIER_NAME",
                skillDescriptionToken = teslaPrefix + "UTILITY_BARRIER_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texTeslaSkillUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ShieldZap)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, rollSkillDef);
        }

        private void InitializeSpecialSkills()
        {
            States.entityStates.Add(typeof(DeployTeslaCoil));

            SkillDef teslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Special_Tower",
                skillNameToken = teslaPrefix + "SPECIAL_TOWER_NAME",
                skillDescriptionToken = teslaPrefix + "SPECIAL_TOWER_DESCRIPTION",
                skillIcon = Resources.Load<Sprite>("textures/itemicons/texteslacoilicon"), //Modules.Assets.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployTeslaCoil)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 15f,
                beginSkillCooldownOnSkillEnd = true,
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

            Modules.Skills.AddSpecialSkills(bodyPrefab, teslaCoilSkillDef);
        }

        public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin

            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(FacelessJoePlugin.developerPrefix + "_TESLA_BODY_DEFAULT_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinDefault"),
                defaultRenderers,
                mainRenderer,
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