using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules {

    internal static class Buffs
    {
        internal static BuffDef TenticleBuff;

        internal static BuffDef DashArmorBuff;

        public static void RegisterBuffs() {
            TenticleBuff = AddNewBuff("JoeTenticle",
                                      Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/WarCryOnMultiKill/bdWarCryBuff.asset").WaitForCompletion().iconSprite,
                                      Color.green,
                                      true,
                                      false);

            DashArmorBuff = AddNewBuff("JoeDashArmor",
                                   LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                                   Color.magenta,
                                   false,
                                   false);
        }

        public static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff) {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.iconSprite = buffIcon;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;

            Modules.Content.AddBuffDef(buffDef);

            return buffDef;
        }
    }
}