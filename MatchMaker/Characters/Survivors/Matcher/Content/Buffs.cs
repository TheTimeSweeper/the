using RoR2;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.MatcherContent
{
    public static class Buffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

        }
    }
}
