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

namespace Modules.Survivors
{
    internal class TeslaTrooperSurvivor : SurvivorBase {
        #region survivor stuff
        public override string bodyName => "TeslaTrooper";

        public const string TESLA_PREFIX = FacelessJoePlugin.DEV_PREFIX + "_TESLA_BODY_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => TESLA_PREFIX;
        
        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            bodyName = "TeslaTrooperBody",
            bodyNameToken = TESLA_PREFIX + "NAME",
            subtitleNameToken = FacelessJoePlugin.DEV_PREFIX + "_TESLA_BODY_SUBTITLE",

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

        private static UnlockableDef masterySkinUnlockableDef;
        private static UnlockableDef grandMasterySkinUnlockableDef;
        private static UnlockableDef recolorsUnlockableDef = null;
        #endregion

        #region cool stuff
        public static float conductiveAllyBoost = 1.3f;

        public static DeployableSlot teslaTowerDeployableSlot;
        public DeployableAPI.GetDeployableSameSlotLimit GetTeslaTowerSlotLimit;
        #endregion

        public override void Initialize() {
            base.Initialize();
            Hooks();
        }

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();
            bodyPrefab.AddComponent<TeslaTrackerComponent>();
            bodyPrefab.AddComponent<TeslaTowerControllerController>();
            bodyPrefab.AddComponent<TeslaWeaponComponent>();
            bodyPrefab.AddComponent<ZapBarrierController>();

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
        }

        public override void InitializeUnlockables() {
            masterySkinUnlockableDef = UnlockableAPI.AddUnlockable<Achievements.TeslaTrooperMastery>();
            grandMasterySkinUnlockableDef = UnlockableAPI.AddUnlockable<Achievements.TeslaTrooperGrandMastery>();
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
            States.entityStates.Add(typeof(Zap));                                                           //this constructor creates a skilldef for a typical primary
            TeslaTrackingSkillDef primarySkillDefZap = Modules.Skills.CreateSkillDef<TeslaTrackingSkillDef>(new SkillDefInfo("Tesla_Primary_Zap",
                                                                                                            TESLA_PREFIX + "PRIMARY_ZAP_NAME",
                                                                                                            TESLA_PREFIX + "PRIMARY_ZAP_DESCRIPTION",
                                                                                                            Modules.Assets.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                                                                            new EntityStates.SerializableEntityStateType(typeof(Zap)),
                                                                                                            "Weapon",
                                                                                                            false));
            if (FacelessJoePlugin.conductiveMechanic && FacelessJoePlugin.conductiveAlly) {
                primarySkillDefZap.keywordTokens = new string[] { "KEYWORD_CHARGED" };
            }

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefZap);

