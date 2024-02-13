using RA2Mod.Survivors.GI.Achievements;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.GI
{
    public static class GIUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                GIMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(GIMasteryAchievement.identifier),
                GISurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
