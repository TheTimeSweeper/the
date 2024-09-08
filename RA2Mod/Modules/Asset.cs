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
using Object = UnityEngine.Object;

namespace RA2Mod.Modules
{
    internal static class Asset
    {
        //cache bundles if multiple characters use the same one
        internal static Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();
        internal static Dictionary<string, Action<AssetBundle>> loadingBundles = new Dictionary<string, Action<AssetBundle>>();

        #region bundle
        internal static void LoadAssetBundleAsync(string bundleName, Action<AssetBundle> onComplete)
        {
            if (bundleName == "myassetbundle")
            {
                Log.Error($"AssetBundle name hasn't been changed. not loading any assets to avoid conflicts. Everything will now break.\nMake sure to rename your assetbundle filename and rename the AssetBundleName field in your character setup code.");
            }

            if (loadedBundles.ContainsKey(bundleName))
            {
                onComplete(loadedBundles[bundleName]);
                return;
            }

            if (loadingBundles.ContainsKey(bundleName))
            {
                loadingBundles[bundleName] += onComplete;
                return;
            }

            loadingBundles[bundleName] = onComplete;

            string path = Path.Combine(Path.GetDirectoryName(RA2Plugin.instance.Info.Location), "AssetBundles", bundleName);

            Action<AssetBundle> onBundleComplete = (bundle) =>
                           {
                               loadedBundles[bundleName] = bundle;
                               if (loadingBundles.ContainsKey(bundleName))
                               {
                                   loadingBundles[bundleName]?.Invoke(bundle);
                                   loadingBundles.Remove(bundleName);
                               }
                           };
            ContentPacks.asyncLoadCoroutines.Add(LoadAssetBundleFromPathAsync(path, onBundleComplete));
        }

        internal static IEnumerator LoadAssetBundleFromPathAsync(string path, Action<AssetBundle> onComplete)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            while (!request.isDone)
            {
                yield return null;
            }
            ContentPacks.asyncLoadCoroutines.Add(ShaderSwapper.ShaderSwapper.UpgradeStubbedShadersAsync(request.assetBundle));
            onComplete?.Invoke(request.assetBundle);
        }
        #endregion bundle

        internal static void LoadAssetAsync<T>(this AssetBundle assetBundle, string name, Action<T> onComplete) where T : Object
        {
            ContentPacks.asyncLoadCoroutines.Add(assetBundle.LoadAssetCoroutine<T>(name, onComplete));
        }
        internal static void LoadAssetAsync<T>(string Path, Action<T> onComplete) where T : Object
        {
            ContentPacks.asyncLoadCoroutines.Add(LoadAssetCoroutine<T>(Path, onComplete));
        }

        internal static AsyncAsset<T> AddAsyncAsset<T>(this AssetBundle assetBundle, string name, Action<T> onComplete = null) where T : Object
        {
            return new AsyncAsset<T>(assetBundle, name, onComplete).AddCoroutine();
        }
        internal static AsyncAsset<T> AddAsyncAsset<T>(string path, Action<T> onComplete = null) where T: Object
        {
            return new AsyncAsset<T>(path, onComplete).AddCoroutine();
        }

        internal static IEnumerator LoadAssetCoroutine<T>(this AssetBundle assetBundle, string name, Action<T> onComplete) where T : Object
        {
            AssetBundleRequest request = assetBundle.LoadAssetAsync<T>(name);
            while (!request.isDone)
                yield return null;
            
            onComplete?.Invoke(request.asset as T);
        }
        internal static IEnumerator LoadAssetCoroutine<T>(object key, Action<T> OnComplete) where T : Object
        {
            AsyncOperationHandle<T> loadAsset = Addressables.LoadAssetAsync<T>(key);
            while (!loadAsset.IsDone) 
                yield return null; 
            
            OnComplete?.Invoke(loadAsset.Result);
        }

