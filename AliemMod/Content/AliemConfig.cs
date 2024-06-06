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

        public static ConfigEntry<float> M1_SawedOff_Duration;
        public static ConfigEntry<float> M1_SawedOff_Damage;
        public static ConfigEntry<int> M1_SawedOff_Bullets;
        public static ConfigEntry<float> M1_SawedOff_Spread;
        public static ConfigEntry<float> M1_SawedOff_SelfKnockback;
        public static ConfigEntry<float> M1_SawedOffCharged_Damage_Min;
        public static ConfigEntry<float> M1_SawedOffCharged_Damage_Max;
        public static ConfigEntry<float> M1_SawedOffCharged_SelfKnockback;

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

        public static ConfigEntry<float> shotgunKnockbackSpeedOverride;

        public static ConfigEntry<float> shotgunBulletRadius;
        public static ConfigEntry<float> shotgunRange;
        public static ConfigEntry<float> shotgunSpreadPitchScale;
        public static ConfigEntry<float> shotgunForce;


        public static ConfigEntry<float> ShotgunChargedPitch;
        public static ConfigEntry<float> shotgunChargedSideShift;
        public static ConfigEntry<float> shotgunChargedSideTurn;

        public static string sectionDebug = "Debug";
        public static string sectionSkills = "Skills";
        public static string sectionBody = "Body";
        public static string sectionShotgun = "Shotgun";

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

            M1_SawedOff_Duration = Config.BindAndOptions(
                sectionShotgun,
                nameof(M1_SawedOff_Duration),
                1f,
                0,
                5,
                "");
            M1_SawedOff_Damage = Config.BindAndOptions(
                sectionShotgun,
                nameof(M1_SawedOff_Damage),
                1f,
                0,
                50,
                "");
            M1_SawedOff_Bullets = Config.BindAndOptions(
                sectionShotgun,
                nameof(M1_SawedOff_Bullets),
                10,
                0,
                50,
                "");
            shotgunBulletRadius = Config.BindAndOptionsSlider(
                sectionShotgun,
                nameof(shotgunBulletRadius),
                0.4f,
                "",
                0,
                5);
            shotgunRange = Config.BindAndOptionsSlider(
                sectionShotgun,
                "shotgunRange",
                50f,
                "",
                0,
                200);
            shotgunForce = Config.BindAndOptionsSlider(
                sectionShotgun,
                nameof(shotgunForce),
                33f,
                "",
                0,
                2000);

            M1_SawedOff_Spread = Config.BindAndOptions(
                sectionShotgun,
                nameof(M1_SawedOff_Spread),
                10f,
                0,
                50,
                "");
            shotgunSpreadPitchScale = Config.BindAndOptionsSlider(
                sectionShotgun,
                nameof(shotgunSpreadPitchScale),
                0.6f,
                "y scale of spread pattern",
                0,
                1);

            M1_SawedOff_SelfKnockback = Config.BindAndOptions(
                sectionShotgun,
                nameof(M1_SawedOff_SelfKnockback),
                22f,
                0,
                200,
                "");
            shotgunKnockbackSpeedOverride = Config.BindAndOptionsSlider(
                sectionShotgun,
                nameof(shotgunKnockbackSpeedOverride),
                0f,
                "1 will fully override your momentum before applying self knockback. 0 will not affect your velocity and add self knockback speed to your current speed",
                0,
                1);

            M1_SawedOffCharged_Damage_Min = Config.BindAndOptions(
                sectionShotgun,
                nameof(M1_SawedOffCharged_Damage_Min),
                2f,
                0,
                20,
                "");

            M1_SawedOffCharged_Damage_Max = Config.BindAndOptions(
                sectionShotgun,
                nameof(M1_SawedOffCharged_Damage_Max),
                6f,
                0,
                20,
                "");
            M1_SawedOffCharged_SelfKnockback = Config.BindAndOptions(
                sectionShotgun,
                nameof(M1_SawedOffCharged_SelfKnockback),
                33f,
                0,
                200,
                "");

            ShotgunChargedPitch = Config.BindAndOptionsSlider(
                sectionShotgun,
                nameof(ShotgunChargedPitch),
                -45,
                "negative values tilt projectile upward. set 0 to shoot straight. edit projectile properties like desiredSpeed with runtimeinspector",
                -90,
                90);

            shotgunChargedSideShift = Config.BindAndOptionsSlider(
                sectionShotgun,
                nameof(shotgunChargedSideShift),
                0.5f,
                "how far laterally do the projectiles spawn apart from each other",
                0,
                1);

            shotgunChargedSideTurn = Config.BindAndOptionsSlider(
                sectionShotgun,
                nameof(shotgunChargedSideTurn),
                0.1f,
                "how much to push the direction the projectiles are shot out",
                -1,
                1);

            M3_Leap_AlwaysRide = Config.BindAndOptions(
                sectionSkills,
                nameof(M3_Leap_AlwaysRide),
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

            #endregion debug
        }
    }
}