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
using RoR2.Projectile;

namespace Modules.Survivors {
    internal class DesolatorSurvivor : SurvivorBase {

        public static DesolatorSurvivor instance;

        public const string DESOLATOR_PREFIX = FacelessJoePlugin.DEV_PREFIX + "_DESOLATOR_BODY_";

        public override string survivorTokenPrefix => DESOLATOR_PREFIX;

        public override string bodyName => "Desolator";

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            bodyName = "DesolatorBody",
            bodyNameToken = DESOLATOR_PREFIX + "NAME",
            subtitleNameToken = DESOLATOR_PREFIX + "SUBTITLE",

            characterPortrait = Modules.Assets.LoadCharacterIcon(Modules.Config.RA2Icon ? "texIconDesolator2" : "texIconDesolator"),
            bodyColor = new Color(160f / 255f, 238f / 255f, 0f / 255f),
            sortPosition = 69.1f,

            //todo deso
            crosshair = Assets.LoadAsset<GameObject>("TeslaCrosshair"),
            podPrefab = Assets.LoadAsset<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 120f,
            healthRegen = 1f,
            armor = 5f,

            jumpCount = 1,

            aimOriginPosition = new Vector3(0, 2.8f, 0),
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; }

        public override Type characterMainState => typeof(DesolatorMain);

        public override ItemDisplaysBase itemDisplays => TeslaTrooperSurvivor.instance.itemDisplays;

        public override UnlockableDef characterUnlockableDef => null;
        private static UnlockableDef masterySkinUnlockableDef;

        public static DeployableSlot irradiatorDeployableSlot;
        public DeployableAPI.GetDeployableSameSlotLimit GetIrradiatorSlotLimit;

        public static SkillDef cancelDeploySkillDef;

        public static float DotDamage = 0.2f;
        public static float DotInterval = 0.5f;
        public static float DotDuration= 8f;

        public override void Initialize() {
            instance = this;
            base.Initialize();

            //todo deso
            RegisterIrradiatorDeployable();
            bodyPrefab.AddComponent<TeslaZapBarrierController>();
            bodyPrefab.AddComponent<DesolatorAuraHolder>();
            
            bodyPrefab.GetComponent<Interactor>().maxInteractionDistance = 5f;

            Hook();
        }

        public override void InitializeDoppelganger(string clone) {
            base.InitializeDoppelganger("Engi");
        }

        private void RegisterIrradiatorDeployable() {

            GetIrradiatorSlotLimit += onGetIrradiatorSlotLimit;

            irradiatorDeployableSlot = DeployableAPI.RegisterDeployableSlot(onGetIrradiatorSlotLimit);
            Assets.DesolatorIrradiatorProjectile.GetComponent<ProjectileDeployToOwner>().deployableSlot = irradiatorDeployableSlot;

            Content.AddEntityState(typeof(DesolatorMain));
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

            InitializeRecolorSkills();

            FinalizeCSSPreviewDisplayController();
        }

        private void InitializePassive() {
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill {
                enabled = true,
                skillNameToken = DESOLATOR_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = DESOLATOR_PREFIX + "PASSIVE_DESCRIPTION",
                icon = Assets.LoadAsset<Sprite>("texDesolatorSkillPrimary"),
            };
        }

