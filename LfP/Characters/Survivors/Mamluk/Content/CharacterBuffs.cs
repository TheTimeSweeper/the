using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LfPMod.Survivors.Mamluk.MamlukContent
{
    public static class CharacterBuffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;

        // buff inflicted with slash combo dot
        public static BuffDef comboFinisherDotBuff;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                canStack: false,
                isDebuff: false);

            comboFinisherDotBuff = Modules.Content.CreateAndAddBuff("HenryDotBuff",
                Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/texBuffBleedingIcon.tif").WaitForCompletion(),
                Color.yellow,
                canStack: false,
                isDebuff: false);//you would think DoTs should be debuffs, but they actually need to be buffs. The DoT itself is counted as a debuff, so this avoids DoTs counting as two debuffs.
        }
    }
}
