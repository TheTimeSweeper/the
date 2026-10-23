using BepInEx.Configuration;
using GorpMod.Characters.Enemies.Gorp.Comopnents;
using GorpMod.Characters.Enemies.Gorp.States;
using GorpMod.Enemies.Gorp.Content;
using GorpMod.Modules;
using GorpMod.Modules.Characters;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Navigation;
using RoR2.Skills;
using RoR2BepInExPack.GameAssetPathsBetter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static GorpMod.Enemies.Gorp.Content.CharacterConfig;

namespace GorpMod.Characters.Enemies.Gorp
{
    internal class GorpCharacter
    {
        public AssetBundle assetBundle;

        public GameObject BodyPrefab;
        public CharacterBody PrefabBody;

        public GameObject MasterPrefab;

        public static GameObject BossEncounter;
        public static SpawnCard SpawnCard;

        public const string TOKEN_PREFIX = GorpPlugin.DEVELOPER_PREFIX + "_GORP_";

        public void Initialize()
        {
            new CharacterConfig().Init();
            CharacterTokens.Init();
            CharacterAssets.Init();

            Asset.LoadAssetBundleAsync("boosgorpbundle", (loadedAssetBundle) =>
            {
                assetBundle = loadedAssetBundle;
                OnBundleLoaded();
            });
        }

