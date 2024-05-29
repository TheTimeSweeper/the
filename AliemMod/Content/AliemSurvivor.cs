﻿using AliemMod.Components;
using AliemMod.Components.Bundled;
using BepInEx.Configuration;
using KinematicCharacterController;
using ModdedEntityStates.Aliem;
using Modules;
using Modules.Characters;
using Modules.Survivors;
using R2API;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static RoR2.CharacterSelectSurvivorPreviewDisplayController;

namespace AliemMod.Content.Survivors {

    internal class AliemSurvivor : SurvivorBase {
        public override string bodyName => "Aliem";

        public const string ALIEM_PREFIX = AliemPlugin.DEV_PREFIX + "_ALIEM_BODY_";
        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => ALIEM_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo {
            bodyPrefabName = "AliemBody",
            bodyNameToken = ALIEM_PREFIX + "NAME",
            subtitleNameToken = ALIEM_PREFIX + "SUBTITLE",

            characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("texIconAliem"),
            bodyColor = Color.yellow,
            sortPosition = 70f,

            crosshair = Assets.LoadCrosshair("Default"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 10f,

            jumpCount = 2,

            aimOriginPosition = new Vector3(0, 0.9f, 0),
            cameraPivotPosition = new Vector3(0, 0.8f, 0),
            modelBasePosition = new Vector3(0, -0.52f, 0),

            cameraParamsDepth = -7,
            cameraParamsVerticalOffset = 0.6f,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "MeshBody",
                },
                new CustomRendererInfo
                {
                    childName = "MeshBlaster",
                },
                new CustomRendererInfo
                {
                    childName = "MeshWeapon2",
                },
                new CustomRendererInfo
                {
                    childName = "MeshKnife",
                },
        };

        public override UnlockableDef characterUnlockableDef => null;

        public override Type characterMainState => typeof(ModdedEntityStates.Aliem.AliemCharacterMain);

        public override ItemDisplaysBase itemDisplays => new AliemItemDisplays();

        //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef masterySkinUnlockableDef;

        public static AliemSurvivor instance;
        private CharacterSelectSurvivorPreviewDisplayController cssPreviewDisplayController;

        public override void InitializeCharacter() {
            base.InitializeCharacter();

            instance = this;

            Hooks();

            if (AliemConfig.Cursed.Value)
            {
                bodyPrefab.AddComponent<RayGunChargeComponent>();
            }
            bodyPrefab.AddComponent<WeaponSecondaryController>();

            Modules.Config.ConfigureBody(bodyPrefab.GetComponent<CharacterBody>(), AliemConfig.sectionBody);

            //VehicleSeat vehicleSeat = bodyPrefab.AddComponent<VehicleSeat>();
            //vehicleSeat.passengerState = new EntityStates.SerializableEntityStateType(typeof(AliemRidingState));
            //vehicleSeat.hidePassenger = false;
            //vehicleSeat.disablePassengerMotor = true;
            //vehicleSeat.isEquipmentActivationAllowed = true;
        }

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();

            CreateBurrowEffect();

            FixMotorCollider();

            cssPreviewDisplayController = displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();

            bodyCharacterModel.GetComponent<ChildLocator>().FindChild("FakeAimOrigin").transform.position = bodyPrefab.GetComponent<CharacterBody>().aimOriginTransform.position;

            //todo animate popping out of the ground for css
            //displayPrefab.AddComponent<AliemMenuSound>();
        }

        private void FixMotorCollider() {
            CapsuleCollider characterCollider = bodyPrefab.GetComponent<CapsuleCollider>();
            //characterCollider.center = new Vector3(0, 0.51f, 0);
            characterCollider.radius = 0.302f;
            characterCollider.height = 1.021f;

            KinematicCharacterMotor motor = bodyPrefab.GetComponent<KinematicCharacterMotor>();
            motor.CapsuleRadius = 0.302f;
            motor.CapsuleHeight = 1.021f;
        }

        private void Hooks() {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.ModelSkinController.ApplySkin += ModelSkinController_ApplySkin;
            On.RoR2.SurvivorMannequins.SurvivorMannequinSlotController.ApplyLoadoutToMannequinInstance += SurvivorMannequinSlotController_ApplyLoadoutToMannequinInstance;
        }

        private void SurvivorMannequinSlotController_ApplyLoadoutToMannequinInstance(On.RoR2.SurvivorMannequins.SurvivorMannequinSlotController.orig_ApplyLoadoutToMannequinInstance orig, RoR2.SurvivorMannequins.SurvivorMannequinSlotController self)
        {
            orig(self);
            CharacterModel model = self.GetComponentInChildren<CharacterModel>();
            if (model != null)
            {
                ApplyWeaponSkin(model.gameObject);
            }
        }

