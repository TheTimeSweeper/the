using RA2Mod.Modules;
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
        //public static BuffDef ivand;
        //public static BuffDef vanishFreeze;

        public static void Init(AssetBundle assetBundle)
        {
            chronoSicknessDebuff = Modules.Content.CreateAndAddBuff("ChronoDebuff",
                null,
                Color.cyan,
                true,
                true);
            ContentPacks.asyncLoadCoroutines.Add(Asset.LoadBuffIconAsync(chronoSicknessDebuff, assetBundle, "texBuffChronoClock"));

            //chronosphereRootDebuff = Modules.Content.CreateAndAddBuff("ChronosphereRoot",
            //    null,
            //    Color.blue,
            //    false,
            //    true);
            //ContentPacks.asyncLoadCoroutines.Add(Assets.LoadBuffIconAsync(chronosphereRootDebuff, "RoR2/Base/Common/texBuffGenericShield.tif"));

            //ivand = Modules.Content.CreateAndAddBuff("ChronoIvand",
            //    null,
            //    Color.blue,
            //    true,
            //    true);
            //ContentPacks.asyncLoadCoroutines.Add(Assets.LoadBuffIconAsync(ivand, assetBundle, "texBuffChronoClock"));

            //vanishFreeze = Modules.Content.CreateAndAddBuff("ChronoVanishfreeze",
            //    null,
            //    Color.cyan,
            //    true,
            //    true);
            //ContentPacks.asyncLoadCoroutines.Add(Assets.LoadBuffIconAsync(vanishFreeze, "RoR2/Base/Common/texBuffGenericShield.tif"));

        }
        
        //private static bool GetHasIvanTempVisualEffect(CharacterBody body)
        //{
        //    return body.HasBuff(ivand);
        //}
    }
}
