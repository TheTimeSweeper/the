using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RA2Mod.Modules;

namespace RA2Mod.Survivors.Tesla 
{ 
    public class TeslaAssets
    {
        #region tesla
        public static GameObject TeslaCoil;
        public static GameObject TeslaCoilBlueprint;
        public static AsyncAsset<GameObject> FullCirclePrefab;

        public static GameObject TeslaIndicatorPrefab;

        public static GameObject TeslaIndicatorPrefabDash;

        public static GameObject TeslaLoaderZapConeProjectile;
        public static GameObject TeslaZapConeEffect;

        public static GameObject TeslaLightningOrbEffectRed;
        public static GameObject TeslaMageLightningOrbEffectRed;
        public static GameObject TeslaMageLightningOrbEffectRedThick;

        public static Material ChainLightningMaterial;

        public static Sprite[] rangeSprites = null;// new Sprite[] { Modules.Assets.LoadAsset<Sprite>("texIndicator1Close"),
                                                         //Modules.Assets.LoadAsset<Sprite>("texIndicator2Med"),
                                                        // Modules.Assets.LoadAsset<Sprite>("texIndicator3Far") };
        public static Sprite allySprite = null;// Modules.Assets.LoadAsset<Sprite>("texIndicatorAlly");
        public static Sprite towerSprite = null;// Modules.Assets.LoadAsset<Sprite>("texIndicatorTowerIcon");

        #endregion

        internal static List<IEnumerator> GetAssetBundleInitializedCoroutines(AssetBundle assetBundle)
        {
            return null;
        }

        internal static void OnCharacterInitialized(AssetBundle assetBundle)
        {
            //new AsyncAsset<GameObject>(assetBundle, "TeslaCoilBlueprint", (result) => {
            //    TeslaCoilBlueprint = result;
            //});
            TeslaCoilBlueprint = new AsyncAsset<GameObject>(assetBundle, "TeslaCoilBlueprint");

            assetBundle.LoadAssetAsync<GameObject>("TeslaCoilBlueprint", (result) => {
                TeslaCoilBlueprint = result;
            });

            ContentPacks.asyncLoadCoroutines.Add(assetBundle.LoadBundleAssetCoroutine<GameObject>("TeslaCoilBlueprint", (result) => {
                TeslaCoilBlueprint = result;
            }));

            FullCirclePrefab = assetBundle.AsyncLoadAsset<GameObject>("TeslaCoilBlueprint");
            GameObject gob = FullCirclePrefab;


            ContentPacks.asyncLoadCoroutines.Add(Assets.LoadAssetsAsyncCoroutine(
                new AsyncAsset<GameObject>(assetBundle, "nig1"),
                new AsyncAsset<Material>(assetBundle, "nig2"),
                (nig1, nig2) =>
                {
                    nig1.GetComponent<Renderer>().sharedMaterial = nig2;
                }));
        }
    }
}
