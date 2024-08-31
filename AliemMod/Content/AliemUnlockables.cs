using AliemMod.Content.Achievements;
using RoR2;
using UnityEngine;

namespace AliemMod.Content
{
    public class AliemUnlockables
    {
        public const string AliemPrerequisiteAchievementIdentifier = null;

        //public static UnlockableDef characterUnlockableDef;
        public static UnlockableDef masterySkinUnlockableDef;
        //public static UnlockableDef grandMasterySkinUnlockableDef;
        public static UnlockableDef ChompEnemiesUnlockableDef;
        public static UnlockableDef BurrowPopOutUnlockableDef;
        public static UnlockableDef SlowMashUnlockableDef;
        public static UnlockableDef ChargedKillUnlockableDef;

        public const string DevResetString = "2";

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                AliemMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(AliemMasteryAchievement.identifier),
                Modules.AliemAssets.mainAssetBundle.LoadAsset<Sprite>("texIconMasteryAchievement"));

            ChompEnemiesUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                AliemChompEnemiesAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(AliemChompEnemiesAchievement.identifier),
                Modules.AliemAssets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimarySword"));

            BurrowPopOutUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                AliemBurrowPopOutAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(AliemBurrowPopOutAchievement.identifier),
                Modules.AliemAssets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimaryRifle"));

            ChargedKillUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                AliemChargedKillAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(AliemChargedKillAchievement.identifier),
                Modules.AliemAssets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimarySawedOff"));

            SlowMashUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                AliemSlowMashAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(AliemSlowMashAchievement.identifier),
                Modules.AliemAssets.mainAssetBundle.LoadAsset<Sprite>("texIconAliemPrimaryGun"));
        }
    }
}