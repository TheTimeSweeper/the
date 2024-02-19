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
using RA2Mod.General.Components;
using System.Runtime.CompilerServices;
using R2API.Utils;

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
            sortPosition = 69.5f,

            crosshair = Assets.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 130f,
            healthRegen = 2.0f,
            armor = 10f,

            jumpCount = 1,

            cameraParams = cameraParams,
        };

        private CharacterCameraParams cameraParams { get
            {
                CharacterCameraParams camera = ScriptableObject.CreateInstance<CharacterCameraParams>();
                camera.data.minPitch = -70;
                camera.data.maxPitch = 70;
                camera.data.wallCushion = 0.1f;
                camera.data.pivotVerticalOffset = 1.37f;
                camera.data.idealLocalCameraPos = new Vector3(0, 0, -10);
                camera.data.fov = new HG.BlendableTypes.BlendableFloat { value = 60f, alpha = 1f };

                return camera;
            } 
        }

        public override UnlockableDef characterUnlockableDef => ChronoUnlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays =>  new RA2Mod.General.JoeItemDisplays();

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
            bodyPrefab.AddComponent<VoiceLines>().prefix = "Play_Chrono_";
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

            if (Modules.Compat.scepterInstalled)
            {
                AddScepterPassiveSkill(sprintSkillDef);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void AddScepterPassiveSkill(SkillDef originalSkillDef)
        {
            HasPhaseIndicatorSkillDef sprintSkillDefScepter = Skills.CreateSkillDef<HasPhaseIndicatorSkillDef>(new SkillDefInfo
            {
                skillName = "chronoPassiveScepter",
                skillNameToken = CHRONO_PREFIX + "PASSIVE_SPRINT_NAME",
                skillDescriptionToken = CHRONO_PREFIX + "PASSIVE_SPRINT_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ChronoSprintStateEpic)),
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

            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(sprintSkillDefScepter, bodyName, originalSkillDef);
        }

        private void AddPrimarySkills()
        {
            SkillDef shootSkillDef = Skills.CreateSkillDef(new SkillDefInfo
                (
                    "chronoShoot",
                    CHRONO_PREFIX + "PRIMARY_SHOOT_NAME",
                    CHRONO_PREFIX + "PRIMARY_SHOOT_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.ChronoShoot)),
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

            Config.ConfigureSkillDef(secondarySkillDef, ChronoConfig.SectionBody, "M2 Bomb");
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
                stockToConsume = 0,
                requiredStock = 1,

                isCombatSkill = false,
                mustKeyPress = true,
                forceSprintDuringState = false,
                cancelSprintingOnActivation = true,
                fullRestockOnAssign = false
            });

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef);

            Config.ConfigureSkillDef(utilitySkillDef, ChronoConfig.SectionBody, "M3 Chronosphere");
        }

        private void AddSpecialSkills()
        {
            //a basic skill
            ChronoTrackerSkillDefVanish vanishSkillDef = Skills.CreateSkillDef<ChronoTrackerSkillDefVanish>(new SkillDefInfo
            {
                skillName = "chronoVanish",
                skillNameToken = CHRONO_PREFIX + "SPECIAL_VANISH_NAME",
                skillDescriptionToken = CHRONO_PREFIX + "SPECIAL_VANISH_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Vanish)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 6f,

                isCombatSkill = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true
            });

            Skills.AddSpecialSkills(bodyPrefab, vanishSkillDef);
            
            Config.ConfigureSkillDef(vanishSkillDef, ChronoConfig.SectionBody, "M4 Vanish");
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
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamageIL;

            IL.EntityStates.GenericCharacterDeath.OnEnter += GenericCharacterDeath_OnEnter1;

            On.RoR2.CharacterBody.OnBuffFinalStackLost += CharacterBody_OnBuffFinalStackLost;
 
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void GenericCharacterDeath_OnEnter1(ILContext il)
        {
            ILCursor cursor = new ILCursor(il);
            cursor.GotoNext(MoveType.After,
                //instruction => instruction.MatchLdarg(0),
                //instruction => instruction.MatchCall<EntityStates.EntityState>("get_characterBody"),
                //instruction => instruction.MatchCall<UnityEngine.Object>("op_Implicit"),
                //instruction => instruction.MatchBrfalse(out _),
                //instruction => instruction.MatchLdarg(0),
                instruction => instruction.MatchCall<EntityStates.EntityState>("get_isAuthority")
                );
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate<Func<bool, EntityStates.GenericCharacterDeath, bool>>((authority, death) =>
            {
                if (death.healthComponent && (death.healthComponent.killingDamageType & DamageType.OutOfBounds) > DamageType.Generic)
                {
                    authority = false;

                    EffectManager.SimpleEffect(ChronoAssets.vanishEffect, death.transform.position, Quaternion.identity, true);
                }

                return authority;
            });
        }

        private void HealthComponent_TakeDamageIL(ILContext il)
        {
            ILCursor cursor = new ILCursor(il);
            cursor.GotoNext(MoveType.After,
                instruction => instruction.MatchLdcI4(0x10000),
                instruction => instruction.MatchOr(),
                instruction => instruction.MatchStfld<DamageInfo>("damageType"),
                instruction => instruction.MatchLdloc(8)
                );
            cursor.Emit(OpCodes.Ldarg_1);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate<Func<bool, DamageInfo, HealthComponent, bool>>((flag5, damageInfo, self) =>
            {
                if (damageInfo.HasModdedDamageType(ChronoDamageTypes.vanishingDamage))
                {
                    int count = 0;
                    if (self.body.inventory)
                    {
                        count = self.body.inventory.GetItemCount(ChronoItems.chronoSicknessItemDef.itemIndex);
                    }
                    if (self.combinedHealthFraction < count / (ChronoConfig.M4_Vanish_ChronoStacksRequired.Value * 2))
                    {
                        flag5 = true;
                        damageInfo.damageType |= DamageType.VoidDeath;
                        damageInfo.damageType |= DamageType.OutOfBounds;
                    }
                }
                return flag5;
            });
        }

        private void CharacterBody_OnBuffFinalStackLost(On.RoR2.CharacterBody.orig_OnBuffFinalStackLost orig, CharacterBody self, BuffDef buffDef)
        {
            orig(self, buffDef);
            if(buffDef == ChronoBuffs.chronoSicknessDebuff)
            {
                self.inventory.RemoveItem(ChronoItems.chronoSicknessItemDef, self.inventory.GetItemCount(ChronoItems.chronoSicknessItemDef));
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            //if (damageInfo.HasModdedDamageType(ChronoDamageTypes.vanishingDamage))
            //{
            //    int count = 0;
            //    if (self.body.inventory)
            //    {
            //        count = self.body.inventory.GetItemCount(ChronoItems.chronoSicknessItemDef.itemIndex);
            //    }
            //    if (self.combinedHealthFraction < count / (ChronoConfig.M4_Vanish_ChronoStacksRequired.Value * 2))
            //    {
            //        EffectManager.SimpleEffect(ChronoAssets.vanishEffect, self.transform.position, Quaternion.identity, true);
            //        if (self.body.modelLocator && self.body.modelLocator.modelTransform)
            //        {
            //            CharacterModel characterModel = self.body.modelLocator.modelTransform.GetComponent<CharacterModel>();
            //            characterModel.invisibilityCount++;
            //        }
            //        self.Suicide(damageInfo.attacker, damageInfo.inflictor);
            //    }
            //}


            if (damageInfo.HasModdedDamageType(ChronoDamageTypes.chronoDamagePierce))
            {
                AddChronoSickness(self.body);
            }
            orig(self, damageInfo);
            if (!damageInfo.rejected)
            {
                if (damageInfo.HasModdedDamageType(ChronoDamageTypes.chronoDamage))
                {
                    AddChronoSickness(self.body);
                }

                if (damageInfo.HasModdedDamageType(ChronoDamageTypes.chronoDamageDouble))
                {
                    AddChronoSickness(self.body);
                    AddChronoSickness(self.body);
                }
            }
        }

        private static void AddChronoSickness(CharacterBody body)
        {
            if (!body.isPlayerControlled)
            {
                body.AddBuff(ChronoBuffs.chronoSicknessDebuff);
            } 
            else
            {
                body.AddTimedBuff(ChronoBuffs.chronoSicknessDebuff, 5);
            }
            int stacks = body.isChampion ? 1 : 2;
            body.inventory?.GiveItem(ChronoItems.chronoSicknessItemDef.itemIndex, stacks);
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {
            //if (sender.HasBuff(ChronoBuffs.chronoSicknessDebuff))
            //{
            //    args.moveSpeedReductionMultAdd += 1;
            //}

            if (sender.HasBuff(ChronoBuffs.chronosphereRootDebuff))
            {
                args.moveSpeedRootCount += 1;
            }
        }
    }
}