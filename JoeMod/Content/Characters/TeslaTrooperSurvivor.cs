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

            characterPortrait = Modules.Assets.LoadCharacterIcon("texIconTeslaTrooper"),
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
            bodyCharacterModel.baseRendererInfos[8].defaultMaterial.SetEmission(2);

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
                result = self.bodyInstanceObject.GetComponent<SkillLocator>().special.maxStock;
            }

            return result;
        }


        protected override void InitializeEntityStateMachine() {
            base.InitializeEntityStateMachine();
        }

        public override void InitializeUnlockables() {
            masterySkinUnlockableDef = UnlockableAPI.AddUnlockable<Achievements.TeslaTrooperMastery>();
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
            if (FacelessJoePlugin.conductiveMechanic && FacelessJoePlugin.conductiveAlly) {
                primarySkillDefZap.keywordTokens = new string[] { "KEYWORD_CHARGED" };
            }

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

            States.entityStates.Add(typeof(ShieldZap));
            SkillDef shieldSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
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

                Modules.Skills.AddSkillToFamily(recolorFamily, skilldefs[i], i == 0? null : masterySkinUnlockableDef);

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

            List<GameObject> allGameObjectActivations = Skins.getAllGameObjectActivations(childLocator, "child1", "child2");

            #region DefaultSkin

            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(FacelessJoePlugin.DEV_PREFIX + "_TESLA_BODY_DEFAULT_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texTeslaSkinDefault"),
                defaultRenderers,
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
            
            //On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            //On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage2;

            //On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            //On.RoR2.Orbs.LightningOrb.OnArrival += LightningOrb_OnArrival;

        }

        private void LightningOrb_OnArrival(On.RoR2.Orbs.LightningOrb.orig_OnArrival orig, RoR2.Orbs.LightningOrb self) {

            Helpers.LogWarning($"lightningorb - target: {self.target} | attacker: {self.attacker}");
            orig(self);


        }

        private static void HealthComponent_TakeDamage2(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {

            Helpers.LogWarning($"TakeDamage pre  - damageInfo.attacker: {damageInfo.attacker}");
            orig(self, damageInfo);
            Helpers.LogWarning($"TakeDamage post - damageInfo.attacker: {damageInfo.attacker}");
        }
        
        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim) {
            orig(self, damageInfo, victim);

            CharacterBody component2 = damageInfo.attacker?.GetComponent<CharacterBody>();

            Helpers.LogWarning($"OnHitEnemy - damageInfo.attacker: {damageInfo.attacker} | attacker CharacterBody: {component2 != null}");

            if (!component2)
                return;
            
            Helpers.LogWarning($" - damageinfo.damage {damageInfo.damage} | component2.damage {component2.damage} | damage/damage {damageInfo.damage/component2.damage} | >=4 {damageInfo.damage / component2.damage >=4f} | buff {component2.HasBuff(RoR2Content.Buffs.ElementalRingsReady)}");
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

            if (damageReport.attackerBodyIndex == BodyCatalog.FindBodyIndex("TeslaTowerBody")) {
                damageReport.damageInfo.attacker = null;
            }

            bool neverRetaliate = self.neverRetaliateFriendlies;

            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DamageTypes.conductive)) {
                self.neverRetaliateFriendlies = true;
            }

            orig(self, damageReport);

            self.neverRetaliateFriendlies = neverRetaliate;

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
                        self.body.AddBuff(Buffs.conductiveBuff);
                    }
                }
            }
        }

        private static void ConsumeConductive(HealthComponent self, DamageInfo damageInfo) {

            bool attackConsuming = damageInfo.HasModdedDamageType(DamageTypes.consumeConductive);
            if (attackConsuming) {
                //consume conductive stacks for damage and shock

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