using System;
using ModdedEntityStates.Joe;

namespace Modules {
    internal static class Tokens {

        public static void GenerateTokens() {

            AddJoeTokens();
            Language.PrintOutput("Joe tokens:");

        }

        private static void AddJoeTokens() {
            #region not henry
            string prefix = JoeModForReal.Content.Survivors.JoeSurivor.JOE_PREFIX;

            string desc = "joe has a funny vertex on his face that's painted wrong.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > goddammit jerry." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > use the Jump Attack to avoid damage and hit kniggas. is this gonna be annoying not being able to swing in the air? probably" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > use the fireball to fill the empty space in his barren kit. seriously i gotta look at this thing." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > idk what R is but it's gonna be cool." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, jerrys' screams lingering in his hears.";
            string outroFailure = "..and so he vanished, his normals never recalculated outside.";

            string fullName = "Faceless Joe";
            Language.Add(prefix + "NAME", fullName);
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "and the zambambos");
            Language.Add(prefix + "LORE", "All the best charcoals come from coconuts. They're easy to use and it's easy to grow more. You don't have to chop down an entire tree just to get your charcoal. But I need to, because the charcoals I want can't come from any coconuts. They need to have the perfect lighting temperature, the perfect lifetime, the perfect shape, the perfect flavor-capturing smoke. No, the charcoals I need can only be made from special trees. Trees in a forest just over this hill, habibi. But we're not gonna burn it down, absolutely not. If the trees are gone, how are we gonna get any charcoal? No, we can’t destroy the trees and take from them. We want them to be happy, to help us achieve our dreams of the perfect smoke because they like us. Make sense? Didn’t think so, hehaha. Nice O’s, you’re getting better at those. Anyways, in this forest, there's someone who’s been able to earn the trust of the trees. Maybe if we hand him a hose, and he’ll put his swords down and join us, haha.");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "skin?");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Joe passive");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SWING_NAME", "Swing");
            Language.Add(prefix + "PRIMARY_SWING_DESCRIPTION", $"{Helpers.agilePrefix} Swing your sword for <style=cIsDamage>{100f * Primary1Swing.swingDamage}% damage</style>.\n Use <style=cIsUtility>in the air while looking down</style> for a <style=cIsUtility>Falling Jump Attack</style>");

            Language.Add(prefix + "PRIMARY_SWING_NAME_CLASSIC", "Swing Classic");

            Language.Add(prefix + "PRIMARY_BOMB_NAME", "Throw");
            Language.Add(prefix + "PRIMARY_BOMB_DESCRIPTION", "is it happening?");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_FIREBALL_NAME", "Fireball");
            Language.Add(prefix + "SECONDARY_FIREBALL_DESCRIPTION", $"{Helpers.agilePrefix} Fire a ball for <style=cIsDamage>{100f * Secondary1Fireball.damageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_DASH_NAME", "Dash");
            Language.Add(prefix + "UTILITY_DASH_DESCRIPTION", $"Dash a short distance. If primary input is held, charge up a {Helpers.UtilityText("Melee Dash Attack")} for {Helpers.DamageText("100-500% damage")}.");
            #endregion

            #region Special

            //Language.Add(prefix + "SPECIAL_BOMB_NAME", "Something Cool, I'm Sure");
            //Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticHenryValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            Language.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            Language.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            Language.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");
            #endregion
            #endregion not henry
        }
    }
}