            if (Config.Cursed) {
                States.entityStates.Add(typeof(ZapPunch));
                SkillDef primarySkillDefPunch = Modules.Skills.CreateSkillDef(new SkillDefInfo("Tesla_Primary_Punch",
                                                                             TESLA_PREFIX + "PRIMARY_PUNCH_NAME",
                                                                             TESLA_PREFIX + "PRIMARY_PUNCH_DESCRIPTION",
                                                                             Modules.Assets.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                                             new EntityStates.SerializableEntityStateType(typeof(ZapPunch)),
                                                                             "Weapon",
                                                                             false));
                Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefPunch);
            }

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
                keywordTokens = new string[] { "KEYWORD_STUNNING" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, bigZapSkillDef);
        }

        private void InitializeUtilitySkills() {

            States.entityStates.Add(typeof(ShieldZapStart));
            States.entityStates.Add(typeof(ShieldZapCollectDamage));
            States.entityStates.Add(typeof(ShieldZapReleaseDamage));
            SkillDef shieldSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tesla_Utility_ShieldZap",
                skillNameToken = TESLA_PREFIX + "UTILITY_BARRIER_NAME",
                skillDescriptionToken = TESLA_PREFIX + "UTILITY_BARRIER_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texTeslaSkillUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ShieldZapCollectDamage)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 6f,
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

            Modules.Skills.AddUtilitySkills(bodyPrefab, shieldSkillDef);
        }

        private void InitializeSpecialSkills() {

            States.entityStates.Add(typeof(DeployTeslaTower));
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

                keywordTokens = new string[] { "KEYWORD_SHOCKING" }
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, teslaCoilSkillDef);
        }

        //todo this is just lysate cell
        //make it not lysate cell
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills() {

            SkillDef scepterTeslaCoilSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tesla_Special_Scepter_Tower",
                skillNameToken = TESLA_PREFIX + "SPECIAL_SCEPTER_TOWER_NAME",
                skillDescriptionToken = TESLA_PREFIX + "SPECIAL_SCEPTER_TOWER_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texTeslaSkillSpecialScepter"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DeployTeslaTower)),
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
            //why'd I do this separately again?
            //skilldefs[0] = createRecolorSkillDef("Red");

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
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            List<GameObject> activatedGameObjects = Skins.createAllActivatedGameObjectsList(childLocator,
                "meshTeslaArmor_Fanservice",
                "meshTeslaEmission");

            #region DefaultSkin

            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(TESLA_PREFIX + "DEFAULT_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinDefault"),
                defaultRenderers,
                model);
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

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(TESLA_PREFIX + "MASTERY_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinMastery"),
                defaultRenderers,
                model,
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
                TeslaTowerNotSurvivor.MasteryMinionSkinReplacement
            };

            skins.Add(masterySkin);
            #endregion

            #region NodSkin

            SkinDef nodSkin = Modules.Skins.CreateSkinDef(TESLA_PREFIX + "NOD_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinNod"),
                defaultRenderers,
                model,
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
                TeslaTowerNotSurvivor.NodMinionSkinReplacement
            };

            skins.Add(nodSkin);
            #endregion

            #region MCSkin
            SkinDef MCSkin = Modules.Skins.CreateSkinDef(TESLA_PREFIX + "MC_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinMC"),
                defaultRenderers,
                model);

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
                TeslaTowerNotSurvivor.MCMinionSkinReplacement
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

            //keep allies from retaliating against trooper charging them
            bool originalNeverRetaliate = self.neverRetaliateFriendlies;
            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DamageTypes.conductive)) {
                self.neverRetaliateFriendlies = true;
            }
            
            orig(self, damageReport);

            self.neverRetaliateFriendlies = originalNeverRetaliate;
            damageReport.damageInfo.attacker = originalAttacker;
        }

        #region tower hacks
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

        private void CharacterMaster_AddDeployable(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot) {
            if (MasterCatalog.FindMasterIndex(deployable.gameObject) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {
                //Helpers.LogWarning("adddeployable true");
                slot = teslaTowerDeployableSlot;
            }

            orig(self, deployable, slot);
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

            orig(self, damageInfo);
        }

        private static void CheckConductive(HealthComponent self, DamageInfo damageInfo) {

            if (FacelessJoePlugin.conductiveMechanic) {
                ApplyConductive(self, damageInfo);
            }

            if (FacelessJoePlugin.conductiveEnemy) {
                ConsumeConductive(self, damageInfo);
            }

            if (FacelessJoePlugin.conductiveAlly) {
                ConsumeConductiveAlly(self, damageInfo);
            }
        }

        private static void ApplyConductive(HealthComponent self, DamageInfo damageInfo) {

            //mark enemies (or allies) conductive
            bool attackConductive = damageInfo.HasModdedDamageType(DamageTypes.conductive);
            if (attackConductive) {
                if (damageInfo.attacker?.GetComponent<TeamComponent>()?.teamIndex == self.body.teamComponent.teamIndex) {
                    if (FacelessJoePlugin.conductiveAlly) {
                        if (self.body.GetBuffCount(Buffs.conductiveBuffTeam) < 1) {
                            self.body.AddBuff(Buffs.conductiveBuffTeam);
                        }
                    }
                } else {
                    if (FacelessJoePlugin.conductiveEnemy) {
                        if (self.body.GetBuffCount(Buffs.conductiveBuff) < 3) {
                            self.body.AddBuff(Buffs.conductiveBuff);
                        }
                    }
                }
            }
        }

        private static void ConsumeConductive(HealthComponent self, DamageInfo damageInfo) {
            
            //consume conductive stacks for damage and shock
            bool attackConsuming = damageInfo.HasModdedDamageType(DamageTypes.consumeConductive);
            if (attackConsuming) {

                int conductiveCount = self.body.GetBuffCount(Buffs.conductiveBuff);

                for (int i = 0; i < conductiveCount; i++) {

                    self.body.RemoveBuff(Buffs.conductiveBuff);
                }

                if (conductiveCount > 0) {

                    damageInfo.AddModdedDamageType(DamageTypes.shockMed);
                    damageInfo.damage *= 1f + (0.2f * conductiveCount);
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

                    damageInfo.AddModdedDamageType(DamageTypes.shockShort);
                    damageInfo.damage *= conductiveAllyBoost;// 1f + (0.1f * buffCount);
                }
            }
        }
        #endregion conductive
        #endregion hooks
    }
}