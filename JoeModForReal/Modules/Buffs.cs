using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules {

    internal static class Buffs
    {
        public static void RegisterBuffs() {

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