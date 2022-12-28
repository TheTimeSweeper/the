using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using ModdedEntityStates.TeslaTrooper;
using Modules.Characters;
using ModdedEntityStates.TeslaTrooper.Tower;
using EntityStates;
using R2API;
using RoR2.CharacterAI;
using TeslaTrooper;

namespace Modules.Survivors {

    internal class TeslaTowerNotSurvivor : CharacterBase {

        public override string bodyName => "TeslaTower";

        public const string TOWER_PREFIX = TeslaTrooperPlugin.DEV_PREFIX + "_TESLA_TOWER_BODY_";

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            armor = 1200f,
            armorGrowth = 0f,
            bodyName = "TeslaTowerBody",
            bodyNameToken = TOWER_PREFIX + "NAME",
            subtitleNameToken = TeslaTrooperPlugin.DEV_PREFIX + "_TESLA_TOWER_BODY_SUBTITLE",
            bodyNameToClone = "EngiTurret",
            bodyColor = new Color(134f / 216f, 234f / 255f, 255f / 255f), //new Color(115f/216f, 216f/255f, 0.93f),
            characterPortrait = Modules.Assets.LoadCharacterIcon("texIconTeslaTower"),
            crosshair = Modules.Assets.LoadCrosshair("TiltedBracket"),
            podPrefab = null,
            bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes | CharacterBody.BodyFlags.Mechanical,

            maxHealth = 200f,
            healthRegen = 0f,
            damage = 12f,
            moveSpeed = 0,
            jumpCount = 0,
            
            //todo camera stuck in tower when you play as it
            aimOriginPosition = new Vector3( 0, 10, 0),
            cameraPivotPosition = new Vector3(0, 5, 0),
            cameraParamsVerticalOffset = 10,
            cameraParamsDepth= -20
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; }
        
        public override Type characterMainState => typeof(TowerIdleSearch);
        public override Type characterSpawnState => typeof(TowerSpawnState);

        public override ItemDisplaysBase itemDisplays => Modules.Config.TowerItemDisplays ? new TeslaTowerItemDisplays() : null;

        public static GameObject masterPrefab;
        internal static SkinDef.MinionSkinReplacement MasteryMinionSkinReplacement;
        internal static SkinDef.MinionSkinReplacement MCMinionSkinReplacement;
        internal static SkinDef.MinionSkinReplacement NodMinionSkinReplacement;

        public override void InitializeCharacter() {
            base.InitializeCharacter();

            bodyPrefab.AddComponent<TowerWeaponComponent>();
            bodyPrefab.AddComponent<TowerOwnerTrackerComponent>();
        }

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();

            bodyPrefab.GetComponent<CharacterBody>().overrideCoreTransform = bodyCharacterModel.GetComponent<ChildLocator>().FindChild("Head");

            bodyPrefab.GetComponent<SfxLocator>().deathSound = "Play_building_uselbuil";
            bodyPrefab.GetComponent<SfxLocator>().aliveLoopStart = "";

            UnityEngine.Object.Destroy(bodyPrefab.GetComponent<SetStateOnHurt>());
            UnityEngine.Object.Destroy(bodyPrefab.GetComponent<AkEvent>());

