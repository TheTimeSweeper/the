using System;
using RA2Mod.Modules;
using RA2Mod.Survivors.Chrono.Achievements;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoTokens
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
            string prefix = ChronoSurvivor.TOKEN_PREFIX;

            string desc = "The Chrono Legionnaire uses manipulation of time to shake up the battlefield.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > I highly recommend setting sprint to alt or on your mouse if you can." + Environment.NewLine + Environment.NewLine
             + "< ! > I.V.A.N. bombs are great for area damage. Blink in, place one, and blink out." + Environment.NewLine + Environment.NewLine
             + "< ! > Chronosphere can be used for movement, for defense, or for grouping up enemies to better be exploded by bombs." + Environment.NewLine + Environment.NewLine
             + "< ! > While building up Chrono Sickness can help execute enemies with special, dealing damage is still important to get them to that threshold." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, without a trace.";
            string outroFailure = "..and so he vanished, as if he never existed.";

            Language.Add(prefix + "NAME", "Chrono Legionnaire BETA");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Time Gun");
            Language.Add(prefix + "LORE", "Never Existed");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_SPRINT_NAME", "Already There");
            Language.Add(prefix + "PASSIVE_SPRINT_DESCRIPTION", $"Instead of sprinting, you time-skip from place to place, after which, you are {Tokens.UtilityText("disabled")} for a short time based on distance.");
            #endregion


            #region Primary
            Language.Add(prefix + "PRIMARY_SHOOT_NAME", "Chrono Gun");
            Language.Add(prefix + "PRIMARY_SHOOT_DESCRIPTION", $"Fire a time-stopping rifle for {Tokens.DamageValueText(ChronoConfig.M1_Shoot_Damage.Value)}. Enemies hit are applied {Tokens.UtilityText("Chrono Sickness")}.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_BOMB_NAME", "Chrono I.V.A.N.");
            Language.Add(prefix + "SECONDARY_BOMB_DESCRIPTION", $"Attach a pulse bomb to an enemy. After 3 seconds, the bomb explodes for {Tokens.DamageValueText(ChronoConfig.M2_Bomb_Damage.Value)}.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_CHRONOSPHERE_NAME", "Chronosphere");
            Language.Add(prefix + "UTILITY_CHRONOSPHERE_DESCRIPTION", $"Highlight an area, then highlight a second area. {Tokens.UtilityText("All units")} in the first area are teleported to the second area.");
            
            Language.Add(prefix + "UTILITY_FREEZOSPHERE_NAME", "Suspend Sphere");
            Language.Add(prefix + "UTILITY_FREEZOSPHERE_DESCRIPTION", $"{Tokens.UtilityText("Stop time")} for enemies and projecitles for {Tokens.UtilityText($"{ChronoConfig.M3_Freezosphere_FreezeDuration.Value} seconds")}.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_VANISH_NAME", "Deconstructing");
            int ticks = UnityEngine.Mathf.RoundToInt(ChronoConfig.M4_Vanish_Duration.Value / ChronoConfig.M4_Vanish_TickInterval.Value);
            Language.Add(prefix + "SPECIAL_VANISH_DESCRIPTION", $"Deal {Tokens.DamageValueText(ChronoConfig.M4_Vanish_TickDamage.Value * ticks)} and apply {Tokens.UtilityText("Chrono Sickness")} over {ChronoConfig.M4_Vanish_Duration.Value} seconds. If an enemy’s health is below the {Tokens.UtilityText("Chrono Sickness")} threshold, they vanish from existence.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(ChronoMasteryAchievement.identifier), "Chrono Legionnaire: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(ChronoMasteryAchievement.identifier), "As Chrono Legionnaire, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
