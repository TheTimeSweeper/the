using GorpMod.Modules;
using GorpMod.Modules.Characters;
using GorpMod.Survivors.Gorp.Content;
using RoR2.CharacterAI;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using GorpMod.Characters.Enemies.Gorp.Comopnents;

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

            BodyPrefab.AddComponent<GorpController>();

            DoSkin();
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
            MasterPrefab.GetComponent<AISkillDriver>().maxDistance = 40;

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

