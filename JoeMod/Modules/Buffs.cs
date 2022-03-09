using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules {

    internal static class Buffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;
        public static BuffDef zapShieldBuff;

        public static BuffDef conductiveBuff;
        public static BuffDef conductiveBuffTeam;

        public static void RegisterBuffs()
        {
            armorBuff = 
                AddNewBuff("HenryArmorBuff", 
                           Assets.LoadAsset<Sprite>("Textures/BuffIcons/texBuffGenericShield"), 
                           Color.white, 
                           false, 
                           false);
            zapShieldBuff = 
                AddNewBuff("Tesla Barrier",
                           Assets.LoadAsset<Sprite>("Textures/BuffIcons/texBuffGenericShield"),
                           Color.cyan,
                           false,
                           false);

            if (FacelessJoePlugin.conductivePassive) {
                conductiveBuff =
                    AddNewBuff("Conductive",
                               Assets.LoadAsset<Sprite>("textures/bufficons/texbuffteslaicon"),
                               Color.blue,
                               true,
                               true);
                conductiveBuffTeam =
                    AddNewBuff("Charged",
                               Assets.LoadAsset<Sprite>("textures/bufficons/texbuffteslaicon"),
                               Color.cyan,
                               false,
                               true);
            }

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