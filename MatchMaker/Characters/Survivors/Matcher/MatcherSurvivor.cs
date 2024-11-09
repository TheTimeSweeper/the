using BepInEx.Configuration;
using EntityStates;
using EntityStates.Engi.Mine;
using MatcherMod.Modules;
using MatcherMod.Modules.Characters;
using MatcherMod.Survivors.Matcher.Components;
using MatcherMod.Survivors.Matcher.Components.UI;
using MatcherMod.Survivors.Matcher.MatcherContent;
using MatcherMod.Survivors.Matcher.SkillDefs;
using MatcherMod.Survivors.Matcher.SkillStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Config = MatcherMod.Survivors.Matcher.MatcherContent.Config;

namespace MatcherMod.Survivors.Matcher
{
    public class MatcherSurvivor : SurvivorBase<MatcherSurvivor>
    {
        public const string CHARACTER_NAME = "Matcher";

        public override string assetBundleName => "joe";

        public override string bodyName => CHARACTER_NAME + "Body";

        public override string masterName => CHARACTER_NAME + "MonsterMaster";

        public override string modelPrefabName => "mdlJoe";
        public override string displayPrefabName => "JoeDisplay";

        public const string TOKEN_PREFIX = MatcherPlugin.DEVELOPER_PREFIX + "_MATCHER_";

        public override string survivorTokenPrefix => TOKEN_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = TOKEN_PREFIX + "NAME",
            subtitleNameToken = TOKEN_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texIconConscript"),
            bodyColor = new Color(1, 0.616f, 0.49f),
            sortPosition = 69.5f,
            
            //crosshairBundlePath = "GICrosshair",
            crosshairAddressablePath = "RoR2/Base/Toolbot/SMGCrosshair.prefab",
            podPrefabAddressablePath = "RoR2/Base/SurvivorPod/SurvivorPod.prefab",

            maxHealth = 110f,
            healthRegen = 1.0f,
            armor = 00f,

            jumpCount = 1,
        };

        public override UnlockableDef characterUnlockableDef => null;// GIUnlockables.characterUnlockableDef;

        public override BaseItemDisplaysSetup itemDisplays { get; } = new MatcherContent.JoeItemDisplays();

        public MatcherGridController matcherGridController;

        public override void Initialize()
        {
            //if (!MatcherContent.Config.MatcherEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void OnCharacterInitialized()
        {
            Modules.Config.ConfigureBody(prefabCharacterBody, MatcherContent.Config.SectionBody);

            MatcherContent.Config.Init();

            MatcherContent.States.Init();
            MatcherContent.Tokens.Init();
            Modules.Language.PrintOutput("matchmaker.txt");

            MatcherContent.Buffs.Init(assetBundle);
            MatcherContent.Assets.Init(assetBundle);
            MatcherContent.Items.Init();

            AdditionalBodySetup();

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AddHooks();
        }
        
        private void AdditionalBodySetup()
        {
            matcherGridController = bodyPrefab.AddComponent<Components.MatcherGridController>();
        }

        public override void InitializeEntityStateMachines() 
        {
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            Skills.ClearGenericSkills(bodyPrefab);

            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddFourthSymbolSkills();
            AddSpecialSkills();
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            GenericSkill genericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);
            matcherGridController.genericSkills.Add(genericSkill);
            MatchBoostedSkillDef primarySkillDef1 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            (
                "MatcherSword",
                TOKEN_PREFIX + "PRIMARY_SWORD_NAME",
                TOKEN_PREFIX + "PRIMARY_SWORD_DESCRIPTION",
                assetBundle.LoadAsset<Sprite>("texTileSword"),
                new EntityStates.SerializableEntityStateType(typeof(SkillStates.Sword)),
                "Weapon",
                true
            ));
            //primarySkillDef1.stepCount = 2;
            //primarySkillDef1.stepGraceDuration = 0.5f;
            primarySkillDef1.matchConsumptionCost = 1;
            primarySkillDef1.matchConsumptionMinimum = 1;
            primarySkillDef1.matchMaxConsumptions = 1;
            primarySkillDef1.associatedBuff = CreateMatchBuff(primarySkillDef1);

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private BuffDef CreateMatchBuff(MatchBoostedSkillDef matchBoostedSkillDef)
        {
            return Content.CreateAndAddBuff(matchBoostedSkillDef.skillName, matchBoostedSkillDef.icon, Color.white, true, false);
        }

        private void AddSecondarySkills()
        {
            GenericSkill genericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);
            matcherGridController.genericSkills.Add(genericSkill);

