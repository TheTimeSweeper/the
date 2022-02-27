using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using JoeMod.ModdedEntityStates.TeslaTrooper;
using HenryMod.Modules.Characters;
using JoeMod.ModdedEntityStates.TeslaTrooper.Tower;
using EntityStates;
using R2API;
using RoR2.CharacterAI;

namespace HenryMod.Modules.Survivors {
    public class TeslaTowerNotSurvivor : CharacterBase {

        public override string bodyName => "TeslaTower";

        public const string TOWER_PREFIX = FacelessJoePlugin.DEV_PREFIX + "_TESLA_TOWER_BODY_";

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            armor = 1200f,
            armorGrowth = 0f,
            bodyName = "TeslaTowerBody",
            bodyNameToken = TOWER_PREFIX + "NAME",
            bodyNameToClone = "EngiTurret",
            bodyColor = new Color(0.8f, 2, 2),
            characterPortrait = Modules.Assets.LoadCharacterIcon("texIconTeslaTower"),
            crosshair = Modules.Assets.LoadCrosshair("TiltedBracket"),
            damage = 24f,
            healthGrowth = 33f,
            healthRegen = 1.5f,
            jumpCount = 0,
            maxHealth = 200f,
            subtitleNameToken = FacelessJoePlugin.DEV_PREFIX + "_TESLA_TOWER_BODY_SUBTITLE",
            podPrefab = null,
            moveSpeed = 0,
            
            aimOriginPosition = new Vector3( 0, 10, 0),
            cameraPivotPosition = new Vector3(0, 9, 0),
            cameraParamsVerticalOffset = 20,
            cameraParamsDepth= -20
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; }
        
        public override Type characterMainState => typeof(TowerIdleSearch);
        public override Type characterSpawnState => typeof(TowerSpawnState);

        public override ItemDisplaysBase itemDisplays => new TeslaTowerItemDisplays();

        public static GameObject masterPrefab;

        public override void InitializeCharacter() {
            base.InitializeCharacter();
            bodyPrefab.AddComponent<TotallyOriginalTrackerComponent>();
            //bodyPrefab.AddComponent<TeslaCoilControllerController>();

        }

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();

            bodyPrefab.GetComponent<SfxLocator>().deathSound = "Play_building_uselbuil";
            bodyPrefab.GetComponent<SfxLocator>().aliveLoopStart = ""; //todo sfx

            UnityEngine.Object.Destroy(bodyPrefab.GetComponent<SetStateOnHurt>());
            UnityEngine.Object.Destroy(bodyPrefab.GetComponent<AkEvent>());

        }

        protected override void InitializeEntityStateMachine() {
            base.InitializeEntityStateMachine();

            UnityEngine.Object.Destroy(EntityStateMachine.FindByCustomName(bodyPrefab, "Weapon"));
            Array.Resize(ref bodyPrefab.GetComponent<NetworkStateMachine>().stateMachines, 1);
            Array.Resize(ref bodyPrefab.GetComponent<CharacterDeathBehavior>().idleStateMachine, 0);

            bodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new SerializableEntityStateType(typeof(TowerSell));
            States.entityStates.Add(typeof(TowerSell));
        }

        protected override void InitializeCharacterMaster() {
            base.InitializeCharacterMaster();

            masterPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/EngiTurretMaster"), "TeslaTowerMaster", true);
            masterPrefab.GetComponent<CharacterMaster>().bodyPrefab = bodyPrefab;

            foreach (AISkillDriver aiSkillDriver in masterPrefab.GetComponents<AISkillDriver>()) {
                UnityEngine.Object.Destroy(aiSkillDriver);
                //todo: proper ai?
            }
            masterPrefab.GetComponent<BaseAI>().skillDrivers = new AISkillDriver[0];

            Modules.Prefabs.masterPrefabs.Add(masterPrefab);
        }

        #region skills

        public override void InitializeSkills() {          //maybe least elegant of my solutions but came with a DRY fix so half and half
            Modules.Skills.CreateSkillFamilies(bodyPrefab, 3);

            InitializePrimarySkills();

            InitializeSecondarySkills();
        }

        private void InitializePrimarySkills() {
            States.entityStates.Add(typeof(TowerZap));
            SkillDef primarySkillDefZap = Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(TowerZap)),
                                                                            "Weapon",
                                                                            "Tower_Primary_Zap",
                                                                            TOWER_PREFIX + "PRIMARY_ZAP_NAME",
                                                                            TOWER_PREFIX + "PRIMARY_ZAP_DESCRIPTION",
                                                                            Modules.Assets.LoadAsset<Sprite>("texTeslaTowerSkillPrimary"),
                                                                            false);

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefZap);
        }

        private void InitializeSecondarySkills() {
            States.entityStates.Add(typeof(TowerBigZap));
            SkillDef bigZapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tower_Secondary_BigZap",
                skillNameToken = TOWER_PREFIX + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = TOWER_PREFIX + "SECONDARY_BIGZAP_DESCRIPTION" + Environment.NewLine,
                skillIcon = Resources.Load<Sprite>("textures/bufficons/texbuffteslaicon"), //Modules.Assets.LoadAsset<Sprite>("skill2_icon"),              //todo .TeslaTrooper
                activationState = new EntityStates.SerializableEntityStateType(typeof(TowerBigZap)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 9f,
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
                keywordTokens = new string[] { "KEYWORD_SHOCKING" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, bigZapSkillDef);
        }
#endregion skills

        public override void InitializeSkins() {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin

            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(FacelessJoePlugin.DEV_PREFIX + "_TESLA_TOWER_BODY_DEFAULT_SKIN_NAME",
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