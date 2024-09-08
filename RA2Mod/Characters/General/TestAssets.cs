using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RA2Mod.Modules;

namespace RA2Mod.General 
{ 
    public class TestAssets
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
            #region just loading one asset
            //manually adding to coroutines, requires oncomplete
            loads.Add(assetBundle.LoadAssetCoroutine<GameObject>("TeslaCoilBlueprint", (result) => TeslaCoilBlueprintGamOb = result));
            loads.Add(Asset.LoadAssetCoroutine<GameObject>(assetBundle, "TeslaCoilBlueprint", (result) => TeslaCoilBlueprintGamOb = result));
            loads.Add(Asset.LoadAssetCoroutine<GameObject>("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab", (result) => TeslaCoilBlueprintGamOb = result)); 

            //adds to coroutines for you, requries oncomplete
            assetBundle.LoadAssetAsync<GameObject>("TeslaCoilBlueprint", (result) => TeslaCoilBlueprintGamOb = result);
            Asset.LoadAssetAsync<GameObject>("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab", (result) => TeslaCoilBlueprintGamOb = result);

            assetBundle.LoadAssetAsync("TeslaCoilBlueprint", (GameObject result) => TeslaCoilBlueprintGamOb = result);
            Asset.LoadAssetAsync("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab", (GameObject result) => TeslaCoilBlueprintGamOb = result);

            ////manually creating AsyncAsset, must call addcoroutine
            ////no different from loadassetasync if you're going to do completion
            new AsyncAsset<GameObject>(assetBundle, "TeslaCoilBlueprint", (result) => TeslaCoilBlueprintGamOb = result).AddCoroutine();
            new AsyncAsset<GameObject>("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab", (result) => TeslaCoilBlueprintGamOb = result).AddCoroutine();
            
            ////create asyncasset for you and adds coroutine
            ////no different from loadassetasync if you're going to do completion.
            assetBundle.AddAsyncAsset<GameObject>("TeslaCoilBlueprint", (result) => TeslaCoilBlueprintGamOb = result);
            Asset.AddAsyncAsset<GameObject>("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab", (result) => TeslaCoilBlueprintGamOb = result);

            //we implicit now baybee
            TeslaCoilBlueprintAsync = new AsyncAsset<GameObject>(assetBundle, "TeslaCoilBlueprint").AddCoroutine();
            TeslaCoilBlueprintAsync = new AsyncAsset<GameObject>("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab").AddCoroutine();

            //implicit but adds coroutine for you
            TeslaCoilBlueprintAsync = assetBundle.AddAsyncAsset<GameObject>("TeslaCoilBlueprint");
            TeslaCoilBlueprintAsync = Asset.AddAsyncAsset<GameObject>("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab");

            //some code later...
            GameObject projectilePrefab = TeslaCoilBlueprintAsync; //use gamobject like normal

            //
            //thinkin
            TeslaCoilBlueprintAsync = assetBundle.AddAsyncAsset<GameObject>("TeslaCoilBlueprint");
            assetBundle.LoadAssetAsync("TeslaCoilBlueprint", (GameObject result) => TeslaCoilBlueprintGamOb = result); //think i'll go with this

            //yeah not even close
            Asset.LoadAssetsAsync( 
                new AsyncAsset<GameObject>(assetBundle, "TeslaCoilBlueprint"), 
                (result) => TeslaCoilBlueprintGamOb = result);

            #endregion

            #region loading an asset and messing with it
            //so realistically we need to do more stuff with it

            //manually adding to coroutines
            loads.Add(assetBundle.LoadAssetCoroutine("TeslaCoilBlueprint", 
                (GameObject result) => {
                    TeslaCoilBlueprintGamOb = result;
                    R2API.PrefabAPI.RegisterNetworkPrefab(TeslaCoilBlueprintGamOb);
                    Modules.Content.AddNetworkedObject(TeslaCoilBlueprintGamOb);
                }));
            loads.Add(Asset.LoadAssetCoroutine("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab",
                (GameObject result) => {
                    TeslaCoilBlueprintGamOb = result;
                    R2API.PrefabAPI.RegisterNetworkPrefab(TeslaCoilBlueprintGamOb);
                    Modules.Content.AddNetworkedObject(TeslaCoilBlueprintGamOb);
                }));

            //adds to coroutines for you
            assetBundle.LoadAssetAsync("TeslaCoilBlueprint",
                (GameObject result) => {
                    TeslaCoilBlueprintGamOb = result;
                    R2API.PrefabAPI.RegisterNetworkPrefab(TeslaCoilBlueprintGamOb);
                    Modules.Content.AddNetworkedObject(TeslaCoilBlueprintGamOb);
                });
            Asset.LoadAssetAsync("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab",
                (GameObject result) => {
                    TeslaCoilBlueprintGamOb = result;
                    R2API.PrefabAPI.RegisterNetworkPrefab(TeslaCoilBlueprintGamOb);
                    Modules.Content.AddNetworkedObject(TeslaCoilBlueprintGamOb);
                });

            //manually creating AsyncAsset, must call addcoroutine
            TeslaCoilBlueprintAsync = new AsyncAsset<GameObject>(assetBundle, "TeslaCoilBlueprint",
                (GameObject result) => {
                    R2API.PrefabAPI.RegisterNetworkPrefab(result);
                    Modules.Content.AddNetworkedObject(result);
                }).AddCoroutine();
            TeslaCoilBlueprintAsync = new AsyncAsset<GameObject>("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab",
                (GameObject result) => {
                    R2API.PrefabAPI.RegisterNetworkPrefab(result);
                    Modules.Content.AddNetworkedObject(result);
                }).AddCoroutine();

