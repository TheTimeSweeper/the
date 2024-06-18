using R2API;
using System;
using ModdedEntityStates.Aliem;
using Modules.Survivors;
using System.Collections.Generic;
using AliemMod.Content;
using AliemMod.Content.Achievements;

namespace AliemMod.Modules
{
    internal static class Tokens
    {

        public static void AddTokens()
        {
            AddAliemTokens();
        }

        private static void AddAliemTokens()
        {
            string prefix = AliemMod.Content.Survivors.AliemSurvivor.ALIEM_PREFIX;

            string desc = "The alien hominid is a highly mobile maker of mischief" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Mash the primary button at a resonable rate to automatically fire at highest attack speed." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Hold instead of mashing to charge a powerful shot. Use secondary as a shorthand to fire at full power " + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Leap on to enemies and chomp to heal, but diving into a group of enemies is risky." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > The grenade is a simple short range high damage crowd clearer." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so it left, mothership pleased.";
            string outroFailure = "..and so it vanished, the feds won.";

            string fullName = "Alien Hominid";
            LanguageAPI.Add(prefix + "NAME", fullName);
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "head chomper");
            LanguageAPI.Add(prefix + "LORE", "do it for mama do it for mama do it for mama do it for mama do it for mama do it for mama do it for mama do it for mama do it for mama do it for mama do it for mama do it for mama do it for mama do it for mama ");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);
            
            #region Skins
            LanguageAPI.Add(prefix + "GUP_SKIN_NAME", "Gip");
            #endregion

            #region Primary

