using RoR2;
using UnityEngine;
using VRAPI;

namespace RA2Mod.Survivors.Tesla.Compat
{
    public static class TeslaVRCompat
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
    }
}
