using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Conscript
{
    public static class ConscriptBuffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;
        public static BuffDef magazineBuff;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("ConscriptArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.red,
                false,
                false);

            magazineBuff = Modules.Content.CreateAndAddBuff("ConscriptMagazineBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.green,
                false,
                false);
        }
    }
}
