using RA2Mod.Modules;
using RA2Mod.Survivors.Chrono.Achievements;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;
        public static UnlockableDef recolorsUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                ChronoMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ChronoMasteryAchievement.identifier),
                null);

            Modules.ContentPacks.asyncLoadCoroutines.Add(ChronoSurvivor.instance.assetBundle.LoadAssetCoroutine<Sprite>("texMasteryAchievement", (result) =>
            {
                masterySkinUnlockableDef.achievementIcon = result;
            }));
        }
    }
}
