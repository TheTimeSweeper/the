using System;
using AliemMod.Modules;
using BepInEx.Configuration;

namespace AliemMod.Content
{
    public static class AliemConfig
    {
        public static ConfigEntry<bool> Debug;

        public static ConfigEntry<bool> Cursed;
        public static ConfigEntry<bool> GupDefault;

        public static ConfigEntry<float> M1_RayGun_Damage;
        public static ConfigEntry<float> M1_RayGun_Duration;
        public static ConfigEntry<float> M1_RayGunCharged_Damage_Min;
        public static ConfigEntry<float> M1_RayGunCharged_Damage_Max;
        public static ConfigEntry<bool> M1_RayGun_Sound_Alt;

        public static ConfigEntry<float> M1_Sword_Damage;
        public static ConfigEntry<float> M1_Sword_Duration;
        public static ConfigEntry<float> M1_SwordCharged_Damage_Min;
        public static ConfigEntry<float> M1_SwordCharged_Damage_Max;
        public static ConfigEntry<float> M1_SwordCharged_Speed_Min;
        public static ConfigEntry<float> M1_SwordCharged_Speed_Max;

        public static ConfigEntry<float> M1_MachineGun_Damage;
        public static ConfigEntry<float> M1_MachineGun_Duration;
        public static ConfigEntry<bool> M1_MachineGun_Falloff;
        public static ConfigEntry<float> M1_MachineGunCharged_Damage;
        public static ConfigEntry<int> M1_MachineGunCharged_Bullets_Min;
        public static ConfigEntry<int> M1_MachineGunCharged_Bullets_Max;
        public static ConfigEntry<float> M1_MachineGunCharged_Interval;
        public static ConfigEntry<float> M1_MachineGunCharged_Spread;

        public static ConfigEntry<float> M1_SawedOff_Duration;
        public static ConfigEntry<float> M1_SawedOff_Damage;
        public static ConfigEntry<int> M1_SawedOff_Bullets;
        public static ConfigEntry<float> M1_SawedOff_Spread;
        public static ConfigEntry<float> M1_SawedOff_SelfKnockback;
        public static ConfigEntry<float> M1_SawedOff_Recoil;
        public static ConfigEntry<float> M1_SawedOffCharged_Damage_Min;
        public static ConfigEntry<float> M1_SawedOffCharged_Damage_Max;
        public static ConfigEntry<float> M1_SawedOffCharged_SelfKnockback;

        public static ConfigEntry<bool> M3_Leap_AlwaysRide;
        public static ConfigEntry<bool> M3_Leap_RidingControl;
        public static ConfigEntry<float> M3_Leap_ImpactDamage;
        public static ConfigEntry<int> M3_Leap_Armor;
        public static ConfigEntry<int> M3_Leap_RidingArmor;

        public static ConfigEntry<float> M3_Burrow_PopOutDamage;
        public static ConfigEntry<float> M3_Burrow_PopOutForce;

        public static ConfigEntry<float> M3_Chomp_Damage;
        public static ConfigEntry<bool> M3_Chomp_Slayer;
        public static ConfigEntry<float> M3_Chomp_Healing;
        public static ConfigEntry<bool> M3_Chomp_HealMissing;

        public static ConfigEntry<float> M4_GrenadeDamage;
        public static ConfigEntry<float> M4_WeaponSwap_Duration;

        public static string sectionDebug = "Debug";
        public static string sectionBody = "Aliem";
        public static string sectionPrimaries = "Primaries";
        public static string sectionUtility = "Utility";
        public static string sectionTheRest = "Other";

