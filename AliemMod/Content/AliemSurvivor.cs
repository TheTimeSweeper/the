using AliemMod.Components;
using AliemMod.Components.Bundled;
using AliemMod.Content.SkillDefs;
using AliemMod.Modules;
using AliemMod.Modules.Characters;
using BepInEx.Configuration;
using EntityStates;
using KinematicCharacterController;
using ModdedEntityStates.Aliem;
using Modules.Survivors;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Orbs;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static RoR2.CharacterSelectSurvivorPreviewDisplayController;

namespace AliemMod.Content.Survivors
{

    public class AliemSurvivor : SurvivorBase {
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
                    childName = "MeshBlasterR",
                },
                new CustomRendererInfo
                {
                    childName = "MeshWeapon2R",
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
        // If you ran out beard shampoo buy more.
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        public static AliemSurvivor instance;
        private CharacterSelectSurvivorPreviewDisplayController cssPreviewDisplayController;

        public static SkillDef ChompSkillDef;
        public static SkillDef GrenadeSkillDef;

        public override void InitializeCharacter() {
            base.InitializeCharacter();

            instance = this;

            Hooks();

            //VehicleSeat vehicleSeat = bodyPrefab.AddComponent<VehicleSeat>();
            //vehicleSeat.passengerState = new EntityStates.SerializableEntityStateType(typeof(AliemRidingState));
            //vehicleSeat.hidePassenger = false;
            //vehicleSeat.disablePassengerMotor = true;
            //vehicleSeat.isEquipmentActivationAllowed = true;
        }
        public override void InitializeAI()
        {
            AliemAI.Init(bodyPrefab, "AliemMonsterMaster");
        }

        protected override void InitializeCharacterBodyAndModel() {

            if (AliemConfig.GupDefault.Value) {

                bodyInfo.characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("texIconAliemGup");
            }
            base.InitializeCharacterBodyAndModel();

            Modules.Config.ConfigureBody(bodyPrefab.GetComponent<CharacterBody>(), AliemConfig.sectionBody);

            CreateBurrowEffect();

            FixMotorCollider();

            cssPreviewDisplayController = displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();

            bodyCharacterModel.GetComponent<ChildLocator>().FindChild("FakeAimOrigin").transform.position = bodyPrefab.GetComponent<CharacterBody>().aimOriginTransform.position;

            bodyPrefab.AddComponent<AliemRidingColliderHolderThatsIt>();
            bodyPrefab.AddComponent<SmallHopController>();

            EntityStateMachine.FindByCustomName(bodyPrefab, "Slide").customName = "Weapon2";
            Modules.Prefabs.AddEntityStateMachine(bodyPrefab, "Inputs1");
            Modules.Prefabs.AddEntityStateMachine(bodyPrefab, "Inputs2");
            Modules.Prefabs.AddEntityStateMachine(bodyPrefab, "Mutation");
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

            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;

            IL.RoR2.Orbs.OrbEffect.Start += OrbEffect_Start;
        }

        private void OrbEffect_Start(MonoMod.Cil.ILContext il)
        {
            ILCursor cursor = new ILCursor(il);

            cursor.GotoNext(MoveType.After,
                instruction => instruction.MatchBrtrue(out _),
                instruction => instruction.MatchLdarg(0),
                instruction => instruction.MatchLdfld<OrbEffect>(nameof(OrbEffect.startPosition))
                );

            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, typeof(OrbEffect).GetFieldCached(nameof(OrbEffect.targetTransform)));
            cursor.Emit(OpCodes.Ldloc_0);
            cursor.EmitDelegate<Func<Vector3, Transform, EffectComponent, Vector3>>((startPosition, targetTransform, effectComponent) =>
            {
                if(targetTransform == null)
                {
                    startPosition = effectComponent.effectData.start;
                }
                return startPosition;
            });
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

            if (sender.HasBuff(Buffs.riddenBuff))
            {
                args.moveSpeedMultAdd += 1.3f;
                //args.attackSpeedMultAdd += 1.2f;
            }

            if (sender.HasBuff(Buffs.diveBuff))
            {
                args.armorAdd += AliemConfig.M3_Leap_Armor.Value;
            }

            if (sender.HasBuff(Buffs.ridingBuff))
            {
                args.armorAdd += AliemConfig.M3_Leap_RidingArmor.Value;
            }

            if (sender.HasBuff(Buffs.attackSpeedBuff))
            {
                args.attackSpeedMultAdd += 1;
            }
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
            AliemUnlockables.Init();
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

            SkillLocator skillLocator = bodyPrefab.GetComponent<SkillLocator>();

            #region Primary
            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Primary);

