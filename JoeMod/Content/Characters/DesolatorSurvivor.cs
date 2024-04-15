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
using TeslaTrooper;
using RoR2.Orbs;
using ModdedEntityStates.Desolator;
using RoR2.Projectile;
using EntityStates;
using RA2Mod.General.Components;

namespace Modules.Survivors {
    internal class DesolatorSurvivor : SurvivorBase {

        public static DesolatorSurvivor instance;

        public const string DESOLATOR_PREFIX = TeslaTrooperPlugin.DEV_PREFIX + "_DESOLATOR_BODY_";
        
        public override string survivorTokenPrefix => DESOLATOR_PREFIX;

        public override string bodyName => "Desolator";

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            bodyName = "DesolatorBody",
            bodyNameToken = DESOLATOR_PREFIX + "NAME",
            subtitleNameToken = DESOLATOR_PREFIX + "SUBTITLE",

            characterPortrait = Modules.Assets.LoadCharacterIcon(Modules.Config.RA2Icon ? "texIconDesolator2" : "texIconDesolator"),
            bodyColor = new Color(160f / 255f, 238f / 255f, 0f / 255f),
            sortPosition = 68.1f,

            crosshair = Assets.LoadAsset<GameObject>("DesolatorCrosshair"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 1,
            
            aimOriginPosition = new Vector3(0, 1.6f, 0),
        };
        
        public override CustomRendererInfo[] customRendererInfos { get; set; }

        public override Type characterMainState => typeof(DesolatorMain);
        
        public override ItemDisplaysBase itemDisplays => new DesolatorItemDisplays();
        
        public override UnlockableDef characterUnlockableDef => null;
        private static UnlockableDef masterySkinUnlockableDef;
        
        public static DeployableSlot irradiatorDeployableSlot;
        public DeployableAPI.GetDeployableSameSlotLimit GetIrradiatorSlotLimit;

        public static SkillDef cancelDeploySkillDef;
        
        public static float DotDamage = 0.07f;
        public static float DotInterval = 0.5f;
        public static float DotDuration = 8f;
        
        public static float DamageMultiplierPerIrradiatedStack = 0.04f;

        //unused
        public static float ArmorShredAmount= 8f;
        public static float ArmorShredDuration = 8f;

        public static List<BuffIndex> compatibleRadiationBuffs = new List<BuffIndex>();

        public static string funTokenString;

        public override void Initialize() {
            instance = this;

            funTokenString = UnityEngine.Random.value <= 0.1f ? "_FUN" : "";

            base.Initialize();
            

            //todo deso
            RegisterIrradiatorDeployable();
            bodyPrefab.AddComponent<TeslaZapBarrierController>();
            bodyPrefab.AddComponent<DesolatorAuraHolder>();
            bodyPrefab.AddComponent<DesolatorWeaponComponent>();

            bodyPrefab.GetComponent<Interactor>().maxInteractionDistance = 5f;

            bodyPrefab.GetComponent<CharacterBody>().spreadBloomCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);

            Hook();
        }

        protected override void InitializeEntityStateMachine() {
            base.InitializeEntityStateMachine();

            EntityStateMachine voiceLineMachine = EntityStateMachine.FindByCustomName(bodyPrefab, "Slide");
            SerializableEntityStateType voiceLineState = new SerializableEntityStateType(typeof(DesolatorVoiceLines));
            voiceLineMachine.initialStateType = voiceLineState;
            voiceLineMachine.mainStateType = voiceLineState;
        }

        public override void InitializeUnlockables() {
            masterySkinUnlockableDef = R2API.UnlockableAPI.AddUnlockable(typeof(Modules.Achievements.DesolatorMastery));
        }

        public override void InitializeDoppelganger(string clone) {
            base.InitializeDoppelganger("Engi");
        }

        private void RegisterIrradiatorDeployable() {

            GetIrradiatorSlotLimit += onGetIrradiatorSlotLimit;

            irradiatorDeployableSlot = DeployableAPI.RegisterDeployableSlot(onGetIrradiatorSlotLimit);
            Assets.DesolatorIrradiatorProjectile.GetComponent<ProjectileDeployToOwner>().deployableSlot = irradiatorDeployableSlot;
        }

        private int onGetIrradiatorSlotLimit(CharacterMaster self, int deployableCountMultiplier) {

            int result = 1;
            if (self.bodyInstanceObject) {

                //would this guy need a limit too?
                //if (Modules.Config.LysateLimit == -1) {
                return self.bodyInstanceObject.GetComponent<SkillLocator>().special.maxStock;
                //}

                //int lysateCount = self.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid);
                //result += Mathf.Min(lysateCount, Modules.Config.LysateLimit);

                //result += SkillsPlusCompat.SkillsPlusAdditionalTowers;

                //result += Modules.Compat.TryGetScepterCount(self.inventory);
            }

            return result;
        }
        
        #region skills

