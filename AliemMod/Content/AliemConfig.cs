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

        public static ConfigEntry<bool> M3_AlwaysRide;
        public static ConfigEntry<float> M3_ImpactDamage;
        public static ConfigEntry<float> M3_BurrowPopOutDamage;
        public static ConfigEntry<float> M3_BurrowPopOutForce;
        public static ConfigEntry<float> M3_ChompDamage;
        public static ConfigEntry<bool> M3_ChompSlayer;
        public static ConfigEntry<float> M3_ChompHealing;

        public static ConfigEntry<float> M4_GrenadeDamage;
        public static ConfigEntry<float> M4_WeaponSwap_Duration;

        public static ConfigEntry<float> rideLerpSpeed;
        public static ConfigEntry<float> rideClimbAnimTime;
        public static ConfigEntry<float> rideLerpTim;
        public static ConfigEntry<float> rideLerpTim2;

        public static ConfigEntry<float> bloom1;
        public static ConfigEntry<float> bloom2;

        public static ConfigEntry<float> radius;
        public static ConfigEntry<float> smallhop;

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
                1.5f,
                0,
                10,
                "");
            M1_SwordCharged_Damage_Max = Config.BindAndOptions(
                sectionSkills,
                nameof(M1_SwordCharged_Damage_Max),
                6f,
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
                1f,
                0,
                360,
                "");

            M3_AlwaysRide = Config.BindAndOptions(
                sectionSkills,
                "M3_AlwaysRide",
                false,
                "While leaping, you will only ride enemies while holding input. Set true to always ride enemy while leaping");
            M3_ImpactDamage = Config.BindAndOptions(
                sectionSkills,
                "M3_ImpactDamage",
                1f,
                0,
                20,
                "");
            M3_BurrowPopOutDamage = Config.BindAndOptions(
                sectionSkills,
                "M3_BurrowPopOutDamage",
                2f,
                0,
                20,
                "");
            M3_BurrowPopOutForce = Config.BindAndOptions(
                sectionSkills,
                "M3_BurrowPopOutForce",
                1000f,
                0,
                10000,
                "");
            M3_ChompDamage = Config.BindAndOptions(
                sectionSkills,
                "M3_ChompDamage",
                5f,
                0,
                20,
                "");
            M3_ChompSlayer = Config.BindAndOptions(
                sectionSkills,
                "M3_ChompSlayer",
                true,
                "Does chomp deal up to 3x damage to low health enemies");
            M3_ChompHealing = Config.BindAndOptions(
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
                10f,
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
                0.2f,
                "");

            bloom1 = Config.BindAndOptionsSlider(
                sectionDebug,
                "bloom1",
                0.5f,
                "",
                0,
                5);

            bloom2 = Config.BindAndOptionsSlider(
                sectionDebug,
                "bloom2",
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
            #endregion debug
        }
    }
}