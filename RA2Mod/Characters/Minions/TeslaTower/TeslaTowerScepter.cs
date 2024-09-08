using RoR2;
using System;
using R2API;
using UnityEngine;
using RA2Mod.Modules.Characters;
using RA2Mod.Survivors.Tesla;
using RA2Mod.Modules;
using RA2Mod.Minions.TeslaTower.States;
using System.Linq;
using RA2Mod.General;

namespace RA2Mod.Minions.TeslaTower
{
    internal class TeslaTowerScepter : CharacterBase<TeslaTowerScepter>
    {
        public override string bodyName => "TeslaTowerScepter";

        public const string TOWER_PREFIX = TeslaTowerNotSurvivor.TOWER_PREFIX;
        public const string TOWER_SCEPTER_PREFIX = RA2Plugin.DEVELOPER_PREFIX + "_TESLA_TOWER_SCEPTER_";

        public override BodyInfo bodyInfo { get; }

        public override ItemDisplaysBase itemDisplays => baseTower.itemDisplays;

        public override string assetBundleName => "teslatrooper";

        public override string modelPrefabName => "racial slur of your choice";

        public static GameObject masterPrefab;
        public TeslaTowerNotSurvivor baseTower => TeslaTowerNotSurvivor.instance;

        internal static SkinDef.MinionSkinReplacement MasteryMinionSkinReplacement;
        internal static SkinDef.MinionSkinReplacement MCMinionSkinReplacement;
        internal static SkinDef.MinionSkinReplacement NodMinionSkinReplacement;

        public override void OnCharacterInitialized() { }

        public override void InitializeEntityStateMachines() { }

        public override void InitializeSkills() { }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            StealCharacter();
            InitializeCharacterMaster();

            //todo scepter: skills when base tower gets ai for their skills
            //InitializeSkills();

            InitializeSkins();
            InitializeItemDisplays();
        }

        private void StealCharacter()
        {
            bodyPrefab = baseTower.bodyPrefab.InstantiateClone("TeslaTowerScepterBody");
            CharacterBody characterBody = bodyPrefab.GetComponent<CharacterBody>();
            characterBody.baseNameToken = TOWER_SCEPTER_PREFIX + "NAME";
            characterBody.portraitIcon = assetBundle.LoadAsset<Texture>("texIconTeslaTower");
            EntityStateMachine bodyMachine = EntityStateMachine.FindByCustomName(bodyPrefab, "Body");
            bodyMachine.initialStateType = new EntityStates.SerializableEntityStateType(typeof(TowerSpawnStateScepter));
            bodyMachine.mainStateType = new EntityStates.SerializableEntityStateType(typeof(TowerIdleSearchScepter));

            Modules.Content.AddCharacterBodyPrefab(bodyPrefab);

            prefabCharacterModel = bodyPrefab.GetComponentInChildren<CharacterModel>();
            characterModelObject = prefabCharacterModel.gameObject;
        }

        public override void InitializeCharacterMaster()
        {
            StealCharacterMaster();
        }

        private void StealCharacterMaster()
        {
            masterPrefab = TeslaTowerNotSurvivor.masterPrefab.InstantiateClone("TeslaTowerScepterMaster");
            masterPrefab.GetComponent<CharacterMaster>().bodyPrefab = bodyPrefab;

            Modules.Content.AddMasterPrefab(masterPrefab);
        }