            //create asyncasset for you and adds coroutine
            TeslaCoilBlueprintAsync = assetBundle.AddAsyncAsset("TeslaCoilBlueprint", 
                (GameObject result) => {
                    R2API.PrefabAPI.RegisterNetworkPrefab(result);
                    Modules.Content.AddNetworkedObject(result);
                });
            TeslaCoilBlueprintAsync = Asset.AddAsyncAsset("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab",
                (GameObject result) => {
                    R2API.PrefabAPI.RegisterNetworkPrefab(result);
                    Modules.Content.AddNetworkedObject(result);
                });

            //bit more cumbersome but more consistent with loading multiple assets.
            Asset.LoadAssetsAsync(
                new AsyncAsset<GameObject>(assetBundle, "TeslaCoilBlueprint"),
                (result) =>
                {
                    TeslaCoilBlueprintGamOb = result;
                    R2API.PrefabAPI.RegisterNetworkPrefab(TeslaCoilBlueprintGamOb);
                    Modules.Content.AddNetworkedObject(TeslaCoilBlueprintGamOb);
                });
            Asset.LoadAssetsAsync(
                new AsyncAsset<GameObject>("RoR2/Base/TeslaTrooper/TeslaCoilBlueprint.prefab"),
                (result) =>
                {
                    TeslaCoilBlueprintGamOb = result;
                    R2API.PrefabAPI.RegisterNetworkPrefab(TeslaCoilBlueprintGamOb);
                    Modules.Content.AddNetworkedObject(TeslaCoilBlueprintGamOb);
                });


            //best I think
            assetBundle.LoadAssetAsync("TeslaCoilBlueprint",
                (GameObject result) => {
                    TeslaCoilBlueprintGamOb = result;
                    R2API.PrefabAPI.RegisterNetworkPrefab(TeslaCoilBlueprintGamOb);
                    Modules.Content.AddNetworkedObject(TeslaCoilBlueprintGamOb);
                });
            //saves a line and is consistent with one-liner single asset, but uses implicit conversion thing that I'm still not sure on
            TeslaCoilBlueprintAsync = assetBundle.AddAsyncAsset("TeslaCoilBlueprint",
                (GameObject result) => {
                    R2API.PrefabAPI.RegisterNetworkPrefab(result);
                    Modules.Content.AddNetworkedObject(result);
                });
            //bit more cumbersome but more consistent with the easily best way of loading multiple assets.
            Asset.LoadAssetsAsync(
                new AsyncAsset<GameObject>(assetBundle, "TeslaCoilBlueprint"),
                (result) => {
                    TeslaCoilBlueprintGamOb = result;
                    R2API.PrefabAPI.RegisterNetworkPrefab(TeslaCoilBlueprintGamOb);
                    Modules.Content.AddNetworkedObject(TeslaCoilBlueprintGamOb);
                });

            #endregion

            #region loading multiple assets
            //it's no contest
            Asset.LoadAssetsAsync(
                new AsyncAsset<GameObject>(assetBundle, "TeslaCoilBlueprint"),
                new AsyncAsset<Material>("RoR2/Base/Engi/matBlueprintsOk.mat"),
                new AsyncAsset<Material>("RoR2/Base/Engi/matBlueprintsInvalid.mat"),
                (blueprintObject, matOK, matNo) =>
                {
                    blueprintObject.GetComponent<RoR2.BlueprintController>().okMaterial = matOK;
                    blueprintObject.GetComponent<RoR2.BlueprintController>().invalidMaterial = matNo;
                    TeslaCoilBlueprintGamOb = blueprintObject;
                });


            //manually adding to coroutines
            loads.Add(Asset.LoadAssetCoroutine(assetBundle, "TeslaCoilBlueprint",
                (GameObject blueprintObject) => {
                    loads.Add(Asset.LoadAssetCoroutine("RoR2/Base/Engi/matBlueprintsOk.mat",
                        (Material matOK) => {
                            loads.Add(Asset.LoadAssetCoroutine("RoR2/Base/Engi/matBlueprintsInvalid.mat",
                                (Material matNo) => {
                                    blueprintObject.GetComponent<RoR2.BlueprintController>().okMaterial = matOK;
                                    blueprintObject.GetComponent<RoR2.BlueprintController>().invalidMaterial = matNo;
                                    TeslaCoilBlueprintGamOb = blueprintObject;
                                }));
                        }));
                }));

            Asset.LoadAssetAsync(assetBundle, "TeslaCoilBlueprint",
                (GameObject blueprintObject) => {
                    Asset.LoadAssetAsync("RoR2/Base/Engi/matBlueprintsOk.mat",
                        (Material matOK) => {
                            Asset.LoadAssetAsync("RoR2/Base/Engi/matBlueprintsInvalid.mat",
                                (Material matNo) => {
                                    blueprintObject.GetComponent<RoR2.BlueprintController>().okMaterial = matOK;
                                    blueprintObject.GetComponent<RoR2.BlueprintController>().invalidMaterial = matNo;
                                    TeslaCoilBlueprintGamOb = blueprintObject;
                                });
                        });
                });
            //I'm just gonna stop here
            #endregion
        }
    }
}
