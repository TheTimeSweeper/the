using EntityStates;
using RoR2;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using VRAPI;

namespace RA2Mod.General
{
    public static class GeneralCompat
    {
        public delegate void Meme_SurivorCatalog_Init();
        public static event Meme_SurivorCatalog_Init Meme_OnSurvivorCatalog_Init;

        public static bool TinkersSatchelInstalled;
        public static bool AetheriumInstalled;
        public static bool ScepterInstalled;
        public static bool VREnabled;
        public static bool driverInstalled;

        public static void Init()
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.ThinkInvisible.TinkersSatchel"))
            {
                TinkersSatchelInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KomradeSpectre.Aetherium"))
            {
                AetheriumInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter"))
            {
                ScepterInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rob.Driver"))
            {
                driverInstalled = true;
            }
        }

        internal static int TryGetScepterCount(Inventory inventory)
        {
            if (!ScepterInstalled)
                return 0;

            return GetScepterCount(inventory);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static int GetScepterCount(Inventory inventory)
        {
            return inventory.GetItemCount(AncientScepter.AncientScepterItem.instance.ItemDef);
        }

        #region vr helpers
        public static Ray GetAimRay(this BaseState state, bool dominant = true)
        {
            if (IsLocalVRPlayer(state.characterBody))
            {
                return GetVrAimRay(dominant);
            }
            return state.GetAimRay();
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static Ray GetVrAimRay(bool dominant)
        {
            return dominant ? MotionControls.dominantHand.aimRay : MotionControls.nonDominantHand.aimRay;
        }

        public static Ray GetAimRayCamera(BaseState state)
        {
            if (IsLocalVRPlayer(state.characterBody))
            {
                return GetVRAimRayCamera();
            }
            return state.GetAimRay();
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static Ray GetVRAimRayCamera()
        {
            //todo teslamove no camera.main in fixedupdate
            return new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        public static ChildLocator GetModelChildLocator(this BaseState state, bool dominant = true)
        {
            if (IsLocalVRPlayer(state.characterBody))
            {
                return GetVRChildLocator(dominant);
            }
            return state.GetModelChildLocator();
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static ChildLocator GetVRChildLocator(bool dominant)
        {
            if (dominant)
            {
                return MotionControls.dominantHand.transform.GetComponentInChildren<ChildLocator>();
            }
            else
            {
                return MotionControls.nonDominantHand.transform.GetComponentInChildren<ChildLocator>();
            }
        }

        public static bool IsLocalVRPlayer(CharacterBody body)
        {
            return General.GeneralCompat.VREnabled && body == LocalUserManager.GetFirstLocalUser().cachedBody;
        }
        #endregion vr helpers
    }
}
