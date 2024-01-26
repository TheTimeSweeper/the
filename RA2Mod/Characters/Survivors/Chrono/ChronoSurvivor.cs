using BepInEx.Configuration;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RA2Mod.Modules;
using RA2Mod.Modules.Characters;
using RA2Mod.Survivors.Chrono.Components;
using RA2Mod.Survivors.Chrono.SkillStates;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using R2API;
using RA2Mod.Survivors.Chrono.SkillDefs;
using UnityEngine.SceneManagement;

namespace RA2Mod.Survivors.Chrono
{
    public class ChronoSurvivor : SurvivorBase<ChronoSurvivor>
    {
        public override string assetBundleName => "joe";

        public override string bodyName => "RA2ChronoBody";

        public override string masterName => "ChronoMonsterMaster";

        public override string modelPrefabName => "mdlJoe";
        public override string displayPrefabName => "JoeDisplay";

        public const string CHRONO_PREFIX = RA2Plugin.DEVELOPER_PREFIX + "_CHRONO_";

        public override string survivorTokenPrefix => CHRONO_PREFIX;
        
        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = CHRONO_PREFIX + "NAME",
            subtitleNameToken = CHRONO_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texIconChrono"),
            bodyColor = Color.cyan,
            sortPosition = 100,

            crosshair = Assets.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 130f,
            healthRegen = 2.0f,
            armor = 10f,

            jumpCount = 1,
        };

