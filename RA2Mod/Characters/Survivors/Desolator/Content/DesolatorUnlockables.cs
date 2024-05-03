using RA2Mod.Survivors.Desolator.Achievements;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorUnlockables
    {
        public static UnlockableDef masterySkinUnlockableDef;
        public static UnlockableDef grandMasterySkinUnlockableDef;
        public static UnlockableDef recolorsUnlockableDef;
        public static UnlockableDef characterUnlockableDef;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                DesolatorMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(DesolatorMasteryAchievement.identifier),
                DesolatorSurvivor.instance.assetBundle.LoadAsset<Sprite>("texDesolatorSkinMastery"));

            //grandMasterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
            //    TeslaGrandMasteryAchievement.unlockableIdentifier,
            //    Modules.Tokens.GetAchievementNameToken(TeslaGrandMasteryAchievement.identifier),
            //    TeslaTrooperSurvivor.instance.assetBundle.LoadAsset<Sprite>("texTeslaSkinNod"));
        }
    }
}