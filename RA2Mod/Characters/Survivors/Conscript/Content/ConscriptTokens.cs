using System;
using RA2Mod.Modules;
using RA2Mod.Survivors.GI.Achievements;

namespace RA2Mod.Survivors.Conscript
{
    public static class ConscriptTokens
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
            //string prefix = GISurvivor.GI_PREFIX;

            //string desc = "ra2 quote.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
            // + "< ! > ra2 quote." + Environment.NewLine + Environment.NewLine
            // + "< ! > ra2 quote." + Environment.NewLine + Environment.NewLine
            // + "< ! > ra2 quote." + Environment.NewLine + Environment.NewLine
            // + "< ! > ra2 quote." + Environment.NewLine + Environment.NewLine;

            //string outro = "..and so he left, ra2 quote.";
            //string outroFailure = "..and so he vanished, ra2 quote.";

            //Language.Add(prefix + "NAME", "G.I.");
            //Language.Add(prefix + "DESCRIPTION", desc);
            //Language.Add(prefix + "SUBTITLE", "ra2 quote");
            //Language.Add(prefix + "LORE", "ra2 quote");
            //Language.Add(prefix + "OUTRO_FLAVOR", outro);
            //Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            //#region Skins
            //Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            //#endregion

            //#region Passive
            //Language.Add(prefix + "PASSIVE_NAME", "Henry passive");
            //Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            //#endregion
            
            //#region Primary
            //string heavyGun = $"Fire Heavy Machine Gun for {Tokens.DamageValueText(GIConfig.M1_HeavyFire_Damage.Value)}.";
            //Language.Add(prefix + "PRIMARY_GUN_NAME", "Gun");
            //Language.Add(prefix + "PRIMARY_GUN_DESCRIPTION", $"Fire Pistol for {Tokens.DamageText($"{GIConfig.M1_Pistol_Shots.Value}x{GIConfig.M1_Pistol_Damage.Value * 100}% damage")}.\nWhile deployed, {heavyGun}");
            //Language.Add(prefix + "PRIMARY_GUN_HEAVY_NAME", "Gun Heavy");
            //Language.Add(prefix + "PRIMARY_GUN_HEAVY_DESCRIPTION", heavyGun);

            //string heavyMissile = $"Fire Heavy Missile for {Tokens.DamageValueText(GIConfig.M1_HeavyMissile_Damage.Value)}.";
            //Language.Add(prefix + "PRIMARY_ROCKET_NAME", "Missile");
            //Language.Add(prefix + "PRIMARY_ROCKET_DESCRIPTION", $"Fire a rocket for {Tokens.DamageValueText(GIConfig.M1_Missile_Damage.Value)}.\nWhile deployed, {heavyMissile}");
            //Language.Add(prefix + "PRIMARY_ROCKET_HEAVY_NAME", "Missile heavy");
            //Language.Add(prefix + "PRIMARY_ROCKET_HEAVY_NAME", heavyMissile);
            //#endregion

            //#region Secondary
            //string heavyMine = $"Throw a {Tokens.DamageText("stunning")} mine for {Tokens.DamageValueText(GIConfig.M2_Mine_Damage.Value)}.";
            //Language.Add(prefix + "SECONDARY_CALTROPS_NAME", "Caltrops");
            //Language.Add(prefix + "SECONDARY_CALTROPS_DESCRIPTION", $"Throw {Tokens.DamageText("slowing")} caltrops, dealing {Tokens.DamageValueText(GIConfig.M2_Caltrops_DotDamage.Value * 3 * 3)} over {3} seconds.\nWhile deployed, {heavyMine}");
            //Language.Add(prefix + "SECONDARY_MINE_NAME", "Mine");
            //Language.Add(prefix + "SECONDARY_MINE_DESCRIPTION", heavyMine);
            //#endregion

            //#region Utility
            //Language.Add(prefix + "UTILITY_SLIDE_NAME", "Basically Commando Slide");
            //Language.Add(prefix + "UTILITY_SLIDE_DESCRIPTION", $"Commando Slide but in the air as well");
            //#endregion

            //#region Special
            //Language.Add(prefix + "SPECIAL_DEPLOY_NAME", "Deploy");
            //Language.Add(prefix + "SPECIAL_DEPLOY_DESCRIPTION", $"Deploy and swap skills and dont move and get {GIConfig.M4_Transform_Armor.Value} armor.");
            //Language.Add(prefix + "SPECIAL_UNDEPLOY_NAME", "UnDeploy");
            //Language.Add(prefix + "SPECIAL_UNDEPLOY_DESCRIPTION", $"Go back I want to b emon ke.");
            //#endregion

            //#region Achievements
            //Language.Add(Tokens.GetAchievementNameToken(GIMasteryAchievement.identifier), "G.I.: Mastery");
            //Language.Add(Tokens.GetAchievementDescriptionToken(GIMasteryAchievement.identifier), "As G.I., beat the game or obliterate on Monsoon.");
            //#endregion
        }
    }
}
