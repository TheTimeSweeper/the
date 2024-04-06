using KatamariMod.Survivors.Katamari.Achievements;
using RoR2;
using UnityEngine;

namespace KatamariMod.Survivors.Katamari {
    public static class KatamariUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                KatamariMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(KatamariMasteryAchievement.identifier),
                KatamariSurvivor.instance.assetBundle.LoadAsset<Sprite>("texHenryIcon"));
        }
    }
}
