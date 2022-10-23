using R2API;
using System;
using ModdedEntityStates.Aliem;
using Modules.Survivors;
using System.Collections.Generic;

namespace Modules
{
    internal static class Tokens {

        public static void AddTokens()
        {
            AddAliemTokens();
        }

        private static void AddAliemTokens()
        {
            #region not henry
            string prefix = AliemPlugin.DEV_PREFIX + "_ALIEM_BODY_";

            string desc = "he gonna zap ya.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > goddammit jerry." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > use the Jump Attack to avoid damage and hit kniggas. is this gonna be annoying not being able to swing in the air? probably" + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > use the fireball to fill the empty space in his barren kit. seriously i gotta look at this thing." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > idk what R is but it's gonna be cool." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so it left, mothership pleased.";
            string outroFailure = "..and so it vanished, the feds won.";

            string fullName = "Alien Hominid";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "head chomper");
            LanguageAPI.Add(prefix + "LORE", "");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "skin?");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Joe passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_GUN_NAME", "Ray gun");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_DESCRIPTION", $"Shoot your ray gun for X damage");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BIGGUN_NAME", "Big gun");
            LanguageAPI.Add(prefix + "SECONDARY_BIGGUN_DESCRIPTION", $"Shoot a charged blast from your raygun for X damage");
            #endregion

            //#region Utility
            //LanguageAPI.Add(prefix + "UTILITY_LEAP_NAME", "Leap");
            //LanguageAPI.Add(prefix + "UTILITY_LEAP_DESCRIPTION", "Leap forward, mounting on to the first hit enemy. While riding, chomp for X damage");
            //#endregion

            //#region Special
            //LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Something Cool, I'm Sure");
            //LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticHenryValues.bombDamageCoefficient}% damage</style>.");
            //#endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");
            #endregion
            #endregion not henry
        }
    }
}