using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ModdedEntityStates.TeslaTrooper;
using Modules.Characters;
using R2API;
using System.Runtime.CompilerServices;
using JoeMod;
using RoR2.Orbs;
using ModdedEntityStates.Desolator;
using Modules.Achievements;
using EntityStates;

namespace Modules.Survivors {

    internal class TeslaTrooperSurvivor : SurvivorBase {

        #region survivor stuff
        public static TeslaTrooperSurvivor instance;

        public override string bodyName => "TeslaTrooper";

        public const string TESLA_PREFIX = FacelessJoePlugin.DEV_PREFIX + "_TESLA_BODY_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => TESLA_PREFIX;
        
        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            bodyName = "TeslaTrooperBody",
            bodyNameToken = TESLA_PREFIX + "NAME",
            subtitleNameToken = FacelessJoePlugin.DEV_PREFIX + "_TESLA_BODY_SUBTITLE",
            sortPosition = 68,

            characterPortrait = Modules.Assets.LoadCharacterIcon(Modules.Config.RA2Icon ? "texIconTeslaTrooper2" : "texIconTeslaTrooper"),
            bodyColor = new Color(134f / 216f, 234f / 255f, 255f / 255f), //new Color(115f/216f, 216f/255f, 0.93f),

            crosshair = Assets.LoadAsset<GameObject>("TeslaCrosshair"),
            podPrefab = Assets.LoadAsset<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 120f,
            healthRegen = 1f,
            armor = 5f,

            jumpCount = 1,

            aimOriginPosition = new Vector3(0, 2.8f, 0),
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; }
        
        public override Type characterMainState => typeof(TeslaTrooperMain);

        public override UnlockableDef characterUnlockableDef => null;

        public override ItemDisplaysBase itemDisplays => new TeslaItemDisplays();

        #region unlock
        private static UnlockableDef masterySkinUnlockableDef;
        private static UnlockableDef grandMasterySkinUnlockableDef;

        private static UnlockableDef cursedPrimaryUnlockableDef;
        private static UnlockableDef secondaryUnlockableDef;
        private static UnlockableDef utilityUnlockableDef;

        public static UnlockableDef recolorsUnlockableDef = null;
        #endregion unlock
        #endregion

        #region cool stuff
        public static float conductiveAllyBoost = 1.3f;

        public static DeployableSlot teslaTowerDeployableSlot;
        public DeployableAPI.GetDeployableSameSlotLimit GetTeslaTowerSlotLimit;
        #endregion

        public override void Initialize() {
            instance = this;
            base.Initialize();
            Hooks();
        }

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();
            bodyPrefab.AddComponent<TeslaTrackerComponent>();
            bodyPrefab.AddComponent<TeslaTrackerComponentZap>();
            bodyPrefab.AddComponent<TeslaTrackerComponentDash>();

            bodyPrefab.AddComponent<TeslaTowerControllerController>();
            bodyPrefab.AddComponent<TeslaWeaponComponent>();
            bodyPrefab.AddComponent<TeslaZapBarrierController>();
            if (Compat.VREnabled) {
                bodyPrefab.AddComponent<TeslaVRComponent>();
            }

            bodyCharacterModel.baseRendererInfos[0].defaultMaterial.SetEmission(2);
            bodyCharacterModel.baseRendererInfos[5].defaultMaterial.SetEmission(2);

