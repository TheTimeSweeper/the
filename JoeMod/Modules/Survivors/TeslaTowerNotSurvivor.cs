using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using JoeMod.ModdedEntityStates.TeslaTrooper;
using HenryMod.Modules.Characters;
using JoeMod.ModdedEntityStates.TeslaTrooper.Tower;

namespace HenryMod.Modules.Survivors {
    internal class TeslaTowerNotSurvivor : CharacterBase {
        internal override string bodyName => "TeslaTower";

        internal override BodyInfo bodyInfo { get; set; } = new BodyInfo {
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

        internal override int mainRendererIndex => 13;

        internal override CustomRendererInfo[] customRendererInfos { get; set; }

        internal override Type characterMainState => typeof(TowerIdleSearch);

        internal override ItemDisplaysBase itemDisplays => new TeslaItemDisplays();

        private static UnlockableDef masterySkinUnlockableDef;

        internal override void InitializeCharacter() {
            base.InitializeCharacter();
            bodyPrefab.AddComponent<TotallyOriginalTrackerComponent>();
            bodyPrefab.AddComponent<TeslaCoilControllerController>();
        }

        internal override void InitializeDoppelganger() {
            base.InitializeDoppelganger();
        }

        internal override void InitializeHitboxes() {
            base.InitializeHitboxes();
        }

        internal override void InitializeSkills() {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            string prefix = FacelessJoePlugin.developerPrefix + "_TESLA_BODY_";

            InitializePrimarySkills(prefix);

            InitializeSecondarySkills(prefix);

            InitializeUtilitySkills(prefix);

            InitializeSpecialSkills(prefix);
        }

        private void InitializePrimarySkills(string prefix) {
            States.entityStates.Add(typeof(Zap));
            SkillDef primarySkillDefZap = Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(Zap)),
                                                                            "Weapon",
                                                                            prefix + "PRIMARY_ZAP_NAME",
                                                                            prefix + "PRIMARY_ZAP_DESCRIPTION",
                                                                            Modules.Assets.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                                            false,
                                                                            false);

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefZap);
        }

        private void InitializeSecondarySkills(string prefix) {
            States.entityStates.Add(typeof(AimBigZap));
            States.entityStates.Add(typeof(BigZap));
            SkillDef bigZapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = prefix + "SECONDARY_BIGZAP_NAME",
                skillNameToken = prefix + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = prefix + "SECONDARY_BIGZAP_DESCRIPTION" + Environment.NewLine,
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

        private void InitializeUtilitySkills(string prefix) {

            States.entityStates.Add(typeof(ShieldZap));
            SkillDef rollSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = prefix + "UTILITY_BARRIER_NAME",
                skillNameToken = prefix + "UTILITY_BARRIER_NAME",
                skillDescriptionToken = prefix + "UTILITY_BARRIER_DESCRIPTION",
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

        private void InitializeSpecialSkills(string prefix) {
            States.entityStates.Add(typeof(DeployTeslaCoil));

            SkillDef teslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = prefix + "SPECIAL_TOWER_NAME",
                skillNameToken = prefix + "SPECIAL_TOWER_NAME",
                skillDescriptionToken = prefix + "SPECIAL_TOWER_DESCRIPTION",
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

        internal override void InitializeSkins() {
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

        private static CharacterModel.RendererInfo[] SkinRendererInfos(CharacterModel.RendererInfo[] defaultRenderers, Material[] materials) {
            CharacterModel.RendererInfo[] newRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(newRendererInfos, 0);

            newRendererInfos[0].defaultMaterial = materials[0];
            newRendererInfos[1].defaultMaterial = materials[1];
            newRendererInfos[instance.mainRendererIndex].defaultMaterial = materials[2];

            return newRendererInfos;
        }
    }
}