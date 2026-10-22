using GorpMod.Modules;
using GorpMod.Modules.Characters;
using GorpMod.Enemies.Gorp.Content;
using RoR2.CharacterAI;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using GorpMod.Characters.Enemies.Gorp.Comopnents;
using RoR2.Skills;
using GorpMod.Characters.Enemies.Gorp.States;
using RoR2BepInExPack.GameAssetPathsBetter;
using System.Linq;

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

            Log.Warning("hu");

            Asset.LoadAssetBundleAsync("boosgorpbundle", (loadedAssetBundle) =>
            {
                Log.Warning("hu2");

                assetBundle = loadedAssetBundle;
                OnBundleLoaded();
            });


            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            //int gorpHealthBuffCount = sender.GetBuffCount(CharacterBuffs.GorpHealthBuff);
            //float t = (float)gorpHealthBuffCount / CharacterConfig.HealthThresholds;
            //if (gorpHealthBuffCount > 0)
            //{
            //    args.attackSpeedMultAdd += Mathf.Lerp(CharacterConfig.ASpeed_MaxHealth, CharacterConfig.ASpeed_MinHealth, t);
            //    args.moveSpeedMultAdd += Mathf.Lerp(CharacterConfig.Speed_MaxHealth, CharacterConfig.Speed_MinHealth, t);
            //    args.damageMultAdd += Mathf.Lerp(CharacterConfig.Damage_MaxHealth, CharacterConfig.Damage_MinHealth, t);
            //    args.cooldownReductionAdd += Mathf.Lerp(CharacterConfig.CDR_MaxHealth, CharacterConfig.CDR_MinHealth, t);
            //}
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
            BodyPrefab = PrefabAPI.InstantiateClone(gupPrefab, "GorpBody", true);
            PrefabBody = BodyPrefab.GetComponent<CharacterBody>();

            PrefabBody.baseMaxHealth = 2500;
            PrefabBody.PerformAutoCalculateLevelStats();

            PrefabBody.baseNameToken = TOKEN_PREFIX + "NAME";
            PrefabBody.subtitleNameToken = TOKEN_PREFIX + "SUBTITLE";

            Modules.Config.ConfigureBody(PrefabBody, CharacterConfig.SectionBody);

            BodyPrefab.transform.GetChild(0).transform.localScale = Vector3.one * CharacterConfig.Size_MaxHealth;

            Content.AddCharacterBodyPrefab(BodyPrefab);

            BodyPrefab.transform.Find("ModelBase/mdlGup/mdlGupArmature/ROOT/mainBody.1/mainBody.2/mainBody.3/mainBody.4/Ear.L").gameObject.AddComponent<FunnyScale>();
            BodyPrefab.transform.Find("ModelBase/mdlGup/mdlGupArmature/ROOT/mainBody.1/mainBody.2/mainBody.3/mainBody.4/Ear.R").gameObject.AddComponent<FunnyScale>();
            //BodyPrefab.transform.Find("ModelBase/mdlGup/mdlGupArmature/InitialHitBox");
            BodyPrefab.GetComponentInChildren<HitBoxGroup>().hitBoxes[0] = BodyPrefab.GetComponentInChildren<HitBoxGroup>().hitBoxes[1];
            BodyPrefab.GetComponentInChildren<HitBoxGroup>().hitBoxes[1].transform.localScale = new Vector3(4, 2.5f, 1);
            BodyPrefab.GetComponentInChildren<HitBoxGroup>().hitBoxes[2].transform.localScale = new Vector3(2.5f, 4, 1);
            var muzzle = new GameObject("MortarMuzzle");
            muzzle.transform.parent = BodyPrefab.transform.Find("ModelBase/mdlGup/mdlGupArmature");
            muzzle.transform.localPosition = new Vector3(0, 0, 1.3f);

            var childLocator = BodyPrefab.GetComponentInChildren<ChildLocator>(true);
            childLocator.transformPairs = childLocator.transformPairs.Append(new ChildLocator.NameTransformPair { name = "MortarMuzzle", transform = muzzle.transform }).ToArray();

            BodyPrefab.AddComponent<TargetingAndPredictionController>();

            Asset.LoadAssetAsync<CharacterCameraParams>(RoR2_Base_Common.ccpStandardHuge_asset,
                (loaded) =>
                {
                    BodyPrefab.GetComponent<CameraTargetParams>().cameraParams = loaded;
                });

            DoSkin();

            DoSkill();
        }

        private void DoSkill()
        {
            //Asset.LoadAssetAsync<EntityStateConfiguration>(RoR2_DLC1_Gup.EntityStates_Gup_GupSpikesState_asset, 
            //    (GupSpikeStateESC) =>
            //    {
            //        var spikeESC = ScriptableObject.CreateInstance<EntityStateConfiguration>();
            //        spikeESC.serializedFieldsCollection = GupSpikeStateESC.serializedFieldsCollection;
            //        spikeESC.targetType = new HG.SerializableSystemType();
            //        spikeESC.targetType.assemblyQualifiedName = typeof(GorpSpikeAttack).AssemblyQualifiedName;

            //        Content.AddEntityStateConfiguration(spikeESC);
            //    });

            //Skills.ClearGenericSkills(BodyPrefab);

            //Skills.CreateSkillFamilies(BodyPrefab, SkillSlot.Primary/*, SkillSlot.Secondary*/);

            //SkillDef newSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            //(newSkillDef as ScriptableObject).name = "GorpAttack1";
            //newSkillDef.skillNameToken = "GorpAttack1";
            //newSkillDef.skillDescriptionToken = "";
            //newSkillDef.icon = null;
            //newSkillDef.activationStateMachineName = "Body";
            //newSkillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(GorpSpikeAttack));
            //newSkillDef.baseRechargeInterval = 1;
            //newSkillDef.baseMaxStock= 1;
            //newSkillDef.rechargeStock = 1;
            //newSkillDef.requiredStock = 1;
            //newSkillDef.stockToConsume = 1;
            //newSkillDef.attackSpeedBuffsRestockSpeed = true;
            //newSkillDef.attackSpeedBuffsRestockSpeed_Multiplier = 2;
            //newSkillDef.resetCooldownTimerOnUse = false;
            //newSkillDef.fullRestockOnAssign = true;
            //newSkillDef.dontAllowPastMaxStocks = false;
            //newSkillDef.beginSkillCooldownOnSkillEnd = true;
            //newSkillDef.isCooldownBlockedUntilManuallyReset = false;
            //newSkillDef.cancelSprintingOnActivation = true;
            //newSkillDef.forceSprintDuringState = false;
            //newSkillDef.canceledFromSprinting = false;
            //newSkillDef.isCombatSkill = true;
            //newSkillDef.mustKeyPress = false;
            //newSkillDef.triggeredByPressRelease = false;
            //newSkillDef.autoHandleLuminousShot = true;
            //newSkillDef.suppressSkillActivation = false;
            //newSkillDef.hideStockCount = false;
            //newSkillDef.hideCooldown = false;

            //Content.AddSkillDef(newSkillDef);

            //Skills.AddPrimarySkills(BodyPrefab, newSkillDef);


            Skills.CreateSkillFamilies(BodyPrefab, SkillSlot.Secondary);

            SkillDef secondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            (secondarySkillDef as ScriptableObject).name = "GorpAttack1";
            secondarySkillDef.skillNameToken = "GorpAttack1";
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

            Content.AddSkillDef(secondarySkillDef);

            Skills.AddSecondarySkills(BodyPrefab, secondarySkillDef);

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
            MasterPrefab.GetComponent<AISkillDriver>().maxDistance = 50;

            UnityEngine.Object.Destroy(MasterPrefab.GetComponents<AISkillDriver>()[1]);

            //some fields omitted that aren't commonly changed. will be set to default values
            AISkillDriver secondaryDriver = MasterPrefab.AddComponent<AISkillDriver>();
            //Selection Conditions
            secondaryDriver.customName = "Use Secondary Shoot";
            secondaryDriver.skillSlot = SkillSlot.Secondary;
            secondaryDriver.requireSkillReady = true;
            secondaryDriver.minDistance = 0;
            secondaryDriver.maxDistance = 120;
            secondaryDriver.minUserHealthFraction = 30;
            secondaryDriver.maxUserHealthFraction = float.PositiveInfinity;
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

            //some fields omitted that aren't commonly changed. will be set to default values
            AISkillDriver helpDriver = MasterPrefab.AddComponent<AISkillDriver>();
            //Selection Conditions
            helpDriver.customName = "FindGups";
            helpDriver.skillSlot = SkillSlot.None;
            helpDriver.requireSkillReady = true;
            helpDriver.minDistance = 0;
            helpDriver.maxDistance = 50;
            helpDriver.minUserHealthFraction = float.NegativeInfinity;
            helpDriver.maxUserHealthFraction = 40;
            helpDriver.selectionRequiresTargetLoS = false;
            helpDriver.selectionRequiresOnGround = false;
            helpDriver.selectionRequiresAimTarget = false;
            helpDriver.maxTimesSelected = 5;

            //Behavior
            helpDriver.moveTargetType = AISkillDriver.TargetType.NearestFriendlyInSkillRange;
            helpDriver.activationRequiresTargetLoS = false;
            helpDriver.activationRequiresAimTargetLoS = false;
            helpDriver.activationRequiresAimConfirmation = false;
            helpDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            helpDriver.moveInputScale = 1;
            helpDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            helpDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            helpDriver.noRepeat = true;
            helpDriver.shouldSprint = true;
            helpDriver.driverUpdateTimerOverride = 2;

            //some fields omitted that aren't commonly changed. will be set to default values
            AISkillDriver chaseLowHealthDriver = MasterPrefab.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaseLowHealthDriver.customName = "chaseLowHealth";
            chaseLowHealthDriver.skillSlot = SkillSlot.None;
            chaseLowHealthDriver.requireSkillReady = true;
            chaseLowHealthDriver.minDistance = 0;
            chaseLowHealthDriver.maxDistance = float.PositiveInfinity;
            chaseLowHealthDriver.minUserHealthFraction = float.NegativeInfinity;
            chaseLowHealthDriver.maxUserHealthFraction = 40;
            chaseLowHealthDriver.selectionRequiresTargetLoS = false;
            chaseLowHealthDriver.selectionRequiresOnGround = false;
            chaseLowHealthDriver.selectionRequiresAimTarget = false;
            chaseLowHealthDriver.maxTimesSelected = -1;

            //Behavior
            chaseLowHealthDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseLowHealthDriver.activationRequiresTargetLoS = false;
            chaseLowHealthDriver.activationRequiresAimTargetLoS = false;
            chaseLowHealthDriver.activationRequiresAimConfirmation = false;
            chaseLowHealthDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaseLowHealthDriver.moveInputScale = 1;
            chaseLowHealthDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaseLowHealthDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            chaseLowHealthDriver.shouldSprint = true;

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
            SpawnCard.directorCreditCost = 100;
            SpawnCard.forbiddenFlags |= RoR2.Navigation.NodeFlags.NoCharacterSpawn;
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
    }
}

