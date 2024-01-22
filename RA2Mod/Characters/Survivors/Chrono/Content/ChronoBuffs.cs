using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RA2Mod.Survivors.Chrono {
    public static class ChronoBuffs
    {
        // armor buff gained during roll
        public static BuffDef chronoDebuff;
        public static BuffDef chronoSphereRootDebuff;
        public static BuffDef ivand;

        public static void Init(AssetBundle assetBundle)
        {
            chronoDebuff = Modules.Content.CreateAndAddBuff("ChronoDebuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.cyan,
                true,
                true);

            chronoSphereRootDebuff = Modules.Content.CreateAndAddBuff("chronoSphereRootDebuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.blue,
                false,
                true);

            ivand = Modules.Content.CreateAndAddBuff("ivand",
                assetBundle.LoadAsset<Sprite>("texBuffChronoClock"),
                Color.blue,
                true,
                true);

            R2API.TempVisualEffectAPI.AddTemporaryVisualEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/MercExposeEffect.prefab").WaitForCompletion(), GetHasIvanTempVisualEffect);
        }
        
        private static bool GetHasIvanTempVisualEffect(CharacterBody body)
        {
            return body.HasBuff(ivand);
        }
    }
}
