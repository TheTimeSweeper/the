using RA2Mod.Survivors.Conscript.Achievements;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Conscript
{
    public static class ConscriptUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                ConscriptMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ConscriptMasteryAchievement.identifier),
                ConscriptSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
