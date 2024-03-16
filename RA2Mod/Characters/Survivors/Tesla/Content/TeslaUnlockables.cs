using RA2Mod.Survivors.Tesla.Achievements;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla
{
    public static class TeslaUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef;
        public static UnlockableDef grandMasterySkinUnlockableDef;
        internal static UnlockableDef recolorsUnlockableDef;
        internal static UnlockableDef utilityUnlockableDef;
        internal static UnlockableDef secondaryUnlockableDef;
        internal static UnlockableDef cursedPrimaryUnlockableDef;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                TeslaMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(TeslaMasteryAchievement.identifier),
                TeslaTrooperSurvivor.instance.assetBundle.LoadAsset<Sprite>("texTeslaSkinMastery"));

            grandMasterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                TeslaGrandMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(TeslaGrandMasteryAchievement.identifier),
                TeslaTrooperSurvivor.instance.assetBundle.LoadAsset<Sprite>("texTeslaSkinNod"));
        }
    }
}
