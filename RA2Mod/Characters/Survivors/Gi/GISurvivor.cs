using BepInEx.Configuration;
using EntityStates;
using EntityStates.Engi.Mine;
using RA2Mod.General.Components;
using RA2Mod.Modules;
using RA2Mod.Modules.Characters;
using RA2Mod.Survivors.GI.Components;
using RA2Mod.Survivors.GI.SkillDefs;
using RA2Mod.Survivors.GI.SkillStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.Survivors.GI
{
    public class GISurvivor : SurvivorBase<GISurvivor>
    {
        public override string assetBundleName => "joeRA2";

        public override string bodyName => "RA2GIBody";
        
        public override string masterName => "GIMonsterMaster";

        public override string modelPrefabName => "mdlJoe";
        public override string displayPrefabName => "JoeDisplay";

        public const string GI_PREFIX = RA2Plugin.DEVELOPER_PREFIX + "_GI_";

        public override string survivorTokenPrefix => GI_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = GI_PREFIX + "NAME",
            subtitleNameToken = GI_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texIconGI"),
            bodyColor = Color.blue,
            sortPosition = 69.4f,
            
            crosshairBundlePath = "GICrosshair",
            podPrefabAddressablePath = "RoR2/Base/SurvivorPod/SurvivorPod.prefab",

            maxHealth = 140f,
            healthRegen = 2.0f,
            armor = 10f,

            jumpCount = 1,
        };

        public override UnlockableDef characterUnlockableDef => GIUnlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays { get; } = new RA2Mod.General.JoeItemDisplays();

        public override void Initialize()
        {
            if (!General.GeneralConfig.GIEnabled.Value)
                return;

            base.Initialize();
        }

        public override void OnCharacterInitialized()
        {
            Config.ConfigureBody(prefabCharacterBody, GIConfig.SectionBody);

            //GIUnlockables.Init();
            GIConfig.Init();

            GIStates.Init();
            GITokens.Init();

            GIAssets.Init(assetBundle);
            GIBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        protected override void InitializeSurvivor()
        {
            base.InitializeSurvivor();

            VoiceLineInLobby voiceLineController = displayPrefab.AddComponent<VoiceLineInLobby>();
            voiceLineController.voiceLineContext = new VoiceLineContext("GI", 6, 6, 6);

            displayPrefab.AddComponent<MenuSoundComponent>().sound = "Play_GIBarricade";
        }

        private void AdditionalBodySetup()
        {
            //AddHitboxes();
            bodyPrefab.AddComponent<GIMissileTracker>();
            VoiceLineController voiceLineController = bodyPrefab.AddComponent<VoiceLineController>();
            voiceLineController.voiceLineContext = new VoiceLineContext("GI", 6, 6, 6);
            //bodyPrefab.AddComponent<HuntressTrackerComopnent>();
            //anything else here
        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //if you set up a custom main characterstate, set it up here
                //don't forget to register custom entitystates in your HenryStates.cs
            //the main "body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Transform", null, null, false, false);
        }

        #region skills
        public override void InitializeSkills()
        {
            Skills.ClearGenericSkills(bodyPrefab);

            Skills.CreateSkillFamilies(bodyPrefab);
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();
        }

        private void AddPrimarySkills()
        {
            UpgradableSkillDef primarySkillDef1 = Skills.CreateSkillDef<UpgradableSkillDef>(new SkillDefInfo
                (
                    "GIGun1",
                    GI_PREFIX + "PRIMARY_GUN_NAME",
                    GI_PREFIX + "PRIMARY_GUN_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(Fire3RoundPistol)),
                    "Weapon",
                    false
                ));
            primarySkillDef1.crosshairOverride = assetBundle.LoadAsset<GameObject>("GICrosshairHMG");
            primarySkillDef1.enterBarricadeState = new SerializableEntityStateType(typeof(EnterBarricade));
            primarySkillDef1.upgradedSkillDef = Skills.CreateSkillDef<GILimitedUseSkillDef>(new SkillDefInfo
            {
                skillName = "GIGun1Heavy",
                skillNameToken = GI_PREFIX + "PRIMARY_GUN_HEAVY_NAME",
                skillDescriptionToken = GI_PREFIX + "PRIMARY_GUN_HEAVY_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(FireGunHeavy)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Any,

                baseRechargeInterval = 0f,
                baseMaxStock = 21,

                rechargeStock = 21,
                requiredStock = 3,
                stockToConsume = 3,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            UpgradableSkillDef primarySkillDef2 = Skills.CreateSkillDef<UpgradableSkillDef>(new SkillDefInfo
                (
                    "GIRocket",
                    GI_PREFIX + "PRIMARY_ROCKET_NAME",
                    GI_PREFIX + "PRIMARY_ROCKET_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(FireMissile)),
                    "Weapon",
                    false
                ));
            primarySkillDef2.crosshairOverride = assetBundle.LoadAsset<GameObject>("GICrosshairMissile");
            primarySkillDef2.enterBarricadeState = new SerializableEntityStateType(typeof(EnterBarricadeMissile));
            primarySkillDef2.upgradedSkillDef = Skills.CreateSkillDef<GIMissileTrackerLimitedUseSkillDef>(new SkillDefInfo
            {
                skillName = "GIRocketHeavy",
                skillNameToken = GI_PREFIX + "PRIMARY_ROCKET_HEAVY_NAME",
                skillDescriptionToken = GI_PREFIX + "PRIMARY_ROCKET_HEAVY_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(FireMissileHeavy)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Any,
                
                baseRechargeInterval = 0f,
                baseMaxStock = 12,

                rechargeStock = 12,
                requiredStock = 2,
                stockToConsume = 2,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1, primarySkillDef2);

            //nvm just doing crosshair overrides instead
            //ShowOnCrosshairWhenSkillDef showOnCrosshairWhenSkillDef = prefabCharacterBody.defaultCrosshairPrefab.GetComponent<ShowOnCrosshairWhenSkillDef>();
            //showOnCrosshairWhenSkillDef.IncludedSkillDefs.Add(primarySkillDef1.upgradedSkillDef);
            //showOnCrosshairWhenSkillDef.IncludedSkillDefs.Add(primarySkillDef2.upgradedSkillDef);

            //Config.ConfigureSkillDef(primarySkillDef1, GIConfig.GISectionStocks, "M1 Pistol", false);
            Config.ConfigureSkillDef(primarySkillDef1.upgradedSkillDef, GIConfig.SectionBody, "M1 Heavy Fire", false, true, true);
            //Config.ConfigureSkillDef(primarySkillDef2, GIConfig.GISectionStocks, "M1 Missile", false);
            Config.ConfigureSkillDef(primarySkillDef2.upgradedSkillDef, GIConfig.SectionBody, "M1 Heavy Missile", false, true, true);
        }

        private void AddSecondarySkills()
        {
            //here is a basic skill def with all fields accounted for
            UpgradableSkillDef secondarySkillDef1 = Skills.CreateSkillDef<UpgradableSkillDef>(new SkillDefInfo
            {
                skillName = "GICaltrops",
                skillNameToken = GI_PREFIX + "SECONDARY_CALTROPS_NAME",
                skillDescriptionToken = GI_PREFIX + "SECONDARY_CALTROPS_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_STUNNING" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                
                activationState = new EntityStates.SerializableEntityStateType(typeof(ThrowCaltrops)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,

            });
            secondarySkillDef1.upgradedSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GIMine",
                skillNameToken = GI_PREFIX + "SECONDARY_MINE_NAME",
                skillDescriptionToken = GI_PREFIX + "SECONDARY_MINE_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(ThrowMine)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });


            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);

            Config.ConfigureSkillDef(secondarySkillDef1, GIConfig.SectionBody, "M2 Caltrops");
            Config.ConfigureSkillDef(secondarySkillDef1.upgradedSkillDef, GIConfig.SectionBody, "M2 Mine");
        }

        private void AddUtiitySkills()
        {
            //here's a skilldef of a typical movement skill.
            SkillDefInfo skillDefInfo = new SkillDefInfo
            {
                skillName = "GICommandoSlide",
                skillNameToken = GI_PREFIX + "UTILITY_SLIDE_NAME",
                skillDescriptionToken = GI_PREFIX + "UTILITY_SLIDE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(LiterallyCommandoSlide)),
                activationStateMachineName = "Transform",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 6f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            };
            var utilitySkillDef1 = Skills.CreateSkillDef<UpgradableSkillDef>(skillDefInfo);
            skillDefInfo.skillName = "GICommandoSlideHeavy";
            //skillDefInfo.interruptPriority = EntityStates.InterruptPriority.Death;
            skillDefInfo.stockToConsume = 0;
            skillDefInfo.fullRestockOnAssign = true;
            utilitySkillDef1.upgradedSkillDef = Skills.CreateSkillDef(skillDefInfo);

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);

            Config.ConfigureSkillDef(utilitySkillDef1, GIConfig.SectionBody, "M3 slide");
            //Config.ConfigureSkillDef(utilitySkillDef1.upgradedSkillDef, GIConfig.GISectionStocks, "M3 slide out of transform");
        }

        private void AddSpecialSkills()
        {
            UpgradableSkillDef specialSkillDef1 = Skills.CreateSkillDef<UpgradableSkillDef>(new SkillDefInfo
            {
                skillName = "GITransform",
                skillNameToken = GI_PREFIX + "SPECIAL_DEPLOY_NAME",
                skillDescriptionToken = GI_PREFIX + "SPECIAL_DEPLOY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.BarricadeTransform)),
                activationStateMachineName = "Transform",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                 
                baseMaxStock = 1,
                baseRechargeInterval = 8f,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                isCombatSkill = false,
                mustKeyPress = true,
                fullRestockOnAssign = false,
                beginSkillCooldownOnSkillEnd = true,
            });
            specialSkillDef1.upgradedSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GIUntransform",
                skillNameToken = GI_PREFIX + "SPECIAL_UNDEPLOY_NAME",
                skillDescriptionToken = GI_PREFIX + "SPECIAL_UNDEPLOY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(UnBarricade)),
                activationStateMachineName = "Transform",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseMaxStock = 0,
                baseRechargeInterval = 0f,

                rechargeStock = 0,
                requiredStock = 0,
                stockToConsume = 0,
                
                isCombatSkill = false,
                mustKeyPress = true,
                dontAllowPastMaxStocks = true,
                fullRestockOnAssign = false
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);

            Config.ConfigureSkillDef(specialSkillDef1, GIConfig.SectionBody, "M4 Transform");
        }
        #endregion skills
        
        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.GetComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin

            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(HENRY_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);

            #endregion
            
            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            GIAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.EntityStates.Engi.Mine.Detonate.Explode += Detonate_Explode;
        }

        private void Detonate_Explode(On.EntityStates.Engi.Mine.Detonate.orig_Explode orig, EntityStates.Engi.Mine.Detonate self)
        {
            if(!self.outer.name.Contains("GIEngiMine"))
            {
                orig(self);
                return;
            }

            ProjectileDamage component = self.GetComponent<ProjectileDamage>();
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            EntityStateMachine armingStateMachine = self.armingStateMachine;
            BaseMineArmingState baseMineArmingState;
            if ((baseMineArmingState = (((armingStateMachine != null) ? armingStateMachine.state : null) as BaseMineArmingState)) != null)
            {
                num = baseMineArmingState.damageScale;
                num2 = baseMineArmingState.forceScale;
                num3 = baseMineArmingState.blastRadiusScale;
            }
            float num4 = Detonate.blastRadius * num3;
            new BlastAttack
            {
                procChainMask = self.projectileController.procChainMask,
                procCoefficient = self.projectileController.procCoefficient,
                attacker = self.projectileController.owner,
                inflictor = self.gameObject,
                teamIndex = self.projectileController.teamFilter.teamIndex,
                baseDamage = component.damage * num,
                baseForce = component.force * num2,
                falloffModel = BlastAttack.FalloffModel.None,
                crit = component.crit,
                radius = num4,
                position = self.transform.position,
                damageColorIndex = component.damageColorIndex,
                damageType = DamageType.Stun1s
            }.Fire();
            if (Detonate.explosionEffectPrefab)
            {
                EffectManager.SpawnEffect(Detonate.explosionEffectPrefab, new EffectData
                {
                    origin = self.transform.position,
                    rotation = self.transform.rotation,
                    scale = num4
                }, true);
            }
            EntityState.Destroy(self.gameObject);
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(GIBuffs.armorBuff))
            {
                args.armorAdd += GIConfig.M4_Transform_Armor.Value;
            }
        }
    }
}