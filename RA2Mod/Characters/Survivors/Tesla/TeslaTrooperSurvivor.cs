using BepInEx.Configuration;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RA2Mod.Modules;
using RA2Mod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using R2API;
using UnityEngine.SceneManagement;
using RA2Mod.General.Components;
using System.Runtime.CompilerServices;
using R2API.Utils;
using System.Collections;
using RA2Mod.General;
using RoR2.Orbs;
using RA2Mod.Survivors.Tesla.States;
using RA2Mod.Survivors.Tesla.SkillDefs;
using RA2Mod.Survivors.Tesla.Orbs;
using RA2Mod.Minions.TeslaTower;
using RA2Mod.Survivors.Tesla.Components;

namespace RA2Mod.Survivors.Tesla
{
    public class TeslaTrooperSurvivor : SurvivorBase<TeslaTrooperSurvivor>
    {
        public override string assetBundleName => "teslatrooper";

        public override string bodyName => "TeslaTrooperBody";

        public override string masterName => "TeslaTrooperMonsterMaster";

        public override string modelPrefabName => "mdlTeslaTrooper";
        public override string displayPrefabName => "TeslaTrooperDisplay";

        public const string TOKEN_PREFIX = RA2Plugin.DEVELOPER_PREFIX + "_TESLA_BODY_";

        public override string survivorTokenPrefix => TOKEN_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = TOKEN_PREFIX + "NAME",
            subtitleNameToken = TOKEN_PREFIX + "SUBTITLE",
            bodyColor = Color.cyan,
            sortPosition = 69.1f,

            characterPortraitBundlePath = General.GeneralConfig.RA2Icon.Value ? "texIconTeslaRA2" : "texIconTesla",
            crosshairBundlePath = "TeslaCrosshair",
            podPrefabAddressablePath = "RoR2/Base/SurvivorPod/SurvivorPod.prefab",

            //characterPortrait = assetBundle.LoadAsset<Texture>("texIconChrono"),
            //crosshair = Assets.LoadCrosshair("Standard"),
            //podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 120f,
            healthRegen = 1.0f,
            armor = 5f,

            jumpCount = 1,

            aimOriginPosition = new Vector3(0, 2.8f, 0),
            cameraPivotPosition = new Vector3(0f, 1.6f, 0f),
            cameraParams = cameraParams
        };

        private CharacterCameraParams cameraParams { get
            {
                CharacterCameraParams camera = ScriptableObject.CreateInstance<CharacterCameraParams>();
                camera.data.minPitch = -70;
                camera.data.maxPitch = 70;
                camera.data.wallCushion = 0.1f;
                camera.data.pivotVerticalOffset = 1.37f;
                camera.data.idealLocalCameraPos = new Vector3(0, 0, -12);
                camera.data.fov = new HG.BlendableTypes.BlendableFloat { value = 60f, alpha = 1f };

                return camera;
            } 
        }

        public override UnlockableDef characterUnlockableDef => TeslaUnlockables.characterUnlockableDef;

        //todo teslamove i bork eitm dispaley
        public override ItemDisplaysBase itemDisplays { get; } = new TeslaItemDisplays();

        public override void Initialize()
        {
            if (!GeneralConfig.TeslaEnabled.Value)
               return;

            base.Initialize();
        }

        public override List<IEnumerator> GetAssetBundleInitializedCoroutines()
        {
            return TeslaAssets.GetAssetBundleInitializedCoroutines(assetBundle);
        }

        public override void OnCharacterInitialized()
        {
            TeslaUnlockables.Init();

            Config.ConfigureBody(prefabCharacterBody, TeslaConfig.SectionBody);

            //some assets are changed based on config
            TeslaConfig.Init();
            TeslaDamageTypes.Init();
            TeslaAssets.OnCharacterInitialized(assetBundle);
            TeslaStates.Init();
            TeslaTokens.Init();
            TeslaColors.Init();
            Modules.Language.PrintOutput("tesla.txt");
            
            TeslaBuffs.Init(assetBundle);
            TeslaCompat.Init();
            TeslaDeployables.Init();

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            new TeslaTowerNotSurvivor().Initialize();

            AddHooks();

            Log.CurrentTime($"{bodyName} initializecharacter done");
        }

