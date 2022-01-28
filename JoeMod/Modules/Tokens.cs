using R2API;
using System;
using HenryMod.ModdedEntityStates.Joe;

namespace HenryMod.Modules
{
    internal static class Tokens {

        internal static void AddTokens()
        {
            AddJoeTokens();
            AddTeslaTokens();
        }

        private static void AddJoeTokens()
        {
            #region not henry
            string prefix = FacelessJoePlugin.developerPrefix + "_JOE_BODY_";

            string desc = "joe has a funny vertex on his face that's painted wrong.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > goddammit jerry." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > use the Jump Attack to avoid damage and hit kniggas. this gonna be annoying not being able to swing in the air? probably" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > use the fireball to fill the empty space in his barren kit. seriously i gotta look at this thing." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > idk what R is but it's gonna be cool." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, jerrys' screams lingering in his hears.";
            string outroFailure = "..and so he vanished, his normals never recalculated outside.";

            string fullName = "Faceless Joe";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "and the zambambos");
            LanguageAPI.Add(prefix + "LORE", "All the best charcoals come from coconuts. They're easy to use and it's easy to grow more. You don't have to chop down an entire tree just to get your charcoal. But I need to, because the charcoals I want can't come from any coconuts. They need to have the perfect lighting temperature, the perfect lifetime, the perfect shape, the perfect flavor-capturing smoke. No, the charcoals I need can only be made from special trees. Trees in a forest just over this hill, habibi. But we're not gonna burn it down, absolutely not. If the trees are gone, how are we gonna get any charcoal? No, we can’t destroy the trees and take from them. We want them to be happy, to help us achieve our dreams of the perfect smoke because they like us. Make sense? Didn’t think so, hehaha. Nice O’s, you’re getting better at those. Anyways, in this forest, there's someone who’s been able to earn the trust of the trees. Maybe if we can hand him a hose, and he’ll put his swords down and join us, haha.");
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

            //I get it makes sense to have all the tokens nice and neat in one place but it's kinda asinine to have these separate from the skilldefs
            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SWING_NAME", "Swing");
            LanguageAPI.Add(prefix + "PRIMARY_SWING_DESCRIPTION", $"{Helpers.agilePrefix} Swing your sword for <style=cIsDamage>{100f * Primary1Swing.swingDamage}% damage</style>.\n use in the air for a <style=cIsUtility>Falling Jump Attack</style>");

            LanguageAPI.Add(prefix + "PRIMARY_SWING_NAME_CLASSIC", "Swing Classic");

            LanguageAPI.Add(prefix + "PRIMARY_BOMB_NAME", "Throw");
            LanguageAPI.Add(prefix + "PRIMARY_BOMB_DESCRIPTION", "is it happening?");

            LanguageAPI.Add(prefix + "PRIMARY_ZAP_NAME", "S.I.C.K.L.E.");
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_DESCRIPTION", "it is happening");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_FIREBALL_NAME", "Fireball");
            LanguageAPI.Add(prefix + "SECONDARY_FIREBALL_DESCRIPTION", $"{Helpers.agilePrefix} Fire a ball for <style=cIsDamage>{100f * StaticHenryValues.gunDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_NAME", "Big Zap");
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_DESCRIPTION", $"2000 Volts");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_DASH_NAME", "Dash");
            LanguageAPI.Add(prefix + "UTILITY_DASH_DESCRIPTION", "very original, i know");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_NAME", "Tesla Tower");
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_DESCRIPTION", "Construction Complete");

            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Something Cool, I'm Sure");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticHenryValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");
            #endregion
            #endregion
        }

        private static void AddTeslaTokens()
        {
            #region not henry 2
            string prefix = FacelessJoePlugin.developerPrefix + "_TESLA_BODY_";

            string desc = "Tesla Trooper Ready.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Charging Up." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Let the Juice Flow" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Electrodes Ready" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Construction Complete." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, rubber shoes in motion.";
            string outroFailure = "..and so he vanished, unit lost.";

            string fullName = "Tesla Trooper";
            LanguageAPI.Add(prefix + "NAME", fullName);
            //todo what the fuck is it teslatrooper when my prefix is tesla
            LanguageAPI.Add("HABIBI_TESLATROOPER_BODY_NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Electrician In the Field");
            LanguageAPI.Add(prefix + "LORE", ".");
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

            //I get it makes sense to have all the tokens nice and neat in one place but it's kinda asinine to have these separate from the skilldefs
            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_NAME", "Tesla Gauntlet");
            LanguageAPI.Add(prefix + "PRIMARY_ZAP_DESCRIPTION", "it is happening");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_NAME", "2000 Volts");
            LanguageAPI.Add(prefix + "SECONDARY_BIGZAP_DESCRIPTION", $"coming up");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_BARRIER_NAME", "Charging Up");
            LanguageAPI.Add(prefix + "UTILITY_BARRIER_DESCRIPTION", "Electrodes Ready");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_NAME", "Tesla Tower");
            LanguageAPI.Add(prefix + "SPECIAL_TOWER_DESCRIPTION", "Construction Complete (does nothing yet but has colliders)");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");
            #endregion
            #endregion not henry 2
        }

        internal static void AddHenryTokens()
        {
            #region Henry
            string prefix = FacelessJoePlugin.developerPrefix + "_HENRY_BODY_";

            string desc = "Henry is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            LanguageAPI.Add(prefix + "NAME", "Henry");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Chosen One");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Henry passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * StaticHenryValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticHenryValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticHenryValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            #endregion
            #endregion
        }
    }
}