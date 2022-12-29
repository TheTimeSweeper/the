using System;
using JoeModForReal.Content.Survivors;
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

            string desc = "joe is a fast-paced in-your-face melee character. never seen any of those before.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > use your primary jump attack when it's dangerous to engage with regular attacks." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > the fireball does hefty damage, but is more useful to get enemies away from you" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > dash can be used freely to mitigate some damage from hits. use the charged melee version to get to enemies up in the air." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > tenticles ward will help you get out of a pinch." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, jerrys' screams lingering in his hears.";
            string outroFailure = "..and so he vanished, his normals never recalculated outside.";

            string fullName = "Faceless Joe";
            Language.Add(prefix + "NAME", fullName);
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "and the Zambambos");
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
            Language.Add(prefix + "PRIMARY_SWING_DESCRIPTION", $"{Helpers.agilePrefix} Swing your sword for <style=cIsDamage>{100f * Primary1Swing.swingDamage}% damage</style>.\n Use <style=cIsUtility>in the air while looking down</style> to perform a <style=cIsUtility>Falling Jump Attack</style> for {Helpers.DamageValueText(Primary1Swing.jumpSwingDamage)}");

            Language.Add(prefix + "PRIMARY_SWING_NAME_CLASSIC", "Swing Classic");

            Language.Add(prefix + "PRIMARY_BOMB_NAME", "Throw");
            Language.Add(prefix + "PRIMARY_BOMB_DESCRIPTION", "is it happening?");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_FIREBALL_NAME", "Fireball");
            Language.Add(prefix + "SECONDARY_FIREBALL_DESCRIPTION", $"{Helpers.agilePrefix} Fire a ball for <style=cIsDamage>{100f * Secondary1Fireball.damageCoefficient}% damage</style>, with heavy knock back.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_DASH_NAME", "Dash");
            Language.Add(prefix + "UTILITY_DASH_DESCRIPTION", $"Dash a short distance. If primary input is held, charge up a {Helpers.UtilityText("Melee Dash Attack")} for {Helpers.DamageText("3x100-500% damage")}.");
            #endregion

            #region Special

            Language.Add(prefix + "SPECIAL_TENTICLES_NAME", "Tenticles");
            Language.Add(prefix + "SPECIAL_TENTICLES_DESCRIPTION", $"Perform a ritual, summoning tentacles that increase your {Helpers.UtilityText("Move Speed")}, {Helpers.UtilityText("Jump Height")}, and grants {Helpers.UtilityText("Life Steal")} up to {JoeSurivor.TenticleMaxHealthMultiplier * 100}% max health.");
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