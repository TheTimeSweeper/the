using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using EntityStates;
using R2API;
using RoR2.CharacterAI;
using RA2Mod.Modules.Characters;
using RA2Mod.Survivors.Tesla;
using RA2Mod.Modules;
using RA2Mod.Survivors.Tesla.Orbs;
using System.Collections;
using RA2Mod.Minions.TeslaTower.States;
using System.Linq;
using RA2Mod.General;

namespace RA2Mod.Minions.TeslaTower
{

    public class TeslaTowerNotSurvivor : CharacterBase<TeslaTowerNotSurvivor> {

        public override string assetBundleName => "teslatrooper";

        public override string bodyName => "TeslaTowerBody";

        public override string modelPrefabName => "mdlTeslaTower";

        public const string TOWER_PREFIX = RA2Mod.RA2Plugin.DEVELOPER_PREFIX + "_TESLA_TOWER_BODY_";

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = TOWER_PREFIX + "NAME",
            subtitleNameToken = TOWER_PREFIX + "SUBTITLE",
            bodyColor = new Color(134f / 216f, 234f / 255f, 255f / 255f), //new Color(115f/216f, 216f/255f, 0.93f),
            sortPosition = 69f,

            bodyToClonePath = "RoR2/Base/Engi/EngiTurretBody.prefab",
            characterPortraitBundlePath = "texIconTeslaTower",
            crosshairAddressablePath = "RoR2/Base/UI/TiltedBracketCrosshair.prefab",
            podPrefab = null,

            bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes | CharacterBody.BodyFlags.Mechanical,

            maxHealth = 200f,
            healthRegen = 0f,
            armor = 1200f,

            jumpCount = 1,

            //every line of code I write to make this mangled mess work without just setting up a boilerplate characterbody
            hasCharacterDirection = false,
            hasFoostepController = false,
            hasRagdoll = false,

            aimOriginPosition = new Vector3(0, 10, 0),
            cameraPivotPosition = new Vector3(0, 5, 0),
            cameraParamsVerticalOffset = 10,
            cameraParamsDepth = -20
        };

        public override ItemDisplaysBase itemDisplays => TeslaConfig.M4_Tower_ItemDisplays.Value ? new TeslaTowerItemDisplays() : null;

        public static GameObject masterPrefab;
        public static SkinDef.MinionSkinReplacement MasteryMinionSkinReplacement;
        public static SkinDef.MinionSkinReplacement MCMinionSkinReplacement;
        public static SkinDef.MinionSkinReplacement NodMinionSkinReplacement;

        public override List<IEnumerator> GetAssetBundleInitializedCoroutines()
        {
            return TeslaTowerAssets.GetAssetBundleInitializedCoroutines(assetBundle);
        }

        public override void OnCharacterInitialized()
        {
            //Config.ConfigureBody(prefabCharacterBody, TeslaConfig.SectionBody);

            //TeslaConfig.Init();
            //some assets are changed based on config
            //TeslaAssets.OnCharacterInitialized(assetBundle);
            TeslaTowerStates.Init();

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();
            if (General.GeneralCompat.ScepterInstalled)
            {
                new TeslaTowerScepter().Initialize();
            }

            Log.CurrentTime($"{bodyName} initializecharacter done");
        }

        private void AdditionalBodySetup()
        {
            bodyPrefab.AddComponent<TowerWeaponComponent>();
            bodyPrefab.AddComponent<TowerOwnerTrackerComponent>();

            bodyPrefab.GetComponent<CharacterBody>().overrideCoreTransform = characterModelObject.GetComponent<ChildLocator>().FindChild("Head");

            bodyPrefab.GetComponent<SfxLocator>().deathSound = "Play_building_uselbuil";
            bodyPrefab.GetComponent<SfxLocator>().aliveLoopStart = "";

            UnityEngine.Object.Destroy(bodyPrefab.GetComponent<SetStateOnHurt>());
            UnityEngine.Object.Destroy(bodyPrefab.GetComponent<AkEvent>());

            characterModelObject.GetComponent<ChildLocator>().FindChild("LightningParticles").GetComponent<ParticleSystemRenderer>().material = TeslaTowerAssets.ChainLightningMaterial;
        }

        public override void InitializeEntityStateMachines() {

            Prefabs.ClearEntityStateMachines(bodyPrefab);

            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(TowerIdleSearch), typeof(TowerSpawnState));

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon", typeof(TowerLifetime), typeof(TowerLifetime));

            bodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new SerializableEntityStateType(typeof(TowerSell));
        }
        
        public override void InitializeCharacterMaster() {

            TeslaTowerAI.Init(bodyPrefab, "TeslaTowerMaster", (result) => {
                masterPrefab = result;
            });
        }

        #region skills

        public override void InitializeSkills() {          
            Modules.Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Primary, SkillSlot.Secondary);

            InitializePrimarySkills();

            InitializeSecondarySkills();
        }

        private void InitializePrimarySkills() {
            SkillDef primarySkillDefZap = Modules.Skills.CreateSkillDef(new SkillDefInfo("Tower_Primary_Zap",
                                                                                         TOWER_PREFIX + "PRIMARY_ZAP_NAME",
                                                                                         TOWER_PREFIX + "PRIMARY_ZAP_DESCRIPTION",
                                                                                         assetBundle.LoadAsset<Sprite>("texIconTeslaTower"),
                                                                                         new EntityStates.SerializableEntityStateType(typeof(TowerZap)),
                                                                                         "Body",
                                                                                         false));

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDefZap);
        }

        private void InitializeSecondarySkills() {
            SkillDef bigZapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "Tower_Secondary_BigZap",
                skillNameToken = TOWER_PREFIX + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = TOWER_PREFIX + "SECONDARY_BIGZAP_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTeslaSkillSecondaryThunderclap"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(TowerBigZap)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 9f,
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
                keywordTokens = new string[] { "KEYWORD_SHOCKING" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, bigZapSkillDef);
        }
