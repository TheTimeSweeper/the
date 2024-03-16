using EntityStates;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using VRAPI;

namespace RA2Mod.Survivors.Tesla.Compat
{
    public class VRCompat
    {
        public static void init()
        {
            if (!VRAPI.VR.enabled || !VRAPI.MotionControls.enabled)
                return;

            General.GeneralCompat.VREnabled = true;

            RoR2Application.onLoad += LoadCustomHands;
        }

        private static void LoadCustomHands()
        {
            //todo teslamove async load
            GameObject domHandPrefab = TeslaTrooperSurvivor.instance.assetBundle.LoadAsset<GameObject>("VRRightHandPrefab");
            GameObject subHandPrefab = TeslaTrooperSurvivor.instance.assetBundle.LoadAsset<GameObject>("VRLeftHandPrefab");

            MotionControls.AddHandPrefab(domHandPrefab);
            MotionControls.AddHandPrefab(subHandPrefab);

            MotionControls.AddSkillBindingOverride("TeslaTrooperBody", SkillSlot.Primary, SkillSlot.Utility, SkillSlot.Special, SkillSlot.Secondary);

        }

        #region helpers
        public static Ray GetAimRay(BaseState state, bool dominant = true)
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
            return new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        public static ChildLocator GetModelChildLocator(BaseState state, bool dominant = true)
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
        #endregion helpers
    }
}