            LanguageAPI.Add(prefix + "PRIMARY_GUN_INPUTS_NAME", "Ray Gun");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_INPUTS_DESCRIPTION", $"Shoot your ray gun for {Helpers.DamageValueText(RayGunFire.RayGunDamageCoefficient)}." +
                $"\n{Helpers.UtilityText("Hold to charge")} and fire a large blast for {Helpers.DamageRangeText(AliemConfig.M1_RayGunCharged_Damage_Min.Value, AliemConfig.M1_RayGunCharged_Damage_Max.Value)}.");

            LanguageAPI.Add(prefix + "PRIMARY_SWORD_INPUTS_NAME", "Energy Sword");
            LanguageAPI.Add(prefix + "PRIMARY_SWORD_INPUTS_DESCRIPTION", $"Slash in a piercing wave for {Helpers.DamageValueText(AliemConfig.M1_Sword_Damage.Value)}." +
                $"\n{Helpers.UtilityText("Hold to charge")} and dash and slash for {Helpers.DamageRangeText(AliemConfig.M1_SwordCharged_Damage_Min.Value, AliemConfig.M1_SwordCharged_Damage_Max.Value)}.");

            LanguageAPI.Add(prefix + "PRIMARY_RIFLE_INPUTS_NAME", "Human Machine Gun");
            LanguageAPI.Add(prefix + "PRIMARY_RIFLE_INPUTS_DESCRIPTION", $"Fire a bullet for {Helpers.DamageValueText(AliemConfig.M1_MachineGun_Damage.Value)}." +
                $"\n{Helpers.UtilityText("Hold to charge")} and fire a volley of {Helpers.UtilityText("piercing")} bullets for {Helpers.DamageText($"{AliemConfig.M1_MachineGunCharged_Bullets_Min.Value}-{AliemConfig.M1_MachineGunCharged_Bullets_Max.Value}x{AliemConfig.M1_MachineGunCharged_Damage.Value * 100}%")}.");

            LanguageAPI.Add(prefix + "PRIMARY_SAWEDOFF_INPUTS_NAME", "Sawed Off");
            LanguageAPI.Add(prefix + "PRIMARY_SAWEDOFF_INPUTS_DESCRIPTION", $"Fire a shotgun blast for {Helpers.DamageText($"{AliemConfig.M1_SawedOff_Bullets.Value}x")}{Helpers.DamageValueText(AliemConfig.M1_SawedOff_Damage.Value)}." +
                $"\n{Helpers.UtilityText("Hold to charge")} and fire large shotgun shells that barrel through enemies for {Helpers.DamageText("2x")}{Helpers.DamageRangeText(AliemConfig.M1_SawedOffCharged_Damage_Min.Value, AliemConfig.M1_SawedOffCharged_Damage_Max.Value)}.");

            #region cursed
            LanguageAPI.Add(prefix + "PRIMARY_GUN_INSTANT_NAME", "Ray Gun (instant)");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_INSTANT_DESCRIPTION", $"Shoot your ray gun for min {Helpers.DamageValueText(Components.PassiveBuildupComponent.minCharge)}. Passively charges for up to {Helpers.DamageValueText(Components.PassiveBuildupComponent.maxCharge)}.");
            #endregion cursed

            #endregion Primary

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_CHARGED_NAME", "Charged Shot");
            LanguageAPI.Add(prefix + "SECONDARY_CHARGED_DESCRIPTION", $"Fire the charged version of your {Helpers.UtilityText("Primary weapon")} at {Helpers.DamageText("maximum charge")}.");

            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Ray Gun Big");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", $"Shoot a charged blast that explodes for {Helpers.DamageValueText(AliemConfig.M1_RayGunCharged_Damage_Max.Value)}.");

            LanguageAPI.Add(prefix + "SECONDARY_SWORD_NAME", "Energy Sword Charged");
            LanguageAPI.Add(prefix + "SECONDARY_SWORD_DESCRIPTION", $"Dash and slash in a wide wave for {Helpers.DamageValueText(AliemConfig.M1_SwordCharged_Damage_Max.Value)}.");

            LanguageAPI.Add(prefix + "SECONDARY_RIFLE_NAME", "Human Machine Gun Charged");
            LanguageAPI.Add(prefix + "SECONDARY_RIFLE_DESCRIPTION", $"Fire a volley of {Helpers.UtilityText("piercing")} bullets for {Helpers.DamageText($"{AliemConfig.M1_MachineGunCharged_Bullets_Max.Value}x{AliemConfig.M1_MachineGunCharged_Damage.Value * 100}% damage")}.");

            LanguageAPI.Add(prefix + "SECONDARY_SAWEDOFF_NAME", "Sawed Off Charged");
            LanguageAPI.Add(prefix + "SECONDARY_SAWEDOFF_DESCRIPTION", $"Fire large shotgun shells that barrel through enemies for {Helpers.DamageText("2x")}{Helpers.DamageValueText(AliemConfig.M1_SawedOffCharged_Damage_Max.Value)}.");

            LanguageAPI.Add(prefix + "SECONDARY_LUNAR_NAME", "Hungering Gaze Charged");
            LanguageAPI.Add(prefix + "SECONDARY_LUNAR_DESCRIPTION", $"Fire a conglomerate of {Helpers.UtilityText("tracking shards")} that explode for {Helpers.DamageText("9x120% damage")}.");
            #endregion Secondary

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_LEAP_NAME", "Leap");
            LanguageAPI.Add(prefix + "UTILITY_LEAP_DESCRIPTION",
                $"Dive forward at hihg speed for {Helpers.DamageValueText(AliemLeap.DamageCoefficient)}. Hold input to either {Helpers.UtilityText("burrow")} into the ground or {Helpers.UtilityText("ride")} enemies.");

            LanguageAPI.Add(prefix + "UTILITY_CHOMP_NAME", "Chomp");
            LanguageAPI.Add(prefix + "UTILITY_CHOMP_DESCRIPTION", $"While {Helpers.UtilityText("riding")}, chomp to <style=cIsHealing>heal for {AliemConfig.M3_Chomp_Healing.Value * 100}% of maximum health</style> and deal {Helpers.DamageValueText(AliemRidingChomp.ChompDamageCoefficient)}, up to {Helpers.DamageText("3x damage")} to low health targets.");

            LanguageAPI.Add("LOADOUT_SKILL_RIDING", "Riding");
            #endregion Utility

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_NAME", "Grenade");
            string specialGrenadeDesc = $"Throw a grenade for {Helpers.DamageValueText(ThrowGrenade.DamageCoefficient)}.";
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_DESCRIPTION", specialGrenadeDesc);

            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_SCEPTER_NAME", "Big Grenade");
            specialGrenadeDesc += $"{Helpers.ScepterDescription("Double area, double damage")}";
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_SCEPTER_DESCRIPTION", specialGrenadeDesc);

            string specialSwapDesc = $"Wield this weapon on your offhand for {AliemConfig.M4_WeaponSwap_Duration.Value} seconds.";
            LanguageAPI.Add(prefix + "SPECIAL_WEAPONSWAP_DESCRIPTION", specialSwapDesc);

            specialSwapDesc += $"{Helpers.ScepterDescription("Increased Attack Speed for duration")}";
            LanguageAPI.Add(prefix + "SPECIAL_WEAPONSWAP_SCEPTER_DESCRIPTION", specialSwapDesc);
            #endregion Special

            #region Achievements
            LanguageAPI.Add(GetAchievementNameToken(AliemMasteryAchievement.identifier), $"{fullName}: Mastery");
            LanguageAPI.Add(GetAchievementDescriptionToken(AliemMasteryAchievement.identifier), $"As {fullName}, beat the game or obliterate on Monsoon.");

            LanguageAPI.Add(GetAchievementNameToken(AliemChompEnemiesAchievement.identifier), $"{fullName}: Chomp");
            LanguageAPI.Add(GetAchievementDescriptionToken(AliemChompEnemiesAchievement.identifier), $"As {fullName}, chomp {AliemChompEnemiesAchievement.Requirement} enemies without touching the ground.");

            LanguageAPI.Add(GetAchievementNameToken(AliemBurrowPopOutAchievement.identifier), $"{fullName}: Surprise!");
            LanguageAPI.Add(GetAchievementDescriptionToken(AliemBurrowPopOutAchievement.identifier), $"As {fullName}, un-burrow near {AliemBurrowPopOutAchievement.Requirement} enemies at once.");

            LanguageAPI.Add(GetAchievementNameToken(AliemSlowMashAchievement.identifier), $"{fullName}: ''Mashing''");
            LanguageAPI.Add(GetAchievementDescriptionToken(AliemSlowMashAchievement.identifier), $"As {fullName}, shoot primary with a rate of fire that is double the rate of input.");
            #endregion Achievements
        }
        /// <summary>
        /// gets langauge token from achievement's registered identifier
        /// </summary>
        ///</BEARD SHAMPOO>
        public static string GetAchievementNameToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_NAME";
        }
        /// <summary>
        /// gets langauge token from achievement's registered identifier
        /// </summary>
        public static string GetAchievementDescriptionToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_DESCRIPTION";
        }
    }
}