        private void ModelSkinController_ApplySkin(On.RoR2.ModelSkinController.orig_ApplySkin orig, ModelSkinController self, int skinIndex)
        {
            orig(self, skinIndex);
            ApplyWeaponSkin(self.gameObject);
        }

        private static void ApplyWeaponSkin(GameObject self)
        {
            if (self.TryGetComponent(out WeaponSkinController weaponSkinController))
            {
                weaponSkinController.ApplyCurrentWeaponSkin();
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {

            if(DamageAPI.HasModdedDamageType(damageInfo, DamageTypes.Decapitating)) {

                if(self.gameObject.GetComponent<ChompedComponent>() == null) {
                    self.gameObject.AddComponent<ChompedComponent>().init(self.body);
                }
            }
            orig(self, damageInfo);
        }

        private void CreateBurrowEffect() {
            GameObject treebot = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TreebotBody");
            Transform treebotburrow = treebot.GetComponent<ModelLocator>().modelTransform.Find("BurrowCenter");

            Transform aliemBurrow = bodyCharacterModel.GetComponent<ChildLocator>().FindChild("Burrow");

            ParticleSystem.MainModule debrisParticles = UnityEngine.Object.Instantiate(treebotburrow.Find("ParticleLoop/Debris").gameObject, aliemBurrow, false).GetComponent<ParticleSystem>().main;
            debrisParticles.loop = true;
            debrisParticles.playOnAwake = true;

            ParticleSystem.MainModule dustParticles = UnityEngine.Object.Instantiate(treebotburrow.Find("ParticleLoop/Dust").gameObject, aliemBurrow, false).GetComponent<ParticleSystem>().main;
            dustParticles.loop = true;
            dustParticles.playOnAwake = true;

            aliemBurrow.gameObject.SetActive(false);
        }

        public override void InitializeUnlockables() {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            masterySkinUnlockableDef = R2API.UnlockableAPI.AddUnlockable<Content.Achievements.AliemMasteryAchievement>();
        }

        public override void InitializeHitboxes() {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;
            
            Transform leapHitbox = childLocator.FindChild("LeapHitbox");
            Prefabs.SetupHitbox(model, leapHitbox, "Leap");

            Transform knifeHitbox = childLocator.FindChild("KnifeHitbox");
            Prefabs.SetupHitbox(model, knifeHitbox, "Knife");

            Transform knifeHitbox2 = childLocator.FindChild("KnifeDetectionHitbox");
            Prefabs.SetupHitbox(model, knifeHitbox2, "KnifeDetection");
        }
    

        public override void InitializeSkills() {
            Skills.ClearGenericSkills(bodyPrefab);


            #region Primary
            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Primary);

            SkillDef primarySimpleGunSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_gun",
                ALIEM_PREFIX + "PRIMARY_GUN_NAME",
                ALIEM_PREFIX + "PRIMARY_GUN_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimaryGun"),
                new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunFireUncharged)),
                "Weapon",
                true));
            AddWeaponSkin(primarySimpleGunSkillDef, 1);

            SkillDef SecondaryGunSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "aliem_secondary_gun",
                skillNameToken = ALIEM_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemSecondaryGunBig"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunChargedFire)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });
            WeaponSecondaryController.skillPairs[primarySimpleGunSkillDef] = SecondaryGunSkillDef;
            Config.ConfigureSkillDef(SecondaryGunSkillDef, AliemConfig.sectionBody, "M2_RayGun_Charged");

            SkillDef primarySwordSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_sword",
                ALIEM_PREFIX + "PRIMARY_SWORD_NAME",
                ALIEM_PREFIX + "PRIMARY_SWORD_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimarySword"),
                new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.SwordFire)),
                "Weapon",
                true));
            AddWeaponSkin(primarySwordSkillDef, 2);

            SkillDef SecondarySwordSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "aliem_secondary_sword",
                skillNameToken = ALIEM_PREFIX + "SECONDARY_SWORD_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SECONDARY_SWORD_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemSecondarySwordBig"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SwordFireCharged)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });
            WeaponSecondaryController.skillPairs[primarySwordSkillDef] = SecondarySwordSkillDef;
            Config.ConfigureSkillDef(SecondarySwordSkillDef, AliemConfig.sectionBody, "M2_Sword_Charged");

            SkillDef primaryRifleSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_rifle",
                ALIEM_PREFIX + "PRIMARY_RIFLE_NAME",
                ALIEM_PREFIX + "PRIMARY_RIFLE_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimaryRifle"),
                new EntityStates.SerializableEntityStateType(typeof(ShootRifleUncharged)),
                "Weapon",
                true));
            AddWeaponSkin(primaryRifleSkillDef, 3);

            SkillDef SecondaryRifleSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "aliem_secondary_rifle",
                skillNameToken = ALIEM_PREFIX + "SECONDARY_RIFLE_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SECONDARY_RIFLE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemSecondaryRifleBig"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ShootRifleCharged)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });
            WeaponSecondaryController.skillPairs[primaryRifleSkillDef] = SecondaryRifleSkillDef;
            Config.ConfigureSkillDef(SecondaryRifleSkillDef, AliemConfig.sectionBody, "M2_Rifle_Charged");

            #region cursed
            SkillDef primaryInputsSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_gun_inputs",
                ALIEM_PREFIX + "PRIMARY_GUN_INPUTS_NAME",
                ALIEM_PREFIX + "PRIMARY_GUN_INPUTS_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimaryGun"),
                new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunInputs)),
                "Slide",
                true));
            WeaponSecondaryController.skillPairs[primarySimpleGunSkillDef] = SecondaryGunSkillDef;
            AddWeaponSkin(primaryInputsSkillDef, 1);

            SkillDef primaryInstantSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_gun_instant",
                ALIEM_PREFIX + "PRIMARY_GUN_INSTANT_NAME",
                ALIEM_PREFIX + "PRIMARY_GUN_INSTANT_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimaryGun"),
                new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunInstant)),
                "Weapon",
                true));
            primaryInstantSkillDef.mustKeyPress = true;
            WeaponSecondaryController.skillPairs[primaryInstantSkillDef] = SecondaryGunSkillDef;
            AddWeaponSkin(primaryInstantSkillDef, 1);

            SkillDef primaryInputsSwordSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_sword_inputs",
                ALIEM_PREFIX + "PRIMARY_SWORD_INPUTS_NAME",
                ALIEM_PREFIX + "PRIMARY_SWORD_INPUTS_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimarySword"),
                new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.SwordInputs)),
                "Slide",
                true));
            WeaponSecondaryController.skillPairs[primaryInputsSwordSkillDef] = SecondaryGunSkillDef;
            AddWeaponSkin(primaryInputsSwordSkillDef, 2);
            #endregion cursed

            Skills.AddPrimarySkills(bodyPrefab, primarySimpleGunSkillDef, primarySwordSkillDef, primaryRifleSkillDef);
            if (AliemConfig.Cursed.Value) {
                Skills.AddPrimarySkills(bodyPrefab, primaryInputsSkillDef, primaryInstantSkillDef, primaryInputsSwordSkillDef);
            }

            SkillDef lunarPrimaryReplacement = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/LunarSkillReplacements/LunarPrimaryReplacement.asset").WaitForCompletion();

            SkillDef SecondaryLunarSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "aliem_secondary_lunar",
                skillNameToken = ALIEM_PREFIX + "SECONDARY_LUNAR_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SECONDARY_LUNAR_DESCRIPTION",
                skillIcon = lunarPrimaryReplacement.icon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(ChargedLunarNeedleFire)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });

            WeaponSecondaryController.skillPairs[lunarPrimaryReplacement] = SecondaryLunarSkillDef;

            AddWeaponSkin(lunarPrimaryReplacement, 0);

            FinalizeCSSPreviewDisplayController();

            #endregion

            #region Secondary
            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Secondary);

            SkillDef SecondaryChargedSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_secondary_Charged",
                skillNameToken = ALIEM_PREFIX + "SECONDARY_CHARGED_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SECONDARY_CHARGED_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemSecondaryGunBig"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunChargedFire)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
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

            SkillDef SecondaryLeapSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_secondary_leap",
                skillNameToken = ALIEM_PREFIX + "UTILITY_LEAP_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "UTILITY_LEAP_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemDive"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.AliemLeapM2)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false, //handled by state itself
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

            Skills.AddSecondarySkills(bodyPrefab, SecondaryChargedSkillDef/*, SecondaryLeapSkillDef*/);
            #endregion

            #region Utility
            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Utility);

            SkillDef UtilityLeapSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_utility_leap",
                skillNameToken = ALIEM_PREFIX + "UTILITY_LEAP_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "UTILITY_LEAP_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemDive"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.AliemLeapM3)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false, //handled by state itself
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] {  }
            });

            Skills.AddUtilitySkills(bodyPrefab, UtilityLeapSkillDef);
            Config.ConfigureSkillDef(UtilityLeapSkillDef, AliemConfig.sectionBody, "M3_Dive");
            #endregion

            #region chomp

            GenericSkill chompSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "LOADOUT_SKILL_RIDING", "Riding");

            SkillDef ChompSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "aliem_utility_Chomp",
                skillNameToken = ALIEM_PREFIX + "UTILITY_CHOMP_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "UTILITY_CHOMP_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemChomp"),
                keywordTokens = new string[] { },

                activationState = new EntityStates.SerializableEntityStateType(typeof(AliemRidingChomp)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            Skills.AddSkillsToFamily(chompSkill.skillFamily, ChompSkillDef);

            #endregion

            #region Special
            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Special);

            SkillDef bombSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_special_grenade",
                skillNameToken = ALIEM_PREFIX + "SPECIAL_GRENADE_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SPECIAL_GRENADE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemSpecialGrenade"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.ThrowGrenade)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });
            
            Skills.AddSpecialSkills(bodyPrefab, bombSkillDef);
            Config.ConfigureSkillDef(bombSkillDef, AliemConfig.sectionBody, "M4_Grenade");
            #endregion

            if (Compat.ScepterInstalled) {
                InitializeScepterSkills();
            }
        }

        private void AddWeaponSkin(SkillDef skillDef, int skin)
        {
            bodyCharacterModel.GetComponent<WeaponSkinController>().AddWeaponSkin(skillDef, skin);

            SkillChangeResponse skillResponse = cssPreviewDisplayController.skillChangeResponses[skin];
            skillResponse.triggerSkillFamily = bodyPrefab.GetComponent<SkillLocator>().primary.skillFamily;
            skillResponse.triggerSkill = skillDef;

            HG.ArrayUtils.ArrayAppend(ref cssPreviewDisplayController.skillChangeResponses, skillResponse);
        }
        
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills() {

            SkillDef scepterBombSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_special_grenade_scepter",
                skillNameToken = ALIEM_PREFIX + "SPECIAL_GRENADE_SCEPTER_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SPECIAL_GRENADE_SCEPTER_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemSpecialGrenadeScepter"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.ScepterThrowGrenade)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });
            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterBombSkillDef, "AliemBody", SkillSlot.Special, 0);
            Config.ConfigureSkillDef(scepterBombSkillDef, AliemConfig.sectionBody, "M4_Grenade_Scepter");
        }
        public override void InitializeSkins() {

            ModelSkinController skinController = bodyCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = bodyCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = bodyCharacterModel.baseRendererInfos;
            
            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef("DEFAULT_SKIN",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconSkinDefault"),
                defaultRendererinfos,
                bodyCharacterModel.gameObject);
            
            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            /*
            //creating a new skindef as we did before
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(AliemPlugin.DEV_PREFIX + "_HENRY_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                defaultRendererinfos,
                model,
                masterySkinUnlockableDef);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "meshHenrySwordAlt",
                null,//no gun mesh replacement. use same gun mesh
                "meshHenryAlt");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            masterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            masterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            masterySkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GunModel"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);
            */
            #endregion
            
            skins.Add(CreateRecolorSkin(defaultSkin, "Red"));
            skins.Add(CreateRecolorSkin(defaultSkin, "Green"));
            skins.Add(CreateRecolorSkin(defaultSkin, "Blue"));
            skins.Add(CreateRecolorSkin(defaultSkin, "Orange"));
            skins.Add(CreateRecolorSkin(defaultSkin, "Brown"));
            skins.Add(CreateRecolorSkin(defaultSkin, "Cyan"));
            skins.Add(CreateRecolorSkin(defaultSkin, "Purple"));
            skins.Add(CreateRecolorSkin(defaultSkin, "Magenta"));
            skins.Add(CreateRecolorSkin(defaultSkin, "Black"));

            skinController.skins = skins.ToArray();
        }

        private SkinDef CreateRecolorSkin(SkinDef defaultSkin, string skinColor) {

            Material material = Materials.CreateHotpooMaterial($"matAliemRecolor_{skinColor}");
            Color color = material.GetColor("_Color");

            string token = $"{ALIEM_PREFIX}SKIN_{skinColor.ToUpperInvariant()}";

            SkinDef skinDef = Modules.Skins.CreateSkinDef(skinColor,
                                                  CreateRecolorIcon(color),
                                                  defaultSkin.rendererInfos,
                                                  defaultSkin.rootObject,
                                                  masterySkinUnlockableDef);
            skinDef.rendererInfos[0].defaultMaterial = material;

            R2API.LanguageAPI.Add(token, skinColor);
            return skinDef;
        }


        internal static Sprite CreateRecolorIcon(Color color) {
            var tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);

            var fillColorArray = tex.GetPixels();
            for (int i = 0; i < fillColorArray.Length; i++) {
                fillColorArray[i] = color;
            }
            tex.SetPixels(fillColorArray);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f));
        }
    }
}