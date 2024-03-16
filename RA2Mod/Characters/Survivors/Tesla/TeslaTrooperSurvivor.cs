using BepInEx.Configuration;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RA2Mod.Modules;
using RA2Mod.Modules.Characters;
using RA2Mod.Survivors.Chrono.Components;
using RA2Mod.Survivors.Chrono.SkillStates;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using R2API;
using RA2Mod.Survivors.Chrono.SkillDefs;
using UnityEngine.SceneManagement;
using RA2Mod.General.Components;
using System.Runtime.CompilerServices;
using R2API.Utils;
using System.Collections;
using RA2Mod.General;
using TeslaTrooper;
using ModdedEntityStates.TeslaTrooper;
using RoR2.Orbs;

namespace RA2Mod.Survivors.Tesla
{
    public class TeslaTrooperSurvivor : SurvivorBase<TeslaTrooperSurvivor>
    {
        public override string assetBundleName => "teslatrooper";

        public override string bodyName => "TeslaTrooperBody";

        public override string masterName => "TeslaTrooperMaster";

        public override string modelPrefabName => "mdlTeslaTrooper";
        public override string displayPrefabName => "TeslaTrooperDisplay";

        public const string TESLA_PREFIX = RA2Plugin.DEVELOPER_PREFIX + "_TESLA_BODY_";

        public override string survivorTokenPrefix => TESLA_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = TESLA_PREFIX + "NAME",
            subtitleNameToken = TESLA_PREFIX + "SUBTITLE",
            bodyColor = Color.cyan,
            sortPosition = 69f,

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

        public override ItemDisplaysBase itemDisplays { get; } = new RA2Mod.General.JoeItemDisplays();

        public override void Initialize()
        {
            //ConfigEntry<bool> characterEnabled = General.GeneralConfig.TeslaEnabled;

            //if (!characterEnabled.Value)
            //   return;

            base.Initialize();
        }

        public override List<IEnumerator> GetAssetBundleInitializedCoroutines()
        {
            return TeslaAssets.GetAssetBundleInitializedCoroutines(assetBundle);
        }

        public override void OnCharacterInitialized()
        {
            //need the character unlockable before you initialize the survivordef
            TeslaUnlockables.Init();

            base.OnCharacterInitialized();

            Config.ConfigureBody(prefabCharacterBody, TeslaConfig.SectionBody);

            TeslaConfig.Init();
            //some assets are changed based on config
            TeslaAssets.OnCharacterInitialized(assetBundle);
            TeslaStates.Init();
            TeslaTokens.Init();
            
            TeslaDamageTypes.Init();
            TeslaBuffs.Init(assetBundle);
            TeslaCompat.Init();
            TeslaDeployables.Init();
            //todo teslamove hitboxes

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();

            Log.CurrentTime($"{bodyName} initializecharacter done");
        }

        //do display prefab stuff here
        protected override void InitializeSurvivor()
        {
            base.InitializeSurvivor();

            VoiceLineInLobby voiceLineController = displayPrefab.AddComponent<VoiceLineInLobby>();
            //todo teslamove
            voiceLineController.voiceLineContext = new VoiceLineContext("Chrono", 4, 5, 5);

            displayPrefab.AddComponent<MenuSoundComponent>().sound = "Play_Tesla_lobby";

            ChildLocator childLocator = displayPrefab.GetComponent<ChildLocator>();

            childLocator.FindChild("LightningParticles").GetComponent<ParticleSystemRenderer>().trailMaterial = TeslaAssets.ChainLightningMaterial;
            childLocator.FindChild("LightningParticles2").GetComponent<ParticleSystemRenderer>().trailMaterial = TeslaAssets.ChainLightningMaterial;

        }

        private void AdditionalBodySetup()
        {
            VoiceLineController voiceLineController = bodyPrefab.AddComponent<VoiceLineController>();
            //todo teslamove
            voiceLineController.voiceLineContext = new VoiceLineContext("Chrono", 4, 5, 5);

            bodyPrefab.AddComponent<TeslaTrackerComponent>();
            bodyPrefab.AddComponent<TeslaTrackerComponentZap>();
            bodyPrefab.AddComponent<TeslaTrackerComponentDash>();

            bodyPrefab.AddComponent<TeslaTowerControllerController>();
            bodyPrefab.AddComponent<TeslaWeaponComponent>();
            bodyPrefab.AddComponent<TeslaZapBarrierController>();
            if (Compat.VREnabled)
            {
                bodyPrefab.AddComponent<TeslaVRComponent>();
            }

            prefabCharacterModel.baseRendererInfos[0].defaultMaterial.SetEmission(2);
            prefabCharacterModel.baseRendererInfos[5].defaultMaterial.SetEmission(2);

            bodyPrefab.GetComponent<CharacterBody>().spreadBloomCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);

            bodyPrefab.GetComponent<Interactor>().maxInteractionDistance = 5f;

            ChildLocator childLocator = characterModelObject.GetComponent<ChildLocator>();

            //todo teslamove chainlightningmaterial in the before init loading
            childLocator.FindChild("LightningParticles").GetComponent<ParticleSystemRenderer>().trailMaterial = TeslaAssets.ChainLightningMaterial;
            childLocator.FindChild("LightningParticles2").GetComponent<ParticleSystemRenderer>().trailMaterial = TeslaAssets.ChainLightningMaterial;
        }
    
