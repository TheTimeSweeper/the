using RA2Mod.Survivors.Chrono.Achievements;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                ChronoMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ChronoMasteryAchievement.identifier),
                ChronoSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
