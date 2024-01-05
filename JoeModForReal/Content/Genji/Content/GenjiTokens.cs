using R2API;
using System;

namespace JoeModForReal.Content.Survivors {

    public static class GenjiTokens {

        public static void Init() {
            string prefix = GenjiSurvivor.GENJI_PREFIX;

            string desc = "Genji Overwatch.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > yes I know his last name is shimada" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > shut up nerd" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > though I'm the bigger nerd for knowing that" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > anyway reset kills on dash to zip zoom all around" + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, not doing a need healing joke.";
            string outroFailure = "..and so he vanished, .";

            string fullName = "Genji";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "the dragon of swag");
            LanguageAPI.Add(prefix + "LORE",
                "My family tells of an ancient legend about two dank dragon brothers. The Dragon of Swag, and the Dragon of Class. Together they fucked bitches and tipped fedoras like it's 2014.");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "skin?");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Cyber Agility");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Genji can Double Jump and Sprint in all directions.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SHURIKEN_NAME", "Shuriken");
            LanguageAPI.Add(prefix + "PRIMARY_SHURIKEN_DESCRIPTION", $"{Helpers.agilePrefix} Throw 3 shurikens in a line for {Helpers.DamageValueText(GenjiConfig.shurikenDamage.Value)}. While sprinting, throw a fan of 3 shurikens at once.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_FIREBALL_NAME", "Deflect");
            LanguageAPI.Add(prefix + "SECONDARY_FIREBALL_DESCRIPTION", $"Deflect all attacks to where you're aiming. Yep all attacks not even factoring direction right now.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_DASH_NAME", "Dash");
            LanguageAPI.Add(prefix + "UTILITY_DASH_DESCRIPTION", $"Dash through enemies for {Helpers.DamageValueText(GenjiConfig.dashDamage.Value)}. {Helpers.UtilityText("Cooldown is reset on kills")}.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_DRAGONBLADE_NAME", "DragonBlade");
            LanguageAPI.Add(prefix + "SPECIAL_DRAGONBLADE_DESCRIPTION", $"Unsheathe a deadly melee weapon for {Helpers.UtilityText($"{GenjiConfig.dragonBladeDuration.Value} seconds")}, which deals {Helpers.DamageValueText(GenjiConfig.dragonBladeDamage.Value)}. {Helpers.UtilityText("must be charged by dealing damage to enemies")}");

            LanguageAPI.Add(prefix + "SPECIAL_SWINGBLADE_NAME", "DragonBlade");
            LanguageAPI.Add(prefix + "SPECIAL_SWINGBLADE_DESCRIPTION", $"Swing for {Helpers.DamageValueText(GenjiConfig.dragonBladeDamage.Value)}");

            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");
            #endregion
        }
    }
}