        private void OnBundleLoaded()
        {
            CharacterBuffs.Init(assetBundle);

            Asset.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GupBody.prefab", (gupPrefab) =>
            {
                OnGupLoaded(gupPrefab);

                Asset.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GupMaster.prefab", (gupMaster) =>
                {
                    OnGupMasterLoaded(gupMaster);
                    CreateSpawnCard();
                    CreateBossEncounter();
                });
            });
        }

        private void OnGupLoaded(GameObject gupPrefab)
        {
            //create
            BodyPrefab = PrefabAPI.InstantiateClone(gupPrefab, "GorpBody", true);
            PrefabBody = BodyPrefab.GetComponent<CharacterBody>();

            //CharacterBody stuff
            PrefabBody.baseNameToken = TOKEN_PREFIX + "NAME";
            PrefabBody.subtitleNameToken = TOKEN_PREFIX + "SUBTITLE";
            PrefabBody.portraitIcon = assetBundle.LoadAsset<Texture>("texIconGorp");

            PrefabBody.baseMaxHealth = 2500;
            PrefabBody.baseMoveSpeed = 8;
            PrefabBody.PerformAutoCalculateLevelStats();

            PrefabBody.isChampion = true;
            PrefabBody.hullClassification = HullClassification.BeetleQueen;

            Modules.Config.ConfigureBody(PrefabBody, CharacterConfig.SectionBody);

            //Body Prefab stuff
            BodyPrefab.AddComponent<GorpController>();
            BodyPrefab.AddComponent<TargetingAndPredictionController>();
            BodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(GorpSplitDeath));
            BodyPrefab.GetComponent<SetStateOnHurt>().canBeHitStunned = false;
            Asset.LoadAssetAsync<CharacterCameraParams>(RoR2_Base_Common.ccpStandardHuge_asset,
                (loaded) =>
                {
                    BodyPrefab.GetComponent<CameraTargetParams>().cameraParams = loaded;
                });

            //Model stuff
            //scale handled by gorpcontroller
            BodyPrefab.transform.Find("ModelBase/mdlGup").localScale = Vector3.one;
            //BodyPrefab.transform.Find("ModelBase/mdlGup/mdlGupArmature").localScale = Vector3.one;;

            BodyPrefab.transform.Find("ModelBase/mdlGup/mdlGupArmature/ROOT/mainBody.1/mainBody.2/mainBody.3/mainBody.4/Ear.L").gameObject.AddComponent<FunnyScale>();
            BodyPrefab.transform.Find("ModelBase/mdlGup/mdlGupArmature/ROOT/mainBody.1/mainBody.2/mainBody.3/mainBody.4/Ear.R").gameObject.AddComponent<FunnyScale>();
            var standableSurface = BodyPrefab.transform.Find("ModelBase/mdlGup/mdlGupArmature/ROOT/mainBody.1/StandableSurfacePosition");
            standableSurface.transform.localScale = new Vector3(0.8742901f, 0.8742901f, 0.5442675f);
            standableSurface.transform.localPosition = new Vector3(0, 0.54f, 0);
            //BodyPrefab.transform.Find("ModelBase/mdlGup/mdlGupArmature/InitialHitBox");
            BodyPrefab.GetComponentInChildren<HitBoxGroup>().hitBoxes[0] = BodyPrefab.GetComponentInChildren<HitBoxGroup>().hitBoxes[1];
            BodyPrefab.GetComponentInChildren<HitBoxGroup>().hitBoxes[1].transform.localScale = new Vector3(4, 2.5f, 1.5f);
            BodyPrefab.GetComponentInChildren<HitBoxGroup>().hitBoxes[2].transform.localScale = new Vector3(2.5f, 4, 1.5f);
            var muzzle = new GameObject("MortarMuzzle");
            muzzle.transform.parent = BodyPrefab.transform.Find("ModelBase/mdlGup/mdlGupArmature");
            muzzle.transform.localPosition = new Vector3(0, 0, 1.3f);

            var childLocator = BodyPrefab.GetComponentInChildren<ChildLocator>(true);
            childLocator.transformPairs = childLocator.transformPairs.Append(new ChildLocator.NameTransformPair { name = "MortarMuzzle", transform = muzzle.transform }).ToArray();
            
            //add
            Content.AddCharacterBodyPrefab(BodyPrefab);

            //mod stuff
            DoHooks();

            DoSkin();

            DoSkill();
        }

        private void DoHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients1;
        }

        private void RecalculateStatsAPI_GetStatCoefficients1(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            int thresholdBuffs = sender.GetBuffCount(CharacterBuffs.GorpThresholdBuff);
            float t = (float)thresholdBuffs / CharacterConfig.HealthThresholds;
            if (thresholdBuffs > 0)
            {
                args.baseMoveSpeedAdd += Mathf.Lerp(CharacterConfig.AdditionalMoveSpeed_MinHealth, 0, t);
                //args.attackSpeedMultAdd += Mathf.Lerp(CharacterConfig.ASpeed_MaxHealth, CharacterConfig.ASpeed_MinHealth, t);
                //args.moveSpeedMultAdd += Mathf.Lerp(CharacterConfig.Speed_MaxHealth, CharacterConfig.Speed_MinHealth, t);
                //args.damageMultAdd += Mathf.Lerp(CharacterConfig.Damage_MaxHealth, CharacterConfig.Damage_MinHealth, t);
                //args.cooldownReductionAdd += Mathf.Lerp(CharacterConfig.CDR_MaxHealth, CharacterConfig.CDR_MinHealth, t);
            }
        }

        private void DoSkill()
        {
            Content.AddEntityState(typeof(GorpSpikeAttack));
            Content.AddEntityState(typeof(GorpBoundAttack));
            Content.AddEntityState(typeof(GorpMortarAttack));
            Content.AddEntityState(typeof(GorpSplitDeath));

            Asset.LoadAssetAsync<EntityStateConfiguration>(RoR2_DLC1_Gup.EntityStates_Gup_GupSpikesState_asset,
                (GupSpikeStateESC) =>
                {
                    var spikeESC = assetBundle.LoadAsset<EntityStateConfiguration>("escGorpSpike");
                    spikeESC.serializedFieldsCollection = GupSpikeStateESC.serializedFieldsCollection;
                    spikeESC.targetType.assemblyQualifiedName = typeof(GorpSpikeAttack).AssemblyQualifiedName;

                    for (int i = 0; i < spikeESC.serializedFieldsCollection.serializedFields.Length; i++)
                    {
                        if (spikeESC.serializedFieldsCollection.serializedFields[i].fieldName == "damageCoefficient")
                        {
                            spikeESC.serializedFieldsCollection.serializedFields[i].fieldValue.stringValue = CharacterConfig.SpikeStateDamage.Value.ToString();
                        }
                    }

                    Content.AddEntityStateConfiguration(spikeESC);
                });

            Skills.ClearGenericSkills(BodyPrefab);

            //primary
            Skills.CreateSkillFamilies(BodyPrefab, SkillSlot.Primary/*, SkillSlot.Secondary*/);

            SkillDef newSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            (newSkillDef as ScriptableObject).name = "GorpAttack1";
            newSkillDef.skillNameToken = "GorpAttack1";
            newSkillDef.skillDescriptionToken = "";
            newSkillDef.icon = null;
            newSkillDef.activationStateMachineName = "Body";
            newSkillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(GorpSpikeAttack));
            newSkillDef.baseRechargeInterval = 5;
            newSkillDef.baseMaxStock = 1;
            newSkillDef.rechargeStock = 1;
            newSkillDef.requiredStock = 1;
            newSkillDef.stockToConsume = 1;
            newSkillDef.attackSpeedBuffsRestockSpeed = true;
            newSkillDef.attackSpeedBuffsRestockSpeed_Multiplier = 2;
            newSkillDef.resetCooldownTimerOnUse = false;
            newSkillDef.fullRestockOnAssign = true;
            newSkillDef.dontAllowPastMaxStocks = false;
            newSkillDef.beginSkillCooldownOnSkillEnd = true;
            newSkillDef.isCooldownBlockedUntilManuallyReset = false;
            newSkillDef.cancelSprintingOnActivation = true;
            newSkillDef.forceSprintDuringState = false;
            newSkillDef.canceledFromSprinting = false;
            newSkillDef.isCombatSkill = true;
            newSkillDef.mustKeyPress = false;
            newSkillDef.triggeredByPressRelease = false;
            newSkillDef.autoHandleLuminousShot = true;
            newSkillDef.suppressSkillActivation = false;
            newSkillDef.hideStockCount = false;
            newSkillDef.hideCooldown = false;

            Config.ConfigureSkillDef(newSkillDef, SectionSkills, "Spike Attack");

            Content.AddSkillDef(newSkillDef);

            Skills.AddPrimarySkills(BodyPrefab, newSkillDef);

            //secondary
            Skills.CreateSkillFamilies(BodyPrefab, SkillSlot.Secondary);

            SkillDef secondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            (secondarySkillDef as ScriptableObject).name = "GorpAttack2";
            secondarySkillDef.skillNameToken = "GorpAttack2";
            secondarySkillDef.skillDescriptionToken = "";
            secondarySkillDef.icon = null;
            secondarySkillDef.activationStateMachineName = "Body";
            secondarySkillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(GorpMortarAttack));
            secondarySkillDef.baseRechargeInterval = 20;
            secondarySkillDef.baseMaxStock = 1;
            secondarySkillDef.rechargeStock = 1;
            secondarySkillDef.requiredStock = 1;
            secondarySkillDef.stockToConsume = 1;
            secondarySkillDef.attackSpeedBuffsRestockSpeed = true;
            secondarySkillDef.attackSpeedBuffsRestockSpeed_Multiplier = 2;
            secondarySkillDef.resetCooldownTimerOnUse = false;
            secondarySkillDef.fullRestockOnAssign = true;
            secondarySkillDef.dontAllowPastMaxStocks = false;
            secondarySkillDef.beginSkillCooldownOnSkillEnd = true;
            secondarySkillDef.isCooldownBlockedUntilManuallyReset = false;
            secondarySkillDef.cancelSprintingOnActivation = true;
            secondarySkillDef.forceSprintDuringState = false;
            secondarySkillDef.canceledFromSprinting = false;
            secondarySkillDef.isCombatSkill = true;
            secondarySkillDef.mustKeyPress = false;
            secondarySkillDef.triggeredByPressRelease = false;
            secondarySkillDef.autoHandleLuminousShot = true;
            secondarySkillDef.suppressSkillActivation = false;
            secondarySkillDef.hideStockCount = false;
            secondarySkillDef.hideCooldown = false;

            Config.ConfigureSkillDef(secondarySkillDef, SectionSkills, "Mortar Skill");

            Content.AddSkillDef(secondarySkillDef);

            Skills.AddSecondarySkills(BodyPrefab, secondarySkillDef);

            //utility
            Skills.CreateSkillFamilies(BodyPrefab, SkillSlot.Utility);

            SkillDef utilitySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            (utilitySkillDef as ScriptableObject).name = "GorpAttack3";
            utilitySkillDef.skillNameToken = "GorpAttack3";
            utilitySkillDef.skillDescriptionToken = "";
            utilitySkillDef.icon = null;
            utilitySkillDef.activationStateMachineName = "Body";
            utilitySkillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(GorpBoundAttack));
            utilitySkillDef.baseRechargeInterval = 10;
            utilitySkillDef.baseMaxStock = 1;
            utilitySkillDef.rechargeStock = 1;
            utilitySkillDef.requiredStock = 1;
            utilitySkillDef.stockToConsume = 1;
            utilitySkillDef.attackSpeedBuffsRestockSpeed = true;
            utilitySkillDef.attackSpeedBuffsRestockSpeed_Multiplier = 2;
            utilitySkillDef.resetCooldownTimerOnUse = false;
            utilitySkillDef.fullRestockOnAssign = true;
            utilitySkillDef.dontAllowPastMaxStocks = false;
            utilitySkillDef.beginSkillCooldownOnSkillEnd = true;
            utilitySkillDef.isCooldownBlockedUntilManuallyReset = false;
            utilitySkillDef.cancelSprintingOnActivation = true;
            utilitySkillDef.forceSprintDuringState = false;
            utilitySkillDef.canceledFromSprinting = false;
            utilitySkillDef.isCombatSkill = true;
            utilitySkillDef.mustKeyPress = false;
            utilitySkillDef.triggeredByPressRelease = false;
            utilitySkillDef.autoHandleLuminousShot = true;
            utilitySkillDef.suppressSkillActivation = false;
            utilitySkillDef.hideStockCount = false;
            utilitySkillDef.hideCooldown = false;

            Config.ConfigureSkillDef(utilitySkillDef, SectionDebug, "Bound Skill");

            Content.AddSkillDef(utilitySkillDef);

            Skills.AddUtilitySkills(BodyPrefab, utilitySkillDef);

        }

        private void DoSkin()
        {

            ModelSkinController modelSkinController = BodyPrefab.GetComponentInChildren<ModelSkinController>();
            var origSkin = modelSkinController.skins[0];

            Asset.LoadAssetAsync<SkinDefParams>(origSkin.skinDefParamsAddress.AssetGUID, (origSkindefParams) =>
            {
                modelSkinController.skins[0] = UnityEngine.Object.Instantiate(origSkin);
                modelSkinController.skins[0].skinDefParamsAddress = new AssetReferenceT<SkinDefParams>("");
                modelSkinController.skins[0].skinDefParams = UnityEngine.Object.Instantiate(origSkindefParams);

                Asset.LoadAssetAsync<Material>(modelSkinController.skins[0].skinDefParams.rendererInfos[0].defaultMaterialAddress.AssetGUID, (origMat) =>
                {
                    modelSkinController.skins[0].skinDefParams.rendererInfos[0].defaultMaterial = new Material(origMat);
                    modelSkinController.skins[0].skinDefParams.rendererInfos[0].defaultMaterial.SetTexture("_MainTex", assetBundle.LoadAsset<Texture2D>("texGorp"));
                    modelSkinController.skins[0].skinDefParams.rendererInfos[0].defaultMaterialAddress = new AssetReferenceT<Material>("");
                    modelSkinController.skins[0].skinDefParams.rendererInfos[1].defaultMaterial = modelSkinController.skins[0].skinDefParams.rendererInfos[0].defaultMaterial;
                    modelSkinController.skins[0].skinDefParams.rendererInfos[1].defaultMaterialAddress = new AssetReferenceT<Material>("");
                });
            });

        }

        private void OnGupMasterLoaded(GameObject gupMaster)
        {
            MasterPrefab = PrefabAPI.InstantiateClone(gupMaster, "GorpMaster", true);
            MasterPrefab.GetComponent<CharacterMaster>().bodyPrefab = BodyPrefab;
            MasterPrefab.GetComponent<AISkillDriver>().minDistance = 0;
            MasterPrefab.GetComponent<AISkillDriver>().maxDistance = 30;

            UnityEngine.Object.Destroy(MasterPrefab.GetComponents<AISkillDriver>()[1]);

            //some fields omitted that aren't commonly changed. will be set to default values
            AISkillDriver secondaryDriver = MasterPrefab.AddComponent<AISkillDriver>();
            //Selection Conditions
            secondaryDriver.customName = "Use Secondary Shoot";
            secondaryDriver.skillSlot = SkillSlot.Secondary;
            secondaryDriver.requireSkillReady = true;
            secondaryDriver.minDistance = 0;
            secondaryDriver.maxDistance = 160;
            secondaryDriver.minUserHealthFraction = 0.30f;
            secondaryDriver.maxUserHealthFraction = 0.9f;
            secondaryDriver.selectionRequiresTargetLoS = false;
            secondaryDriver.selectionRequiresOnGround = false;
            secondaryDriver.selectionRequiresAimTarget = true;
            secondaryDriver.maxTimesSelected = -1;

            //Behavior
            secondaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            secondaryDriver.activationRequiresTargetLoS = false;
            secondaryDriver.activationRequiresAimTargetLoS = false;
            secondaryDriver.activationRequiresAimConfirmation = true;
            secondaryDriver.movementType = AISkillDriver.MovementType.Stop;
            secondaryDriver.moveInputScale = 1;
            secondaryDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            secondaryDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //eh
            ////some fields omitted that aren't commonly changed. will be set to default values
            //AISkillDriver helpDriver = MasterPrefab.AddComponent<AISkillDriver>();
            ////Selection Conditions
            //helpDriver.customName = "FindGups";
            //helpDriver.skillSlot = SkillSlot.None;
            //helpDriver.requireSkillReady = true;
            //helpDriver.minDistance = 0;
            //helpDriver.maxDistance = 50;
            //helpDriver.minUserHealthFraction = float.NegativeInfinity;
            //helpDriver.maxUserHealthFraction = 0.40f;
            //helpDriver.selectionRequiresTargetLoS = false;
            //helpDriver.selectionRequiresOnGround = false;
            //helpDriver.selectionRequiresAimTarget = false;
            //helpDriver.maxTimesSelected = 5;

            ////Behavior
            //helpDriver.moveTargetType = AISkillDriver.TargetType.NearestFriendlyInSkillRange;
            //helpDriver.activationRequiresTargetLoS = false;
            //helpDriver.activationRequiresAimTargetLoS = false;
            //helpDriver.activationRequiresAimConfirmation = false;
            //helpDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            //helpDriver.moveInputScale = 1;
            //helpDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            //helpDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //helpDriver.noRepeat = true;
            //helpDriver.shouldSprint = true;
            //helpDriver.driverUpdateTimerOverride = 2;

            //he's getting move speed in a more tunable way. doesn't need sprint lol
            ////some fields omitted that aren't commonly changed. will be set to default values
            //AISkillDriver chaseLowHealthDriver = MasterPrefab.AddComponent<AISkillDriver>();
            ////Selection Conditions
            //chaseLowHealthDriver.customName = "chaseLowHealth";
            //chaseLowHealthDriver.skillSlot = SkillSlot.None;
            //chaseLowHealthDriver.requireSkillReady = true;
            //chaseLowHealthDriver.minDistance = 0;
            //chaseLowHealthDriver.maxDistance = float.PositiveInfinity;
            //chaseLowHealthDriver.minUserHealthFraction = float.NegativeInfinity;
            //chaseLowHealthDriver.maxUserHealthFraction = 0.40f;
            //chaseLowHealthDriver.selectionRequiresTargetLoS = false;
            //chaseLowHealthDriver.selectionRequiresOnGround = false;
            //chaseLowHealthDriver.selectionRequiresAimTarget = false;
            //chaseLowHealthDriver.maxTimesSelected = -1;

            ////Behavior
            //chaseLowHealthDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            //chaseLowHealthDriver.activationRequiresTargetLoS = false;
            //chaseLowHealthDriver.activationRequiresAimTargetLoS = false;
            //chaseLowHealthDriver.activationRequiresAimConfirmation = false;
            //chaseLowHealthDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            //chaseLowHealthDriver.moveInputScale = 1;
            //chaseLowHealthDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            //chaseLowHealthDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //chaseLowHealthDriver.shouldSprint = true;

            //some fields omitted that aren't commonly changed. will be set to default values
            AISkillDriver chaseDriver = MasterPrefab.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaseDriver.customName = "Chase";
            chaseDriver.skillSlot = SkillSlot.None;
            chaseDriver.requireSkillReady = true;
            chaseDriver.minDistance = 0;
            chaseDriver.maxDistance = float.PositiveInfinity;
            chaseDriver.selectionRequiresTargetLoS = false;
            chaseDriver.selectionRequiresOnGround = false;
            chaseDriver.selectionRequiresAimTarget = false;
            chaseDriver.maxTimesSelected = -1;

            //Behavior
            chaseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseDriver.activationRequiresTargetLoS = false;
            chaseDriver.activationRequiresAimTargetLoS = false;
            chaseDriver.activationRequiresAimConfirmation = false;
            chaseDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaseDriver.moveInputScale = 1;
            chaseDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaseDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;


            Content.AddMasterPrefab(MasterPrefab);
        }

        private void CreateSpawnCard()
        {
            SpawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            SpawnCard.name = "cscGorp";
            SpawnCard.prefab = MasterPrefab;
            SpawnCard.sendOverNetwork = true;
            SpawnCard.hullSize = HullClassification.BeetleQueen;
            SpawnCard.directorCreditCost = CharacterConfig.spawnCost;
            SpawnCard.forbiddenFlags |= RoR2.Navigation.NodeFlags.NoCharacterSpawn;

            DirectorCard card = new DirectorCard
            {
                spawnCard = SpawnCard,
                selectionWeight = 1,
                preventOverhead = false,
                minimumStageCompletions = CharacterConfig.minimumStageCount,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard
            };                             //can you tell this was a copy paste job
            DirectorAPI.DirectorCardHolder regigigasCard = new DirectorAPI.DirectorCardHolder
            {
                Card = card,
                MonsterCategory = DirectorAPI.MonsterCategory.Champions,
            };

            DirectorCard cardLoop = new DirectorCard
            {
                spawnCard = SpawnCard,
                selectionWeight = 1,
                preventOverhead = false,
                minimumStageCompletions = 5,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard
            };
            DirectorAPI.DirectorCardHolder regigigasCardLoop = new DirectorAPI.DirectorCardHolder
            {
                Card = cardLoop,
                MonsterCategory = DirectorAPI.MonsterCategory.Champions,
            };

            DirectorCardCategorySelection dissonanceSpawns = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/MixEnemy/dccsMixEnemy.asset").WaitForCompletion();
            dissonanceSpawns.AddCard(0, card);  //0 is Champions

            foreach (StageSpawnInfo spawnInfo in CharacterConfig.StageList)
            {
                DirectorAPI.DirectorCardHolder toAdd = spawnInfo.GetMinStages() == 0 ? regigigasCard : regigigasCardLoop;
                Log.Warning("adding to stage " + DirectorAPI.ParseInternalStageName(spawnInfo.GetStageName()));
                DirectorAPI.Helpers.AddNewMonsterToStage(toAdd, false, DirectorAPI.ParseInternalStageName(spawnInfo.GetStageName()), spawnInfo.GetStageName());
            }
        }

        private void CreateBossEncounter()
        {
            BossEncounter = assetBundle.LoadAsset<GameObject>("GorpBossEncounter");
            PrefabAPI.RegisterNetworkPrefab(BossEncounter);
            BossEncounter.GetComponent<ScriptedCombatEncounter>().spawns = new ScriptedCombatEncounter.SpawnInfo[]
            {
                new ScriptedCombatEncounter.SpawnInfo
                {
                    spawnCard = SpawnCard,
                    cullChance = 0
                }
            };
            Content.AddNetworkedObject(BossEncounter);
        }

        public static List<string> gupNames = new List<string>
        {
            "GorpBody",
            "GupBody",
            "GeepBody",
            "GipBody",
        };

        public static bool IsGup(CharacterBody characterBody)
        {
            for (int i = 0; i < gupNames.Count; i++)
            {
                if (characterBody.bodyIndex == BodyCatalog.FindBodyIndex(gupNames[i]))
                    return true;
            }
            return false;
        }
    }
}

