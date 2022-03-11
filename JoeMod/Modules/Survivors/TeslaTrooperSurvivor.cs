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

namespace Modules.Survivors
{
    public class TeslaTrooperSurvivor : SurvivorBase {

        public override string bodyName => "TeslaTrooper";

        public const string TESLA_PREFIX = FacelessJoePlugin.DEV_PREFIX + "_TESLA_BODY_";

        public override string survivorTokenPrefix => TESLA_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            bodyName = "TeslaTrooperBody",
            bodyNameToken = TESLA_PREFIX + "NAME",
            subtitleNameToken = FacelessJoePlugin.DEV_PREFIX + "_TESLA_BODY_SUBTITLE",
            bodyColor = new Color(0.8f, 2, 2),
            characterPortrait = Modules.Assets.LoadCharacterIcon("texIconTeslaTrooper"),

            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            podPrefab = Assets.LoadAsset<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 140f,
            healthRegen = 1.5f,
            armor = 10f,

            damage = 12f,
            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; }

        public override Type characterMainState => typeof(TeslaTrooperMain);

        public override UnlockableDef characterUnlockableDef => null;

        public override ItemDisplaysBase itemDisplays => null;// new TeslaItemDisplays();

        private static UnlockableDef masterySkinUnlockableDef;

        public static DeployableSlot teslaTowerDeployableSlot;

        public override void Initialize() {
            base.Initialize();
            Hooks();
        }

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();
            bodyPrefab.AddComponent<TotallyOriginalTrackerComponent>();
            bodyPrefab.AddComponent<TeslaCoilControllerController>();