        public static void ReadConfig()
        {
            Debug = AliemPlugin.instance.Config.Bind(
                sectionDebug,
                "Debug Logs",
                false,
                "in case I forget to delete them when I upload");

            Cursed = Config.BindAndOptions(
                sectionBody,
                "Cursed",
                false,
                "Enable wip/unused content and recolor skins",
                true);

            GupDefault = Config.BindAndOptions(
                sectionBody,
                "GupDefault",
                false,
                "Change the ror-friendly-ish gup skin to be default skin and icon." +
                "\nshould also change the name and sounds but I can't be arsed",
                true);

            M1_RayGun_Damage = Config.BindAndOptions(
                sectionPrimaries,
                "M1_RayGun_Damage",
                2f,
                0,
                10,
                "");
            M1_RayGun_Duration = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_RayGun_Duration),
                0.32f,
                0,
                10,
                "lower duration means higher attack speed");
            M1_RayGunCharged_Damage_Min = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_RayGunCharged_Damage_Min),
                2f,
                0,
                20,
                "");
            M1_RayGunCharged_Damage_Max = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_RayGunCharged_Damage_Max),
                6f,
                0,
                20,
                "");
            M1_RayGun_Sound_Alt = Config.BindAndOptions(
                sectionPrimaries,
                "M1_RayGun_Sound_Alt",
                false,
                "Lighter sound");

            M1_Sword_Damage = Config.BindAndOptions(
                sectionPrimaries,
                "M1_Sword_Damage",
                1.2f,
                0,
                10,
                "");
            M1_Sword_Duration = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_Sword_Duration),
                0.2f,
                0,
                10,
                "lower duration means higher attack speed");
            M1_SwordCharged_Damage_Min = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SwordCharged_Damage_Min),
                2f,
                0,
                10,
                "");
            M1_SwordCharged_Damage_Max = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SwordCharged_Damage_Max),
                8f,
                0,
                10,
                "");

            M1_SwordCharged_Speed_Min = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SwordCharged_Speed_Min),
                80f,
                -20,
                5000,
                "");
            M1_SwordCharged_Speed_Max = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SwordCharged_Speed_Max),
                122f,
                0,
                500,
                "");

            M1_MachineGun_Damage = Config.BindAndOptions(
                sectionPrimaries,
                "M1_MachineGun_Damage",
                1.6f,
                0,
                10,
                "");

            M1_MachineGun_Duration = Config.BindAndOptions(
                sectionPrimaries,
                "M1_MachineGun_Duration",
                0.22f,
                0,
                10,
                "lower duration means higher attack speed");

            M1_MachineGun_Falloff = Config.BindAndOptions(
                sectionPrimaries, 
                nameof(M1_MachineGun_Falloff),
                true,
                "If true, default falloff like commando. if false none\nMachine gun damage is balanced around this. if you disable falloff, reduce MachineGun_Damage a bit");

            M1_MachineGunCharged_Damage = Config.BindAndOptions(
                sectionPrimaries,
                "M1_MachineGunCharged_Damage",
                1.2f,
                0,
                20,
                "if it seems like it's lower than uncharged, it's not really cause falloff");
            M1_MachineGunCharged_Bullets_Min = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_MachineGunCharged_Bullets_Min),
                1,
                0,
                50,
                "");
            M1_MachineGunCharged_Bullets_Max = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_MachineGunCharged_Bullets_Max),
                8,
                0,
                50,
                "");
            M1_MachineGunCharged_Spread = Config.BindAndOptions(
                sectionPrimaries,
                "M1_MachineGunCharged_Spread",
                3f,
                0,
                20,
                "");
            M1_MachineGunCharged_Interval = Config.BindAndOptions(
                sectionPrimaries,
                "M1_MachineGunCharged_Interval",
                0.1f,
                0,
                1,
                "lower interval means volley of shots fire faster");

            M1_SawedOff_Duration = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SawedOff_Duration),
                1f,
                0,
                5,
                "lower duration means higher attack speed");
            M1_SawedOff_Damage = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SawedOff_Damage),
                0.7f,
                0,
                50,
                "");
            M1_SawedOff_Bullets = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SawedOff_Bullets),
                12,
                0,
                50,
                "");

            M1_SawedOff_Recoil = Config.BindAndOptionsSlider(
                sectionPrimaries,
                nameof(M1_SawedOff_Recoil),
                3f,
                "screenshake",
                0,
                20);

            M1_SawedOff_Spread = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SawedOff_Spread),
                12f,
                0,
                50,
                "");

            M1_SawedOff_SelfKnockback = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SawedOff_SelfKnockback),
                12f,
                0,
                200,
                "");

            M1_SawedOffCharged_Damage_Min = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SawedOffCharged_Damage_Min),
                2f,
                0,
                20,
                "");

            M1_SawedOffCharged_Damage_Max = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SawedOffCharged_Damage_Max),
                5f,
                0,
                20,
                "");
            M1_SawedOffCharged_SelfKnockback = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_SawedOffCharged_SelfKnockback),
                18f,
                0,
                200,
                "");

            M3_Leap_AlwaysRide = Config.BindAndOptions(
                sectionUtility,
                nameof(M3_Leap_AlwaysRide),
                false,
                "While leaping, you will only ride enemies while holding input. Set true to always ride enemy while leaping");

            M3_Leap_RidingControl = Config.BindAndOptions(
                sectionUtility,
                nameof(M3_Leap_RidingControl),
                false,
                "Should you be able to control an enemy's movement if you're riding it (doesn't override attacking, they'll still try to attack you)");

            M3_Leap_ImpactDamage = Config.BindAndOptions(
                sectionUtility,
                nameof(M3_Leap_ImpactDamage),
                1f,
                0,
                20,
                "");

            M3_Leap_Armor = Config.BindAndOptions(
                sectionUtility,
                nameof(M3_Leap_Armor),
                50,
                0,
                100,
                "");
            M3_Leap_RidingArmor = Config.BindAndOptions(
                sectionUtility,
                nameof(M3_Leap_RidingArmor),
                20,
                0,
                100,
                "");

            M3_Burrow_PopOutDamage = Config.BindAndOptions(
                sectionUtility,
                "M3_BurrowPopOutDamage",
                2f,
                0,
                20,
                "");
            M3_Burrow_PopOutForce = Config.BindAndOptions(
                sectionUtility,
                "M3_BurrowPopOutForce",
                1000f,
                0,
                10000,
                "");
            M3_Chomp_Damage = Config.BindAndOptions(
                sectionUtility,
                "M3_ChompDamage",
                3f,
                0,
                20,
                "");
            M3_Chomp_Slayer = Config.BindAndOptions(
                sectionUtility,
                "M3_ChompSlayer",
                true,
                "Does chomp deal up to 3x damage to low health enemies");

            M3_Chomp_Healing = Config.BindAndOptions(
                sectionUtility,
                "M3_ChompHealing",
                0.3f,
                0,
                1,
                "");

            M3_Chomp_HealMissing = Config.BindAndOptions(
                sectionUtility,
                nameof(M3_Chomp_HealMissing),
                true,
                "Does chomp heal based on missing health or max health");

            M4_GrenadeDamage = Config.BindAndOptions(
                sectionTheRest,
                "M4_GrenadeDamage",
                12f,
                0,
                20,
                "");

            M4_WeaponSwap_Duration = Config.BindAndOptions(
                sectionTheRest,
                nameof(M4_WeaponSwap_Duration),
                6f,
                0,
                20,
                "");

            #region debug

            #endregion debug
        }
    }
}