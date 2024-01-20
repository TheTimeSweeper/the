using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono {
    public static class ChronoBuffs
    {
        // armor buff gained during roll
        public static BuffDef chronoDebuff;

        public static void Init(AssetBundle assetBundle)
        {
            chronoDebuff = Modules.Content.CreateAndAddBuff("ChronoDebuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                true,
                true);

        }
    }
}