            SkillDef primaryGunInputsSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_gun_inputs",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_GUN_INPUTS_NAME",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_GUN_INPUTS_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimaryGun"),
                new EntityStates.SerializableEntityStateType(typeof(InputRayGun)),
                "Inputs1",
                true));
            AddWeaponSkin(primaryGunInputsSkillDef, 1);

            SkillDef primarySwordInputsSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_sword_inputs",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_SWORD_INPUTS_NAME",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_SWORD_INPUTS_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimarySword"),
                new EntityStates.SerializableEntityStateType(typeof(InputSword)),
                "Inputs1",
                true));
            AddWeaponSkin(primarySwordInputsSkillDef, 2);

            SkillDef primaryRifleInputsSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_rifle_inputs",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_RIFLE_INPUTS_NAME",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_RIFLE_INPUTS_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimaryRifle"),
                new EntityStates.SerializableEntityStateType(typeof(InputRifle)),
                "Inputs1",
                true));
            if (AliemConfig.M1_MachineGun_Falloff.Value)
            {
                primaryRifleInputsSkillDef.keywordTokens = new string[] { ALIEM_PREFIX + "KEYWORD_FALLOFF" };
            }
            AddWeaponSkin(primaryRifleInputsSkillDef, 3);

            SkillDef primarySawedOffInputsSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_sawedOff_inputs",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_SAWEDOFF_INPUTS_NAME",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_SAWEDOFF_INPUTS_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimarySawedOff"),
                new EntityStates.SerializableEntityStateType(typeof(InputSawedOff)),
                "Inputs1",
                true));
            AddWeaponSkin(primarySawedOffInputsSkillDef, 4);

            SkillDef primaryBBGunInputsSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                "aliem_primary_bbgun_inputs",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_BBGUN_INPUTS_NAME",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_BBGUN_INPUTS_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimaryBBGun"),
                new EntityStates.SerializableEntityStateType(typeof(InputBBGun)),
                "Inputs1",
                true));
            AddWeaponSkin(primaryBBGunInputsSkillDef, 5);

            #region cursed

            PassiveBuildupComponentSkillDef primaryInstantSkillDef = Skills.CreateSkillDef<PassiveBuildupComponentSkillDef>(new SkillDefInfo(
                "aliem_primary_gun_instant",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_GUN_INSTANT_NAME",
                AliemSurvivor.ALIEM_PREFIX + "PRIMARY_GUN_INSTANT_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimaryGun"),
                new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunInstant)),
                "Weapon",
                true));
            primaryInstantSkillDef.mustKeyPress = true;
            AddWeaponSkin(primaryInstantSkillDef, 1);
            #endregion cursed

            Skills.AddPrimarySkills(bodyPrefab, primaryGunInputsSkillDef, primarySwordInputsSkillDef, primaryRifleInputsSkillDef, primarySawedOffInputsSkillDef, primaryBBGunInputsSkillDef);
            if (AliemConfig.Cursed.Value) {
                Skills.AddPrimarySkills(bodyPrefab, primaryInstantSkillDef);
            }

            SkillDef lunarPrimaryReplacement = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/LunarSkillReplacements/LunarPrimaryReplacement.asset").WaitForCompletion();
            AddWeaponSkin(lunarPrimaryReplacement, 0);
            
            FinalizeCSSPreviewDisplayController();

            #endregion

            #region Secondary
            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Secondary);

            WeaponSecondaryComponentSkillDef SecondaryChargedSkillDef = Skills.CreateSkillDef<WeaponSecondaryComponentSkillDef>(new SkillDefInfo
            {
                skillName = "aliem_secondary_Charged",
                skillNameToken = AliemSurvivor.ALIEM_PREFIX + "SECONDARY_CHARGED_NAME",
                skillDescriptionToken = AliemSurvivor.ALIEM_PREFIX + "SECONDARY_CHARGED_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemSecondaryGunBig"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunChargedFire)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
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
            Config.ConfigureSkillDef(SecondaryChargedSkillDef, AliemConfig.sectionBody, "M2_ChargedShot");

            SkillDef SecondaryGunSkillDef = Skills.CloneSkillDef<SkillDef>(
                SecondaryChargedSkillDef,
                "secondary_gun",
                ALIEM_PREFIX,
                "texIconAliemSecondaryGunBig",
                new EntityStates.SerializableEntityStateType(typeof(RayGunChargedFire)));
            WeaponChargedSecondaryController.skillPairs[primaryGunInputsSkillDef] = SecondaryGunSkillDef;
            WeaponChargedSecondaryController.skillPairs[primaryInstantSkillDef] = SecondaryGunSkillDef;

            SkillDef SecondarySwordSkillDef = Skills.CloneSkillDef<SkillDef>(
                SecondaryChargedSkillDef,
                "secondary_sword",
                ALIEM_PREFIX,
                "texIconAliemSecondarySwordBig",
                new EntityStates.SerializableEntityStateType(typeof(SwordFireCharged)));
            WeaponChargedSecondaryController.skillPairs[primarySwordInputsSkillDef] = SecondarySwordSkillDef;

            SkillDef SecondaryRifleSkillDef = Skills.CloneSkillDef<SkillDef>(
                SecondaryChargedSkillDef,
                "secondary_rifle",
                ALIEM_PREFIX,
                "texIconAliemSecondaryRifleBig",
                new EntityStates.SerializableEntityStateType(typeof(ShootRifleCharged)));
            WeaponChargedSecondaryController.skillPairs[primaryRifleInputsSkillDef] = SecondaryRifleSkillDef;

            SkillDef SecondarySawedOffSkillDef = Skills.CloneSkillDef<SkillDef>(
                SecondaryChargedSkillDef,
                "secondary_sawedoff",
                ALIEM_PREFIX,
                "texIconAliemSecondarySawedOffBig",
                new EntityStates.SerializableEntityStateType(typeof(ShootSawedOffCharged)));
            WeaponChargedSecondaryController.skillPairs[primarySawedOffInputsSkillDef] = SecondarySawedOffSkillDef;

            SkillDef SecondaryBBGunSkillDef = Skills.CloneSkillDef<SkillDef>(
                SecondaryChargedSkillDef,
                "secondary_bbgun",
                ALIEM_PREFIX,
                "texIconAliemSecondaryBBGunBig",
                new EntityStates.SerializableEntityStateType(typeof(FireBBGunCharged)));
            WeaponChargedSecondaryController.skillPairs[primaryBBGunInputsSkillDef] = SecondaryBBGunSkillDef;

            SkillDef SecondaryLunarSkillDef = Skills.CloneSkillDef<SkillDef>(
                SecondaryChargedSkillDef,
                "secondary_lunar",
                ALIEM_PREFIX,
                lunarPrimaryReplacement.icon,
                new EntityStates.SerializableEntityStateType(typeof(ChargedLunarNeedleFire)));
            WeaponChargedSecondaryController.skillPairs[lunarPrimaryReplacement] = SecondaryLunarSkillDef;

            SkillDef SecondaryLeapSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "aliem_secondary_leap",
                skillNameToken = AliemSurvivor.ALIEM_PREFIX + "UTILITY_LEAP_NAME",
                skillDescriptionToken = AliemSurvivor.ALIEM_PREFIX + "UTILITY_LEAP_DESCRIPTION",
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

            SkillDef UtilityLeapSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "aliem_utility_leap",
                skillNameToken = AliemSurvivor.ALIEM_PREFIX + "UTILITY_LEAP_NAME",
                skillDescriptionToken = AliemSurvivor.ALIEM_PREFIX + "UTILITY_LEAP_DESCRIPTION",
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

            GenericSkill chompGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "LOADOUT_SKILL_RIDING", "Riding");

            SkillDef chompSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "aliem_utility_Chomp",
                skillNameToken = AliemSurvivor.ALIEM_PREFIX + "UTILITY_CHOMP_NAME",
                skillDescriptionToken = AliemSurvivor.ALIEM_PREFIX + "UTILITY_CHOMP_DESCRIPTION",
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
            ChompSkillDef = chompSkillDef;

            Skills.AddSkillsToFamily(chompGenericSkill.skillFamily, chompSkillDef);

            #endregion

            #region Special
            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Special);
            
            SkillDef bombSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "aliem_special_grenade",
                skillNameToken = AliemSurvivor.ALIEM_PREFIX + "SPECIAL_GRENADE_NAME",
                skillDescriptionToken = AliemSurvivor.ALIEM_PREFIX + "SPECIAL_GRENADE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemSpecialGrenade"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.ThrowGrenade)),
                activationStateMachineName = "Mutation",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
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
                stockToConsume = 1
            });
            Config.ConfigureSkillDef(bombSkillDef, AliemConfig.sectionBody, "M4_Grenade");
            GrenadeSkillDef = bombSkillDef;

            WeaponSwapSkillDef weaponSwapSkillDefBase = Skills.CreateSkillDef<WeaponSwapSkillDef>(new SkillDefInfo
            {
                skillName = "aliem_special_weaponSwap",
                skillNameToken = primaryGunInputsSkillDef.skillNameToken,
                skillDescriptionToken = ALIEM_PREFIX + "SPECIAL_WEAPONSWAP_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texIconAliemPrimaryGun"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SwapSecondaryWeapon)),
                activationStateMachineName = "Mutation",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 6f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });
            weaponSwapSkillDefBase.swapSkillDef = CloneSwapSkillDef(primaryGunInputsSkillDef);
            Config.ConfigureSkillDef(weaponSwapSkillDefBase, AliemConfig.sectionBody, "M4_WeaponSwaps");
            AddWeaponSkinSecondary(weaponSwapSkillDefBase.swapSkillDef, 1);

            WeaponSwapSkillDef weaponSwapSkillDefSword = CreateWeeponSwapSkillDef(weaponSwapSkillDefBase, primarySwordInputsSkillDef);
            AddWeaponSkinSecondary(weaponSwapSkillDefSword.swapSkillDef, 2);

            WeaponSwapSkillDef weaponSwapSkillDefRifle = CreateWeeponSwapSkillDef(weaponSwapSkillDefBase, primaryRifleInputsSkillDef);
            AddWeaponSkinSecondary(weaponSwapSkillDefRifle.swapSkillDef, 3);

            WeaponSwapSkillDef weaponSwapSkillDefSawedOff = CreateWeeponSwapSkillDef(weaponSwapSkillDefBase, primarySawedOffInputsSkillDef);
            AddWeaponSkinSecondary(weaponSwapSkillDefSawedOff.swapSkillDef, 4);

            WeaponSwapSkillDef weaponSwapSkillDefBBGun = CreateWeeponSwapSkillDef(weaponSwapSkillDefBase, primaryBBGunInputsSkillDef);
            AddWeaponSkinSecondary(weaponSwapSkillDefBBGun.swapSkillDef, 5);

            Skills.AddSpecialSkills(bodyPrefab, bombSkillDef, weaponSwapSkillDefBase, weaponSwapSkillDefSword, weaponSwapSkillDefRifle, weaponSwapSkillDefSawedOff, weaponSwapSkillDefBBGun);
            #endregion
            
            if (Compat.ScepterInstalled) {
                InitializeScepterSkills();
                CreateScepterWeaponSwap(weaponSwapSkillDefBase, "texIconAliemPrimaryGunScepter");
                CreateScepterWeaponSwap(weaponSwapSkillDefSword, "texIconAliemPrimarySwordScepter");
                CreateScepterWeaponSwap(weaponSwapSkillDefRifle, "texIconAliemPrimaryRifleScepter");
                CreateScepterWeaponSwap(weaponSwapSkillDefSawedOff, "texIconAliemPrimarySawedOffScepter");
                CreateScepterWeaponSwap(weaponSwapSkillDefBBGun, "texIconAliemPrimaryBBGunScepter");
            }


            Skills.AddUnlockablesToFamily(skillLocator.primary.skillFamily, null, AliemUnlockables.ChompEnemiesUnlockableDef, AliemUnlockables.BurrowPopOutUnlockableDef, AliemUnlockables.ChargedKillUnlockableDef);
            Skills.AddUnlockablesToFamily(skillLocator.special.skillFamily, null, AliemUnlockables.SlowMashUnlockableDef, AliemUnlockables.ChompEnemiesUnlockableDef, AliemUnlockables.BurrowPopOutUnlockableDef, AliemUnlockables.ChargedKillUnlockableDef);
        }

        private OffHandSkillDef CloneSwapSkillDef(SkillDef primarySkillDef)
        {
            OffHandSkillDef newSkillDef = Modules.Skills.CloneSkillDef<OffHandSkillDef>(primarySkillDef);
            (newSkillDef as ScriptableObject).name += "Secondary";
            newSkillDef.skillName += "Secondary";
            newSkillDef.activationStateMachineName = "Inputs2";
            return newSkillDef;
        }

        private WeaponSwapSkillDef CreateWeeponSwapSkillDef(SkillDef originalSwapSkillDef, SkillDef primarySkillDef)
        {
            WeaponSwapSkillDef newSkillDef = Modules.Skills.CloneSkillDef<WeaponSwapSkillDef>(originalSwapSkillDef);
            (newSkillDef as ScriptableObject).name += primarySkillDef.skillName;
            newSkillDef.skillName += primarySkillDef.skillName;
            newSkillDef.skillNameToken = primarySkillDef.skillNameToken;
            newSkillDef.icon = primarySkillDef.icon;
            newSkillDef.swapSkillDef = CloneSwapSkillDef(primarySkillDef);

            return newSkillDef;
        }

        private void CreateScepterWeaponSwap(WeaponSwapSkillDef originalSkillDef, string iconPath)
        {
            WeaponSwapSkillDef scepterSwapSkillDef = Modules.Skills.CloneSkillDef<WeaponSwapSkillDef>(originalSkillDef);
            (scepterSwapSkillDef as ScriptableObject).name += "scepter";
            scepterSwapSkillDef.skillName += "scepter";
            scepterSwapSkillDef.skillDescriptionToken = ALIEM_PREFIX + "SPECIAL_WEAPONSWAP_SCEPTER_DESCRIPTION";
            scepterSwapSkillDef.icon = Assets.LoadAsset<Sprite>(iconPath);
            scepterSwapSkillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(SwapSecondaryWeaponScepter));
            scepterSwapSkillDef.swapSkillDef = originalSkillDef.swapSkillDef;

            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterSwapSkillDef, "AliemBody", originalSkillDef);
        }

        private void AddWeaponSkin(SkillDef skillDef, int skin)
        {
            bodyCharacterModel.GetComponent<WeaponSkinController>().AddWeaponSkin(skillDef, skin);

            SkillChangeResponse skillResponse = cssPreviewDisplayController.skillChangeResponses[skin];
            skillResponse.triggerSkillFamily = bodyPrefab.GetComponent<SkillLocator>().primary.skillFamily;
            skillResponse.triggerSkill = skillDef;

            HG.ArrayUtils.ArrayAppend(ref cssPreviewDisplayController.skillChangeResponses, skillResponse);
        }

        private void AddWeaponSkinSecondary(SkillDef skillDef, int skin)
        {
            bodyCharacterModel.GetComponent<WeaponSkinController>().AddWeaponSkinSecondary(skillDef, skin);
        }
        
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills() {

            SkillDef scepterBombSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_special_grenade_scepter",
                skillNameToken = ALIEM_PREFIX + "SPECIAL_GRENADE_SCEPTER_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SPECIAL_GRENADE_SCEPTER_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemSpecialGrenadeScepter"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.ThrowGrenadeScepter)),
                activationStateMachineName = "Mutation",
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
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(
                "DEFAULT_SKIN",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconSkinDefault"),
                defaultRendererinfos,
                bodyCharacterModel.gameObject);
            
            defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "meshAliembody",
                "meshAliemWeapon_Blaster",
                "meshAliemWeapon_Blaster.R",
                "meshAliemWeapon_SecondMesh",
                "meshAliemWeapon_SecondMesh.R",
                "meshAliemKnife");

            #endregion

            #region MasterySkin
            
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(ALIEM_PREFIX + "GUP_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconSkinGup"),
                defaultRendererinfos,
                bodyCharacterModel.gameObject);
            
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "meshGupbody",
                null,
                null,
                null,
                null,
                null);

            Material gipMat = Addressables.LoadAssetAsync<Material>("RoR2/DLC1/Gup/matGipBody.mat").WaitForCompletion();
            masterySkin.rendererInfos[0].defaultMaterial = gipMat;
            masterySkin.rendererInfos[5].defaultMaterial = gipMat;

            if (!AliemConfig.GupDefault.Value)
            {
                skins.Add(defaultSkin);
                skins.Add(masterySkin);
            } 
            else
            {
                skins.Add(masterySkin);
                skins.Add(defaultSkin);
            }
            skins[1].unlockableDef = AliemUnlockables.masterySkinUnlockableDef;

            #endregion
            if (AliemConfig.Cursed.Value)
            {
                skins.Add(CreateRecolorSkin(defaultSkin, "Red"));
                skins.Add(CreateRecolorSkin(defaultSkin, "Green"));
                skins.Add(CreateRecolorSkin(defaultSkin, "Blue"));
                skins.Add(CreateRecolorSkin(defaultSkin, "Orange"));
                skins.Add(CreateRecolorSkin(defaultSkin, "Brown"));
                skins.Add(CreateRecolorSkin(defaultSkin, "Cyan"));
                skins.Add(CreateRecolorSkin(defaultSkin, "Purple"));
                skins.Add(CreateRecolorSkin(defaultSkin, "Magenta"));
                skins.Add(CreateRecolorSkin(defaultSkin, "Black"));
            }

            skinController.skins = skins.ToArray();
        }

        public SkinDef CreateRecolorSkin(SkinDef defaultSkin, string skinColor) {

            Material material = Materials.CreateHotpooMaterial($"matAliemRecolor_{skinColor}");
            Color color = material.GetColor("_Color");

            string token = $"{ALIEM_PREFIX}SKIN_{skinColor.ToUpperInvariant()}";

            SkinDef skinDef = Modules.Skins.CreateSkinDef(skinColor,
                                                  CreateRecolorIcon(color),
                                                  defaultSkin.rendererInfos,
                                                  defaultSkin.rootObject,
                                                  AliemUnlockables.masterySkinUnlockableDef);
            skinDef.meshReplacements = defaultSkin.meshReplacements;
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