        public override void InitializeSkills() {

            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            InitializePassive();

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

        private void InitializePassive() {
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill {
                enabled = true,
                skillNameToken = DESOLATOR_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = DESOLATOR_PREFIX + "PASSIVE_DESCRIPTION",
                icon = Assets.LoadAsset<Sprite>("texDesolatorSkillPassive"),
            };
        }

        private void InitializePrimarySkills() {

            SkillDef primarySkillDefPunch =
                Skills.CreateSkillDef(new SkillDefInfo("Desolator_Primary_Beam",
                                                       DESOLATOR_PREFIX + "PRIMARY_BEAM_NAME",
                                                       DESOLATOR_PREFIX + "PRIMARY_BEAM_DESCRIPTION",
                                                       Modules.Assets.LoadAsset<Sprite>("texDesolatorSkillPrimary"),
                                                       new EntityStates.SerializableEntityStateType(typeof(RadBeam)),
                                                       "Weapon",
                                                       false));
            primarySkillDefPunch.keywordTokens = new string[] { "KEYWORD_RADIATION_PRIMARY" };
            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefPunch);
        }

        private void InitializeSecondarySkills() {

            SkillDef bigRadBeamSkillDef =
                Skills.CreateSkillDef(new SkillDefInfo {
                    skillName = "Desolator_Secondary_BigBeam",
                    skillNameToken = DESOLATOR_PREFIX + "SECONDARY_BIGBEAM_NAME",
                    skillDescriptionToken = DESOLATOR_PREFIX + "SECONDARY_BIGBEAM_DESCRIPTION",
                    skillIcon = Modules.Assets.LoadAsset<Sprite>("texDesolatorSkillSecondary"),
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
            bigRadBeamSkillDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            Modules.Skills.AddSecondarySkills(bodyPrefab, bigRadBeamSkillDef);
        }

        private void InitializeUtilitySkills() {

            SkillDef shieldSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Desolator_Utility_Aura",
                skillNameToken = DESOLATOR_PREFIX + "UTILITY_AURA_NAME",
                skillDescriptionToken = DESOLATOR_PREFIX + "UTILITY_AURA_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texDesolatorSkillUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(RadiationAura)),
                activationStateMachineName = "Slide",
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

        private void InitializeSpecialSkills() {
            
            SkillDef deploySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Desolator_Special_Deploy",
                skillNameToken = DESOLATOR_PREFIX + "SPECIAL_DEPLOY_NAME",
                skillDescriptionToken = DESOLATOR_PREFIX + "SPECIAL_DEPLOY_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texDesolatorSkillSpecial"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployEnter)),
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

            cancelDeploySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {

                skillName = "Desolator_Special_Deploy_Cancel",
                skillNameToken = DESOLATOR_PREFIX + "SPECIAL_DEPLOY_CANCEL_NAME",
                skillDescriptionToken = DESOLATOR_PREFIX + "SPECIAL_DEPLOY_CANCEL_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texDesolatorSkillSpecialCancel"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployCancel)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Frozen,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 0,
                requiredStock = 0,
                stockToConsume = 0,
            });

