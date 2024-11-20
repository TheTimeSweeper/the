using MatcherMod.Survivors.Matcher.Achievements;
using RoR2;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Content
{
    public static class CharacterUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                MasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(MasteryAchievement.identifier),
                MatcherSurvivor.instance.assetBundle.LoadAsset<Sprite>("texIconSkinMatcherClassic"));
        }
    }
}
