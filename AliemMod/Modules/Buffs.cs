using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules {

    internal static class Buffs
    {
        //Aliem
        public static BuffDef riddenBuff;
        public static BuffDef diveBuff;
        public static BuffDef attackSpeedBuff;

        public static void RegisterBuffs() {

            //Aliem
            riddenBuff =
                AddNewBuff("RiddenByAliem",
                           Assets.mainAssetBundle.LoadAsset<Sprite>("texIconBuffAliem"),
                           Color.yellow,
                           false,
                           false);

            diveBuff =
                AddNewBuff("AliemDive",
                           Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/texBuffGenericShield.tif").WaitForCompletion(),
                           Color.yellow,
                           false,
                           false);

            attackSpeedBuff =
                AddNewBuff("AliemAttackSpeedBuff",
                           Assets.mainAssetBundle.LoadAsset<Sprite>("texIconBuffAliemGun"),
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