        private void InitializePrimarySkills() {
            States.entityStates.Add(typeof(RadBeam));
            SkillDef primarySkillDefPunch =
                Skills.CreateSkillDef(new SkillDefInfo("Desolator_Primary_Beam",
                                                       DESOLATOR_PREFIX + "PRIMARY_BEAM_NAME",
                                                       DESOLATOR_PREFIX + "PRIMARY_BEAM_DESCRIPTION",
                                                       Modules.Assets.LoadAsset<Sprite>("texDesolatorSkillPrimary"),
                                                       new EntityStates.SerializableEntityStateType(typeof(RadBeam)),
                                                       "Weapon",
                                                       false));
            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefPunch);
        }

        private void InitializeSecondarySkills() {

            States.entityStates.Add(typeof(AimBigRadBeam));
            SkillDef bigRadBeamSkillDef =
                Skills.CreateSkillDef(new SkillDefInfo {
                    skillName = "Desolator_Secondary_BigBeam",
                    skillNameToken = DESOLATOR_PREFIX + "SECONDARY_BIGBEAM_NAME",
                    skillDescriptionToken = DESOLATOR_PREFIX + "SECONDARY_BIGBEAM_DESCRIPTION",
                    skillIcon = Modules.Assets.LoadAsset<Sprite>("texDesolatorSkillSecondary"),
                    activationState = new EntityStates.SerializableEntityStateType(typeof(AimBigRadBeam)),
                    activationStateMachineName = "Weapon",
                    baseMaxStock = 1,
                    baseRechargeInterval = 6f,
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
                    stockToConsume = 1
                });
            bigRadBeamSkillDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            Modules.Skills.AddSecondarySkills(bodyPrefab, bigRadBeamSkillDef);
        }

        private void InitializeUtilitySkills() {

            States.entityStates.Add(typeof(RadiationAura));
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
            
            States.entityStates.Add(typeof(DeployEnter));
            States.entityStates.Add(typeof(DeployIrradiate));
            SkillDef deploySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Desolator_Special_Deploy",
                skillNameToken = DESOLATOR_PREFIX + "SPECIAL_DEPLOY_NAME",
                skillDescriptionToken = DESOLATOR_PREFIX + "SPECIAL_DEPLOY_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texDesolatorSkillSpecial"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployEnter)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                //keywordTokens = new string[] { "KEYWORD_WEAK" }
            });

            States.entityStates.Add(typeof(DeployCancel));
            cancelDeploySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {

                skillName = "Desolator_Special_Deploy_Cancel",
                skillNameToken = DESOLATOR_PREFIX + "SPECIAL_DEPLOY_CANCEL_NAME",
                skillDescriptionToken = DESOLATOR_PREFIX + "SPECIAL_DEPLOY_CANCEL_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texTeslaSkillUtilityEpic"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployCancel)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 0,
                requiredStock = 0,
                stockToConsume = 0,
            });

            States.entityStates.Add(typeof(ThrowIrradiator));
            SkillDef irradiatorSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Desolator_Special_Tower",
                skillNameToken = DESOLATOR_PREFIX + "SPECIAL_IRRADIATOR_NAME",
                skillDescriptionToken = DESOLATOR_PREFIX + "SPECIAL_IRRADIATOR_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texDesolatorSkillSpecial"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ThrowIrradiator)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = 7f,
                beginSkillCooldownOnSkillEnd = false,
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
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_WEAK"}
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, deploySkillDef, irradiatorSkillDef);
        }

        //todo deso
        //todo DRY
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
            //why'd I do this separately again?
            //skilldefs[0] = createRecolorSkillDef("Red");

            if (Modules.Config.NewColor) {
                skilldefs.Add(createRecolorSkillDef("Black"));
            }

            for (int i = 0; i < skilldefs.Count; i++) {

                Modules.Skills.AddSkillToFamily(recolorFamily, skilldefs[i], i == 0 ? null : TeslaTrooperSurvivor.recolorsUnlockableDef);

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
                skillIcon = R2API.LoadoutAPI.CreateSkinIcon(color1, color1, color1, color1, color1),
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

            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(DESOLATOR_PREFIX + "DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                bodyCharacterModel.gameObject);

            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");
            
            skins.Add(defaultSkin);
            #endregion

            skinController.skins = skins.ToArray();
        }

        #endregion skins

        #region hook

        private void Hook() {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            //On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport) {

            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DamageTypes.DesolatorArmorShred)) {
                if (damageReport.victimBody.GetBuffCount(Buffs.desolatorArmorShredDeBuff) < 10) {
                    damageReport.victimBody.AddBuff(Buffs.desolatorArmorShredDeBuff);
                }
            }

            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DamageTypes.DesolatorDot)) {
                DotController.InflictDot(damageReport.victim.gameObject, damageReport.attacker, Dots.DesolatorDot, DotDuration * Mathf.Min(damageReport.damageInfo.procCoefficient * 2, 1));
            }
        }

        #endregion hook
    }
}