            SkillDef irradiatorSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Desolator_Special_Tower",
                skillNameToken = DESOLATOR_PREFIX + "SPECIAL_IRRADIATOR_NAME" + funTokenString,
                skillDescriptionToken = DESOLATOR_PREFIX + "SPECIAL_IRRADIATOR_DESCRIPTION" + funTokenString,
                skillIcon = Assets.LoadAsset<Sprite>("texDesolatorSkillSpecialAlt"),
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
        private void InitializeScepterSkills() {

            SkillDef scepterDeploySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Desolator_Special_Deploy_Scepter",
                skillNameToken = DESOLATOR_PREFIX + "SPECIAL_SCEPTER_DEPLOY_NAME",
                skillDescriptionToken = DESOLATOR_PREFIX + "SPECIAL_SCEPTER_DEPLOY_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texDesolatorSkillSpecialScepter"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ScepterDeployEnter)),
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

            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterDeploySkillDef, "DesolatorBody", SkillSlot.Special, 0);

            SkillDef scepterIrradiatorSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Desolator_Special_Tower_Scepter",
                skillNameToken = DESOLATOR_PREFIX + "SPECIAL_SCEPTER_IRRADIATOR_NAME" + funTokenString,
                skillDescriptionToken = DESOLATOR_PREFIX + "SPECIAL_SCEPTER_IRRADIATOR_DESCRIPTION" + funTokenString,
                skillIcon = Assets.LoadAsset<Sprite>("texDesolatorSkillSpecialAltScepter"),
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

        //todo deso
        //todo DRY
        private void InitializeRecolorSkills() {

            if (bodyCharacterModel.GetComponent<SkinRecolorController>().Recolors == null) {
                TeslaTrooperPlugin.Log.LogWarning("Could not load recolors. Make sure you have FixPluginTypesSerialization Installed");
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

                Modules.Skills.AddSkillToFamily(recolorFamily, skilldefs[i], null);

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
                skillNameToken = $"{TeslaTrooperSurvivor.TESLA_PREFIX}RECOLOR_{name.ToUpperInvariant()}_NAME",
                skillDescriptionToken = "",
                skillIcon = Modules.Skins.CreateRecolorIcon(color1),
            });
        }

        #endregion skills

        #region skins

        public override void InitializeSkins() {
            ModelSkinController skinController = bodyCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = bodyCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = bodyCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin

            SkinDef defaultSkin = Modules.Skins.CreateSkinDef("DEFAULT_SKIN",
                Assets.LoadAsset<Sprite>("texIconSkinDesolatorDefault"),
                defaultRendererinfos,
                bodyCharacterModel.gameObject);

            defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "DesoArmor",
                "DesoBody",
                "DesoCannon",
                "DesoArmorColor");

            defaultSkin.rendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matDesolatorArmor");
            defaultSkin.rendererInfos[1].defaultMaterial = Materials.CreateHotpooMaterial("matDesolatorBody");
            defaultSkin.rendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matDesolatorCannon");
            defaultSkin.rendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matDesolatorArmorColor");
            
            skins.Add(defaultSkin);

            #endregion

            #region MasterySkin 

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(DESOLATOR_PREFIX + "MASTERY_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texIconSkinDesolatorMastery"),
                defaultRendererinfos,
                bodyCharacterModel.gameObject, 
                masterySkinUnlockableDef);

            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "DesoMasteryArmor",
                "DesoMasteryBody",
                "DesoMasteryCannon",
                "DesoMasteryArmorColor");

            masterySkin.rendererInfos[0].defaultMaterial = Materials.CreateHotpooMaterial("matDesoMasteryArmor");
            masterySkin.rendererInfos[1].defaultMaterial = Materials.CreateHotpooMaterial("matDesoMasteryBody");
            masterySkin.rendererInfos[2].defaultMaterial = Materials.CreateHotpooMaterial("matDesoMasteryArmor");
            masterySkin.rendererInfos[3].defaultMaterial = Materials.CreateHotpooMaterial("matDesoMasteryCrystal");

            skins.Add(masterySkin);

            #endregion

            skinController.skins = skins.ToArray();
        }

        #endregion skins

        #region hook

        private void Hook() {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.BuffCatalog.Init += BuffCatalog_Init;
        }

        private void BuffCatalog_Init(On.RoR2.BuffCatalog.orig_Init orig) {
            orig();

            for (int i = 0; i < BuffCatalog.buffDefs.Length; i++) {

                string buffName = BuffCatalog.buffDefs[i].name.ToLowerInvariant();
                if (buffName.Contains("nuclea") || buffName.Contains("radiat") || buffName.Contains("nuke")) {

                    compatibleRadiationBuffs.Add(BuffCatalog.buffDefs[i].buffIndex);
                    if (!BuffCatalog.buffDefs[i].canStack) {
                        compatibleRadiationBuffs.Add(BuffCatalog.buffDefs[i].buffIndex);
                    }
                }
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {

            if (DamageAPI.HasModdedDamageType(damageInfo, DamageTypes.DesolatorDot) || DamageAPI.HasModdedDamageType(damageInfo, DamageTypes.DesolatorDotPrimary)) {

                int radStacks;
                if (self.body == null) {
                    radStacks = 0;
                } else {
                    radStacks = self.body.GetBuffCount(Buffs.desolatorDotDeBuff) + GetCompatibleRadiationBuffs(self.body);
                }
                damageInfo.damage += DamageMultiplierPerIrradiatedStack * radStacks * damageInfo.damage;
            }

            orig(self, damageInfo);
        }

        private int GetCompatibleRadiationBuffs(CharacterBody body) {
            int count = 0;
            for (int i = 0; i < compatibleRadiationBuffs.Count; i++) {
                count += body.GetBuffCount(compatibleRadiationBuffs[i]);
            }
            return count;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport) {

            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DamageTypes.DesolatorArmorShred)) {
            //    if (damageReport.victimBody.GetBuffCount(Buffs.desolatorArmorShredDeBuff) < 3) {
            //        damageReport.victimBody.AddBuff(Buffs.desolatorArmorShredDeBuff);
            //    }
                damageReport.victimBody.AddTimedBuff(Buffs.desolatorArmorShredDeBuff, ArmorShredDuration);
            }

            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DamageTypes.DesolatorDot)) {

                inflictRadiation(damageReport.victim.gameObject, damageReport.attacker, damageReport.damageInfo.procCoefficient, false);
            }
            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DamageTypes.DesolatorDotPrimary)) {
                for (int i = 0; i < RadBeam.RadStacks; i++) {
                    inflictRadiation(damageReport.victim.gameObject, damageReport.attacker, damageReport.damageInfo.procCoefficient * RadBeam.RadDamageMultiplier, false);
                }
            }
        }

        private void inflictRadiation(GameObject victim, GameObject attacker, float proc, bool crit) {
            DotController.InflictDot(victim, attacker, Dots.DesolatorDot, DotDuration, (crit ? 2 : 1) * proc);
        }

        #endregion hook
    }
}