        #region there is a thin line between jank and genius
        internal static void LoadAssetsAsync<T1>(
            AsyncAsset<T1> load1,
            Action<T1> onComplete) where T1 : Object
        {
            ContentPacks.asyncLoadCoroutines.Add(LoadAssetsAsyncCoroutine(load1, onComplete));
        }
        internal static IEnumerator LoadAssetsAsyncCoroutine<T1>(
            AsyncAsset<T1> load1,
            Action<T1> onComplete) where T1 : Object
        {
            while (!load1.coroutine.MoveNext())
                yield return null;

            onComplete(load1.result);
        }
        internal static void LoadAssetsAsync<T1, T2>(
            AsyncAsset<T1> load1,
            AsyncAsset<T2> load2,
            Action<T1, T2> onComplete) where T1 : Object where T2 : Object
        {
            ContentPacks.asyncLoadCoroutines.Add(LoadAssetsAsyncCoroutine(load1, load2, onComplete));
        }
        internal static IEnumerator LoadAssetsAsyncCoroutine<T1, T2>(
            AsyncAsset<T1> load1, 
            AsyncAsset<T2> load2, 
            Action<T1, T2> onComplete) where T1: Object where T2: Object
        {
            while (!load1.coroutine.MoveNext())
                yield return null;
            while (!load2.coroutine.MoveNext())
                yield return null;

            onComplete(load1.result, load2.result);
        }
        internal static void LoadAssetsAsync<T1, T2, T3>(
            AsyncAsset<T1> load1, 
            AsyncAsset<T2> load2,
            AsyncAsset<T3> load3, 
            Action<T1, T2, T3> onComplete) where T1 : Object where T2 : Object where T3: Object
        {
            ContentPacks.asyncLoadCoroutines.Add(LoadAssetsAsyncCoroutine(load1, load2, load3, onComplete));
        }
        internal static IEnumerator LoadAssetsAsyncCoroutine<T1, T2, T3>(
            AsyncAsset<T1> load1, 
            AsyncAsset<T2> load2,
            AsyncAsset<T3> load3, 
            Action<T1, T2, T3> onComplete) where T1 : Object where T2 : Object where T3: Object
        {
            while (!load1.coroutine.MoveNext())
                yield return null;
            while (!load2.coroutine.MoveNext())
                yield return null;
            while (!load3.coroutine.MoveNext())
                yield return null;

            onComplete(load1.result, load2.result, load3.result);
        }
        internal static void LoadAssetsAsync<T1, T2, T3, T4>(
            AsyncAsset<T1> load1,
            AsyncAsset<T2> load2,
            AsyncAsset<T3> load3,
            AsyncAsset<T4> load4,
            Action<T1, T2, T3, T4> onComplete) where T1 : Object where T2 : Object where T3 : Object where T4 : Object
        {
            ContentPacks.asyncLoadCoroutines.Add(LoadAssetsAsyncCoroutine(load1, load2, load3, load4, onComplete));
        }
        internal static IEnumerator LoadAssetsAsyncCoroutine<T1, T2, T3, T4>(
            AsyncAsset<T1> load1, 
            AsyncAsset<T2> load2,
            AsyncAsset<T3> load3,
            AsyncAsset<T4> load4,
            Action<T1, T2, T3, T4> onComplete) where T1 : Object where T2 : Object where T3 : Object where T4 : Object
        {
            while (!load1.coroutine.MoveNext())
                yield return null;
            while (!load2.coroutine.MoveNext())
                yield return null;
            while (!load3.coroutine.MoveNext())
                yield return null;
            while (!load4.coroutine.MoveNext())
                yield return null;

            onComplete(load1.result, load2.result, load3.result, load4.result);
        }
        #endregion sometimes a a very thin line

        #region sillyzone
        internal static IEnumerator LoadFromAddressableOrBundle<T>(AssetBundle assetBundle, string bundlePath, string addressablePath, Action<T> OnComplete) where T : Object
        {
            if (!string.IsNullOrEmpty(bundlePath))
            {
                return assetBundle.LoadAssetCoroutine<T>(bundlePath, OnComplete);
            }
            if (!string.IsNullOrEmpty(addressablePath))
            {
                return Asset.LoadAssetCoroutine<T>(addressablePath, OnComplete);
            }
            return null;
        }