            bodyPrefab.GetComponent<CharacterBody>().spreadBloomCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);

            bodyPrefab.GetComponent<Interactor>().maxInteractionDistance = 5f;

            ChildLocator childLocator = bodyCharacterModel.GetComponent<ChildLocator>();

            childLocator.FindChild("LightningParticles").GetComponent<ParticleSystemRenderer>().trailMaterial = Assets.ChainLightningMaterial;
            childLocator.FindChild("LightningParticles2").GetComponent<ParticleSystemRenderer>().trailMaterial = Assets.ChainLightningMaterial;

            RegisterTowerDeployable();
        }

        private void RegisterTowerDeployable() {

            GetTeslaTowerSlotLimit += onGetTeslaTowerSlotLimit;

            teslaTowerDeployableSlot = DeployableAPI.RegisterDeployableSlot(onGetTeslaTowerSlotLimit);
        }

        private int onGetTeslaTowerSlotLimit(CharacterMaster self, int deployableCountMultiplier) {

            int result = 1;
            if (self.bodyInstanceObject) {

                if (Modules.Config.LysateLimit == -1) {
                    return self.bodyInstanceObject.GetComponent<SkillLocator>().special.maxStock;
                }

                int lysateCount = self.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid);
                result += Mathf.Min(lysateCount, Modules.Config.LysateLimit);                

                result += SkillsPlusCompat.SkillsPlusAdditionalTowers;
                
                result += Modules.Compat.TryGetScepterCount(self.inventory);
            }

            return result;
        }

        protected override void InitializeEntityStateMachine() {
            base.InitializeEntityStateMachine();

            EntityStateMachine voiceLineMachine = EntityStateMachine.FindByCustomName(bodyPrefab, "Slide");
            SerializableEntityStateType voiceLineState = new SerializableEntityStateType(typeof(TeslaVoiceLines));
            voiceLineMachine.initialStateType = voiceLineState;
            voiceLineMachine.mainStateType = voiceLineState;
        }

        public override void InitializeUnlockables() {
            masterySkinUnlockableDef = UnlockableAPI.AddUnlockable<TeslaTrooperMastery>();
            grandMasterySkinUnlockableDef = UnlockableAPI.AddUnlockable<TeslaTrooperGrandMastery>();
            
            utilityUnlockableDef = UnlockableAPI.AddUnlockable<TeslaTrooperTowerBigZapAchievement>(typeof(TeslaTrooperTowerBigZapAchievement.TeslaTrooperTowerBigZapServerAchievement));
            secondaryUnlockableDef = UnlockableAPI.AddUnlockable<TeslaTrooperShieldZapKillAchievement>(typeof(TeslaTrooperShieldZapKillAchievement.TeslaTrooperShieldZapKillAchievementServer));
            if (Modules.Config.Cursed) {
                cursedPrimaryUnlockableDef = UnlockableAPI.AddUnlockable<TeslaTrooperAllyZapAchievement>();
            }
        }

        public override void InitializeDoppelganger(string clone) {
            base.InitializeDoppelganger("Engi");
        }

        public override void InitializeHitboxes() {
            base.InitializeHitboxes();

            Modules.Prefabs.SetupHitbox(bodyCharacterModel.gameObject, "PunchHitbox", "PunchHitbox");
        }
        
        protected override void InitializeSurvivor() {
            base.InitializeSurvivor();
        }

        protected override void InitializeDisplayPrefab() {
            base.InitializeDisplayPrefab();

            displayPrefab.AddComponent<TeslaMenuSound>();

            ChildLocator childLocator = displayPrefab.GetComponent<ChildLocator>();

            childLocator.FindChild("LightningParticles").GetComponent<ParticleSystemRenderer>().trailMaterial = Assets.ChainLightningMaterial;
            childLocator.FindChild("LightningParticles2").GetComponent<ParticleSystemRenderer>().trailMaterial = Assets.ChainLightningMaterial;
        }
                
        #region skills
        public override void InitializeSkills() {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            InitializePrimarySkills();

            InitializeSecondarySkills();

            InitializeUtilitySkills();

            InitializeSpecialSkills();

            if (Modules.Compat.ScepterInstalled) {
                InitializeScepterSkills();
            }

            InitializeRecolorSkills();

            FinalizeCSSPreviewDisplayController();
        }

        private void InitializePrimarySkills() {
            TeslaTrackingSkillDef primarySkillDefZap =           //this constructor creates a skilldef for a typical primary
                Skills.CreateSkillDef<TeslaTrackingSkillDef>(new SkillDefInfo("Tesla_Primary_Zap",
                                                                              TESLA_PREFIX + "PRIMARY_ZAP_NAME",
                                                                              TESLA_PREFIX + "PRIMARY_ZAP_DESCRIPTION",
                                                                              Modules.Assets.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                                              new EntityStates.SerializableEntityStateType(typeof(Zap)),
                                                                              "Weapon",
                                                                              false));
            
                primarySkillDefZap.keywordTokens = new string[] { "KEYWORD_CHARGED" };

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefZap);

            if (Config.Cursed) {
                SkillDef primarySkillDefPunch =
                    Skills.CreateSkillDef(new SkillDefInfo("Tesla_Primary_Punch",
                                                           TESLA_PREFIX + "PRIMARY_PUNCH_NAME",
                                                           TESLA_PREFIX + "PRIMARY_PUNCH_DESCRIPTION",
                                                           Modules.Assets.LoadAsset<Sprite>("texTeslaSkillSecondaryAlt"),
                                                           new EntityStates.SerializableEntityStateType(typeof(ZapPunchWithDeflect)),
                                                           "Weapon",
                                                           false));
                Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefPunch);
                Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().primary.skillFamily, null, cursedPrimaryUnlockableDef);
            }
        }
        
        private void InitializeSecondarySkills()
        {
            SkillDef bigZapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tesla_Secondary_BigZap",
                skillNameToken = TESLA_PREFIX + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SECONDARY_BIGZAP_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texTeslaSkillSecondary"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(AimBigZap)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 5.5f,
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
                keywordTokens = new string[] { "KEYWORD_STUNNING", "KEYWORD_SHOCKING" }
            });

            SkillDef bigZapPunchSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tesla_Secondary_BigZapPunch",
                skillNameToken = TESLA_PREFIX + "SECONDARY_BIGZAPPUNCH_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SECONDARY_BIGZAPPUNCH_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texTeslaSkillSecondaryAlt"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ChargeZapPunch)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 6.0f,
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
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_SHOCKING" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, bigZapSkillDef, bigZapPunchSkillDef);
            Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().secondary.skillFamily, null, secondaryUnlockableDef);
        }

        private void InitializeUtilitySkills() {

            SkillDef shieldSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tesla_Utility_ShieldZap",
                skillNameToken = TESLA_PREFIX + "UTILITY_BARRIER_NAME",
                skillDescriptionToken = TESLA_PREFIX + "UTILITY_BARRIER_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texTeslaSkillUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ShieldZapCollectDamage)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 7f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            TeslaTrackingResettingSkillDef blinkZapSkillDef = Modules.Skills.CreateSkillDef<TeslaTrackingResettingSkillDef>(new SkillDefInfo {
                skillName = "Tesla_Utility_BlinkZap",
                skillNameToken = TESLA_PREFIX + "UTILITY_BLINK_NAME",
                skillDescriptionToken = TESLA_PREFIX + "UTILITY_BLINK_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texTeslaSkillUtilityAlt"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(BlinkZap)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
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
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_STUNNING" }
            });
            blinkZapSkillDef.refreshedIcon = Modules.Assets.LoadAsset<Sprite>("texTeslaSkillUtilityAltRecast");
            blinkZapSkillDef.timeoutDuration = 3;

            Modules.Skills.AddUtilitySkills(bodyPrefab, shieldSkillDef, blinkZapSkillDef);
            Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().utility.skillFamily, null, utilityUnlockableDef);
        }

        private void InitializeSpecialSkills() {

            SkillDef teslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tesla_Special_Tower",
                skillNameToken = TESLA_PREFIX + "SPECIAL_TOWER_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SPECIAL_TOWER_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texTeslaSkillSpecial"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployTeslaTower)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 24f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 0,
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, teslaCoilSkillDef);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills() {

            SkillDef scepterTeslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tesla_Special_Scepter_Tower",
                skillNameToken = TESLA_PREFIX + "SPECIAL_SCEPTER_TOWER_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SPECIAL_SCEPTER_TOWER_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texTeslaSkillSpecialScepter"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployTeslaTowerScepter)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = 18f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 0,

                keywordTokens = new string[] { "KEYWORD_SHOCKING" }
            });

            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterTeslaCoilSkillDef, "TeslaTrooperBody", SkillSlot.Special, 0);
        }

        private void InitializeRecolorSkills() {

            if (bodyCharacterModel.GetComponent<SkinRecolorController>().Recolors == null) {
                FacelessJoePlugin.Log.LogWarning("Could not load recolors. Make sure you have FixPluginTypesSerialization Installed");
                return;
            }

            SkillFamily recolorFamily = Modules.Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "Recolor", true).skillFamily;

            List<SkillDef> skilldefs = new List<SkillDef> {
                createRecolorSkillDef("Red"),
                createRecolorSkillDef("Blue"),
                createRecolorSkillDef("Green"),
                createRecolorSkillDef("Yellow"),
                createRecolorSkillDef("Orange"),
                createRecolorSkillDef("Cyan"),
                createRecolorSkillDef("Purple"),
                createRecolorSkillDef("Pink"),
            };

            if (Modules.Config.NewColor) {
                skilldefs.Add(createRecolorSkillDef("Black"));
            }

            for (int i = 0; i < skilldefs.Count; i++) {

                Modules.Skills.AddSkillToFamily(recolorFamily, skilldefs[i], i == 0? null : recolorsUnlockableDef);

                AddCssPreviewSkill(i, recolorFamily, skilldefs[i]);
            }
        }

        private SkillDef createRecolorSkillDef(string name) {

            Color color1 = Color.white;

            Recolor[] thing = bodyCharacterModel.GetComponent<SkinRecolorController>().Recolors;

            for (int i = 0; i < thing.Length; i++) {

                Recolor recolor = thing[i];

                if (recolor.recolorName == name.ToLowerInvariant()) {

                    color1 = recolor.colors[0] * 0.69f;
                }
            }

            return Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = name,
                skillNameToken = $"{TESLA_PREFIX}RECOLOR_{name.ToUpper()}_NAME",
                skillDescriptionToken = "",
                skillIcon = R2API.LoadoutAPI.CreateSkinIcon(color1, color1, color1, color1, color1),
            });
        }
        #endregion skills

        public override void InitializeSkins() {

            ModelSkinController skinController = bodyCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = bodyCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRenderers = bodyCharacterModel.baseRendererInfos;

            List<TeslaSkinDef> skins = new List<TeslaSkinDef>();

            List<GameObject> activatedGameObjects = Skins.createAllActivatedGameObjectsList(childLocator,
                "meshTeslaArmor_Fanservice",
                "meshTeslaEmission");

            #region DefaultSkin

            TeslaSkinDef defaultSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>("DEFAULT_SKIN",
                Assets.LoadAsset<Sprite>("texTeslaSkinDefault"),
                defaultRenderers,
                bodyCharacterModel.gameObject);
                                                                                                             //probably better to use strings for childnames instead of ints
            defaultSkin.gameObjectActivations = Skins.getGameObjectActivationsFromList(activatedGameObjects, 0);

            defaultSkin.meshReplacements = Skins.getMeshReplacements(defaultRenderers,
                "meshTeslaArmor",
                "meshTeslaArmor_Fanservice",
                null,//"meshTeslaEmission",
                "meshTeslaBody",
                "meshTeslaArmorColor",
                "meshTeslaBodyColor",
                "meshTeslaHammer");
            
            skins.Add(defaultSkin);
            #endregion

            #region MasterySkin

            TeslaSkinDef masterySkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TESLA_PREFIX + "MASTERY_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinMastery"),
                defaultRenderers,
                bodyCharacterModel.gameObject,
                masterySkinUnlockableDef);

            masterySkin.gameObjectActivations = Modules.Skins.getGameObjectActivationsFromList(activatedGameObjects, 1);

            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
                "meshMasteryArmor",
                null,//"meshMasteryArmor_Fanservice",
                "meshMasteryEmission",
                "meshMasteryBody",
                "meshMasteryArmorColor",
                "meshMasteryBodyColor",
                "meshMasteryHammer");
            
            masterySkin.rendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matMastery");
            //masterySkin.rendererInfos[1].defaultMaterial = Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matMastery").SetEmission(2);
            masterySkin.rendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[4].defaultMaterial = Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[5].defaultMaterial = Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[6].defaultMaterial = Materials.CreateHotpooMaterial("matMastery");

            masterySkin.minionSkinReplacements = new SkinDef.MinionSkinReplacement[] {
                TeslaTowerNotSurvivor.MasteryMinionSkinReplacement,
                TeslaTowerScepter.MasteryMinionSkinReplacement
            };

            skins.Add(masterySkin);
            #endregion

            #region NodSkin

            TeslaSkinDef nodSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TESLA_PREFIX + "NOD_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinNod"),
                defaultRenderers,
                bodyCharacterModel.gameObject,
                grandMasterySkinUnlockableDef);

            nodSkin.gameObjectActivations = Modules.Skins.getGameObjectActivationsFromList(activatedGameObjects, 1);

            nodSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
                "meshNodArmor",
                null,//"meshNodArmor_Fanservice",
                "meshNodEmission",
                "meshNodBody",
                "meshNodArmorColor",
                "meshNodBodyColor",
                null);// "meshNodHammer");

            nodSkin.rendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matNod_Armor");
            //nodSkin.rendererInfos[1].defaultMaterial = Materials.CreateHotpooMaterial("matNod");
            nodSkin.rendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matNod_Emission").SetEmission(2);
            nodSkin.rendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matNod_Body");
            nodSkin.rendererInfos[4].defaultMaterial = Materials.CreateHotpooMaterial("matNod_ArmorColor");
            nodSkin.rendererInfos[5].defaultMaterial = Materials.CreateHotpooMaterial("matNod_Body");
            //nodSkin.rendererInfos[6].defaultMaterial = Materials.CreateHotpooMaterial("matNod");

            nodSkin.minionSkinReplacements = new SkinDef.MinionSkinReplacement[] {
                TeslaTowerNotSurvivor.NodMinionSkinReplacement,
                TeslaTowerScepter.NodMinionSkinReplacement
            };

            skins.Add(nodSkin);

            nodSkin.ZapLightningType = ModdedLightningType.Nod;
            nodSkin.ZapBounceLightningType = LightningOrb.LightningType.Count + 10;
            #endregion
            

            #region MCSkin
            TeslaSkinDef MCSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TESLA_PREFIX + "MC_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinMC"),
                defaultRenderers,
                bodyCharacterModel.gameObject);

            MCSkin.gameObjectActivations = Skins.getGameObjectActivationsFromList(activatedGameObjects);

            MCSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
                "meshMCArmor",
                null,
                null,
                "meshMCBody",
                "meshMCArmorColor",
                "meshMCBodyColor",
                "meshMCHammer");

            MCSkin.rendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matMC_Armor");
            //MCSkin.rendererInfos[1].defaultMaterial = Materials.CreateHotpooMaterial("matMC_Armor");
            //MCSkin.rendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matMC_Armor");
            MCSkin.rendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matMC_Body");
            MCSkin.rendererInfos[4].defaultMaterial = Materials.CreateHotpooMaterial("matMC_ArmorColor");
            MCSkin.rendererInfos[5].defaultMaterial = Materials.CreateHotpooMaterial("matMC_BodyColor");
            MCSkin.rendererInfos[6].defaultMaterial = Materials.CreateHotpooMaterial("matMC_Hammer");

            MCSkin.minionSkinReplacements = new SkinDef.MinionSkinReplacement[] {
                TeslaTowerNotSurvivor.MCMinionSkinReplacement,
                TeslaTowerScepter.MCMinionSkinReplacement
            };

            if (Modules.Config.Cursed) {
                skins.Add(MCSkin);
            }
            #endregion MCSkin

            skinController.skins = skins.ToArray();
        }

        #region hooks
        protected void Hooks() {


            if (bodyCharacterModel.GetComponent<SkinRecolorController>().Recolors == null) {
                FacelessJoePlugin.Log.LogWarning("Could not load recolors. Make sure you have FixPluginTypesSerialization Installed");
            } else {
                On.RoR2.ModelSkinController.ApplySkin += ModelSkinController_ApplySkin;
            }

            On.RoR2.CharacterAI.BaseAI.OnBodyDamaged += BaseAI_OnBodyDamaged;

            On.RoR2.CharacterMaster.AddDeployable += CharacterMaster_AddDeployable;
            On.RoR2.Inventory.CopyItemsFrom_Inventory_Func2 += Inventory_CopyItemsFrom_Inventory_Func2;

            On.RoR2.Inventory.AddItemsFrom_Int32Array_Func2 += Inventory_AddItemsFrom_Int32Array_Func2;
            //On.RoR2.MasterSummon.Perform += MasterSummon_Perform;
            //On.RoR2.CharacterBody.HandleConstructTurret += CharacterBody_HandleConstructTurret;
            
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;

            On.RoR2.Orbs.LightningOrb.Begin += LightningOrb_Begin;
        }

        private void LightningOrb_Begin(On.RoR2.Orbs.LightningOrb.orig_Begin orig, LightningOrb self) {

            GameObject effect = null;
            switch(self.lightningType) {
                case LightningOrb.LightningType.Count + 10:
                    effect = Modules.Assets.TeslaMageLightningOrbEffectRed;
                    self.duration = 0.01f;
                    break;
            }

            if (effect != null) {

                EffectData effectData = new EffectData {
                    origin = self.origin,
                    genericFloat = self.duration
                };
                effectData.SetHurtBoxReference(self.target);
                EffectManager.SpawnEffect(effect, effectData, true);
            }else {

                orig(self);
            }
        }

        private void ModelSkinController_ApplySkin(On.RoR2.ModelSkinController.orig_ApplySkin orig, ModelSkinController self, int skinIndex) {
            orig(self, skinIndex);

            SkinRecolorController skinRecolorController = self.GetComponent<SkinRecolorController>();
            if (skinRecolorController) {

                SkillDef color = self.characterModel.body?.skillLocator?.FindSkill("Recolor")?.skillDef;
                if (color)
                    skinRecolorController.SetRecolor(color.skillName.ToLowerInvariant());
            }
        }

        private void BaseAI_OnBodyDamaged(On.RoR2.CharacterAI.BaseAI.orig_OnBodyDamaged orig, RoR2.CharacterAI.BaseAI self, DamageReport damageReport) {

            //keep tower from drawing aggro
            GameObject originalAttacker = damageReport.damageInfo.attacker;
            if (damageReport.attackerBodyIndex == BodyCatalog.FindBodyIndex("TeslaTowerBody")) {
                damageReport.damageInfo.attacker = null;
            }

            //no longer needed as ally zap orb is new custom harmless buff orb
            ////keep allies from retaliating against trooper charging them
            //bool originalNeverRetaliate = self.neverRetaliateFriendlies;
            //if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DamageTypes.conductive)) {
            //    self.neverRetaliateFriendlies = true;
            //}
            
            orig(self, damageReport);

            //self.neverRetaliateFriendlies = originalNeverRetaliate;
            damageReport.damageInfo.attacker = originalAttacker;
        }

        #region tower hacks

        private void CharacterMaster_AddDeployable(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot) {
            if (MasterCatalog.FindMasterIndex(deployable.gameObject) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {
                //Helpers.LogWarning("adddeployable true");
                slot = teslaTowerDeployableSlot;
            }

            orig(self, deployable, slot);
        }

        private void Inventory_CopyItemsFrom_Inventory_Func2(On.RoR2.Inventory.orig_CopyItemsFrom_Inventory_Func2 orig, Inventory self, Inventory other, Func<ItemIndex, bool> filter) {
            if (MasterCatalog.FindMasterIndex(self.gameObject) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {
                filter = TeslaTowerCopyFilterDelegate;
            }
            orig(self, other, filter);
        }

        private void Inventory_AddItemsFrom_Int32Array_Func2(On.RoR2.Inventory.orig_AddItemsFrom_Int32Array_Func2 orig, Inventory self, int[] otherItemStacks, Func<ItemIndex, bool> filter) {
            if (MasterCatalog.FindMasterIndex(self.gameObject) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {

                for (ItemIndex itemIndex = (ItemIndex)0; itemIndex < (ItemIndex)self.itemStacks.Length; itemIndex++) {
                    int itemstack = otherItemStacks[(int)itemIndex];
                    if (itemstack > 0) {
                    }
                }
            }
            orig(self, otherItemStacks, filter);
        }

        private Func<ItemIndex, bool> TeslaTowerCopyFilterDelegate = new Func<ItemIndex, bool>(TeslaTowerCopyFilter);

        private static bool TeslaTowerCopyFilter(ItemIndex itemIndex) {
            return !ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.CannotCopy) &&
                (ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.Damage) ||
                ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.OnKillEffect));
            //return ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.Damage);
        }
        private void CharacterBody_HandleConstructTurret(On.RoR2.CharacterBody.orig_HandleConstructTurret orig, UnityEngine.Networking.NetworkMessage netMsg) {
            orig(netMsg);
        }
        #endregion tower hacks

        #region conductive
        private static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {

            CheckConductive(self, damageInfo);

            //handled by harmlessbufforb
            //if(DamageAPI.HasModdedDamageType(damageInfo, DamageTypes.ApplyBlinkCooldown)) {
            //        self.body.AddTimedBuff(Buffs.blinkCooldownBuff, 4f);
            //}

            orig(self, damageInfo);
        }

        private static void CheckConductive(HealthComponent self, DamageInfo damageInfo) {

            ApplyConductive(self, damageInfo);

            ConsumeConductiveAlly(self, damageInfo);
        }

        private static void ApplyConductive(HealthComponent self, DamageInfo damageInfo) {

            //mark enemies (or allies) conductive
            bool attackConductive = damageInfo.HasModdedDamageType(DamageTypes.Conductive);
            if (attackConductive) {
                if (damageInfo.attacker?.GetComponent<TeamComponent>()?.teamIndex == self.body.teamComponent.teamIndex) {
                    if (self.body.GetBuffCount(Buffs.conductiveBuffTeam) < 1) {
                        self.body.AddBuff(Buffs.conductiveBuffTeam);
                    }
                }
            }
        }

        private static void ConsumeConductiveAlly(HealthComponent self, DamageInfo damageInfo) {

            if (!damageInfo.attacker)
                return;

            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            if (attackerBody) {
                bool teamCharged = attackerBody.HasBuff(Buffs.conductiveBuffTeam) || attackerBody.HasBuff(Buffs.conductiveBuffTeamGrace);
                if (teamCharged) {
                    //consume allied charged stacks for damage boost and shock

                    int buffCount = attackerBody.GetBuffCount(Buffs.conductiveBuffTeam);
                    for (int i = 0; i < buffCount; i++) {

                        attackerBody.RemoveBuff(Buffs.conductiveBuffTeam);
                        if (!attackerBody.HasBuff(Buffs.conductiveBuffTeamGrace)) {
                            attackerBody.AddTimedBuff(Buffs.conductiveBuffTeamGrace, 0.1f);
                        }
                    }

                    damageInfo.AddModdedDamageType(DamageTypes.ShockShort);
                    damageInfo.damage *= conductiveAllyBoost;// 1f + (0.1f * buffCount);
                }
            }
        }
        #endregion conductive
        #endregion hooks
    }
}