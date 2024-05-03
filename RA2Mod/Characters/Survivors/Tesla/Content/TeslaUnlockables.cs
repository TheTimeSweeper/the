using RA2Mod.General;
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
        public static UnlockableDef recolorsUnlockableDef;
        public static UnlockableDef utilityUnlockableDef;
        public static UnlockableDef secondaryUnlockableDef;
        public static UnlockableDef cursedPrimaryUnlockableDef;

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

            utilityUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                TeslaTowerBigZapAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(TeslaTowerBigZapAchievement.identifier),
                TeslaTrooperSurvivor.instance.assetBundle.LoadAsset<Sprite>("texTeslaSkillUtilityAlt"));
            
            secondaryUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                TeslaShieldZapKillAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(TeslaShieldZapKillAchievement.identifier),
                TeslaTrooperSurvivor.instance.assetBundle.LoadAsset<Sprite>("texTeslaSkillUtilityAlt"));

            //if (false)//GeneralConfig.Cursed.Value)
            //{
                cursedPrimaryUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                TeslaAllyZapAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(TeslaAllyZapAchievement.identifier),
                TeslaTrooperSurvivor.instance.assetBundle.LoadAsset<Sprite>("texTeslaSkillSecondaryAlt")); 
            //}
        }
    }
}
