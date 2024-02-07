using System;
using RA2Mod.Modules;
using RA2Mod.Survivors.GI.Achievements;

namespace RA2Mod.Survivors.GI
{
    public static class GITokens
    {
        public static void Init()
        {
            AddHenryTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddHenryTokens()
        {
            string prefix = GISurvivor.GI_PREFIX;

            string desc = "ra2 quote.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > ra2 quote." + Environment.NewLine + Environment.NewLine
             + "< ! > ra2 quote." + Environment.NewLine + Environment.NewLine
             + "< ! > ra2 quote." + Environment.NewLine + Environment.NewLine
             + "< ! > ra2 quote." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, ra2 quote.";
            string outroFailure = "..and so he vanished, ra2 quote.";

            Language.Add(prefix + "NAME", "G.I.");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "ra2 quote");
            Language.Add(prefix + "LORE", "ra2 quote");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Henry passive");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            string heavyGun = $"Fire Heavy Machine Gun for {Tokens.DamageValueText(GIConfig.M1HeavyFireDamage.Value)}.";
            Language.Add(prefix + "PRIMARY_GUN_NAME", "Gun");
            Language.Add(prefix + "PRIMARY_GUN_DESCRIPTION", $"Fire Pistol for {Tokens.DamageText($"{GIConfig.M1PistolShots.Value}x{GIConfig.M1PistolDamage.Value * 100}% damage")}.\nWhile deployed, {heavyGun}");
            Language.Add(prefix + "PRIMARY_GUN_HEAVY_NAME", "Gun Heavy");
            Language.Add(prefix + "PRIMARY_GUN_HEAVY_DESCRIPTION", heavyGun);
            #endregion

            #region Secondary
            string heavyMissile = $"Fire Heavy Missile for {Tokens.DamageValueText(GIConfig.M1MissileDamage.Value)}.";
            Language.Add(prefix + "SECONDARY_ROCKET_NAME", "Missile");
            Language.Add(prefix + "SECONDARY_ROCKET_DESCRIPTION", $"Fire Literally Mul-T scrap launcher for {Tokens.DamageValueText(3.6f)}.\nWhile deployed, {heavyMissile}");
            Language.Add(prefix + "SECONDARY_ROCKET_HEAVY_NAME", "Missile heavy");
            Language.Add(prefix + "SECONDARY_ROCKET_HEAVY_NAME", heavyMissile);
            #endregion

            #region Utility
            string heavySlide = $"Throw Literally Engi Mine for {Tokens.DamageText("300-900% damage")}.";
            Language.Add(prefix + "UTILITY_SLIDE_NAME", "Literally Commando Slide");
            Language.Add(prefix + "UTILITY_SLIDE_DESCRIPTION", $"Literally Commando Slide \nWhile deployed, {heavySlide}");
            Language.Add(prefix + "UTILITY_MINE_NAME", "Literally Engi Mine");
            Language.Add(prefix + "UTILITY_MINE_DESCRIPTION", heavySlide);
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_DEPLOY_NAME", "Deploy");
            Language.Add(prefix + "SPECIAL_DEPLOY_DESCRIPTION", $"Deploy and swap skills and dont move and get {GIConfig.M4TransformArmor.Value} armor.");
            Language.Add(prefix + "SPECIAL_UNDEPLOY_NAME", "UnDeploy");
            Language.Add(prefix + "SPECIAL_UNDEPLOY_DESCRIPTION", $"Go back I want to b emonk e.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier), "Henry: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(HenryMasteryAchievement.identifier), "As Henry, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