        //do display prefab stuff here
        protected override void InitializeSurvivor()
        {
            base.InitializeSurvivor();

            VoiceLineInLobby voiceLineController = displayPrefab.AddComponent<VoiceLineInLobby>();
            //todo teslamove
            voiceLineController.voiceLineContext = new VoiceLineContext("Tesla", 5, 4, 6);

            displayPrefab.AddComponent<MenuSoundComponent>().sound = "Play_Tesla_lobby";

            ChildLocator childLocator = displayPrefab.GetComponent<ChildLocator>();

            childLocator.FindChild("LightningParticles").GetComponent<ParticleSystemRenderer>().trailMaterial = TeslaAssets.ChainLightningMaterial;
            childLocator.FindChild("LightningParticles2").GetComponent<ParticleSystemRenderer>().trailMaterial = TeslaAssets.ChainLightningMaterial;

        }

        private void AdditionalBodySetup()
        {
            VoiceLineController voiceLineController = bodyPrefab.AddComponent<VoiceLineController>();
            //todo teslamove
            voiceLineController.voiceLineContext = new VoiceLineContext("Tesla", 5, 4, 6);

            bodyPrefab.AddComponent<TeslaTrackerComponent>();
            bodyPrefab.AddComponent<TeslaTrackerComponentZap>();
            bodyPrefab.AddComponent<TeslaTrackerComponentDash>();

            bodyPrefab.AddComponent<TeslaTowerControllerController>();
            bodyPrefab.AddComponent<TeslaWeaponComponent>();
            bodyPrefab.AddComponent<TeslaZapBarrierController>();
            if (GeneralCompat.VREnabled)
            {
                bodyPrefab.AddComponent<TeslaVRComponent>();
            }

            prefabCharacterModel.baseRendererInfos[0].defaultMaterial.SetEmission(2);
            prefabCharacterModel.baseRendererInfos[5].defaultMaterial.SetEmission(2);

            bodyPrefab.GetComponent<CharacterBody>().spreadBloomCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);

            bodyPrefab.GetComponent<Interactor>().maxInteractionDistance = 5f;

            ChildLocator childLocator = characterModelObject.GetComponent<ChildLocator>();

            childLocator.FindChild("LightningParticles").GetComponent<ParticleSystemRenderer>().trailMaterial = TeslaAssets.ChainLightningMaterial;
            childLocator.FindChild("LightningParticles2").GetComponent<ParticleSystemRenderer>().trailMaterial = TeslaAssets.ChainLightningMaterial;
            