        public override void InitializeSkins()
        {

            ModelSkinController skinController = characterModelObject.GetComponent<ModelSkinController>();

            SkinDef[] teslaSkins = TeslaTrooperSurvivor.instance.characterModelObject.GetComponent<ModelSkinController>().skins;

            #region Default
            TeslaSkinDef scepterDefault = Skins.DuplicateScepterSkinDef(skinController.skins[0] as TeslaSkinDef);

            scepterDefault.meshReplacements = Modules.Skins.GetMeshReplacements(assetBundle, scepterDefault.rendererInfos,
                "Tower_Base_Pillars_Color_Scepter",
                null,//"Mastery_Base_Platform",
                null,//"Mastery_Base_Center",
                null,//"Mastery_Base_Tubes",

                "Tower_Circles_Scepter",
                null,//"Mastery_Pole",
                null,//"Mastery_Pole_Tracer",
                null,//"Mastery_Emission",
                null);//"Mastery_Orb");

            skinController.skins[0] = scepterDefault;
            #endregion Default

            #region Mastery
            TeslaSkinDef scepterMastery = Skins.DuplicateScepterSkinDef(skinController.skins[1] as TeslaSkinDef);

            scepterMastery.meshReplacements = Modules.Skins.GetMeshReplacements(assetBundle, scepterMastery.rendererInfos,
                "Mastery_Base_Pillars_Color_Scepter",
                null,//"Mastery_Base_Platform",
                "Mastery_Base_Center_Scepter",
                null,//"Mastery_Base_Tubes",

                "Mastery_Circles_Scepter",
                null,//"Mastery_Pole",
                null,//"Mastery_Pole_Tracer",
                null,//"Mastery_Emission",
                null);//"Mastery_Orb");

            skinController.skins[1] = scepterMastery;

            MasteryMinionSkinReplacement = new SkinDef.MinionSkinReplacement
            {
                minionBodyPrefab = bodyPrefab,
                minionSkin = scepterMastery,
            };

            teslaSkins[1].minionSkinReplacements = teslaSkins[1].minionSkinReplacements.Append(MasteryMinionSkinReplacement).ToArray();

            #endregion Mastery

            #region Nod
            TeslaSkinDef scepterNod = Skins.DuplicateScepterSkinDef(skinController.skins[2] as TeslaSkinDef);
            scepterNod.ZapLightningType = (skinController.skins[2] as TeslaSkinDef).ZapLightningType;

            scepterNod.meshReplacements = Modules.Skins.GetMeshReplacements(assetBundle, scepterNod.rendererInfos,
                null,//"Nod_Base_Pillars_Color_Scepter",
                null,//"Nod_Base_Platform",
                "Nod_Tower_Scepter",//"Nod_Base_Center_Scepter",
                null,//"Nod_Base_Tubes",

                null,//"Nod_Circles_Scepter",
                null,//"Nod_Pole",
                null,//"Nod_Pole_Tracer",
                "Nod_Emission_Scepter",
                null);//"Nod_Orb");


            skinController.skins[2] = scepterNod;

            NodMinionSkinReplacement = new SkinDef.MinionSkinReplacement
            {
                minionBodyPrefab = bodyPrefab,
                minionSkin = scepterNod,
            };

            teslaSkins[2].minionSkinReplacements = teslaSkins[2].minionSkinReplacements.Append(NodMinionSkinReplacement).ToArray();

            #endregion Nod

            #region MC

            if (GeneralConfig.Cursed.Value)
            {
                TeslaSkinDef scepterMC = Skins.DuplicateScepterSkinDef(skinController.skins[3] as TeslaSkinDef);

                scepterMC.meshReplacements = Modules.Skins.GetMeshReplacements(assetBundle, scepterMC.rendererInfos,
                    null,//"MC_Base_Pillars_Color_Scepter",
                    null,//"MC_Base_Platform",
                    null,//"MC_Tower_Scepter",//"MC_Base_Center_Scepter",
                    null,//"MC_Base_Tubes",

                    "MC_Circles_Scepter",
                    null,//"MC_Pole",
                    null,//"MC_Pole_Tracer",
                    null,//"MC_Emission_Scepter",
                    "MC_Orb_Scepter");


                skinController.skins[3] = scepterMC;

                MCMinionSkinReplacement = new SkinDef.MinionSkinReplacement
                {
                    minionBodyPrefab = bodyPrefab,
                    minionSkin = scepterMC,
                };

                teslaSkins[3].minionSkinReplacements = teslaSkins[3].minionSkinReplacements.Append(MCMinionSkinReplacement).ToArray();
            }
            #endregion MC
        }
    }
}