            RegisterTowerDeployable();
        }

        public DeployableAPI.GetDeployableSameSlotLimit GetTeslaTowerSlotLimit;

        private void RegisterTowerDeployable() {

            GetTeslaTowerSlotLimit += onGetTeslaTowerSlotLimit;

            teslaTowerDeployableSlot = DeployableAPI.RegisterDeployableSlot(onGetTeslaTowerSlotLimit);
        }

        private int onGetTeslaTowerSlotLimit(CharacterMaster self, int deployableCountMultiplier) {
            int result = 1;
            if (self.bodyInstanceObject) {
                result = self.bodyInstanceObject.GetComponent<SkillLocator>().special.maxStock;
            }

            return result;
        }


        protected override void InitializeEntityStateMachine() {
            base.InitializeEntityStateMachine();
        }

        public override void InitializeUnlockables() {
            masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.TeslaTrooperMastery>();
        }

        public override void InitializeDoppelganger(string clone) {
            base.InitializeDoppelganger("Engi");
        }

        public override void InitializeHitboxes() {
            base.InitializeHitboxes();
        }
        
        protected override void InitializeSurvivor() {
            base.InitializeSurvivor();
        }

        protected override void InitializeDisplayPrefab() {
            base.InitializeDisplayPrefab();
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
            States.entityStates.Add(typeof(Zap));
            TeslaTrackingSkillDef primarySkillDefZap = Modules.Skills.CreateSkillDef<TeslaTrackingSkillDef>(new SkillDefInfo("Tesla_Primary_Zap",
                                                                                                            TESLA_PREFIX + "PRIMARY_ZAP_NAME",
                                                                                                            TESLA_PREFIX + "PRIMARY_ZAP_DESCRIPTION",
                                                                                                            Modules.Assets.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                                                                            new EntityStates.SerializableEntityStateType(typeof(Zap)),
                                                                                                            "Weapon",
                                                                                                            false));
            primarySkillDefZap.keywordTokens = new string[] { "KEYWORD_CHARGED" };

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefZap);
        }

        private void InitializeSecondarySkills()
        {
            States.entityStates.Add(typeof(AimBigZap));
            States.entityStates.Add(typeof(BigZap));
            SkillDef bigZapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Secondary_BigZap",
                skillNameToken = TESLA_PREFIX + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SECONDARY_BIGZAP_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("textures/bufficons/texbuffteslaicon"), //Modules.Assets.LoadAsset<Sprite>("skill2_icon"),              //todo .TeslaTrooper
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
                keywordTokens = new string[] { "KEYWORD_STUNNING" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, bigZapSkillDef);
        }

        private void InitializeUtilitySkills()
        {

            States.entityStates.Add(typeof(ShieldZap));
            SkillDef rollSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Utility_ShieldZap",
                skillNameToken = TESLA_PREFIX + "UTILITY_BARRIER_NAME",
                skillDescriptionToken = TESLA_PREFIX + "UTILITY_BARRIER_DESCRIPTION",
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
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, rollSkillDef);
        }

        private void InitializeSpecialSkills() {
            States.entityStates.Add(typeof(DeployTeslaTower));

            SkillDef teslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tesla_Special_Tower",
                skillNameToken = TESLA_PREFIX + "SPECIAL_TOWER_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SPECIAL_TOWER_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("textures/itemicons/texteslacoilicon"), //Modules.Assets.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployTeslaTower)),
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
                stockToConsume = 0,

                keywordTokens = new string[] { "KEYWORD_SHOCKING" }
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, teslaCoilSkillDef);
        }

        private void InitializeRecolorSkills() {
            SkillFamily recolorFamily = Modules.Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "Recolor", true).skillFamily;

            List<SkillDef> skilldefs = new List<SkillDef> {
                null,
                createRecolorSkillDef("Blue", Color.blue),
                createRecolorSkillDef("Green", Color.green),
                createRecolorSkillDef("Yellow", Color.yellow),
                createRecolorSkillDef("Orange", new Color(255f/255f, 156f/255f, 0f)),
                createRecolorSkillDef("Cyan", Color.cyan),
                createRecolorSkillDef("Purple", new Color(145f/255f, 0, 200f/255f)),
                createRecolorSkillDef("Pink", new Color(255f/255f, 132f/255f, 235f/255f)),
            };

            skilldefs[0] = createRecolorSkillDef("Red", Color.red);

            if (Modules.Config.NewColor) {
                skilldefs.Add(createRecolorSkillDef("Black", Color.black));
            }

            for (int i = 0; i < skilldefs.Count; i++) {
                Modules.Skills.AddSkillToFamily(recolorFamily, skilldefs[i], masterySkinUnlockableDef);

                AddCssPreviewSkill(i, recolorFamily, skilldefs[i]);
            }
        }

        private SkillDef createRecolorSkillDef(string name, Color iconColor) {

            Color color1 = Color.white;
            Color color2 = Color.white;

            Recolor[] thing = characterBodyModel.GetComponent<SkinRecolorController>().Recolors;

            for (int i = 0; i < thing.Length; i++) {

                Recolor recolor = thing[i];

                if (recolor.recolorName == name.ToLower()) {

                    color1 = recolor.mainColor * 0.69f;
                    color2 = recolor.offColor;
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
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin

            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(FacelessJoePlugin.DEV_PREFIX + "_TESLA_BODY_DEFAULT_SKIN_NAME",
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

        #region hooks
        protected void Hooks() {

            On.RoR2.CharacterMaster.AddDeployable += CharacterMaster_AddDeployable;
            On.RoR2.Inventory.CopyItemsFrom_Inventory_Func2 += Inventory_CopyItemsFrom_Inventory_Func2; ;
            //On.RoR2.MasterSummon.Perform += MasterSummon_Perform;
            //On.RoR2.CharacterBody.HandleConstructTurret += CharacterBody_HandleConstructTurret;

            On.RoR2.ModelSkinController.ApplySkin += ModelSkinController_ApplySkin;
            
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void ModelSkinController_ApplySkin(On.RoR2.ModelSkinController.orig_ApplySkin orig, ModelSkinController self, int skinIndex) {
            orig(self, skinIndex);

            SkinRecolorController skinRecolorController = self.GetComponent<SkinRecolorController>();
            if (skinRecolorController) {

                SkillDef color = self.characterModel.body?.skillLocator?.FindSkill("Recolor")?.skillDef;
                if (color)
                    skinRecolorController.SetRecolor(color.skillName.ToLower());
            }
        }

        #region tower hacks
        private void Inventory_CopyItemsFrom_Inventory_Func2(On.RoR2.Inventory.orig_CopyItemsFrom_Inventory_Func2 orig, Inventory self, Inventory other, Func<ItemIndex, bool> filter) {
            if (MasterCatalog.FindMasterIndex(self.gameObject) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {
                //Helpers.LogWarning("copyitemsfrom true");
                filter = TeslaTowerCopyFilter;
            }
            orig(self, other, filter);
        }
        private void CharacterMaster_AddDeployable(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot) {
            if (MasterCatalog.FindMasterIndex(deployable.gameObject) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {
                //Helpers.LogWarning("adddeployable true");
                slot = teslaTowerDeployableSlot;
            }

            orig(self, deployable, slot);
        }

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

            if (FacelessJoePlugin.conductiveMechanic) {
                Conductive(self, damageInfo);
            }

            orig(self, damageInfo);
        }

        private static void Conductive(HealthComponent self, DamageInfo damageInfo) {

            //mark enemies (or allies) conductive
            bool attackConductive = damageInfo.HasModdedDamageType(DamageTypes.conductive);
            if (attackConductive) {
                if (damageInfo.attacker.GetComponent<TeamComponent>()?.teamIndex == self.body.teamComponent.teamIndex) {
                    if (self.body.GetBuffCount(Buffs.conductiveBuffTeam) < 3) {
                        self.body.AddBuff(Buffs.conductiveBuffTeam);
                    }
                } else {
                    self.body.AddBuff(Buffs.conductiveBuff);
                }
            }

            //consume conductive stacks for damage and shock
            bool attackConsuming = damageInfo.HasModdedDamageType(DamageTypes.consumeConductive);
            if (attackConsuming) {
                int conductiveCount = self.body.GetBuffCount(Buffs.conductiveBuff);

                for (int i = 0; i < conductiveCount; i++) {

                    self.body.RemoveBuff(Buffs.conductiveBuff);
                }

                if (conductiveCount > 0) {

                    damageInfo.AddModdedDamageType(DamageTypes.shockXs);
                    damageInfo.damage *= 1f + (0.2f * conductiveCount);
                }
            }

            //consume allied charged stacks for damage boost
            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            bool teamCharged = attackerBody && attackerBody.HasBuff(Buffs.conductiveBuffTeam);
            if (teamCharged) {

                int buffCount = attackerBody.GetBuffCount(Buffs.conductiveBuffTeam);
                for (int i = 0; i < buffCount; i++) {
                    self.body.RemoveBuff(Buffs.conductiveBuff);
                }

                damageInfo.damage *= 1f + (0.1f * buffCount);
            }
        }
        #endregion
        #endregion hooks
    }
}