            InitializeHitboxes();
        }

        private void InitializeHitboxes()
        {
            Modules.Prefabs.SetupHitBoxGroup(characterModelObject, "PunchHitbox", "PunchHitbox");
        }

        //todo teslamove esm
        public override void InitializeEntityStateMachines() 
        {
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(TeslaCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }
        
        #region skills
        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            InitializePrimarySkills();

            InitializeSecondarySkills();

            InitializeUtilitySkills();

            InitializeSpecialSkills();

            if (GeneralCompat.ScepterInstalled)
            {
                InitializeScepterSkills();
            }

            InitializeRecolorSkills();

            FinalizeCSSPreviewDisplayController();
        }

        private void InitializePrimarySkills()
        {
            TeslaTrackingSkillDef primarySkillDefZap =           //this constructor creates a skilldef for a typical primary
                Skills.CreateSkillDef<TeslaTrackingSkillDef>(new SkillDefInfo("Tesla_Primary_Zap",
                                                                              TOKEN_PREFIX + "PRIMARY_ZAP_NAME",
                                                                              TOKEN_PREFIX + "PRIMARY_ZAP_DESCRIPTION",
                                                                              assetBundle.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                                              new EntityStates.SerializableEntityStateType(typeof(Zap)),
                                                                              "Weapon",
                                                                              false));

            primarySkillDefZap.keywordTokens = new string[] { TOKEN_PREFIX + "KEYWORD_CHARGED" };

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefZap);

            if (General.GeneralConfig.Cursed.Value)
            {
                SkillDef primarySkillDefPunch =
                    Skills.CreateSkillDef(new SkillDefInfo("Tesla_Primary_Punch",
                                                           TOKEN_PREFIX + "PRIMARY_PUNCH_NAME",
                                                           TOKEN_PREFIX + "PRIMARY_PUNCH_DESCRIPTION",
                                                           assetBundle.LoadAsset<Sprite>("texTeslaSkillSecondaryAlt"),
                                                           new EntityStates.SerializableEntityStateType(typeof(ZapPunchWithDeflect)),
                                                           "Weapon",
                                                           false));
                Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefPunch);
                Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().primary.skillFamily, null, TeslaUnlockables.cursedPrimaryUnlockableDef);
            }
        }

        private void InitializeSecondarySkills()
        {
            SkillDef bigZapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Secondary_BigZap",
                skillNameToken = TOKEN_PREFIX + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SECONDARY_BIGZAP_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTeslaSkillSecondary"),
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

            SkillDef bigZapPunchSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Secondary_BigZapPunch",
                skillNameToken = TOKEN_PREFIX + "SECONDARY_BIGZAPPUNCH_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SECONDARY_BIGZAPPUNCH_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTeslaSkillSecondaryAlt"),
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
            Config.ConfigureSkillDef(bigZapSkillDef, TeslaConfig.SectionBody, "M2 BigZap");
            Config.ConfigureSkillDef(bigZapPunchSkillDef, TeslaConfig.SectionBody, "M2 BigZapPunch");
            Modules.Skills.AddSecondarySkills(bodyPrefab, bigZapSkillDef, bigZapPunchSkillDef);
            Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().secondary.skillFamily, null, TeslaUnlockables.secondaryUnlockableDef);
        }

        private void InitializeUtilitySkills()
        {
            SkillDef shieldSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Utility_ShieldZap",
                skillNameToken = TOKEN_PREFIX + "UTILITY_BARRIER_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "UTILITY_BARRIER_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTeslaSkillUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ShieldZapCollectDamage)),
                activationStateMachineName = "Weapon2",
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

            TeslaTrackingResettingSkillDef blinkZapSkillDef = Modules.Skills.CreateSkillDef<TeslaTrackingResettingSkillDef>(new SkillDefInfo
            {
                skillName = "Tesla_Utility_BlinkZap",
                skillNameToken = TOKEN_PREFIX + "UTILITY_BLINK_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "UTILITY_BLINK_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTeslaSkillUtilityAlt"),
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
            blinkZapSkillDef.refreshedIcon = assetBundle.LoadAsset<Sprite>("texTeslaSkillUtilityAltRecast");
            blinkZapSkillDef.timeoutDuration = 3;

            Config.ConfigureSkillDef(shieldSkillDef, TeslaConfig.SectionBody, "M3 Shield");
            Config.ConfigureSkillDef(blinkZapSkillDef, TeslaConfig.SectionBody, "M2 BlinkZap");
            Modules.Skills.AddUtilitySkills(bodyPrefab, shieldSkillDef, blinkZapSkillDef);
            Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().utility.skillFamily, null, TeslaUnlockables.utilityUnlockableDef);
        }

        private void InitializeSpecialSkills()
        {
            SkillDef teslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Special_Tower",
                skillNameToken = TOKEN_PREFIX + "SPECIAL_TOWER_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SPECIAL_TOWER_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTeslaSkillSpecial"),
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

            Config.ConfigureSkillDef(teslaCoilSkillDef, TeslaConfig.SectionBody, "M4 Tower");
            Modules.Skills.AddSpecialSkills(bodyPrefab, teslaCoilSkillDef);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills()
        {
            SkillDef scepterTeslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Special_Scepter_Tower",
                skillNameToken = TOKEN_PREFIX + "SPECIAL_SCEPTER_TOWER_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SPECIAL_SCEPTER_TOWER_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTeslaSkillSpecialScepter"),
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

        private void InitializeRecolorSkills()
        {

            if (characterModelObject.GetComponent<SkinRecolorController>().Recolors == null)
            {
                Log.Warning("Could not load recolors. types not serialized?");
                return;
            }

            SkillFamily recolorFamily = Modules.Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "LOADOUT_SKILL_COLOR", "Recolor", true).skillFamily;

            SkinRecolorController recolorController = characterModelObject.GetComponent<SkinRecolorController>();

            List<SkillDef> skilldefs = new List<SkillDef> {
                recolorController.createRecolorSkillDef("Red"),
                recolorController.createRecolorSkillDef("Blue"),
                recolorController.createRecolorSkillDef("Green"),
                recolorController.createRecolorSkillDef("Yellow"),
                recolorController.createRecolorSkillDef("Orange"),
                recolorController.createRecolorSkillDef("Cyan"),
                recolorController.createRecolorSkillDef("Purple"),
                recolorController.createRecolorSkillDef("Pink"),
            };

            if (General.GeneralConfig.NewColor.Value)
            {
                skilldefs.Add(recolorController.createRecolorSkillDef("Black"));
            }

            for (int i = 0; i < skilldefs.Count; i++)
            {
                Modules.Skills.AddSkillToFamily(recolorFamily, skilldefs[i], i == 0 ? null : TeslaUnlockables.recolorsUnlockableDef);

                AddCssPreviewSkill(i, recolorFamily, skilldefs[i]);
            }
        }
        #endregion skills

        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.GetComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<TeslaSkinDef> skins = new List<TeslaSkinDef>();

            List<GameObject> activatedGameObjects = Skins.CreateAllActivatedGameObjectsList(childLocator,
                "meshTeslaArmor_Fanservice",
                "meshTeslaEmission");

            #region DefaultSkin

            TeslaSkinDef defaultSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texTeslaSkinDefault"),
                defaultRendererinfos,
                characterModelObject);
            //probably better to use strings for childnames instead of ints
            defaultSkin.gameObjectActivations = Skins.GetGameObjectActivationsFromList(activatedGameObjects, 0);

            defaultSkin.meshReplacements = assetBundle.GetMeshReplacements(defaultRendererinfos,
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

            TeslaSkinDef masterySkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TOKEN_PREFIX + "MASTERY_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texTeslaSkinMastery"),
                defaultRendererinfos,
                characterModelObject,
                TeslaUnlockables.masterySkinUnlockableDef);

            masterySkin.gameObjectActivations = Modules.Skins.GetGameObjectActivationsFromList(activatedGameObjects, 1);

            masterySkin.meshReplacements = assetBundle.GetMeshReplacements(defaultRendererinfos,
                "meshMasteryArmor",
                null,//"meshMasteryArmor_Fanservice",
                "meshMasteryEmission",
                "meshMasteryBody",
                "meshMasteryArmorColor",
                "meshMasteryBodyColor",
                "meshMasteryHammer");

            masterySkin.rendererInfos[0].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery");
            //masterySkin.rendererInfos[1].defaultMaterial = Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[2].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery").SetEmission(2);
            masterySkin.rendererInfos[3].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery");
            masterySkin.rendererInfos[4].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery");
            masterySkin.rendererInfos[5].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery");
            masterySkin.rendererInfos[6].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery");

            //moved to tower initialization
            //masterySkin.minionSkinReplacements = new SkinDef.MinionSkinReplacement[] {
            //    TeslaTowerNotSurvivor.MasteryMinionSkinReplacement,
            //    TeslaTowerScepter.MasteryMinionSkinReplacement
            //};

            skins.Add(masterySkin);
            #endregion

            #region NodSkin

            TeslaSkinDef nodSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TOKEN_PREFIX + "NOD_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texTeslaSkinNod"),
                defaultRendererinfos,
                characterModelObject,
                TeslaUnlockables.grandMasterySkinUnlockableDef);

            nodSkin.gameObjectActivations = Modules.Skins.GetGameObjectActivationsFromList(activatedGameObjects, 1);

            nodSkin.meshReplacements = assetBundle.GetMeshReplacements(defaultRendererinfos,
                "meshNodArmor",
                null,//"meshNodArmor_Fanservice",
                "meshNodEmission",
                "meshNodBody",
                "meshNodArmorColor",
                "meshNodBodyColor",
                null);// "meshNodHammer");

            nodSkin.rendererInfos[0].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matNod_Armor");
          //nodSkin.rendererInfos[1].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matNod");
            nodSkin.rendererInfos[2].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matNod_Emission").SetEmission(2);
            nodSkin.rendererInfos[3].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matNod_Body");
            nodSkin.rendererInfos[4].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matNod_ArmorColor");
            nodSkin.rendererInfos[5].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matNod_Body");
            //nodSkin.rendererInfos[6].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matNod");

            //moved to tower initialization
            //nodSkin.minionSkinReplacements = new SkinDef.MinionSkinReplacement[] {
            //    TeslaTowerNotSurvivor.NodMinionSkinReplacement,
            //    TeslaTowerScepter.NodMinionSkinReplacement
            //};

            skins.Add(nodSkin);

            nodSkin.ZapLightningType = ModdedLightningType.Nod;
            nodSkin.ZapBounceLightningType = LightningOrb.LightningType.Count + 10;
            #endregion

            #region MCSkin

            if (GeneralConfig.Cursed.Value)
            {
                TeslaSkinDef MCSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TOKEN_PREFIX + "MC_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texTeslaSkinMC"),
                defaultRendererinfos,
                characterModelObject);

                MCSkin.gameObjectActivations = Skins.GetGameObjectActivationsFromList(activatedGameObjects);

                MCSkin.meshReplacements = assetBundle.GetMeshReplacements(defaultRendererinfos,
                    "meshMCArmor",
                    null,
                    null,
                    "meshMCBody",
                    "meshMCArmorColor",
                    "meshMCBodyColor",
                    "meshMCHammer");

                MCSkin.rendererInfos[0].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMC_Armor");
                //MCSkin.rendererInfos[1].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMC_Armor");
                //MCSkin.rendererInfos[2].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMC_Armor");
                MCSkin.rendererInfos[3].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMC_Body");
                MCSkin.rendererInfos[4].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMC_ArmorColor");
                MCSkin.rendererInfos[5].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMC_BodyColor");
                MCSkin.rendererInfos[6].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMC_Hammer");

                //moved to tower initialization
                //MCSkin.minionSkinReplacements = new SkinDef.MinionSkinReplacement[] {
                //    TeslaTowerNotSurvivor.MCMinionSkinReplacement,
                //    TeslaTowerScepter.MCMinionSkinReplacement
                //};

                skins.Add(MCSkin);
            }
            #endregion MCSkin
            
            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Engi");
            Prefabs.CloneDopplegangerMasterAsync(bodyPrefab, masterName, "RoR2/Base/Engi/EngiMonsterMaster.prefab");

            //how to set up AI in code
            //TeslaAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        #region hooks
        protected void AddHooks()
        {
            On.RoR2.CharacterAI.BaseAI.OnBodyDamaged += BaseAI_OnBodyDamaged;

            On.RoR2.CharacterMaster.AddDeployable += CharacterMaster_AddDeployable;
            //On.RoR2.Inventory.CopyItemsFrom_Inventory_Func2 += Inventory_CopyItemsFrom_Inventory_Func2;

            //On.RoR2.Inventory.AddItemsFrom_Int32Array_Func2 += Inventory_AddItemsFrom_Int32Array_Func2;
            //On.RoR2.MasterSummon.Perform += MasterSummon_Perform;

            //todo teslamove global hooks
            Hooks.RoR2.HealthComponent.TakeDamage_Pre += HealthComponent_TakeDamage;

            On.RoR2.Orbs.LightningOrb.Begin += LightningOrb_Begin;

            //On.RoR2.BodyCatalog.Init += BodyCatalog_Init;

            //todo teslamove global hooks
            On.RoR2.SetStateOnHurt.OnTakeDamageServer += SetStateOnHurt_OnTakeDamageServer;

            On.RoR2.JitterBones.RebuildBones += JitterBones_RebuildBones;
        }

        private void JitterBones_RebuildBones(On.RoR2.JitterBones.orig_RebuildBones orig, JitterBones self)
        {
            if (self._skinnedMeshRenderer && self._skinnedMeshRenderer.name == "Tower_Base_Pillars_Color")
                return;
            orig(self);
        }

        private static void SetStateOnHurt_OnTakeDamageServer(On.RoR2.SetStateOnHurt.orig_OnTakeDamageServer orig, SetStateOnHurt self, DamageReport damageReport)
        {
            orig(self, damageReport);

            DamageInfo damageInfo = damageReport.damageInfo;
            HealthComponent victim = damageReport.victim;

            bool flag = damageInfo.procCoefficient >= Mathf.Epsilon;

            if (!victim.isInFrozenState)
            {

                //if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(StunLong))
                //{
                //    self.SetStun(StunLongTime);
                //    return;
                //}

                if ((damageInfo.damageType & DamageType.Shock5s) == damageInfo.damageType)
                {
                    if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(TeslaDamageTypes.ShockMed))
                    {
                        self.SetShock(TeslaDamageTypes.ShockTimeMed);
                        return;
                    }
                    ////how scuffed is this?
                    //if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(ShockShort))
                    //{
                    //    self.SetShock(ShockTimeShort);
                    //    return;
                    //}
                }
            }
        }

        private void BodyCatalog_Init(On.RoR2.BodyCatalog.orig_Init orig)
        {
            orig();
            GameObject shockDroneBody = BodyCatalog.FindBodyPrefab("ShockDroneBody");
            if (shockDroneBody)
            {
                //Helpers.LogWarning("hello hello hello");
                shockDroneBody.AddComponent<AddMinionToOwnerTeslaTracker>();
            }
        }

        private void LightningOrb_Begin(On.RoR2.Orbs.LightningOrb.orig_Begin orig, LightningOrb self)
        {
            GameObject effect = null;
            switch (self.lightningType)
            {
                case LightningOrb.LightningType.Count + 10:
                    effect = TeslaAssets.TeslaMageLightningOrbEffectRed;
                    self.duration = 0.01f;
                    break;
            }

            if (effect != null)
            {
                EffectData effectData = new EffectData
                {
                    origin = self.origin,
                    genericFloat = self.duration
                };
                effectData.SetHurtBoxReference(self.target);
                EffectManager.SpawnEffect(effect, effectData, true);
            }
            else
            {
                orig(self);
            }
        }

        private void BaseAI_OnBodyDamaged(On.RoR2.CharacterAI.BaseAI.orig_OnBodyDamaged orig, RoR2.CharacterAI.BaseAI self, DamageReport damageReport)
        {
            //keep tower from drawing aggro
            GameObject originalAttacker = damageReport.damageInfo.attacker;
            if (damageReport.attackerBodyIndex == BodyCatalog.FindBodyIndex("TeslaTowerBody"))
            {
                damageReport.damageInfo.attacker = null;
            }

            orig(self, damageReport);

            damageReport.damageInfo.attacker = originalAttacker;
        }
        
        #region tower hacks

        private void CharacterMaster_AddDeployable(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot)
        {
            MasterCatalog.MasterIndex masterIndex = MasterCatalog.FindMasterIndex(deployable.gameObject);

            if (CompareMasterIndex(masterIndex, MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) ||
                CompareMasterIndex(masterIndex, MasterCatalog.FindMasterIndex(TeslaTowerScepter.masterPrefab)))
            {
                slot = TeslaDeployables.teslaTowerDeployableSlot;
            }

            orig(self, deployable, slot);
        }

        private bool CompareMasterIndex(MasterCatalog.MasterIndex masterindex1, MasterCatalog.MasterIndex masterindex2)
        {
            if (masterindex2.i == -1 || masterindex1.i == -1)
                return false;
            return masterindex1 == masterindex2;
        }

        //private void Inventory_CopyItemsFrom_Inventory_Func2(On.RoR2.Inventory.orig_CopyItemsFrom_Inventory_Func2 orig, Inventory self, Inventory other, Func<ItemIndex, bool> filter) {
        //    if (MasterCatalog.FindMasterIndex(self.gameObject) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {
        //        filter = TeslaTowerCopyFilterDelegate;
        //    }
        //    orig(self, other, filter);
        //}

        //private void Inventory_AddItemsFrom_Int32Array_Func2(On.RoR2.Inventory.orig_AddItemsFrom_Int32Array_Func2 orig, Inventory self, int[] otherItemStacks, Func<ItemIndex, bool> filter) {
        //    if (MasterCatalog.FindMasterIndex(self.gameObject) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {

        //        for (ItemIndex itemIndex = (ItemIndex)0; itemIndex < (ItemIndex)self.itemStacks.Length; itemIndex++) {
        //            int itemstack = otherItemStacks[(int)itemIndex];
        //            if (itemstack > 0) {
        //            }
        //        }
        //    }
        //    orig(self, otherItemStacks, filter);
        //}

        private Func<ItemIndex, bool> TeslaTowerCopyFilterDelegate = new Func<ItemIndex, bool>(TeslaTowerCopyFilter);

        private static bool TeslaTowerCopyFilter(ItemIndex itemIndex)
        {
            return !ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.CannotCopy) &&
                (ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.Damage) ||
                ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.OnKillEffect));
            //return ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.Damage);
        }
        #endregion tower hacks

        #region conductive
        private static void HealthComponent_TakeDamage(HealthComponent self, DamageInfo damageInfo)
        {
            CheckConductive(self, damageInfo);

            bool bypassArmor = (damageInfo.damageType & DamageType.BypassArmor) > DamageType.Generic;
            if (self && self.body)
            {
                if (self.body.HasBuff(TeslaBuffs.zapShieldBuff) && !bypassArmor)
                {
                    float mitigatedDamage = damageInfo.damage;

                    if (TeslaConfig.UtilityDamageAbsorption >= 1.0f)
                    {
                        damageInfo.rejected = true;
                    }
                    //else
                    //{
                    //    mitigatedDamage = (1.0f - TeslaConfig.UtilityDamageAbsorption) * damageInfo.damage;
                    //}

                    IReflectionBarrier bar = self.GetComponent<IReflectionBarrier>();
                    if (bar != null)
                    {
                        bar.StoreDamage(damageInfo, damageInfo.damage);
                        damageInfo.damage = mitigatedDamage;
                    }
                }
            }
        }

        private static void CheckConductive(HealthComponent self, DamageInfo damageInfo)
        {
            ApplyConductive(self, damageInfo);

            ConsumeConductiveAlly(self, damageInfo);
        }

        private static void ApplyConductive(HealthComponent self, DamageInfo damageInfo)
        {
            //mark enemies (or allies) conductive
            bool attackConductive = damageInfo.HasModdedDamageType(TeslaDamageTypes.Conductive);
            if (attackConductive)
            {
                if (damageInfo.attacker?.GetComponent<TeamComponent>()?.teamIndex == self.body.teamComponent.teamIndex)
                {
                    if (self.body.GetBuffCount(TeslaBuffs.conductiveBuffTeam) < 1)
                    {
                        self.body.AddBuff(TeslaBuffs.conductiveBuffTeam);
                    }
                }
            }
        }

        private static void ConsumeConductiveAlly(HealthComponent self, DamageInfo damageInfo)
        {
            if (!damageInfo.attacker)
                return;

            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            if (attackerBody)
            {
                bool teamCharged = attackerBody.HasBuff(TeslaBuffs.conductiveBuffTeam) || attackerBody.HasBuff(TeslaBuffs.conductiveBuffTeamGrace);
                if (teamCharged)
                {
                    //consume allied charged stacks for damage boost and shock

                    int buffCount = attackerBody.GetBuffCount(TeslaBuffs.conductiveBuffTeam);
                    for (int i = 0; i < buffCount; i++)
                    {
                        attackerBody.RemoveBuff(TeslaBuffs.conductiveBuffTeam);
                        if (!attackerBody.HasBuff(TeslaBuffs.conductiveBuffTeamGrace))
                        {
                            attackerBody.AddTimedBuff(TeslaBuffs.conductiveBuffTeamGrace, 0.1f);
                        }
                    }

                    damageInfo.AddModdedDamageType(TeslaDamageTypes.ShockMed);
                    damageInfo.damage *= TeslaConfig.M1_Zap_ConductiveAllyBoost.Value;// 1f + (0.1f * buffCount);
                    damageInfo.damageColorIndex = TeslaColors.ChargedColor;
                }
            }
        }
        #endregion conductive
        #endregion hooks
    }
}