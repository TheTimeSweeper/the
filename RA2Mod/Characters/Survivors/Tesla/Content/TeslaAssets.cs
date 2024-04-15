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
        public static GameObject TeslaCoilBlueprintGamOb;
        public static AsyncAsset<GameObject> TeslaCoilBlueprintAsync;

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

        public static List<IEnumerator> loads => ContentPacks.asyncLoadCoroutines;
        #endregion

        internal static List<IEnumerator> GetAssetBundleInitializedCoroutines(AssetBundle assetBundle)
        {
            return null;
        }

        internal static void OnCharacterInitialized(AssetBundle assetBundle)
        {
            return; 
        }
    }
}
