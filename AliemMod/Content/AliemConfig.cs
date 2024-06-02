using System;
using BepInEx.Configuration;
using UnityEngine;
using Modules;

namespace AliemMod.Content
{
    public static class AliemConfig
    {
        public static ConfigEntry<bool> Debug;

        public static ConfigEntry<bool> Cursed;

        public static ConfigEntry<float> M1_RayGun_Damage;
        public static ConfigEntry<float> M1_RayGunCharged_Damage_Min;
        public static ConfigEntry<float> M1_RayGunCharged_Damage_Max;
        public static ConfigEntry<bool> M1_RayGun_Sound_Alt;

        public static ConfigEntry<float> M1_Sword_Damage;
        public static ConfigEntry<float> M1_SwordCharged_Damage_Min;
        public static ConfigEntry<float> M1_SwordCharged_Damage_Max;
        public static ConfigEntry<float> M1_SwordCharged_Speed_Min;
        public static ConfigEntry<float> M1_SwordCharged_Speed_Max;

        public static ConfigEntry<float> M1_MachineGun_Damage;
        public static ConfigEntry<float> M1_MachineGunCharged_Damage;
        public static ConfigEntry<int> M1_MachineGunCharged_Bullets_Min;
        public static ConfigEntry<int> M1_MachineGunCharged_Bullets_Max;
        public static ConfigEntry<float> M1_MachineGunCharged_Interval;
        public static ConfigEntry<float> M1_MachineGunCharged_Spread;

        public static ConfigEntry<float> M1_SawedOff_Damage;
        public static ConfigEntry<int> M1_SawedOff_Bullets;
        public static ConfigEntry<float> M1_SawedOffCharged_Damage_Min;
        public static ConfigEntry<float> M1_SawedOffCharged_Damage_Max;

        public static ConfigEntry<bool> M3_Leap_AlwaysRide;
        public static ConfigEntry<float> M3_Leap_ImpactDamage;
        public static ConfigEntry<int> M3_Leap_Armor;
        public static ConfigEntry<int> M3_Leap_RidingArmor;
        public static ConfigEntry<float> M3_Burrow_PopOutDamage;
        public static ConfigEntry<float> M3_Burrow_PopOutForce;
        public static ConfigEntry<float> M3_Chomp_Damage;
        public static ConfigEntry<bool> M3_Chomp_Slayer;
        public static ConfigEntry<float> M3_Chomp_Healing;

        public static ConfigEntry<float> M4_GrenadeDamage;
        public static ConfigEntry<float> M4_WeaponSwap_Duration;

        public static ConfigEntry<float> rideLerpSpeed;
        public static ConfigEntry<float> rideClimbAnimTime;
        public static ConfigEntry<float> rideLerpTim;
        public static ConfigEntry<float> rideLerpTim2;

        public static ConfigEntry<float> bloomRifle;
        public static ConfigEntry<float> bloomCharged;

        public static ConfigEntry<float> radius;
        public static ConfigEntry<float> smallhop;

        public static ConfigEntry<float> shotgunPitch;

        public static string sectionDebug = "Debug";
        public static string sectionSkills = "Skills";
        public static string sectionBody = "Body";