        //todo teslamove esm
        public override void InitializeEntityStateMachines() 
        {
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(ChronoCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            
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

            if (GeneralCompat.scepterInstalled)
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
                                                                              TESLA_PREFIX + "PRIMARY_ZAP_NAME",
                                                                              TESLA_PREFIX + "PRIMARY_ZAP_DESCRIPTION",
                                                                              assetBundle.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                                              new EntityStates.SerializableEntityStateType(typeof(Zap)),
                                                                              "Weapon",
                                                                              false));

            primarySkillDefZap.keywordTokens = new string[] { "KEYWORD_CHARGED" };

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefZap);

            if (General.GeneralConfig.Cursed.Value)
            {
                SkillDef primarySkillDefPunch =
                    Skills.CreateSkillDef(new SkillDefInfo("Tesla_Primary_Punch",
                                                           TESLA_PREFIX + "PRIMARY_PUNCH_NAME",
                                                           TESLA_PREFIX + "PRIMARY_PUNCH_DESCRIPTION",
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
                skillNameToken = TESLA_PREFIX + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SECONDARY_BIGZAP_DESCRIPTION",
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
                skillNameToken = TESLA_PREFIX + "SECONDARY_BIGZAPPUNCH_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SECONDARY_BIGZAPPUNCH_DESCRIPTION",
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

            Modules.Skills.AddSecondarySkills(bodyPrefab, bigZapSkillDef, bigZapPunchSkillDef);
            Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().secondary.skillFamily, null, TeslaUnlockables.secondaryUnlockableDef);
        }

        private void InitializeUtilitySkills()
        {

            SkillDef shieldSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Utility_ShieldZap",
                skillNameToken = TESLA_PREFIX + "UTILITY_BARRIER_NAME",
                skillDescriptionToken = TESLA_PREFIX + "UTILITY_BARRIER_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTeslaSkillUtility"),
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

            TeslaTrackingResettingSkillDef blinkZapSkillDef = Modules.Skills.CreateSkillDef<TeslaTrackingResettingSkillDef>(new SkillDefInfo
            {
                skillName = "Tesla_Utility_BlinkZap",
                skillNameToken = TESLA_PREFIX + "UTILITY_BLINK_NAME",
                skillDescriptionToken = TESLA_PREFIX + "UTILITY_BLINK_DESCRIPTION",
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

            Modules.Skills.AddUtilitySkills(bodyPrefab, shieldSkillDef, blinkZapSkillDef);
            Modules.Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().utility.skillFamily, null, TeslaUnlockables.utilityUnlockableDef);
        }

        private void InitializeSpecialSkills()
        {

            SkillDef teslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Special_Tower",
                skillNameToken = TESLA_PREFIX + "SPECIAL_TOWER_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SPECIAL_TOWER_DESCRIPTION",
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

            Modules.Skills.AddSpecialSkills(bodyPrefab, teslaCoilSkillDef);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills()
        {

            SkillDef scepterTeslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Special_Scepter_Tower",
                skillNameToken = TESLA_PREFIX + "SPECIAL_SCEPTER_TOWER_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SPECIAL_SCEPTER_TOWER_DESCRIPTION",
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
                Log.Warning("Could not load recolors. Make sure you have FixPluginTypesSerialization Installed");
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

            if (General.GeneralConfig.NewColor.Value)
            {
                skilldefs.Add(createRecolorSkillDef("Black"));
            }

            for (int i = 0; i < skilldefs.Count; i++)
            {

                Modules.Skills.AddSkillToFamily(recolorFamily, skilldefs[i], i == 0 ? null : TeslaUnlockables.recolorsUnlockableDef);

                AddCssPreviewSkill(i, recolorFamily, skilldefs[i]);
            }
        }

        private SkillDef createRecolorSkillDef(string name)
        {

            Color color1 = Color.white;

            Recolor[] thing = characterModelObject.GetComponent<SkinRecolorController>().Recolors;

            for (int i = 0; i < thing.Length; i++)
            {

                Recolor recolor = thing[i];

                if (recolor.recolorName == name.ToLowerInvariant())
                {

                    color1 = recolor.colors[0] * 0.69f;
                }
            }

            return Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = name,
                skillNameToken = $"{TESLA_PREFIX}RECOLOR_{name.ToUpper()}_NAME",
                skillDescriptionToken = "",
                skillIcon = Modules.Skins.CreateRecolorIcon(color1),
            });
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

            TeslaSkinDef masterySkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TESLA_PREFIX + "MASTERY_SKIN_NAME",
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

            masterySkin.minionSkinReplacements = new SkinDef.MinionSkinReplacement[] {
                //TeslaTowerNotSurvivor.MasteryMinionSkinReplacement,
                //TeslaTowerScepter.MasteryMinionSkinReplacement
            };

            skins.Add(masterySkin);
            #endregion

            #region NodSkin

            TeslaSkinDef nodSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TESLA_PREFIX + "NOD_SKIN_NAME",
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

            nodSkin.minionSkinReplacements = new SkinDef.MinionSkinReplacement[] {
                //TeslaTowerNotSurvivor.NodMinionSkinReplacement,
                //TeslaTowerScepter.NodMinionSkinReplacement
            };

            skins.Add(nodSkin);

            nodSkin.ZapLightningType = ModdedLightningType.Nod;
            nodSkin.ZapBounceLightningType = LightningOrb.LightningType.Count + 10;
            #endregion

            #region MCSkin
            TeslaSkinDef MCSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TESLA_PREFIX + "MC_SKIN_NAME",
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

            MCSkin.minionSkinReplacements = new SkinDef.MinionSkinReplacement[] {
                //TeslaTowerNotSurvivor.MCMinionSkinReplacement,
                //TeslaTowerScepter.MCMinionSkinReplacement
            };

            if (GeneralConfig.Cursed.Value)
            {
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

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

        }
    }
}