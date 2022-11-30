using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules {

    internal static class Buffs
    {
        public static BuffDef zapShieldBuff;
        
        public static BuffDef conductiveBuffTeam;
        public static BuffDef conductiveBuffTeamGrace;

        //public static BuffDef blinkCooldownBuff;

        public static BuffDef desolatorArmorBuff;
        public static BuffDef desolatorDeployBuff;

        public static BuffDef desolatorArmorShredDeBuff;
        public static BuffDef desolatorDotDeBuff;

        //Aliem
        public static BuffDef riddenBuff;

        public static void RegisterBuffs() {
            zapShieldBuff =
                AddNewBuff("Tesla Barrier",
                           LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                           Color.cyan,
                           false,
                           false);

            Sprite teslaIcon = LegacyResourcesAPI.Load<BuffDef>("BuffDefs/TeslaField").iconSprite;

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
            //blinkCooldownBuff =
            //    AddNewBuff("BlinkCooldown",
            //               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Cloak").iconSprite,
            //               Color.gray,
            //               false,
            //               false);

            if (FacelessJoePlugin.Desolator) {
                Color lime = new Color(0.486f, 1, 0);

                desolatorArmorBuff =
                    AddNewBuff("DesolatorArmor",
                               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                               lime,
                               false,
                               false);

                desolatorDeployBuff =
                    AddNewBuff("DesolatorArmorMini",
                               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                               lime,
                               false,
                               false);

                desolatorArmorShredDeBuff =
                    AddNewBuff("DesolatorArmorShred",
                               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Pulverized").iconSprite,
                               lime,
                               true,
                               true);

                desolatorDotDeBuff =
                    AddNewBuff("DesolatorIrradiated",
                               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Blight").iconSprite,
                               lime,
                               true,
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