#endregion skills

        public override void InitializeSkins() 
        {
            ModelSkinController skinController = characterModelObject.GetComponent<ModelSkinController>();
            ChildLocator childLocator = characterModelObject.GetComponent<ChildLocator>();

            SkinDef[] teslaSkins = TeslaTrooperSurvivor.instance.characterModelObject.GetComponent<ModelSkinController>().skins;
            CharacterModel.RendererInfo[] defaultRenderers = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            List<GameObject> activatedGameObjects = Skins.CreateAllActivatedGameObjectsList(childLocator,
                "Tower_Base_Pillars_Color",
                "Tower_Base_Platform",
                "Tower_Base_Center",
                "Tower_Base_Tubes",
                 
                "Tower_Circles",//4
                "Tower_Pole",
                "Tower_Pole_Tracer",
                "Tower_Emission",
                "Tower_Orb",
                
                "LightningParticles");//9

            #region DefaultSkin

            TeslaSkinDef defaultSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texTeslaSkinDefault"),
                defaultRenderers,
                characterModelObject);

            defaultSkin.ZapLightningType = ModdedLightningType.Loader;

            defaultSkin.gameObjectActivations = Skins.GetGameObjectActivationsFromList(activatedGameObjects,
                0, 
                1,
                2,
                3, 
                4, 
                5, 
                6, 
                //7,
                8,
                9);

            skins.Add(defaultSkin);
            #endregion

            #region mastery

            TeslaSkinDef masterySkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TOWER_PREFIX + "MASTERY_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texTeslaSkinDefault"),
                defaultRenderers,
                characterModelObject);

            masterySkin.ZapLightningType = ModdedLightningType.Loader;

            masterySkin.gameObjectActivations = Skins.GetGameObjectActivationsFromList(activatedGameObjects,
                0,
                1,
                2,
                //3,
                4,
                5,
                //6,
                7,
                8,
                9);

            masterySkin.meshReplacements = Modules.Skins.GetMeshReplacements(assetBundle, defaultRenderers,
                "Mastery_Base_Pillars_Color",
                "Mastery_Base_Platform",
                "Mastery_Base_Center",
                null,//"Mastery_Base_Tubes",

                "Mastery_Circles",
                "Mastery_Pole",
                null,//"Mastery_Pole_Tracer",
                "Mastery_Emission",
                "Mastery_Orb");

            masterySkin.rendererInfos[0].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery");
            masterySkin.rendererInfos[1].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery");
            masterySkin.rendererInfos[2].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery");
            //masterySkin.rendererInfos[3].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matMastery");

            masterySkin.rendererInfos[4].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery");
            masterySkin.rendererInfos[5].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMastery");
            masterySkin.rendererInfos[6].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("WHITE");
            //masterySkin.rendererInfos[7].defaultMaterial = Modules.Materials.CreateHotpooMaterial("matMastery");
            masterySkin.rendererInfos[8].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matMasteryOrb");

            skins.Add(masterySkin);
            MasteryMinionSkinReplacement = new SkinDef.MinionSkinReplacement {
                minionBodyPrefab = bodyPrefab,
                minionSkin = masterySkin,
            };

            teslaSkins[1].minionSkinReplacements = teslaSkins[1].minionSkinReplacements.Append(MasteryMinionSkinReplacement).ToArray();

            #endregion mastery

            #region nod

            TeslaSkinDef NodSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TOWER_PREFIX + "NOD_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texTeslaSkinNod"),
                defaultRenderers,
                characterModelObject);

            NodSkin.ZapLightningType = ModdedLightningType.NodMageThick;

            NodSkin.gameObjectActivations = Skins.GetGameObjectActivationsFromList(activatedGameObjects,
                0,
                //1,
                2,
                //3,
                //4,
                //5,
                //6,
                7);
                //8
                //9);

            NodSkin.meshReplacements = Modules.Skins.GetMeshReplacements(assetBundle, defaultRenderers,
                "Nod_Base_Pillars_Colors",
                null,//"Nod_Base_Platform",
                "Nod_Tower",//"Nod_Base_Center",
                null,//"Nod_Base_Tubes",
                
                null,//"Nod_Circles",
                null,//"Nod_Pole",
                null,//"Nod_Pole_Tracer",
                "Nod_Emission",
                null);//"Nod_Orb");
            
            NodSkin.rendererInfos[0].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerNod");
          //NodSkin.rendererInfos[1].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerCobblestone");
            NodSkin.rendererInfos[2].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerNod");
          //NodSkin.rendererInfos[3].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerBlack");

          //NodSkin.rendererInfos[4].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerQuartz");
          //NodSkin.rendererInfos[5].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerCobblestone");
          //NodSkin.rendererInfos[6].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("WHITE");
            NodSkin.rendererInfos[7].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerNod");
          //NodSkin.rendererInfos[8].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerDiamond");

            skins.Add(NodSkin);
            NodMinionSkinReplacement = new SkinDef.MinionSkinReplacement {
                minionBodyPrefab = bodyPrefab,
                minionSkin = NodSkin,
            };

            teslaSkins[2].minionSkinReplacements = teslaSkins[2].minionSkinReplacements.Append(NodMinionSkinReplacement).ToArray();

            #endregion

            #region mince

            if (GeneralConfig.Cursed.Value)
            {
                TeslaSkinDef MCSkin = Modules.Skins.CreateSkinDef<TeslaSkinDef>(TOWER_PREFIX + "MC_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texTeslaSkinMC"),
                defaultRenderers,
                characterModelObject);

                MCSkin.ZapLightningType = ModdedLightningType.Loader;

                MCSkin.gameObjectActivations = Skins.GetGameObjectActivationsFromList(activatedGameObjects,
                    0,
                    1,
                    2,
                    //3,
                    4,
                    5,
                    6,
                    //7,
                    8,
                    9);

                MCSkin.meshReplacements = Modules.Skins.GetMeshReplacements(assetBundle, defaultRenderers,
                    "MC_Base_Pillars_Colors",
                    "MC_Base_Platform",
                    "MC_Base_Center",
                    null,//"MC_Base_Tubes",

                    "MC_Circles",
                    "MC_Pole",
                    "MC_Pole_Tracer",
                    null,//"MC_Emission",
                    "MC_Orb");
                
                MCSkin.rendererInfos[0].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerRedstone");
                MCSkin.rendererInfos[1].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerCobblestone");
                MCSkin.rendererInfos[2].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerIron");
                //MCSkin.rendererInfos[3].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerRedstone");

                MCSkin.rendererInfos[4].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerQuartz");
                MCSkin.rendererInfos[5].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerCobblestone");
                MCSkin.rendererInfos[6].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("WHITE");
                //MCSkin.rendererInfos[7].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerIron");
                MCSkin.rendererInfos[8].defaultMaterial = assetBundle.CreateHopooMaterialFromBundle("matTowerDiamond");

                skins.Add(MCSkin);
                MCMinionSkinReplacement = new SkinDef.MinionSkinReplacement
                {
                    minionBodyPrefab = bodyPrefab,
                    minionSkin = MCSkin,
                };

                teslaSkins[3].minionSkinReplacements = teslaSkins[3].minionSkinReplacements.Append(MCMinionSkinReplacement).ToArray();
            }
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}