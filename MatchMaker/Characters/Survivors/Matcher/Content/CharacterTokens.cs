using System;
using MatcherMod.Modules;
using MatcherMod.Survivors.Matcher.Achievements;

namespace MatcherMod.Survivors.Matcher.Content
{
    public static class CharacterTokens
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
            string prefix = MatcherSurvivor.TOKEN_PREFIX;

            string desc = "The Match Maker matches tiles to increase the potency of his abilities.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Match sword tiles to be epic." + Environment.NewLine + Environment.NewLine
             + "< ! > Match staff tiles to be epic." + Environment.NewLine + Environment.NewLine
             + "< ! > Match shield tiles to stay alive so you can be epic." + Environment.NewLine + Environment.NewLine
             + "< ! > Match key tiles even when they are not needed so you can match the other tiles more." + Environment.NewLine + Environment.NewLine;

            Language.Add(prefix + "NAME", "Match Maker");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Boat Builder");
            Language.Add(prefix + "LORE", "Society grows best when the old plant trees in whose shade they'll never sit.");
            Language.Add(prefix + "OUTRO_FLAVOR", "..and so he left, to the east wind.");
            Language.Add(prefix + "OUTRO_FAILURE", "..and so he vanished, into ten million pieces.");

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SWORD_NAME", "Sword Swing");
            Language.Add(prefix + "PRIMARY_SWORD_DESCRIPTION", Tokens.agilePrefix + $"Swing for <style=cIsDamage>{100f * CharacterConfig.M1_Sword_Damage.Value}% damage</style>.\n{Tokens.UtilityText("Consumes 1 Match")} to deal {Tokens.DamageText($"{CharacterConfig.M1_Sword_MatchMultiplier.Value}x damage")}.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_STAFF_NAME", "Staff Fireball");
            Language.Add(prefix + "SECONDARY_STAFF_DESCRIPTION", $"Expend all stocks and fire a large fireblast for <style=cIsDamage>{100f * CharacterConfig.M2_Staff_Damage.Value}% damage per stock</style>. {Tokens.UtilityText("Consumes up to 8 Matches")} to {Tokens.DamageText("increase damage dealt")} by {Tokens.DamageText($"{CharacterConfig.M2_Staff_Damage_Match * 100}% damage per match")}.");