            bodyCharacterModel.GetComponent<ChildLocator>().FindChild("LightningParticles").GetComponent<ParticleSystemRenderer>().material = Assets.ChainLightningMaterial;
        }

        protected override void InitializeEntityStateMachine() {
            base.InitializeEntityStateMachine();

            //UnityEngine.Object.Destroy(EntityStateMachine.FindByCustomName(bodyPrefab, "Weapon"));
            //Array.Resize(ref bodyPrefab.GetComponent<NetworkStateMachine>().stateMachines, 1);
            //Array.Resize(ref bodyPrefab.GetComponent<CharacterDeathBehavior>().idleStateMachine, 0);
            EntityStateMachine entityStateMachine = EntityStateMachine.FindByCustomName(bodyPrefab, "Weapon");
            entityStateMachine.initialStateType = new SerializableEntityStateType(typeof(TowerLifetime));
            entityStateMachine.mainStateType = new SerializableEntityStateType(typeof(TowerLifetime));

            bodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new SerializableEntityStateType(typeof(TowerSell));
        }

        protected override void InitializeCharacterMaster() {
            base.InitializeCharacterMaster();

            masterPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/EngiTurretMaster"), "TeslaTowerMaster", true);
            masterPrefab.GetComponent<CharacterMaster>().bodyPrefab = bodyPrefab;

            foreach (AISkillDriver aiSkillDriver in masterPrefab.GetComponents<AISkillDriver>()) {
                UnityEngine.Object.Destroy(aiSkillDriver);
                //todo: proper ai?
            }
            masterPrefab.GetComponent<BaseAI>().skillDrivers = new AISkillDriver[0];

            Modules.Content.AddMasterPrefab(masterPrefab);
        }

        #region skills

        public override void InitializeSkills() {          //maybe least elegant of my solutions but it's a DRY fix so half and half
            Modules.Skills.CreateSkillFamilies(bodyPrefab, 3);

            InitializePrimarySkills();

            InitializeSecondarySkills();
        }

        private void InitializePrimarySkills() {
            SkillDef primarySkillDefZap = Modules.Skills.CreateSkillDef(new SkillDefInfo("Tower_Primary_Zap",
                                                                                         TOWER_PREFIX + "PRIMARY_ZAP_NAME",
                                                                                         TOWER_PREFIX + "PRIMARY_ZAP_DESCRIPTION",
                                                                                         Modules.Assets.LoadAsset<Sprite>("texIconTeslaTower"),
                                                                                         new EntityStates.SerializableEntityStateType(typeof(TowerZap)),
                                                                                         "Body",
                                                                                         false));

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefZap);
        }

        private void InitializeSecondarySkills() {
            SkillDef bigZapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tower_Secondary_BigZap",
                skillNameToken = TOWER_PREFIX + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = TOWER_PREFIX + "SECONDARY_BIGZAP_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texTeslaSkillSecondaryThunderclap"),
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

            ModelSkinController skinController = bodyCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = bodyCharacterModel.gameObject.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRenderers = bodyCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            List<GameObject> activatedGameObjects = Skins.createAllActivatedGameObjectsList(childLocator,
                "Tower_Base_Pillars_Color",
                "Tower_Base_Platform",
                "Tower_Base_Center",
                "Tower_Base_Tubes",
                 
                "Tower_Circles",//4
                "Tower_Pole",
                "Tower_Pole_Tracer",
                "Tower_Emission",
                "Tower_Orb",
                
                "LightningParticles");//9

            #region DefaultSkin

            TowerSkinDef defaultSkin = Modules.Skins.CreateSkinDef<TowerSkinDef>("DEFAULT_SKIN",
                Assets.LoadAsset<Sprite>("texTeslaSkinDefault"),
                defaultRenderers,
                bodyCharacterModel.gameObject);

            defaultSkin.gameObjectActivations = Skins.getGameObjectActivationsFromList(activatedGameObjects,
                0, 
                1,
                2,
                3, 
                4, 
                5, 
                6, 
                //7,
                8,
                9);

            skins.Add(defaultSkin);
            #endregion

            #region mastery

            TowerSkinDef masterySkin = Modules.Skins.CreateSkinDef<TowerSkinDef>(TOWER_PREFIX + "MASTERY_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinDefault"),
                defaultRenderers,
                bodyCharacterModel.gameObject);

            masterySkin.gameObjectActivations = Skins.getGameObjectActivationsFromList(activatedGameObjects,
                0,
                1,
                2,
                //3,
                4,
                5,
                //6,
                7,
                8,
                9);

            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
                "Mastery_Base_Pillars_Color",
                "Mastery_Base_Platform",
                "Mastery_Base_Center",
                null,//"Mastery_Base_Tubes",

                "Mastery_Circles",
                "Mastery_Pole",
                null,//"Mastery_Pole_Tracer",
                "Mastery_Emission",
                "Mastery_Orb");

            masterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matMastery");
            //masterySkin.rendererInfos[3].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matMastery");

            masterySkin.rendererInfos[4].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[5].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[6].defaultMaterial = Modules.Materials.CreateHotpooMaterial("WHITE");
            //masterySkin.rendererInfos[7].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[8].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matMasteryOrb");

            skins.Add(masterySkin);
            MasteryMinionSkinReplacement = new SkinDef.MinionSkinReplacement {
                minionBodyPrefab = bodyPrefab,
                minionSkin = masterySkin,
            };

            #endregion mastery

            #region nod

            TowerSkinDef NodSkin = Modules.Skins.CreateSkinDef<TowerSkinDef>(TOWER_PREFIX + "NOD_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinNod"),
                defaultRenderers,
                bodyCharacterModel.gameObject);

            NodSkin.gameObjectActivations = Skins.getGameObjectActivationsFromList(activatedGameObjects,
                0,
                //1,
                2,
                //3,
                //4,
                //5,
                //6,
                7);
                //8
                //9);

            NodSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
                "Nod_Base_Pillars_Colors",
                null,//"Nod_Base_Platform",
                "Nod_Tower",//"Nod_Base_Center",
                null,//"Nod_Base_Tubes",
                
                null,//"Nod_Circles",
                null,//"Nod_Pole",
                null,//"Nod_Pole_Tracer",
                "Nod_Emission",
                null);//"Nod_Orb");
            
            NodSkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerNod");
            //NodSkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerCobblestone");
            NodSkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerNod");
            //NodSkin.rendererInfos[3].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerBlack");

            //NodSkin.rendererInfos[4].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerQuartz");
            //NodSkin.rendererInfos[5].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerCobblestone");
            //NodSkin.rendererInfos[6].defaultMaterial = Modules.Materials.CreateHotpooMaterial("WHITE");
            NodSkin.rendererInfos[7].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerNod");
            //NodSkin.rendererInfos[8].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerDiamond");
            
            NodSkin.ZapLightningType = ModdedLightningType.NodMageThick;

            skins.Add(NodSkin);
            NodMinionSkinReplacement = new SkinDef.MinionSkinReplacement {
                minionBodyPrefab = bodyPrefab,
                minionSkin = NodSkin,
            };

            #endregion
            
            #region mince

            TowerSkinDef MCSkin = Modules.Skins.CreateSkinDef<TowerSkinDef>(TOWER_PREFIX + "MC_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinMC"),
                defaultRenderers,
                bodyCharacterModel.gameObject);

            MCSkin.gameObjectActivations = Skins.getGameObjectActivationsFromList(activatedGameObjects,
                0,
                1,
                2,
                //3,
                4,
                5,
                6,
                //7,
                8,
                9);

            MCSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
                "MC_Base_Pillars_Colors",
                "MC_Base_Platform",
                "MC_Base_Center",
                null,//"MC_Base_Tubes",

                "MC_Circles",
                "MC_Pole",
                "MC_Pole_Tracer",
                null,//"MC_Emission",
                "MC_Orb");

            MCSkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerRedstone");
            MCSkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerCobblestone");
            MCSkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerIron");
            //MCSkin.rendererInfos[3].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerRedstone");

            MCSkin.rendererInfos[4].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerQuartz");
            MCSkin.rendererInfos[5].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerCobblestone");
            MCSkin.rendererInfos[6].defaultMaterial = Modules.Materials.CreateHotpooMaterial("WHITE");
            //MCSkin.rendererInfos[7].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerIron");
            MCSkin.rendererInfos[8].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matTowerDiamond");
            
            skins.Add(MCSkin);
            MCMinionSkinReplacement = new SkinDef.MinionSkinReplacement {
                minionBodyPrefab = bodyPrefab,
                minionSkin = MCSkin,
            };

            #endregion


            skinController.skins = skins.ToArray();
        }
    }
}