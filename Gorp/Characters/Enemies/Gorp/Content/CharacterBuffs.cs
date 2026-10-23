using RoR2;
using UnityEngine;

namespace GorpMod.Enemies.Gorp.Content
{
    public static class CharacterBuffs
    {
        // armor buff gained during roll
        public static BuffDef GorpThresholdBuff;

        ////creasted in matchersurvivor skill setup
        //public static BuffDef shieldMatchBuff;

        public static void Init(AssetBundle assetBundle)
        {
            GorpThresholdBuff = Modules.Content.CreateAndAddBuff("GorpBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.yellow,
                canStack: true,
                isDebuff: false,
                isHidden: true);
        }
    }
}