            Language.Add(prefix + "SECONDARY_STAFF2_NAME", "Staff Explosion");
            Language.Add(prefix + "SECONDARY_STAFF2_DESCRIPTION", $"Expend all stocks and channel a large fireblast for <style=cIsDamage>{100f * CharacterConfig.M2_Staff_Damage.Value}% damage per stock</style>. {Tokens.UtilityText("Consumes all Matches")} to {Tokens.UtilityText("increase range")} and {Tokens.DamageText("increase damage dealt")} by {Tokens.DamageText($"{CharacterConfig.M2_Staff2_Damage_Match * 100}% damage per match")} .");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Shield Roll");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", $"Roll a short distance. {Tokens.UtilityText("Consumes 1 Match")} to {Tokens.DamageText("increase distance")} and {Tokens.DamageText("grant invincibility")} while rolling.\n{Tokens.UtilityText("Shield matches")} passively grant {Tokens.DamageText($"{CharacterConfig.M3_Shield_BuffArmor.Value} armor")} (up to {Tokens.DamageText($"{CharacterConfig.M3_Shield_BuffArmorMax.Value}")}) and are broken on hit");
            #endregion

            #region Extra Symbol
            Language.Add("LOADOUT_FOURTH_SYMBOL", "Extra Tile");

            Language.Add(prefix + "EXTRA_KEY_NAME", "Key");
            Language.Add(prefix + "EXTRA_KEY_DESCRIPTION", $"Matching slightly {Tokens.UtilityText("lowers the money cost")} on a targeted purchasable.");

            Language.Add(prefix + "EXTRA_CRATE_NAME", "Crate <color=red>Host only</color>");
            Language.Add(prefix + "EXTRA_CRATE_DESCRIPTION", $"Matching has a slight chance to {Tokens.UtilityText("give a random item")}.");

            Language.Add(prefix + "EXTRA_BRAIN_NAME", "Thought <color=red>Host only</color>");
            Language.Add(prefix + "EXTRA_BRAIN_DESCRIPTION", $"Matching gives a small amount of {Tokens.UtilityText("experience")} for each enemy nearby.");

            Language.Add(prefix + "EXTRA_CHICKEN_NAME", "Food <color=red>Host only</color>");
            Language.Add(prefix + "EXTRA_CHICKEN_DESCRIPTION", $"Matching while above 90% health permanently increases your {Tokens.HealthText("max health")} by {CharacterConfig.M4_Chicken_HealthPerLevel.Value}.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_MATCH_NAME", "Match");
            Language.Add(prefix + "SPECIAL_MATCH_DESCRIPTION", $"Pull up a grid of skill tiles for {CharacterConfig.M5_MatchGrid_Duration.Value} seconds. Match 3 or more tiles in a row to store {Tokens.UtilityText("Matches")} that can be used to boost each respective skill.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(MasteryAchievement.identifier), "Match Maker: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(MasteryAchievement.identifier), "As Match Maker, beat the game or obliterate on Monsoon.");
            #endregion

            #region grid changing
            Language.Add(CharacterItems.ExpandTileGrid.nameToken, "Expand Grid");
            Language.Add(CharacterItems.ExpandTileGrid.descriptionToken, "Expands Match Grid by 1 row or column.");
            Language.Add(CharacterItems.ExpandTileGrid.pickupToken, "Expands Match Grid by 1 row or column.");
            Language.Add(CharacterItems.ExpandTileGrid.loreToken, "society grows great when the old plant trees in whose shade they'll never sit");

            Language.Add(CharacterItems.AddTile2X.nameToken, "2X tiles");
            Language.Add(CharacterItems.AddTile2X.pickupToken, "Tiles have a chance to have a 2X multiplier.");
            Language.Add(CharacterItems.AddTile2X.descriptionToken, "Tiles have a chance to have a 2X multiplier.\nMatch with this tile to multiply total result.");
            Language.Add(CharacterItems.AddTile2X.loreToken, "society grows great when the old plant trees in whose shade they'll never sit");

            Language.Add(CharacterItems.AddTile3X.nameToken, "3X tiles");
            Language.Add(CharacterItems.AddTile3X.pickupToken, "Tiles have a chance to have a 3X multiplier.");
            Language.Add(CharacterItems.AddTile3X.descriptionToken, "Tiles have a chance to have a 3X multiplier.\nMatch with this tile to multiply total result.");
            Language.Add(CharacterItems.AddTile3X.loreToken, "society grows great when the old plant trees in whose shade they'll never sit");

            Language.Add(CharacterItems.AddTileWild.nameToken, "Wild Tiles");
            Language.Add(CharacterItems.AddTileWild.pickupToken, "Matches with any tile.");
            Language.Add(CharacterItems.AddTileWild.descriptionToken, "Adds Wild tiles to the grid.\nCan match with any tile.");
            Language.Add(CharacterItems.AddTileWild.loreToken, "society grows great when the old plant trees in whose shade they'll never sit");

            Language.Add(CharacterItems.AddTileBomb.nameToken, "Tile Bomb");
            Language.Add(CharacterItems.AddTileBomb.pickupToken, "Destroys all tiles of a type.");
            Language.Add(CharacterItems.AddTileBomb.descriptionToken, "Adds Interactable Bomb Tiles to the grid.\nActivate to destroy the displayed tile.");
            Language.Add(CharacterItems.AddTileBomb.loreToken, "society grows great when the old plant trees in whose shade they'll never sit");

            Language.Add(CharacterItems.AddTileScroll.nameToken, "Combat Scroll");
            Language.Add(CharacterItems.AddTileScroll.pickupToken, "Transforms all tiles to primary and secondary tiles.");
            Language.Add(CharacterItems.AddTileScroll.descriptionToken, "Adds Interactable Scroll Tiles to the grid.\nActivate to transform all tiles to primary and secondary tiles.");
            Language.Add(CharacterItems.AddTileScroll.loreToken, "society grows great when the old plant trees in whose shade they'll never sit");

            Language.Add(CharacterItems.AddTileTimeStop.nameToken, "Time Stop");
            Language.Add(CharacterItems.AddTileTimeStop.pickupToken, "Freely move tiles for 3 seconds.");
            Language.Add(CharacterItems.AddTileTimeStop.descriptionToken, "Adds Interactable Scroll Tiles to the grid.\nActivate to stop time on the grid, allowing you to mive tiles freely for 3 seconds.");
            Language.Add(CharacterItems.AddTileTimeStop.loreToken, "society grows great when the old plant trees in whose shade they'll never sit");
            #endregion grid changing
        }
    }
}
