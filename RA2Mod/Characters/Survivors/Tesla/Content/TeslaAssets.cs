using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RA2Mod.Modules;

namespace RA2Mod.Survivors.Tesla

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
        #endregion

        internal static List<IEnumerator> GetAssetBundleInitializedCoroutines(AssetBundle assetBundle)
        {
            throw new NotImplementedException();
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
                };
        }
    }
}
