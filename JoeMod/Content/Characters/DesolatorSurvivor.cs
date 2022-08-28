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

        public override void Initialize() {
            instance = this;
            base.Initialize();

            //todo deso
            RegisterIrradiatorDeployable();
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

            InitializePrimarySkills();

            InitializeSecondarySkills();

            InitializeUtilitySkills();

            InitializeSpecialSkills();

            InitializeRecolorSkills();

            FinalizeCSSPreviewDisplayController();
        }


        private void InitializePrimarySkills() {
            States.entityStates.Add(typeof(RadBeam));
            SkillDef primarySkillDefPunch =
                Skills.CreateSkillDef(new SkillDefInfo("Desolator_Primary_Beam",
                                                       DESOLATOR_PREFIX + "bem",
                                                       DESOLATOR_PREFIX + "shoot a beam",
                                                       Modules.Assets.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                       new EntityStates.SerializableEntityStateType(typeof(RadBeam)),
                                                       "Weapon",
                                                       false));
            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefPunch);
        }

        private void InitializeSecondarySkills() {

            States.entityStates.Add(typeof(AimBigRadBeam));
            SkillDef bigRadBeamSkillDef =
                Skills.CreateSkillDef(new SkillDefInfo {
                    skillName = "Desolator_Secondary_BigZap",
                    skillNameToken = DESOLATOR_PREFIX + "SECONDARY_BIGBEAM_NAME",
                    skillDescriptionToken = DESOLATOR_PREFIX + "SECONDARY_BIGBEAM_DESCRIPTION",
                    skillIcon = Modules.Assets.LoadAsset<Sprite>("texTeslaSkillSecondary"),
                    activationState = new EntityStates.SerializableEntityStateType(typeof(AimBigRadBeam)),
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
                    stockToConsume = 1
                });
            bigRadBeamSkillDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            Modules.Skills.AddSecondarySkills(bodyPrefab, bigRadBeamSkillDef);
        }

        private void InitializeUtilitySkills() {

            States.entityStates.Add(typeof(AimBigRadBeam));
            SkillDef primarySkillDefPunch =
                Skills.CreateSkillDef(new SkillDefInfo("Desolator_Primary_Beam",
                                                       DESOLATOR_PREFIX + " bem",
                                                       DESOLATOR_PREFIX + " shoot a big bem",
                                                       Modules.Assets.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                       new EntityStates.SerializableEntityStateType(typeof(AimBigRadBeam)),
                                                       "Weapon",
                                                       false));
            Modules.Skills.AddUtilitySkills(bodyPrefab, primarySkillDefPunch);
        }

        private void InitializeSpecialSkills() {

            States.entityStates.Add(typeof(ThrowIrradiator));
            SkillDef irradiatorSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Desolator_Special_Tower",
                skillNameToken = DESOLATOR_PREFIX + "SPECIAL_IRRADIATOR_NAME",
                skillDescriptionToken = DESOLATOR_PREFIX + "SPECIAL_IRRADIATOR_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texTeslaSkillSpecial"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ThrowIrradiator)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 9f,
                beginSkillCooldownOnSkillEnd = false,
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
                stockToConsume = 1,
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, irradiatorSkillDef);
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
                skillNameToken = $"{DESOLATOR_PREFIX}RECOLOR_{name.ToUpper()}_NAME",
                skillDescriptionToken = "",
                skillIcon = R2API.LoadoutAPI.CreateSkinIcon(color1, color1, color1, color1, color1),
            });
        }

        #endregion skills

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
    }
}