            //here is a basic skill def with all fields accounted for
            MatchBoostedSkillDef secondarySkillDef1 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherStaff",
                skillNameToken = TOKEN_PREFIX + "SECONDARY_STAFF_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SECONDARY_STAFF_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texTileStaff"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Secondary1Fireball)),
                activationStateMachineName = "Weapon",
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
            secondarySkillDef1.matchConsumptionCost = 1;
            secondarySkillDef1.matchConsumptionMinimum = 1;
            secondarySkillDef1.matchMaxConsumptions = 8;
            secondarySkillDef1.associatedBuff = CreateMatchBuff(secondarySkillDef1);

            //here is a basic skill def with all fields accounted for
            MatchBoostedSkillDef secondarySkillDef2 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherStaff2",
                skillNameToken = TOKEN_PREFIX + "SECONDARY_STAFF2_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SECONDARY_STAFF2_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texTileStaff"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Secondary1Explosion)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
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
            secondarySkillDef2.matchConsumptionCost = 1;
            secondarySkillDef2.matchConsumptionMinimum = 1;
            secondarySkillDef2.matchMaxConsumptions = int.MaxValue;
            secondarySkillDef2.associatedBuff = CreateMatchBuff(secondarySkillDef2);

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1, secondarySkillDef2);
        }
        
        private void AddUtiitySkills()
        {
            GenericSkill genericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);
            matcherGridController.genericSkills.Add(genericSkill);
            //here's a skilldef of a typical movement skill.
            MatchBoostedSkillDef utilitySkillDef1 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherShield",
                skillNameToken = TOKEN_PREFIX + "UTILITY_ROLL_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTileShield"),
                
                activationState = new EntityStates.SerializableEntityStateType(typeof(Roll)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });
            utilitySkillDef1.matchConsumptionCost = 5;
            utilitySkillDef1.matchConsumptionMinimum = 5;
            utilitySkillDef1.matchMaxConsumptions = 1;
            Buffs.shieldMatchBuff = CreateMatchBuff(utilitySkillDef1);
            utilitySkillDef1.associatedBuff = Buffs.shieldMatchBuff;

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
        }

        private void AddFourthSymbolSkills()
        {
            GenericSkill genericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "FourthSymbol");
            matcherGridController.genericSkills.Add(genericSkill);

            MatchBoostedSkillDef passiveSkillDef1 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherKey",
                skillNameToken = TOKEN_PREFIX + "EXTRA_KEY_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "EXTRA_KEY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTileKey"),
            });
            passiveSkillDef1.matchConsumptionCost = 1;
            passiveSkillDef1.matchMaxConsumptions = 1;
            passiveSkillDef1.matchConsumptionMinimum = 1;
            //passiveSkillDef1.associatedBuff = CreateMatchBuff(passiveSkillDef1);
            passiveSkillDef1.CustomMatchAction = KeyMatchAction;

            CrateMatchSkillDef passiveSkillDef2 = Skills.CreateSkillDef<CrateMatchSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherCrate",
                skillNameToken = TOKEN_PREFIX + "EXTRA_CRATE_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "EXTRA_CRATE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTileCrate"),
                
            });
            passiveSkillDef2.matchConsumptionCost = 1;
            passiveSkillDef2.matchMaxConsumptions = 1;
            passiveSkillDef2.matchConsumptionMinimum = 1;
            //passiveSkillDef3.associatedBuff = CreateMatchBuff(passiveSkillDef3);
            passiveSkillDef2.CustomMatchAction = CrateMatchSkillDef.CrateMatchAction;

            MatchBoostedSkillDef passiveSkillDef3 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherBrain",
                skillNameToken = TOKEN_PREFIX + "EXTRA_BRAIN_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "EXTRA_BRAIN_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTileBrain"),

            });
            passiveSkillDef3.matchConsumptionCost = 1;
            passiveSkillDef3.matchMaxConsumptions = 1;
            passiveSkillDef3.matchConsumptionMinimum = 1;
            //passiveSkillDef3.associatedBuff = CreateMatchBuff(passiveSkillDef3);
            passiveSkillDef3.CustomMatchAction = BrainMatchAction;

            //MatchBoostedSkillDef passiveSkillDef4 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            //{
            //    skillName = "MatcherMuscle",
            //    skillNameToken = TOKEN_PREFIX + "PASSIVE_NAME",
            //    skillDescriptionToken = TOKEN_PREFIX + "PASSIVE_DESCRIPTION",
            //    skillIcon = assetBundle.LoadAsset<Sprite>("texTileMuscle"),

            //});
            //passiveSkillDef4.matchConsumptionCost = 1;
            //passiveSkillDef4.matchMaxConsumptions = 1;
            //passiveSkillDef4.matchConsumptionMinimum = 1;
            //passiveSkillDef4.associatedBuff = CreateMatchBuff(passiveSkillDef2);

            MatchBoostedSkillDef passiveSkillDef5 = Skills.CreateSkillDef<MatchBoostedSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherChicken",
                skillNameToken = TOKEN_PREFIX + "EXTRA_CHICKEN_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "EXTRA_CHICKEN_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTileChicken"),
            });
            passiveSkillDef5.matchConsumptionCost = 1;
            passiveSkillDef5.matchMaxConsumptions = 1;
            passiveSkillDef5.matchConsumptionMinimum = 1;
            //passiveSkillDef5.associatedBuff = CreateMatchBuff(passiveSkillDef3);
            passiveSkillDef5.CustomMatchAction = ChickenMatchAction;

            Skills.AddSkillsToFamily(genericSkill.skillFamily, passiveSkillDef1, passiveSkillDef2, passiveSkillDef3/*, passiveSkillDef4*//*, passiveSkillDef5*/);
        }

        private static GameObject KeyMatchAction(MatcherGridController matcherGridController, GenericSkill genericSkill, int matches)
        {
            GameObject currentInteractable = matcherGridController.GetComponent<InteractionDriver>().currentInteractable;
            if (currentInteractable != null && currentInteractable.TryGetComponent(out PurchaseInteraction purchaseInteraction))
            {
                if (purchaseInteraction.costType == CostTypeIndex.Money)
                {
                    int costReduce = matches * Run.instance.GetDifficultyScaledCost(MatcherContent.Config.M4_Key_UnlockBaseValue.Value);

                    matcherGridController.CmdKeyReduceInteractableCost(purchaseInteraction.gameObject, costReduce);
                    //purchaseInteraction.cost = Mathf.Max(0, purchaseInteraction.Networkcost - costReduce);
                    return purchaseInteraction.gameObject;
                }
            }
            return null;
        }

        private GameObject BrainMatchAction(MatcherGridController controller, GenericSkill genericSkill, int matches)
        {
            int nearBodies = 0;
            for (TeamIndex teamIndex = TeamIndex.Player; teamIndex < TeamIndex.Count; teamIndex += 1)
            {
                ReadOnlyCollection<TeamComponent> teamComponents = TeamComponent.GetTeamMembers(teamIndex);

                for (int i = 0; i < teamComponents.Count; i++)
                {
                    if ((teamComponents[i].transform.position - controller.transform.position).sqrMagnitude < Config.M4_Brain_NearDistance.Value)
                    {
                        if (teamComponents[i].TryGetComponent(out CharacterBody body))
                        {
                            if(teamComponents[i].teamIndex != controller.CharacterBody.teamComponent.teamIndex)
                            {
                                nearBodies++;
                            }
                        }
                    }
                }
            }
            nearBodies = Mathf.Max(nearBodies, 1);

            controller.CharacterBody.master.GiveExperience((ulong)(nearBodies * matches * Config.M4_Brain_Experience.Value * controller.CharacterBody.level));
            return controller.gameObject;
        }

        private GameObject ChickenMatchAction(MatcherGridController controller, GenericSkill genericSkill, int matches)
        {
            HealthComponent healthComponent = controller.CharacterBody.healthComponent;
            if (healthComponent.combinedHealthFraction > 0.9f)
            {
                controller.CharacterBody.master.inventory.GiveItem(Items.ChickenItem);
                return controller.gameObject;
            }
            return null;
        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            HasMatcherGridControllerSkillDef specialSkillDef1 = Skills.CreateSkillDef<HasMatcherGridControllerSkillDef>(new SkillDefInfo
            {
                skillName = "MatcherGrid",
                skillNameToken = TOKEN_PREFIX + "SPECIAL_MATCH_NAME",
                skillDescriptionToken = TOKEN_PREFIX + "SPECIAL_MATCH_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.MatchMenu)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseMaxStock = 1,
                requiredStock = 1,
                stockToConsume = 0,
                baseRechargeInterval = 5f,

                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
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
            MatcherContent.AI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }
        
        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;

            On.RoR2.UI.HUD.Awake += HUD_Awake;

            On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;

            On.RoR2.TeleporterInteraction.OnInteractionBegin += TeleporterInteraction_OnInteractionBegin;
        }

        private void TeleporterInteraction_OnInteractionBegin(On.RoR2.TeleporterInteraction.orig_OnInteractionBegin orig, TeleporterInteraction self, Interactor activator)
        {
            orig(self, activator);

            if (self.currentState is TeleporterInteraction.IdleState)
            {
                List<MatcherGridController> matchers = InstanceTracker.GetInstancesList<MatcherGridController>();
                if (matchers.Count > 0)
                {

                    Vector3 random = UnityEngine.Random.onUnitSphere + self.transform.position;
                    random.y = self.transform.position.y;

                    GameObject box = UnityEngine.Object.Instantiate(MatcherContent.Assets.BoxToOpenByMatching, random, Quaternion.identity);

                    int difficulty = Run.instance.stageClearCount * matchers.Count;

                    int randomAmountsAmount = UnityEngine.Random.Range(1, 5);
                    int[] randomAmounts = new int[randomAmountsAmount];
                    for (int i = 0; i < randomAmountsAmount; i++)
                    {
                        randomAmounts[i] = UnityEngine.Random.Range(4, 7) * Mathf.Max(difficulty / 2, 1);
                    }

                    box.GetComponent<BoxToOpenByMatching>().Init(matchers[0].TileTypes, randomAmounts);
                }
            }
        }

        private void HealthComponent_TakeDamageProcess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);

            if (self.body.HasBuff(Buffs.shieldMatchBuff))
            {
                self.body.RemoveBuff(Buffs.shieldMatchBuff);
            }
        }

        private void HUD_Awake(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            orig(self);
            self.gameObject.AddComponent<MatcherHUDManager>();
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(Buffs.shieldMatchBuff))
            {
                args.armorAdd += MatcherContent.Config.M3_Shield_BuffArmor.Value;
            }
        }
    }
}