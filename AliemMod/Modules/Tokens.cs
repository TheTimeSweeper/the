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
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            //LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "skin?");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Joe passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_GUN_NAME", "Ray Gun");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_DESCRIPTION", $"Shoot your ray gun for {Helpers.DamageValueText(RayGun.DamageCoefficient)}");

            LanguageAPI.Add(prefix + "PRIMARY_GUN_INPUTS_NAME", "Ray Gun (chargeable)");
            LanguageAPI.Add(prefix + "PRIMARY_GUN_INPUTS_DESCRIPTION", $"Shoot your ray gun for {Helpers.DamageValueText(RayGun.DamageCoefficient)}.\nHold to charge up and fire a large blast for up to {Helpers.DamageValueText(ChargeRayGunBig.MaxDamageCoefficient)}");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Ray Gun Big");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", $"Shoot a charged blast from your raygun for {Helpers.DamageValueText(RayGunBig.BaseDamage)}");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_LEAP_NAME", "Leap");
            LanguageAPI.Add(prefix + "UTILITY_LEAP_DESCRIPTION", $"Dive forward at hihg speed. If landing on the ground, hold to burrow, also at hihg speed. If an enemy is hit, ride them. While riding, chomp for {Helpers.DamageValueText(AliemRidingChomp.ChompDamageCoefficient)}.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_NAME", "Grenade");
            LanguageAPI.Add(prefix + "SPECIAL_GRENADE_DESCRIPTION", $"Throw a grenade for {Helpers.DamageValueText(ThrowGrenade.DamageCoefficient)}.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", $"{fullName}: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", $"As {fullName}, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", $"{fullName}: Mastery");
            #endregion
        }
    }
}