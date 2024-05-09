using AliemMod.Components;
using BepInEx.Configuration;
using KinematicCharacterController;
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

            jumpCount = 1,

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
                    childName = "MeshBlaster",
                },
                new CustomRendererInfo
                {
                    childName = "MeshBody",
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

        public override void InitializeCharacter() {
            base.InitializeCharacter();

            instance = this;

            Hooks();

            bodyPrefab.AddComponent<RayGunChargeComponent>();
        }

        protected override void InitializeCharacterBodyAndModel() {
            base.InitializeCharacterBodyAndModel();

            CreateBurrowEffect();

            FixMotorCollider();

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
        }
    

        public override void InitializeSkills() {
            Skills.CreateSkillFamilies(bodyPrefab);

            #region Primary
            SkillDef primarySimpleGunSkillDef = Skills.CreateSkillDef(new SkillDefInfo("aliem_primary_gun",
                                                                              ALIEM_PREFIX + "PRIMARY_GUN_NAME",
                                                                              ALIEM_PREFIX + "PRIMARY_GUN_DESCRIPTION",
                                                                              Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPrimary"),
                                                                              new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunFireUncharged)),
                                                                              "Weapon",
                                                                              true));

            SkillDef primaryInputsSkillDef = Skills.CreateSkillDef(new SkillDefInfo("aliem_primary_gun_inputs",
                                                                              ALIEM_PREFIX + "PRIMARY_GUN_INPUTS_NAME",
                                                                              ALIEM_PREFIX + "PRIMARY_GUN_INPUTS_DESCRIPTION",
                                                                              Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPrimary"),
                                                                              new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunInputs)),
                                                                              "Slide",
                                                                              true));

            SkillDef primaryInstantSkillDef= Skills.CreateSkillDef(new SkillDefInfo("aliem_primary_gun_instant",
                                                                              ALIEM_PREFIX + "PRIMARY_GUN_INSTANT_NAME",
                                                                              ALIEM_PREFIX + "PRIMARY_GUN_INSTANT_DESCRIPTION",
                                                                              Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPrimary"),
                                                                              new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.RayGunInstant)),
                                                                              "Weapon",
                                                                              true));

            SkillDef primaryInputsSwordSkillDef = Skills.CreateSkillDef(new SkillDefInfo("aliem_primary_sword_inputs",
                                                                              ALIEM_PREFIX + "PRIMARY_SWORD_INPUTS_NAME",
                                                                              ALIEM_PREFIX + "PRIMARY_SWORD_INPUTS_DESCRIPTION",
                                                                              Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPrimary"),
                                                                              new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.SwordInputs)),
                                                                              "Slide",
                                                                              true));
            primaryInstantSkillDef.mustKeyPress = true;

            Skills.AddPrimarySkills(bodyPrefab, primarySimpleGunSkillDef);
            if (AliemConfig.Cursed.Value) {
                Skills.AddPrimarySkills(bodyPrefab, primaryInputsSkillDef, primaryInstantSkillDef, primaryInputsSwordSkillDef);
            }
            #endregion

            #region Secondary
            SkillDef SecondaryGunSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_secondary_gun",
                skillNameToken = ALIEM_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconSecondary"),
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
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.AliemLeapM2)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
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

            Skills.AddSecondarySkills(bodyPrefab, SecondaryGunSkillDef/*, SecondaryLeapSkillDef*/);
            #endregion

            #region Utility

            SkillDef UtilityLeapSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_utility_leap",
                skillNameToken = ALIEM_PREFIX + "UTILITY_LEAP_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "UTILITY_LEAP_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ModdedEntityStates.Aliem.AliemLeapM3)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
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
            #endregion

            #region Special
            SkillDef bombSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_special_grenade",
                skillNameToken = ALIEM_PREFIX + "SPECIAL_GRENADE_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SPECIAL_GRENADE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconSpecial"),
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
            #endregion

            if (Compat.ScepterInstalled) {
                InitializeScepterSkills();
            }
        }
        
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitializeScepterSkills() {

            SkillDef scepterBombSkillDef = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "aliem_special_grenade_scepter",
                skillNameToken = ALIEM_PREFIX + "SPECIAL_GRENADE_SCEPTER_NAME",
                skillDescriptionToken = ALIEM_PREFIX + "SPECIAL_GRENADE_SCEPTER_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconSpecialScepter"),
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
        }
        public override void InitializeSkins() {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = characterModel.baseRendererInfos;
            
            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef("DEFAULT_SKIN",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texIconSkinDefault"),
                defaultRendererinfos,
                model);
            
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
            skinDef.rendererInfos[1].defaultMaterial = material;

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