        public override UnlockableDef characterUnlockableDef => ChronoUnlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays => null;// new ChronoItemDisplays();

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }
        
        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            //ConfigEntry<bool> characterEnabled = General.GeneralConfig.ChronoEnabled;

            //if (!characterEnabled.Value)
            //   return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            //ChronoUnlockables.Init();

            Log.CurrentTime("SURVIVOR INIT 1");

            base.InitializeCharacter();

            Log.CurrentTime("SURVIVOR INIT 2");
            ChronoConfig.Init();
            ChronoStates.Init();
            ChronoTokens.Init();

            Log.CurrentTime("SURVIVOR INIT 3");
            ChronoHealthBars.Init();
            ChronoDamageTypes.Init();
            if (!RA2Plugin.testAsyncLoading)
            {
                ChronoAssets.Init(assetBundle);
            }
            ChronoBuffs.Init(assetBundle);
            ChronoItems.Init();
            ChronoCompat.Init();
            Log.CurrentTime("SURVIVOR INIT 4");

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();

            Log.CurrentTime("SURVIVOR INIT END");

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if(arg0.name == "title")
            {
                Log.CurrentTime("TITLE SCREEN");
                Log.AllTimes();
            }
        }

        private void AdditionalBodySetup()
        {
            prefabCharacterBody.bodyFlags |= CharacterBody.BodyFlags.SprintAnyDirection;
            bodyPrefab.AddComponent<ChronoTrackerBomb>();
            bodyPrefab.AddComponent<ChronoTrackerVanish>();
            bodyPrefab.AddComponent<PhaseIndicatorController>();
        }

        public override void InitializeEntityStateMachines() 
        {
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(ChronoCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }
        
        #region skills
        public override void InitializeSkills()
        {
            AddPassiveSkill();
            Skills.CreateSkillFamilies(bodyPrefab);
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();
        }

        private void AddPassiveSkill()
        {
            GenericSkill passiveSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "LOADOUT_PASSIVE", "chronopassive");

            HasPhaseIndicatorSkillDef sprintSkillDef = Skills.CreateSkillDef<HasPhaseIndicatorSkillDef>(new SkillDefInfo
            {
                skillName = "chronoPassive",
                skillNameToken = CHRONO_PREFIX + "PASSIVE_SPRINT_NAME",
                skillDescriptionToken = CHRONO_PREFIX + "PASSIVE_SPRINT_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ChronoSprintState)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 0.0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 0,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            Skills.AddSkillsToFamily(passiveSkill.skillFamily, sprintSkillDef);
        }

        private void AddPrimarySkills()
        {
            SkillDef shootSkillDef = Skills.CreateSkillDef(new SkillDefInfo
                (
                    "chronoShoot",
                    CHRONO_PREFIX + "PRIMARY_SHOOT_NAME",
                    CHRONO_PREFIX + "PRIMARY_SHOOT_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                    "Weapon",
                    false
                ));

            Skills.AddPrimarySkills(bodyPrefab, shootSkillDef);
        }
        
        private void AddSecondarySkills()
        {
            ChronoTrackerSkillDefBomb secondarySkillDef = Skills.CreateSkillDef<ChronoTrackerSkillDefBomb> (new SkillDefInfo
            {
                skillName = "chronoIvan",
                skillNameToken = CHRONO_PREFIX + "SECONDARY_BOMB_NAME",
                skillDescriptionToken = CHRONO_PREFIX + "SECONDARY_BOMB_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ChronoBomb)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 6f,
                baseMaxStock = 2,

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

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef);
        }
        
        private void AddUtiitySkills()
        {
            //here's a skilldef of a typical movement skill. some fields are omitted and will just have default values
            SkillDef utilitySkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "chronosphere",
                skillNameToken = CHRONO_PREFIX + "UTILITY_CHRONOSPHERE_NAME",
                skillDescriptionToken = CHRONO_PREFIX + "UTILITY_CHRONOSPHERE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(AimChronosphere1)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 16f,

                isCombatSkill = false,
                mustKeyPress = true,
                forceSprintDuringState = false,
                cancelSprintingOnActivation = true,
                fullRestockOnAssign = false
            });

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef);
        }

        private void AddSpecialSkills()
        {
            //a basic skill
            ChronoTrackerSkillDefVanish vanishSkillDef = Skills.CreateSkillDef<ChronoTrackerSkillDefVanish> (new SkillDefInfo
            {
                skillName = "chronoVanish",
                skillNameToken = CHRONO_PREFIX + "SPECIAL_VANISH_NAME",
                skillDescriptionToken = CHRONO_PREFIX + "SPECIAL_VANISH_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Vanish)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon", 
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 2,
                baseRechargeInterval = 8f,

                isCombatSkill = true,
                mustKeyPress = true,
            });

            Skills.AddSpecialSkills(bodyPrefab, vanishSkillDef);
        }
        #endregion skills
        
        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
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
            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            ChronoAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
 
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);
            if (!damageInfo.rejected)
            {
                if (damageInfo.HasModdedDamageType(ChronoDamageTypes.chronoDamage))
                {
                    self.body.AddBuff(ChronoBuffs.chronoDebuff);
                    self.body.inventory?.GiveItem(ChronoItems.chronoSicknessItemDef.itemIndex);
                }

                if (damageInfo.HasModdedDamageType(ChronoDamageTypes.chronoDamageDouble))
                {
                    self.body.AddBuff(ChronoBuffs.chronoDebuff);
                    self.body.AddBuff(ChronoBuffs.chronoDebuff);
                    self.body.inventory?.GiveItem(ChronoItems.chronoSicknessItemDef.itemIndex, 2);
                }
            }

            if (damageInfo.HasModdedDamageType(ChronoDamageTypes.vanishingDamage))
            {
                int count = self.body.GetBuffCount(ChronoBuffs.chronoDebuff);
                if(self.combinedHealthFraction < count / ChronoConfig.M4ChronoStacksToVanish.Value)
                {
                    EffectManager.SimpleEffect(ChronoAssets.vanishEffect, self.transform.position, Quaternion.identity, true);
                    self.Suicide(damageInfo.attacker, damageInfo.inflictor);
                }
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(ChronoBuffs.chronoDebuff))
            {
                args.moveSpeedReductionMultAdd += 1;
            }

            if (sender.HasBuff(ChronoBuffs.chronosphereRootDebuff))
            {
                args.moveSpeedRootCount += 1;
            }
        }
    }
}