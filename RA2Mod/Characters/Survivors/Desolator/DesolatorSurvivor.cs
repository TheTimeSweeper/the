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
using RA2Mod.Survivors.Desolator.States;
using RA2Mod.Survivors.Desolator.Components;
using RA2Mod.Survivors.Desolator.SkillDefs;
using EntityStates;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorSurvivor : SurvivorBase<DesolatorSurvivor>
    {
        public override string assetBundleName => "desolator";

        public override string bodyName => "DesolatorBody";

        public override string masterName => "DesolatorMonsterMaster";

        public override string modelPrefabName => "mdlDesolator";
        public override string displayPrefabName => "DesolatorDisplay";

        public const string TOKEN_PREFIX = RA2Plugin.DEVELOPER_PREFIX + "_DESOLATOR_BODY_";

        public override string survivorTokenPrefix => TOKEN_PREFIX;

        public static float DotDamage = 0.07f;
        public static float DotInterval = 0.5f;
        public static float DotDuration = 8f;

        public static float DamageMultiplierPerIrradiatedStack = 0.04f;

        //unused
        public static float ArmorShredAmount = 8f;
        public static float ArmorShredDuration = 8f;

        public static List<BuffIndex> compatibleRadiationBuffs = new List<BuffIndex>();

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = TOKEN_PREFIX + "NAME",
            subtitleNameToken = TOKEN_PREFIX + "SUBTITLE",
            bodyColor = new Color(160f / 255f, 238f / 255f, 0f / 255f),
            sortPosition = 69.2f,

            characterPortraitBundlePath = General.GeneralConfig.RA2Icon.Value ? "texIconDesolatorRA2" : "texIconDesolator",
            crosshairBundlePath = "DesolatorCrosshair",
            podPrefabAddressablePath = "RoR2/Base/SurvivorPod/SurvivorPod.prefab",

            //characterPortrait = assetBundle.LoadAsset<Texture>("texIconChrono"),
            //crosshair = Assets.LoadCrosshair("Standard"),
            //podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 5f,

            jumpCount = 1,

            aimOriginPosition = new Vector3(0, 1.6f, 0),
            cameraPivotPosition = new Vector3(0f, 1.6f, 0f),
            cameraParams = cameraParams
        };

        private CharacterCameraParams cameraParams { get
            {
                CharacterCameraParams camera = ScriptableObject.CreateInstance<CharacterCameraParams>();
                camera.data.minPitch = -70;
                camera.data.maxPitch = 70;
                camera.data.wallCushion = 0.1f;
                camera.data.pivotVerticalOffset = 1.2f;
                camera.data.idealLocalCameraPos = new Vector3(0, 0, -12);
                camera.data.fov = new HG.BlendableTypes.BlendableFloat { value = 60f, alpha = 1f };

                return camera;
            } 
        }

        public override UnlockableDef characterUnlockableDef => DesolatorUnlockables.characterUnlockableDef;

        //todo teslamove i bork eitm dispaley
        public override ItemDisplaysBase itemDisplays { get; } = new DesolatorItemDisplays();

        public static string funTokenString;

        public static SkillDef cancelDeploySkillDef;

        public override void Initialize()
        {
            if (!GeneralConfig.DesolatorEnabled.Value)
               return;

            base.Initialize();
        }

        public override List<IEnumerator> GetAssetBundleInitializedCoroutines()
        {
            return DesolatorAssets.GetAssetBundleInitializedCoroutines(assetBundle);
        }

        public override void OnCharacterInitialized()
        {
            DesolatorUnlockables.Init();

            Config.ConfigureBody(prefabCharacterBody, DesolatorConfig.SectionBody);

            //some assets are changed based on config
            DesolatorConfig.Init();
            DesolatorDamageTypes.Init();
            DesolatorAssets.OnCharacterInitialized(assetBundle);
            DesolatorDeployables.Init();
            DesolatorStates.Init();
            funTokenString = UnityEngine.Random.value <= 0.1f ? "_FUN" : "";
            DesolatorTokens.Init();
            Modules.Language.PrintOutput("desolator.txt");

            DesolatorBuffs.Init(assetBundle);
            DesolatorDots.Init();
            DesolatorCompat.Init();

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
            voiceLineController.voiceLineContext = new VoiceLineContext("Desolator", 6, 5, 4);

            displayPrefab.AddComponent<MenuSoundComponent>().sound = "Play_Desolator_Deploy";

        }

        private void AdditionalBodySetup()
        {
            VoiceLineController voiceLineController = bodyPrefab.AddComponent<VoiceLineController>();
            voiceLineController.voiceLineContext = new VoiceLineContext("Desolator", 6, 5, 4);

            bodyPrefab.GetComponent<CharacterBody>().spreadBloomCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);

            bodyPrefab.GetComponent<Interactor>().maxInteractionDistance = 5f;

            bodyPrefab.AddComponent<DesolatorAuraHolder>();
            bodyPrefab.AddComponent<DesolatorWeaponComponent>();
        }

        public override void InitializeEntityStateMachines() 
        {
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(DesolatorCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }
        
        #region skills
        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            InitializePassive();

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

        private void InitializePassive()
        {
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = TOKEN_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "PASSIVE_DESCRIPTION",
                icon = assetBundle.LoadAsset<Sprite>("texDesolatorSkillPassive"),
            };
        }

        private void InitializePrimarySkills()
        {
            SkillDef primarySkillDefBeam =
                Skills.CreateSkillDef(new SkillDefInfo("Desolator_Primary_Beam",
                                                       TOKEN_PREFIX + "PRIMARY_BEAM_NAME",
                                                       TOKEN_PREFIX + "PRIMARY_BEAM_DESCRIPTION",
                                                       assetBundle.LoadAsset<Sprite>("texDesolatorSkillPrimary"),
                                                       new EntityStates.SerializableEntityStateType(typeof(RadBeam)),
                                                       "Weapon",
                                                       false));
            primarySkillDefBeam.keywordTokens = new string[] { "KEYWORD_RADIATION_PRIMARY" };
            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefBeam);
        }

        private void InitializeSecondarySkills()
        {
            SkillDef bigRadBeamSkillDef =
                Skills.CreateSkillDef(new SkillDefInfo
                {
                    skillName = "Desolator_Secondary_BigBeam",
                    skillNameToken = TOKEN_PREFIX + "SECONDARY_BIGBEAM_NAME",
                    skillDescriptionToken = TOKEN_PREFIX + "SECONDARY_BIGBEAM_DESCRIPTION",
                    skillIcon = assetBundle.LoadAsset<Sprite>("texDesolatorSkillSecondary"),
                    activationState = new EntityStates.SerializableEntityStateType(typeof(AimBigRadBeam)),
                    activationStateMachineName = "Weapon",
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
                    cancelSprintingOnActivation = true,
                    rechargeStock = 1,
                    requiredStock = 1,
                    stockToConsume = 1,
                    keywordTokens = new string[] { "KEYWORD_RADIATION_SECONDARY" }
                });
            Modules.Skills.AddSecondarySkills(bodyPrefab, bigRadBeamSkillDef);
        }

        private void InitializeUtilitySkills()
        {
            SkillDef shieldSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Desolator_Utility_Aura",
                skillNameToken = TOKEN_PREFIX + "UTILITY_AURA_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "UTILITY_AURA_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texDesolatorSkillUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(RadiationAura)),
                activationStateMachineName = "Weapon2",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_WEAK" }
            });
            Modules.Skills.AddUtilitySkills(bodyPrefab, shieldSkillDef);
        }

        private void InitializeSpecialSkills()
        {
            FuckingDesolatorDeploySkillDef deploySkillDef = Modules.Skills.CreateSkillDef<FuckingDesolatorDeploySkillDef>(new SkillDefInfo
            {
                skillName = "Desolator_Special_Deploy",
                skillNameToken = TOKEN_PREFIX + "SPECIAL_DEPLOY_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SPECIAL_DEPLOY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texDesolatorSkillSpecial"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployIrradiate)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
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
                keywordTokens = new string[] { "KEYWORD_RADIATION_SPECIAL" }
            });
            deploySkillDef.actualActivationState = new SerializableEntityStateType(typeof(DeployEnter));
            deploySkillDef.cancelIcon = assetBundle.LoadAsset<Sprite>("texDesolatorSkillSpecialCancel");

            SkillDef irradiatorSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Desolator_Special_Tower",
                skillNameToken = TOKEN_PREFIX + "SPECIAL_IRRADIATOR_NAME" + funTokenString,
                skillDescriptionToken = TOKEN_PREFIX + "SPECIAL_IRRADIATOR_DESCRIPTION" + funTokenString,
                skillIcon = assetBundle.LoadAsset<Sprite>("texDesolatorSkillSpecialAlt"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ThrowIrradiator)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = false,
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
                keywordTokens = new string[] { "KEYWORD_RADIATION_SPECIAL" }
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, deploySkillDef, irradiatorSkillDef);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills()
        {
            FuckingDesolatorDeploySkillDef scepterDeploySkillDef = Modules.Skills.CreateSkillDef<FuckingDesolatorDeploySkillDef>(new SkillDefInfo
            {
                skillName = "Desolator_Special_Deploy_Scepter",
                skillNameToken = TOKEN_PREFIX + "SPECIAL_SCEPTER_DEPLOY_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SPECIAL_SCEPTER_DEPLOY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texDesolatorSkillSpecialScepter"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ScepterDeployIrradiate)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                keywordTokens = new string[] { "KEYWORD_RADIATION_SPECIAL" }
            });
            scepterDeploySkillDef.actualActivationState = new SerializableEntityStateType(typeof(ScepterDeployEnter));
            scepterDeploySkillDef.cancelIcon = assetBundle.LoadAsset<Sprite>("texDesolatorSkillSpecialCancel");

            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterDeploySkillDef, "DesolatorBody", SkillSlot.Special, 0);

            SkillDef scepterIrradiatorSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Desolator_Special_Tower_Scepter",
                skillNameToken = TOKEN_PREFIX + "SPECIAL_SCEPTER_IRRADIATOR_NAME" + funTokenString,
                skillDescriptionToken = TOKEN_PREFIX + "SPECIAL_SCEPTER_IRRADIATOR_DESCRIPTION" + funTokenString,
                skillIcon = assetBundle.LoadAsset<Sprite>("texDesolatorSkillSpecialAltScepter"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ScepterThrowIrradiator)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = false,
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
                keywordTokens = new string[] { "KEYWORD_RADIATION_SPECIAL" }
            });

            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterIrradiatorSkillDef, "DesolatorBody", SkillSlot.Special, 1);
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
                Modules.Skills.AddSkillToFamily(recolorFamily, skilldefs[i], i == 0 ? null : DesolatorUnlockables.recolorsUnlockableDef);

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

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin

            SkinDef defaultSkin = Modules.Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texIconSkinDesolatorDefault"),
                defaultRendererinfos,
                characterModelObject);

            defaultSkin.meshReplacements = Modules.Skins.GetMeshReplacements(assetBundle, defaultRendererinfos,
                "DesoArmor",
                "DesoBody",
                "DesoCannon",
                "DesoArmorColor");

            defaultSkin.rendererInfos[0].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matDesolatorArmor");
            defaultSkin.rendererInfos[1].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matDesolatorBody");
            defaultSkin.rendererInfos[2].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matDesolatorCannon");
            defaultSkin.rendererInfos[3].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matDesolatorArmorColor");

            skins.Add(defaultSkin);

            #endregion

            #region MasterySkin 

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(TOKEN_PREFIX + "MASTERY_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texIconSkinDesolatorMastery"),
                defaultRendererinfos,
                characterModelObject,
                DesolatorUnlockables.masterySkinUnlockableDef);

            masterySkin.meshReplacements = Modules.Skins.GetMeshReplacements(assetBundle, defaultRendererinfos,
                "DesoMasteryArmor",
                "DesoMasteryBody",
                "DesoMasteryCannon",
                "DesoMasteryArmorColor");

            masterySkin.rendererInfos[0].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matDesoMasteryArmor");
            masterySkin.rendererInfos[1].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matDesoMasteryBody");
            masterySkin.rendererInfos[2].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matDesoMasteryArmor");
            masterySkin.rendererInfos[3].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matDesoMasteryCrystal");

            skins.Add(masterySkin);

            #endregion

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
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            Hooks.RoR2.HealthComponent.TakeDamage_Pre += HealthComponent_TakeDamage;
            On.RoR2.BuffCatalog.Init += BuffCatalog_Init;

            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(DesolatorBuffs.desolatorArmorBuff))
            {
                args.armorAdd += 100f;
                args.moveSpeedMultAdd += 0.4f;
            }

            if (sender.HasBuff(DesolatorBuffs.desolatorDeployBuff))
            {
                args.armorAdd += 30f;
            }

            if (sender.HasBuff(DesolatorBuffs.desolatorArmorShredDeBuff))
            {
                args.armorAdd -= DesolatorSurvivor.ArmorShredAmount * sender.GetBuffCount(DesolatorBuffs.desolatorArmorShredDeBuff);
            }

            //if (sender.HasBuff(DesolatorBuffs.desolatorDotDeBuff)) {
            //    args.armorAdd -= 3f * sender.GetBuffCount(Modules.Buffs.desolatorDotDeBuff);
            //}
        }

        private void BuffCatalog_Init(On.RoR2.BuffCatalog.orig_Init orig)
        {
            orig();

            for (int i = 0; i < BuffCatalog.buffDefs.Length; i++)
            {
                string buffName = BuffCatalog.buffDefs[i].name.ToLowerInvariant();
                if (buffName.Contains("nuclea") || buffName.Contains("radiat") || buffName.Contains("nuke"))
                {

                    compatibleRadiationBuffs.Add(BuffCatalog.buffDefs[i].buffIndex);
                    if (!BuffCatalog.buffDefs[i].canStack)
                    {
                        compatibleRadiationBuffs.Add(BuffCatalog.buffDefs[i].buffIndex);
                        compatibleRadiationBuffs.Add(BuffCatalog.buffDefs[i].buffIndex);
                    }
                }
            }
        }

        private void HealthComponent_TakeDamage(HealthComponent self, DamageInfo damageInfo)
        {

            if (DamageAPI.HasModdedDamageType(damageInfo, DesolatorDamageTypes.DesolatorDot) || DamageAPI.HasModdedDamageType(damageInfo, DesolatorDamageTypes.DesolatorDotPrimary))
            {
                int radStacks;
                if (self.body == null)
                {
                    radStacks = 0;
                }
                else
                {
                    radStacks = self.body.GetBuffCount(DesolatorBuffs.desolatorDotDeBuff) + GetCompatibleRadiationBuffs(self.body);
                }
                damageInfo.damage += DamageMultiplierPerIrradiatedStack * radStacks * damageInfo.damage;
            }

            if (self && self.body)
            {
                if (self.body.HasBuff(DesolatorBuffs.desolatorDeployBuff))
                {
                    damageInfo.force = Vector3.zero;
                }
            }
        }

        private int GetCompatibleRadiationBuffs(CharacterBody body)
        {
            int count = 0;
            for (int i = 0; i < compatibleRadiationBuffs.Count; i++)
            {
                count += body.GetBuffCount(compatibleRadiationBuffs[i]);
            }
            return count;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {

            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DesolatorDamageTypes.DesolatorArmorShred))
            {
                //    if (damageReport.victimBody.GetBuffCount(Buffs.desolatorArmorShredDeBuff) < 3) {
                //        damageReport.victimBody.AddBuff(Buffs.desolatorArmorShredDeBuff);
                //    }
                damageReport.victimBody.AddTimedBuff(DesolatorBuffs.desolatorArmorShredDeBuff, ArmorShredDuration);
            }

            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DesolatorDamageTypes.DesolatorDot))
            {
                inflictRadiation(damageReport.victim.gameObject, damageReport.attacker, damageReport.damageInfo.procCoefficient, false);
            }
            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DesolatorDamageTypes.DesolatorDotPrimary))
            {
                for (int i = 0; i < RadBeam.RadPrimaryStacks; i++)
                {
                    inflictRadiation(damageReport.victim.gameObject, damageReport.attacker, damageReport.damageInfo.procCoefficient * RadBeam.RadDamageMultiplier, false);
                }
            }
        }

        private void inflictRadiation(GameObject victim, GameObject attacker, float proc, bool crit)
        {
            DotController.InflictDot(victim, attacker, DesolatorDots.DesolatorDot, DotDuration, (crit ? 2 : 1) * proc);
        }

        #endregion hooks
    }
}