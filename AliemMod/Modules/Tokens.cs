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
            string prefix = AliemMod.Content.Survivors.AliemSurvivor.ALIEM_PREFIX;

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
            //LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "skin?");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Joe passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_GUN_NAME", "Ray Gun");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_DESCRIPTION", $"Shoot your ray gun for {Helpers.DamageValueText(RayGunFire.BaseDamageCoefficient)}");

            LanguageAPI.Add(prefix + "PRIMARY_GUN_INPUTS_NAME", "Ray Gun (chargeable)");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_INPUTS_DESCRIPTION", $"Shoot your ray gun for {Helpers.DamageValueText(RayGunFire.BaseDamageCoefficient)}.\nHold to charge up and fire a large blast for up to {Helpers.DamageValueText(10/*RayGunCharging.MaxDamageCoefficient*/)}");

            LanguageAPI.Add(prefix + "PRIMARY_GUN_INSTANT_NAME", "Ray Gun (instant)");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_INSTANT_DESCRIPTION", $"Shoot your ray gun for min {Helpers.DamageValueText(AliemMod.Components.RayGunChargeComponent.minCharge)}. Passively charges for up to {Helpers.DamageValueText(AliemMod.Components.RayGunChargeComponent.maxCharge)}.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Ray Gun Big");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", $"Shoot a charged blast from your raygun for {Helpers.DamageValueText(RayGunChargedFire.BaseDamage)}");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_LEAP_NAME", "Leap");
            LanguageAPI.Add(prefix + "UTILITY_LEAP_DESCRIPTION", 
                $"Dive forward at hihg speed for {Helpers.DamageValueText(AliemLeap.DamageCoefficient)}. Hold input to either {Helpers.UtilityText("burrow")} into the ground or {Helpers.UtilityText("ride")} enemies.");

            LanguageAPI.Add(prefix + "UTILITY_CHOMP_NAME", "Chomp");
            LanguageAPI.Add(prefix + "UTILITY_CHOMP_DESCRIPTION", $"While {Helpers.UtilityText("riding")}, chomp to <style=cIsHealing>heal for 30% of maximum health</style> and deal {Helpers.DamageValueText(AliemRidingChomp.ChompDamageCoefficient)}, up to {Helpers.DamageText("3x damage")} to low health targets.");

            
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