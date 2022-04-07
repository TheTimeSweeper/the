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
        public static BuffDef conductiveBuffTeamGrace;

        public static void RegisterBuffs()
        {
            zapShieldBuff = 
                AddNewBuff("Tesla Barrier",
                           LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                           Color.cyan,
                           false,
                           false);

            if (FacelessJoePlugin.conductiveMechanic) {

                Sprite teslaIcon = LegacyResourcesAPI.Load<BuffDef>("BuffDefs/TeslaField").iconSprite;

                conductiveBuff =
                    AddNewBuff("Conductive",
                               teslaIcon,
                               Color.blue,
                               true,
                               true);
                conductiveBuffTeam =
                    AddNewBuff("Charged",
                               teslaIcon,
                               Color.cyan,
                               false,
                               false);
                conductiveBuffTeamGrace =
                    AddNewBuff("Charged2",
                               teslaIcon,
                               Color.blue,
                               false,
                               false);
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