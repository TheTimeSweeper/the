using EntityStates;
using RoR2.Skills;
using RoR2;
using System;
using UnityEngine;
using VRAPI;
using System.Runtime.CompilerServices;

namespace Modules {

    public class VRCompat {

        public static void init() {

            if (!VRAPI.VR.enabled || !VRAPI.MotionControls.enabled)
                return;

            Compat.VREnabled = true;

            RoR2Application.onLoad += LoadCustomHands;
        }

        private static void LoadCustomHands() {

            GameObject domHandPrefab = Assets.LoadAsset<GameObject>("VRRightHandPrefab");
            GameObject subHandPrefab = Assets.LoadAsset<GameObject>("VRLeftHandPrefab");

            MotionControls.AddHandPrefab(domHandPrefab);
            MotionControls.AddHandPrefab(subHandPrefab);

            MotionControls.AddSkillBindingOverride("TeslaTrooperBody", SkillSlot.Primary, SkillSlot.Utility, SkillSlot.Special, SkillSlot.Secondary);

        }

        #region helpers
        public static Ray GetAimRay(BaseState state, bool dominant = true) {
            if (IsLocalVRPlayer(state.characterBody)) {
                return GetVrAimRay(dominant);
            }
            return state.GetAimRay();
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static Ray GetVrAimRay(bool dominant) {
            return dominant ? MotionControls.dominantHand.aimRay : MotionControls.nonDominantHand.aimRay;
        }

        public static Ray GetAimRayCamera(BaseState state) {

            if (IsLocalVRPlayer(state.characterBody)) {
                return GetVRAimRayCamera();
            }
            return state.GetAimRay();
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static Ray GetVRAimRayCamera() {
            return new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        public static ChildLocator GetModelChildLocator(BaseState state, bool dominant = true) {
            if (Modules.VRCompat.IsLocalVRPlayer(state.characterBody)) {
                return GetVRChildLocator(dominant);
            }
            return state.GetModelChildLocator();
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static ChildLocator GetVRChildLocator(bool dominant) {
            if (dominant) {
                return MotionControls.dominantHand.transform.GetComponentInChildren<ChildLocator>();
            } else {
                return MotionControls.nonDominantHand.transform.GetComponentInChildren<ChildLocator>();
            }
        }

        public static bool IsLocalVRPlayer(CharacterBody body) {
            return Compat.VREnabled && body == LocalUserManager.GetFirstLocalUser().cachedBody;
        }
        #endregion helpers
    }
}