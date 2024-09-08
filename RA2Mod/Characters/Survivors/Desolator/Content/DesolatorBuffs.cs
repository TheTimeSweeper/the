using RA2Mod.General;
using RA2Mod.Modules;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorBuffs
    {
        public static BuffDef desolatorArmorBuff;
        public static BuffDef desolatorDeployBuff;

        public static BuffDef desolatorArmorShredDeBuff;
        public static BuffDef desolatorDotDeBuff;

        public static void Init(AssetBundle assetBundle)
        {
            Color lime = new Color(0.486f, 1, 0);

            desolatorArmorBuff =
                Content.CreateAndAddBuff("DesolatorArmor",
                           LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                           lime,
                           false,
                           false);

            string buffPath = GeneralConfig.RA2Icon.Value ? "texBuffDesolatorDeployClassic2" : "texBuffDesolatorDeploy";
            Sprite buffIcon = assetBundle.LoadAsset<Sprite>(buffPath);
            desolatorDeployBuff =
                Content.CreateAndAddBuff("DesolatorArmorMini",
                           buffIcon,
                           GeneralConfig.RA2Icon.Value ? Color.white : lime,
                           false,
                           false);

            desolatorArmorShredDeBuff =
                Content.CreateAndAddBuff("DesolatorArmorShred",
                           LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Pulverized").iconSprite,
                           lime,
                           true,
                           true);

            desolatorDotDeBuff =
                Content.CreateAndAddBuff("DesolatorIrradiated",
                           assetBundle.LoadAsset<Sprite>("texBuffDesolatorRadiation"),
                           lime,
                           true,
                           false);
        }
    }
}