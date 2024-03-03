using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using RoR2.UI;
using RoR2.Projectile;
using Path = System.IO.Path;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Runtime.CompilerServices;
using System;

namespace RA2Mod.Modules
{
    internal static class Assets
    {
        //cache bundles if multiple characters use the same one
        internal static Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();

        internal static void LoadAssetBundleAsync(string bundleName, Action<AssetBundle> onComplete = null)
        {
            if (bundleName == "myassetbundle")
            {
                Log.Error($"AssetBundle name hasn't been changed. not loading any assets to avoid conflicts. Everything will now break.\nMake sure to rename your assetbundle filename and rename the AssetBundleName field in your character setup code.");
            }

            if (loadedBundles.ContainsKey(bundleName))
            {
                onComplete?.Invoke(loadedBundles[bundleName]);
            }

            loadedBundles[bundleName] = null;
            string path = Path.Combine(Path.GetDirectoryName(RA2Plugin.instance.Info.Location), "AssetBundles", bundleName);

            ContentPacks.asyncLoadCoroutines.Add(LoadAssetBundleFromPathAsync(path, (bundle) =>
            {
                loadedBundles[bundleName] = bundle;
                ContentPacks.asyncLoadCoroutines.Add(ShaderSwapper.ShaderSwapper.UpgradeStubbedShadersAsync(bundle));
                onComplete?.Invoke(bundle);
            }));
        }

        internal static IEnumerator LoadFromAddressableOrBundle<T>(AssetBundle assetBundle, string bundlePath, string addressablePath, Action<T> OnComplete) where T : UnityEngine.Object
        {
            if (!string.IsNullOrEmpty(bundlePath))
            {
                return assetBundle.LoadAssetAsync<T>(bundlePath, OnComplete);
            }
            if (!string.IsNullOrEmpty(addressablePath))
            {
                return Assets.LoadAddressableAssetAsync<T>(addressablePath, OnComplete);
            }
            return null;
        }

        internal static IEnumerator LoadAssetBundleFromPathAsync(string path, Action<AssetBundle> onComplete)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            while (!request.isDone)
            {
                yield return null;
            }
            onComplete?.Invoke(request.assetBundle);
        }

        internal static IEnumerator LoadAssetAsync<T>(this AssetBundle assetBundle, string name, Action<T> onComplete) where T : UnityEngine.Object
        {
            AssetBundleRequest request = assetBundle.LoadAssetAsync<T>(name);
            while (!request.isDone)
            {
                yield return null;
            }
            onComplete?.Invoke(request.asset as T);
        }

        internal static IEnumerator LoadAssetToCollection<T>(this AssetBundle assetBundle, string name, Dictionary<string, T> assetCollection) where T : UnityEngine.Object
        {
            AssetBundleRequest request = assetBundle.LoadAssetAsync<T>(name);
            while (!request.isDone)
            {
                yield return null;
            }
            assetCollection[name] = request.asset as T;
        }

        internal static IEnumerator LoadAddressableAssetAsync<T>(object key, Action<T> OnComplete) where T : UnityEngine.Object
        {
            AsyncOperationHandle<T> loadAsset = Addressables.LoadAssetAsync<T>(key);
            while (!loadAsset.IsDone) { yield return null; }
            OnComplete(loadAsset.Result);
        }

        //credit to groove salad with ivyl, unused tho
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static AsyncOperationHandle LoadAddressableAssetAsync<TObject>(object key, out AsyncOperationHandle<TObject> handle)
        {
            return handle = Addressables.LoadAssetAsync<TObject>(key);
        }

        internal static IEnumerator LoadBuffIconAsync(BuffDef buffDef, AssetBundle assetBundle, string bundleLoadPath)
        {
            return assetBundle.LoadAssetAsync<Sprite>(bundleLoadPath, (result) => {
                buffDef.iconSprite = result;
            });
        }
        internal static IEnumerator LoadBuffIconAsync(BuffDef buffDef, string addressablePath)
        {
            return LoadAddressableAssetAsync<Sprite>(addressablePath, (result) => {
                buffDef.iconSprite = result;
            });
        }

        #region legacy non-async helpers
        internal static GameObject CloneTracer(string originalTracerName, string newTracerName)
        {
            GameObject loadedTracer = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName);
            if (loadedTracer == null) 
                return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(loadedTracer, newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();
            
            newTracer.GetComponent<Tracer>().speed = 250f;
            newTracer.GetComponent<Tracer>().length = 50f;

            Modules.Content.CreateAndAddEffectDef(newTracer);

            return newTracer;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (Renderer i in objectToConvert.GetComponentsInChildren<Renderer>())
            {
                i?.sharedMaterial?.ConvertDefaultShaderToHopoo();
            }
        }

        internal static GameObject LoadCrosshair(string crosshairName)
        {
            GameObject loadedCrosshair = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
            if (loadedCrosshair == null)
            {
                Log.Error($"could not load crosshair with the name {crosshairName}. defaulting to Standard");

                return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            }

            return loadedCrosshair;
        }

        internal static GameObject LoadEffect(this AssetBundle assetBundle, string resourceName, bool parentToTransform) => LoadEffect(assetBundle, resourceName, "", parentToTransform);
        internal static GameObject LoadEffect(this AssetBundle assetBundle, string resourceName, string soundName = "", bool parentToTransform = false)
        {
            GameObject newEffect = assetBundle.LoadAsset<GameObject>(resourceName);

            if (!newEffect)
            {
                Log.ErrorAssetBundle(resourceName, assetBundle.name);
                return null;
            }

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            EffectComponent effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            Modules.Content.CreateAndAddEffectDef(newEffect);

            return newEffect;
        }

        internal static GameObject CreateProjectileGhostPrefab(this AssetBundle assetBundle, string ghostName)
        {
            GameObject ghostPrefab = assetBundle.LoadAsset<GameObject>(ghostName);
            if (ghostPrefab == null)
            {
                Log.Error($"Failed to load ghost prefab {ghostName}");
            }
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        internal static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }

        internal static GameObject LoadAndAddProjectilePrefab(this AssetBundle assetBundle, string newPrefabName)
        {
            GameObject newPrefab = assetBundle.LoadAsset<GameObject>(newPrefabName);
            if(newPrefab == null)
            {
                Log.ErrorAssetBundle(newPrefabName, assetBundle.name);
                return null;
            }

            Content.AddProjectilePrefab(newPrefab);
            return newPrefab;
        }
        #endregion 
    }
}