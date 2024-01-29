using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RA2Mod.Survivors.Chrono {
    public static class ChronoBuffs
    {
        // armor buff gained during roll
        public static BuffDef chronoSicknessDebuff;
        public static BuffDef chronosphereRootDebuff;
        public static BuffDef ivand;
        public static BuffDef vanishFreeze;

        public static void Init(AssetBundle assetBundle)
        {
            chronoSicknessDebuff = Modules.Content.CreateAndAddBuff("ChronoDebuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.cyan,
                true,
                true);

            chronosphereRootDebuff = Modules.Content.CreateAndAddBuff("ChronosphereRoot",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.blue,
                false,
                true);

            ivand = Modules.Content.CreateAndAddBuff("ChronoIvand",
                assetBundle.LoadAsset<Sprite>("texBuffChronoClock"),
                Color.blue,
                true,
                true);

            vanishFreeze = Modules.Content.CreateAndAddBuff("ChronoVanishfreeze",
                assetBundle.LoadAsset<Sprite>("texBuffChronoClock"),
                Color.cyan,
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
