using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using ModdedEntityStates.Joe;
using UnityEngine;
using Modules.Characters;
using Modules.Survivors;
using Modules;
using JoeModForReal.Components;
using System.Runtime.CompilerServices;
using ModdedEntityStates.Genji;
using UnityEngine.Networking;
using System.Linq;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using R2API;

namespace JoeModForReal.Content.Survivors {

    internal class GenjiSurvivor : SurvivorBase
    {
        public override string characterName => "Genji";
        public override string mdlName => "Joe";

        public const string GENJI_PREFIX = FacelessJoePlugin.DEV_PREFIX + "_GENJI_";

        public override string survivorTokenPrefix => GENJI_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            bodyName = "GenjiBody",
            bodyNameToken = GENJI_PREFIX + "NAME",
            subtitleNameToken = GENJI_PREFIX + "SUBTITLE",
            sortPosition = 69.6f,

            characterPortrait = Modules.Asset.LoadCharacterIcon("texIconWeeb"),
            bodyColor = Color.green,

            crosshair = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 2f,
            armor = 0f,

            jumpCount = 2,
            
            aimOriginPosition = new Vector3(0, 1.3f, 0),
            cameraParamsDepth = -10,
        
            cameraParamsVerticalOffset = 1.0f,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; }

        public override Type characterMainState => typeof(ModdedEntityStates.Joe.JoeMain);

        public override ItemDisplaysBase itemDisplays => new JoeItemDisplays();

        public override ConfigEntry<bool> characterEnabledConfig => null;
        
        public override UnlockableDef characterUnlockableDef { get; }
        private static UnlockableDef masterySkinUnlockableDef;

        public static SteppedSkillDef swingBladeSkillDef;

        public override void Initialize() {

            if (!Config.Cursed.Value)
                return;

            GenjiConfig.Init();
            GenjiProjectiles.Init();
            GenjiDamageTypes.Init();
            GenjiTokens.Init();

            base.Initialize();
            
            Hook();
        }

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();
            bodyPrefab.GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.SprintAnyDirection;

            GenjiAssistTracker damageTracker = bodyPrefab.AddComponent<GenjiAssistTracker>();
            damageTracker.skillLocator = bodyPrefab.GetComponent<SkillLocator>();
            bodyPrefab.AddComponent<UltimateBuildup>();

            bodyPrefab.AddComponent<DeflectAttackReceiver>();