        /// <summary>
        /// used to simply call the load on objects that will be loaded again later
        /// </summary>
        internal static IEnumerator PreLoadAssetsAsync<T>(AssetBundle assetBundle, params string[] paths) where T : Object
        {
            List<IEnumerator> coroutines = PreLoadAssetsAsyncCoroutines<T>(assetBundle, paths);

            for (int i = 0; i < coroutines.Count; i++)
            {
                while (coroutines[i].MoveNext())
                {
                    yield return null;
                }
            }
        }

        /// <summary>
        /// used to simply call the load on objects that will be loaded again later
        /// </summary>
        internal static List<IEnumerator> PreLoadAssetsAsyncCoroutines<T>(AssetBundle assetBundle, params string[] paths) where T: Object
        {
            List<IEnumerator> coroutines = new List<IEnumerator>();

            for (int i = 0; i < paths.Length; i++)
            {
                coroutines.Add(assetBundle.LoadAssetCoroutine<T>(paths[i], null));
            }

            return coroutines;
        }

        /// <summary>
        /// if I got tired of loading buff icons async maybe there's a better way
        /// </summary>
        internal static IEnumerator LoadBuffIconAsync(BuffDef buffDef, AssetBundle assetBundle, string bundleLoadPath)
        {
            return assetBundle.LoadAssetCoroutine<Sprite>(bundleLoadPath, (result) => buffDef.iconSprite = result);
        }
        /// <summary>
        /// if I got tired of loading buff icons async maybe there's a better way
        /// </summary>
        internal static IEnumerator LoadBuffIconAsync(BuffDef buffDef, string addressablePath)
        {
            return LoadAssetCoroutine<Sprite>(addressablePath, (result) => buffDef.iconSprite = result);
        }
        #endregion sillyzone

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

            foreach (MeshRenderer i in objectToConvert.GetComponentsInChildren<MeshRenderer>())
            {
                i.sharedMaterial?.ConvertDefaultShaderToHopoo();
            }

            foreach (SkinnedMeshRenderer i in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                i.sharedMaterial?.ConvertDefaultShaderToHopoo();
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

            CreateEffectFromObject(newEffect, soundName, parentToTransform);

            return newEffect;
        }

        internal static void CreateEffectFromObject(GameObject newEffect, string soundName, bool parentToTransform)
        {
            newEffect.AddComponent<DestroyOnTimer>().duration = 6;
            newEffect.AddComponent<NetworkIdentity>();
            if (!newEffect.GetComponent<VFXAttributes>())
            {
                newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            }

            EffectComponent effect = newEffect.GetComponent<EffectComponent>();
            if (!effect)
            {
                effect = newEffect.AddComponent<EffectComponent>();
                effect.applyScale = true;
                effect.effectIndex = EffectIndex.Invalid;
                effect.parentToReferencedTransform = parentToTransform;
                effect.positionAtReferencedTransform = true;
                effect.soundName = soundName;
            }

            Content.CreateAndAddEffectDef(newEffect);
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

            Modules.Asset.ConvertAllRenderersToHopooShader(ghostPrefab);

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

    public class AsyncAsset<T> where T : Object
    {
        public T result;
        public bool isDone;
        public IEnumerator coroutine;
        public Action<T> onComplete;

        public static implicit operator T(AsyncAsset<T> asset)
        {
            return asset.result;
        }

        public AsyncAsset(string path_, Action<T> onComplete_ = null)
        {
            onComplete = onComplete_;
            coroutine = Asset.LoadAssetCoroutine<T>(path_, OnCoroutineComplete);
        }

        public AsyncAsset(AssetBundle bundle_, string name_, Action<T> onComplete_ = null)
        {
            onComplete = onComplete_;
            coroutine = bundle_.LoadAssetCoroutine<T>(name_, OnCoroutineComplete);
        }

        public AsyncAsset<T> AddCoroutine()
        {
            Modules.ContentPacks.asyncLoadCoroutines.Add(coroutine);
            return this;
        }

        public void OnCoroutineComplete(T loadResult)
        {
            result = loadResult;
            isDone = true;
            onComplete?.Invoke(result);
        }
    }
}