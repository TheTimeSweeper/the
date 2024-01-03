using PlagueMod.Survivors.Plague.Achievements;
using RoR2;
using UnityEngine;

namespace PlagueMod.Survivors.Plague {
    public static class PlagueUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                PlagueMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(PlagueMasteryAchievement.identifier),
                PlagueSurvivor.instance.assetBundle.LoadAsset<Sprite>("texHenryIcon"));
        }
    }
}