            HurtBoxGroup hurtBoxGroup = bodyCharacterModel.GetComponent<HurtBoxGroup>();
            HurtBox deflectHurtbox = bodyCharacterModel.GetComponent<ChildLocator>().FindChildComponent<HurtBox>("DeflectHitbox");
            deflectHurtbox.healthComponent = bodyPrefab.GetComponent<HealthComponent>();
            deflectHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            hurtBoxGroup.hurtBoxes = hurtBoxGroup.hurtBoxes.Concat(new HurtBox[] { deflectHurtbox }).ToArray();
            hurtBoxGroup.OnValidate();
        }

        protected override void InitializeCharacterModel() {
            base.InitializeCharacterModel();
        }

        public override void InitializeDoppelganger(string clone) {

            base.InitializeDoppelganger("Loader");
        }

        public override void InitializeUnlockables()
        {
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
        }

        public override void InitializeHitboxes()
        {
            //hitboxes already set up baybee
            return;

            //ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            //GameObject model = childLocator.gameObject;
            
            //Modules.Prefabs.SetupHitbox(model, "Sword1", childLocator.FindChild("hitboxSwing1"));
            //Modules.Prefabs.SetupHitbox(model, "Sword2", childLocator.FindChild("hitboxSwing2"));
            //Modules.Prefabs.SetupHitbox(model, "SwordJump", childLocator.FindChild("hitboxJumpSwing"));
        }

        public override void InitializeSkills() {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            InitializePassive();

            InitializePrimarySkills();

            InitializeSecondarySkills();

            InitializeUtilitySkills();

            InitializeSpecialSkills();

            if (Compat.ScepterInstalled) {
                InitializeScepterSkills();
            }
        }

        private void InitializePassive() {
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill {
                enabled = true,
                icon = null,
                skillNameToken = GENJI_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = GENJI_PREFIX + "PASSIVE_DESCRIPTION"
            };
        }

        private void InitializePrimarySkills() {

            SkillDef primarySkillDef = Modules.Skills.CreateSkillDef(
                new SkillDefInfo("GenjiShuriken",
                                 GENJI_PREFIX + "PRIMARY_SHURIKEN_NAME",
                                 GENJI_PREFIX + "PRIMARY_SHURIKEN_DESCRIPTION",
                                 Modules.Asset.LoadAsset<Sprite>("texIconPrimary"),
                                 new EntityStates.SerializableEntityStateType(typeof(ThrowShuriken)),
                                 "Weapon",
                                 true));

            SkillDef primarySkillDef2 = Modules.Skills.CreateSkillDef(
                new SkillDefInfo("GenjiShurikenAlt",
                                 GENJI_PREFIX + "PRIMARY_SHURIKENALT_NAME",
                                 GENJI_PREFIX + "PRIMARY_SHURIKENALT_DESCRIPTION",
                                 Modules.Asset.LoadAsset<Sprite>("texIconPrimaryJumpSwing"),
                                 new EntityStates.SerializableEntityStateType(typeof(ThrowShurikenAlt)),
                                 "Weapon",
                                 true));

            ConditionalSkillOverride2 skillOverrideComponent = bodyPrefab.AddComponent<ConditionalSkillOverride2>();
            skillOverrideComponent.characterBody = bodyPrefab.GetComponent<CharacterBody>();
            skillOverrideComponent.conditionalSkillInfos = new ConditionalSkillOverride2.ConditionalSkillInfo[]{
                new ConditionalSkillOverride2.ConditionalSkillInfo {
                    skillSlot = bodyPrefab.GetComponent<SkillLocator>().primary,
                    sprintSkillDef = primarySkillDef2,
                }
            };

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);
        }

        private void InitializeSecondarySkills() {

            SkillDef deflectSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = GENJI_PREFIX + "SECONDARY_FIREBALL_NAME",
                skillNameToken = GENJI_PREFIX + "SECONDARY_FIREBALL_NAME",
                skillDescriptionToken = GENJI_PREFIX + "SECONDARY_FIREBALL_DESCRIPTION",
                skillIcon = Modules.Asset.LoadAsset<Sprite>("texIconSecondary"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Deflect)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = GenjiConfig.deflectCooldown.Value,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, deflectSkillDef);
        }

        private void InitializeUtilitySkills() {

            AssistResetSkillDef dashSkillDef = Modules.Skills.CreateSkillDef<AssistResetSkillDef>(new SkillDefInfo {
                skillName = GENJI_PREFIX + "UTILITY_DASH_NAME",
                skillNameToken = GENJI_PREFIX + "UTILITY_DASH_NAME",
                skillDescriptionToken = GENJI_PREFIX + "UTILITY_DASH_DESCRIPTION",
                skillIcon = Modules.Asset.LoadAsset<Sprite>("texIconUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Genji.Dash)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = GenjiConfig.dashCooldown.Value,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, dashSkillDef);
        }

        private void InitializeSpecialSkills() {

            DamageBuildupSkillDef dragonbladeSkillDef = Modules.Skills.CreateSkillDef<DamageBuildupSkillDef>(new SkillDefInfo {
                skillName = GENJI_PREFIX + "SPECIAL_DRAGONBLADE_NAME",
                skillNameToken = GENJI_PREFIX + "SPECIAL_DRAGONBLADE_NAME",
                skillDescriptionToken = GENJI_PREFIX + "SPECIAL_DRAGONBLADE_DESCRIPTION",
                skillIcon = Modules.Asset.LoadAsset<Sprite>("texIconSpecial"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DragonBlade)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = GenjiConfig.dragonBladeChargeRequired.Value,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, dragonbladeSkillDef);

            swingBladeSkillDef = Modules.Skills.CreateSkillDef<LookingDownSkillDef>(
                new SkillDefInfo("Onion",
                                 GENJI_PREFIX + "SPECIAL_SWINGBLADE_NAME",
                                 GENJI_PREFIX + "SPECIAL_SWINGBLADE_DESCRIPTION",
                                 Modules.Asset.LoadAsset<Sprite>("texIconPrimary"),
                                 new EntityStates.SerializableEntityStateType(typeof(SwingBlade)),
                                 "Weapon",
                                 true));
            swingBladeSkillDef.stepCount = 2;
            swingBladeSkillDef.stepGraceDuration = 1.2f;
        }


        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills() {

            //AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(primarySkillDef, "GenjiBody", SkillSlot.Primary, 0);
            //if (Config.Cursed) {
            //    AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(primarySkillDef, "GenjiBody", SkillSlot.Primary, 1);
            //}
        }

        public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();
            
            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef("DEFAULT_SKIN",
                Asset.LoadAsset<Sprite>("texiconSkinDefault"),
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


        private void Hook() {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.BulletAttack.ProcessHit += BulletAttack_ProcessHit;

            IL.EntityStates.GolemMonster.FireLaser.OnEnter += FireLaser_OnEnter;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport) {

            if (damageReport.damageInfo.HasModdedDamageType(GenjiDamageTypes.GolemLaser)) {
                if (damageReport.victimBody != null && damageReport.victimBody.gameObject.TryGetComponent(out IGolemLaserReceiver laserReceiver)) {
                    laserReceiver.receiveGolemLaser();
                }
            }
        }

        private bool BulletAttack_ProcessHit(On.RoR2.BulletAttack.orig_ProcessHit orig,
                                             BulletAttack self,
                                             ref BulletAttack.BulletHit hitInfo) {

            bool origReturn = orig(self, ref hitInfo);
            //if (origReturn) {
            if (hitInfo.hitHurtBox && hitInfo.hitHurtBox.healthComponent && hitInfo.hitHurtBox.healthComponent.body) {
                if (hitInfo.hitHurtBox.healthComponent.body.gameObject != self.owner) {
                    if (hitInfo.hitHurtBox.healthComponent.body.TryGetComponent(out IBulletAttackReceiver receiver)) {

                        receiver.receiveBulletAttack(self, hitInfo);
                    }
                }
            }
            //}

            return origReturn;
        }

        //add a damagetype to golem laser so they can be detected in a hook
        //wow i can do il
        private void FireLaser_OnEnter(MonoMod.Cil.ILContext il) {

            ILCursor cursor = new ILCursor(il);
            cursor.GotoNext(MoveType.After,
                //these instructions match right after blastattack is created (make sure to minus cursor index by 1)
                instruction => instruction.MatchNewobj<BlastAttack>(),
                instruction => instruction.MatchDup(),
                instruction => instruction.MatchLdarg(0)
                );
            cursor.Index--;
            cursor.EmitDelegate<Func<BlastAttack, BlastAttack>>((blastAttack) => {
                blastAttack.AddModdedDamageType(GenjiDamageTypes.GolemLaser);
                return blastAttack;
            });
        }
    }
}