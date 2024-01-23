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
            string prefix = ChronoSurvivor.CHRONO_PREFIX;

            string desc = "The Chrono Legionnaire uses manipulation of time to shake up the battlefield.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > I highly recommend setting sprint to alt or on your mouse if you can." + Environment.NewLine + Environment.NewLine
             + "< ! > I.V.A.N. bombs are great for area damage. Blink in, place one, and blink out." + Environment.NewLine + Environment.NewLine
             + "< ! > Chronosphere can be used for movement, for defense, or for grouping up enemies to better be exploded by bombs." + Environment.NewLine + Environment.NewLine
             + "< ! > While building up [Chrono Sickness] can help execute enemies with special, dealing damage is still important to get them to that threshold." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, without a trace.";
            string outroFailure = "..and so he vanished, as if he never existed.";

            Language.Add(prefix + "NAME", "Chrono Legionnaire");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Chosen One");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_SPRINT_NAME", "Already Gone");
            Language.Add(prefix + "PASSIVE_SPRINT_DESCRIPTION", "Instead of sprinting, you time-skip from place to place, after which, you are Disabled for a short time based on distance.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SHOOT_NAME", "Neutron Rifle");
            Language.Add(prefix + "PRIMARY_SHOOT_DESCRIPTION", $"Fire a time-stopping rifle for % damage. Enemies hit are applied [Chrono Sickness] for x% of their health.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_BOMB_NAME", "Chrono I.V.A.N.");
            Language.Add(prefix + "SECONDARY_BOMB_DESCRIPTION", $"Attach a pulse bomb to an enemy. After a short delay, the bomb explodes for x% damage.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_CHRONOSPHERE_NAME", "Chronosphere");
            Language.Add(prefix + "UTILITY_CHRONOSPHERE_DESCRIPTION", "Highlight an area, then highlight a second area. Enemies and Allies in the first area are teleported to the second area.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_VANISH_NAME", "Never Existed");
            Language.Add(prefix + "SPECIAL_VANISH_DESCRIPTION", $"Shortly focus your rifle on an enemy. Dealing up to x% damage and continuously applying [Chrono Sickness] for up to x% their health. If an enemy’s health is below the [Chrono Sickness] threshold, they vanish from existence.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(ChronoMasteryAchievement.identifier), "Henry: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(ChronoMasteryAchievement.identifier), "As Henry, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
