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

        internal static AssetBundle LoadAssetBundle(string bundleName)
        {

            if (bundleName == "myassetbundle")
            {
                Log.Error($"AssetBundle name hasn't been changed. not loading any assets to avoid conflicts.\nMake sure to rename your assetbundle filename and rename the AssetBundleName field in your character setup code ");
                return null;
            }

            if (loadedBundles.ContainsKey(bundleName))
            {
                return loadedBundles[bundleName];
            }

            AssetBundle assetBundle = null;
            try
            {
                assetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(RA2Plugin.instance.Info.Location), "AssetBundles", bundleName));
            }
            catch (System.Exception e)
            {
                Log.Error($"Error loading asset bundle, {bundleName}. Your asset bundle must be in a folder next to your mod dll called 'AssetBundles'. Follow the guide to build and install your mod correctly!\n{e}");
            }

            loadedBundles[bundleName] = assetBundle;
            RA2Plugin.instance.StartCoroutine(ShaderSwapper.ShaderSwapper.UpgradeStubbedShadersAsync(assetBundle));

            return assetBundle;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerator LoadAssetAsync<T>(this AssetBundle assetBundle, string name, Action<T> OnComplete) where T : UnityEngine.Object
        {
            AssetBundleRequest request = assetBundle.LoadAssetAsync<T>(name);
            yield return request;
            OnComplete(request.asset as T);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerator LoadAssetAsync<T>(this AssetBundle assetBundle, string name, Func<T, IEnumerator> OnComplete) where T : UnityEngine.Object
        {
            AssetBundleRequest request = assetBundle.LoadAssetAsync<T>(name);
            yield return request;
            yield return OnComplete(request.asset as T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerator LoadAddressableAssetAsync<T>(object key, Action<T> OnComplete) where T : UnityEngine.Object
        {
            AsyncOperationHandle<T> loadAsset = Addressables.LoadAssetAsync<T>(key);
            if (!loadAsset.IsDone) { yield return loadAsset; }
            OnComplete(loadAsset.Result);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerator LoadAddressableAssetAsync<T>(object key, Func<T, IEnumerator> OnComplete) where T : UnityEngine.Object
        {
            AsyncOperationHandle<T> loadAsset = Addressables.LoadAssetAsync<T>(key);
            if (!loadAsset.IsDone) { yield return loadAsset; }
            yield return OnComplete(loadAsset.Result);
        }
        //credit to groove salad with ivyl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static AsyncOperationHandle LoadAddressableAssetAsync<TObject>(object key, out AsyncOperationHandle<TObject> handle)
        {
            return handle = Addressables.LoadAssetAsync<TObject>(key);
        }

        internal static GameObject CloneTracer(string originalTracerName, string newTracerName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) 
                return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

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
    }
}