        public static void ReadConfig()
        {
            Debug = AliemPlugin.instance.Config.Bind(
                sectionDebug,
                "Debug Logs",
                false,
                "in case I forget to delete them when I upload");

            Cursed = Config.BindAndOptions(
                sectionSkills,
                "Cursed",
                false,
                "Enable wip/unused content",
                true);

            M1_RayGun_Damage = Config.BindAndOptions(
                sectionSkills,
                "M1_RayGun_Damage",
                2f,
                0,
                10,
                "");
            M1_RayGunCharged_Damage_Min = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_RayGunCharged_Damage_Min),
                2f,
                0,
                20,
                "");
            M1_RayGunCharged_Damage_Max = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_RayGunCharged_Damage_Max),
                6f,
                0,
                20,
                "");
            M1_RayGun_Sound_Alt = Config.BindAndOptions(
                sectionSkills,
                "M1_RayGun_Sound_Alt",
                false,
                "Lighter sound");

            M1_Sword_Damage = Config.BindAndOptions(
                sectionSkills,
                "M1_Sword_Damage",
                1.5f,
                0,
                10,
                "");
            M1_SwordCharged_Damage_Min = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_SwordCharged_Damage_Min),
                3f,
                0,
                10,
                "");
            M1_SwordCharged_Damage_Max = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_SwordCharged_Damage_Max),
                8f,
                0,
                10,
                "");

            M1_SwordCharged_Speed_Min = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_SwordCharged_Speed_Min),
                8f,
                -20,
                100,
                "");
            M1_SwordCharged_Speed_Max = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_SwordCharged_Speed_Max),
                18f,
                0,
                100,
                "");

            M1_MachineGun_Damage = Config.BindAndOptions(
                sectionSkills,
                "M1_MachineGun_Damage",
                1f,
                0,
                10,
                "");

            M1_MachineGunCharged_Damage = Config.BindAndOptions(
                sectionSkills,
                "M1_MachineGunCharged_Damage",
                1.5f,
                0,
                20,
                "");
            M1_MachineGunCharged_Bullets_Min = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_MachineGunCharged_Bullets_Min),
                2,
                0,
                50,
                "");
            M1_MachineGunCharged_Bullets_Max = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_MachineGunCharged_Bullets_Max),
                8,
                0,
                50,
                "");
            M1_MachineGunCharged_Interval = Config.BindAndOptions(
                sectionSkills,
                "M1_MachineGunCharged_Interval",
                0.1f,
                0,
                1,
                "");
            M1_MachineGunCharged_Spread = Config.BindAndOptions(
                sectionSkills,
                "M1_MachineGunCharged_Spread",
                1.5f,
                0,
                360,
                "");

            M1_SawedOff_Damage = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_SawedOff_Damage),
                1f,
                0,
                50,
                "");

            M1_SawedOff_Bullets = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_SawedOff_Bullets),
                5,
                0,
                50,
                "");

            M1_SawedOffCharged_Damage_Min = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_SawedOffCharged_Damage_Min),
                5f,
                0,
                20,
                "");

            M1_SawedOffCharged_Damage_Min = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_SawedOffCharged_Damage_Min),
                5f,
                0,
                20,
                "");

            M3_Leap_AlwaysRide = Config.BindAndOptions(
                sectionSkills,
                "M3_AlwaysRide",
                false,
                "While leaping, you will only ride enemies while holding input. Set true to always ride enemy while leaping");
            M3_Leap_ImpactDamage = Config.BindAndOptions(
                sectionSkills,
                nameof(M3_Leap_ImpactDamage),
                1f,
                0,
                20,
                "");

            M3_Leap_Armor = Config.BindAndOptions(
                sectionSkills,
                nameof(M3_Leap_Armor),
                50,
                0,
                100,
                "");
            M3_Leap_RidingArmor = Config.BindAndOptions(
                sectionSkills,
                nameof(M3_Leap_RidingArmor),
                20,
                0,
                100,
                "");

            M3_Burrow_PopOutDamage = Config.BindAndOptions(
                sectionSkills,
                "M3_BurrowPopOutDamage",
                2f,
                0,
                20,
                "");
            M3_Burrow_PopOutForce = Config.BindAndOptions(
                sectionSkills,
                "M3_BurrowPopOutForce",
                1000f,
                0,
                10000,
                "");
            M3_Chomp_Damage = Config.BindAndOptions(
                sectionSkills,
                "M3_ChompDamage",
                5f,
                0,
                20,
                "");
            M3_Chomp_Slayer = Config.BindAndOptions(
                sectionSkills,
                "M3_ChompSlayer",
                true,
                "Does chomp deal up to 3x damage to low health enemies");
            M3_Chomp_Healing = Config.BindAndOptions(
                sectionSkills,
                "M3_ChompHealing",
                0.3f,
                0,
                1,
                "");

            M4_GrenadeDamage = Config.BindAndOptions(
                sectionSkills,
                "M4_GrenadeDamage",
                12f,
                0,
                20,
                "");

            M4_WeaponSwap_Duration = Config.BindAndOptions(
                sectionSkills,
                nameof(M4_WeaponSwap_Duration),
                6f,
                0,
                20,
                "");
            #region debug
            rideLerpSpeed = Config.BindAndOptions(
                sectionDebug,
                "rideLerpSpeed",
                3f,
                0,
                1000,
                "");

            rideLerpTim = Config.BindAndOptionsSlider(
                sectionDebug,
                "rideLerpTim",
                0.5f,
                "",
                0,
                1);
            rideLerpTim2 = Config.BindAndOptionsSlider(
                sectionDebug,
                "rideLerpTimRot",
                0.1f,
                "",
                0,
                1);

            rideClimbAnimTime = Config.BindAndOptions(
                sectionDebug,
                "rideClimbAnimTime",
                0.4f,
                "");

            bloomRifle = Config.BindAndOptionsSlider(
                sectionDebug,
                "bloomRifle",
                0.5f,
                "",
                0,
                5);

            bloomCharged = Config.BindAndOptionsSlider(
                sectionDebug,
                "bloomCharged",
                0.25f,
                "",
                0,
                5);

            radius = Config.BindAndOptionsSlider(
                sectionDebug,
                "radius",
                1f,
                "",
                0,
                5);

            smallhop = Config.BindAndOptionsSlider(
                sectionDebug,
                "smallhop",
                0f,
                "",
                0,
                5);

            shotgunPitch = Config.BindAndOptionsSlider(
                sectionDebug,
                "shotgunPitch",
                0f,
                "",
                -110,
                110);
            #endregion debug
        }
    }
}