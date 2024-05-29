using R2API;
using System;
using ModdedEntityStates.Aliem;
using Modules.Survivors;
using System.Collections.Generic;
using AliemMod.Content;

namespace Modules
{
    internal static class Tokens {
        
        public static void AddTokens()
        {
            AddAliemTokens();
        }

        private static void AddAliemTokens()
        {
            string prefix = AliemMod.Content.Survivors.AliemSurvivor.ALIEM_PREFIX;

            string desc = "The alien hominid is a highly mobile maker of mischief" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Ray Gun can by used while sprinting, diving, and riding." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Charged Shot will be a powerful shot based on your Primary weapon" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Leap for movement, and to latch on to enemies, then chomp to deal heavy damage and heal." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Your Special mutation will have other cool options in the future." + Environment.NewLine + Environment.NewLine;

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
            //LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "skin?");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_GUN_NAME", "Ray Gun");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_DESCRIPTION", $"Shoot your ray gun for {Helpers.DamageValueText(RayGunFire.RayGunDamageCoefficient)}." +
                $"\n{Helpers.UtilityText("Charged:")} Shoot a charged blast that explodes for {Helpers.DamageValueText(RayGunChargedFire.BaseDamage)}");

            LanguageAPI.Add(prefix + "PRIMARY_SWORD_NAME", "Energy Sword");
            LanguageAPI.Add(prefix + "PRIMARY_SWORD_DESCRIPTION", $"Slash in a piercing wave for {Helpers.DamageValueText(AliemConfig.M1_Sword_Damage.Value)}." +
                $"\n{Helpers.UtilityText("Charged:")} Dash and slash in a wide wave for {Helpers.DamageValueText(AliemConfig.M2_SwordCharged_Damage.Value)}");
            
            LanguageAPI.Add(prefix + "PRIMARY_RIFLE_NAME", "Human Machine Gun");
            LanguageAPI.Add(prefix + "PRIMARY_RIFLE_DESCRIPTION", $"Fire a bullet for {Helpers.DamageValueText(AliemConfig.M1_MachineGun_Damage.Value)}." +
                $"\n{Helpers.UtilityText("Charged:")} Fire a volley of {Helpers.UtilityText("piercing")} bullets for {Helpers.DamageText($"{AliemConfig.M2_MachineGunCharged_Bullets.Value}x{AliemConfig.M2_MachineGunCharged_Damage.Value * 100}%")}");

            LanguageAPI.Add(prefix + "PRIMARY_GUN_INPUTS_NAME", "Ray Gun (chargeable)");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_INPUTS_DESCRIPTION", $"Shoot your ray gun for {Helpers.DamageValueText(RayGunFire.RayGunDamageCoefficient)}.\nHold to charge up and fire a large blast for up to {Helpers.DamageValueText(10/*RayGunCharging.MaxDamageCoefficient*/)}");

            LanguageAPI.Add(prefix + "PRIMARY_GUN_INSTANT_NAME", "Ray Gun (instant)");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_INSTANT_DESCRIPTION", $"Shoot your ray gun for min {Helpers.DamageValueText(AliemMod.Components.RayGunChargeComponent.minCharge)}. Passively charges for up to {Helpers.DamageValueText(AliemMod.Components.RayGunChargeComponent.maxCharge)}.");

            LanguageAPI.Add(prefix + "PRIMARY_SWORD_INPUTS_NAME", "Energy Sword (chargable)");
            LanguageAPI.Add(prefix + "PRIMARY_SWORD_INPUTS_DESCRIPTION", $"Slash in a piercing wave for {Helpers.DamageValueText(RayGunFire.RayGunDamageCoefficient)}.\nHold to charge up and dash and slash for up to {Helpers.DamageValueText(6)}");

            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_CHARGED_NAME", "Charged Shot");
            LanguageAPI.Add(prefix + "SECONDARY_CHARGED_DESCRIPTION", $"Shoot a charged version of your {Helpers.UtilityText("Primary weapon")} for enhanced effect");

            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Ray Gun Big");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", $"Shoot a charged blast that explodes for {Helpers.DamageValueText(RayGunChargedFire.BaseDamage)}");
            
            LanguageAPI.Add(prefix + "SECONDARY_SWORD_NAME", "Energy Sword Charged");
            LanguageAPI.Add(prefix + "SECONDARY_SWORD_DESCRIPTION", $"Dash and slash in a wide wave for {Helpers.DamageValueText(AliemConfig.M2_SwordCharged_Damage.Value)}");

            LanguageAPI.Add(prefix + "SECONDARY_RIFLE_NAME", "Human Machine Gun Charged");
            LanguageAPI.Add(prefix + "SECONDARY_RIFLE_DESCRIPTION", $"Fire a volley of {Helpers.UtilityText("piercing")} bullets for {Helpers.DamageText($"{AliemConfig.M2_MachineGunCharged_Bullets.Value}x{AliemConfig.M2_MachineGunCharged_Damage.Value*100}%")}");

            LanguageAPI.Add(prefix + "SECONDARY_LUNAR_NAME", "Hungering Gaze Charged");
            LanguageAPI.Add(prefix + "SECONDARY_LUNAR_DESCRIPTION", $"Fire a conglomerate of {Helpers.UtilityText("tracking shards")} that explode for {Helpers.DamageText("9x120% damage")}.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_LEAP_NAME", "Leap");
            LanguageAPI.Add(prefix + "UTILITY_LEAP_DESCRIPTION", 
                $"Dive forward at hihg speed for {Helpers.DamageValueText(AliemLeap.DamageCoefficient)}. Hold input to either {Helpers.UtilityText("burrow")} into the ground or {Helpers.UtilityText("ride")} enemies.");

            LanguageAPI.Add(prefix + "UTILITY_CHOMP_NAME", "Chomp");
            LanguageAPI.Add(prefix + "UTILITY_CHOMP_DESCRIPTION", $"While {Helpers.UtilityText("riding")}, chomp to <style=cIsHealing>heal for {AliemConfig.M3_ChompHealing.Value * 100}% of maximum health</style> and deal {Helpers.DamageValueText(AliemRidingChomp.ChompDamageCoefficient)}, up to {Helpers.DamageText("3x damage")} to low health targets.");

            
            LanguageAPI.Add("LOADOUT_SKILL_RIDING", "Riding");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_NAME", "Grenade");
            string specialDesc = $"Throw a grenade for {Helpers.DamageValueText(ThrowGrenade.DamageCoefficient)}.";
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_DESCRIPTION", specialDesc);

            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_SCEPTER_NAME", "Big Grenade");
            specialDesc += $"{Helpers.ScepterDescription("Double area, double damage")}";
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_SCEPTER_DESCRIPTION", specialDesc);
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");
            #endregion
        }
    }
}