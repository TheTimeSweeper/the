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
        public static ConfigEntry<bool> Mashing;

        public static ConfigEntry<float> M1_RayGun_Damage;
        public static ConfigEntry<float> M1_RayGun_Duration;
        public static ConfigEntry<float> M1_RayGun_CloseRangeKnife_Damage_Multiplier;
        public static ConfigEntry<float> M1_RayGun_CloseRangeKnife_Duration;
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

        public static ConfigEntry<float> M1_BBGun_Damage;
        public static ConfigEntry<float> M1_BBGun_ProcCoefficient;
        public static ConfigEntry<float> M1_BBGun_Interval;
        public static ConfigEntry<float> M1_BBGun_Spread;
        public static ConfigEntry<bool> M1_BBGun_VFXAlways;
        public static ConfigEntry<bool> M1_BBGun_VFXPooled;
        public static ConfigEntry<float> M1_BBGunCharged_Damage_Min;
        public static ConfigEntry<float> M1_BBGunCharged_Damage_Max;
        public static ConfigEntry<float> M1_BBGunCharged_HitInterval;

        public static ConfigEntry<float> BBGunRange;
        public static ConfigEntry<float> bbgunMinSpeed;
        public static ConfigEntry<float> bbgunMaxSpeed;
        public static ConfigEntry<float> bbgunSpreadYaw;
        public static ConfigEntry<int> bbgunBaseBs;
        public static ConfigEntry<int> bbgunRadius;

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

            Mashing = Config.BindAndOptions(
                sectionBody,
                "Mashing",
                true,
                "Set false to disable mash to shoot, hold to charge functionality. Sword is a lot more boring tho");

            #region primary
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
            M1_RayGun_CloseRangeKnife_Damage_Multiplier = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_RayGun_CloseRangeKnife_Damage_Multiplier),
                1f,
                0,
                10,
                "based on raygun damage");
            M1_RayGun_CloseRangeKnife_Duration = Config.BindAndOptions(
                sectionPrimaries,
                nameof(M1_RayGun_CloseRangeKnife_Duration),
                0.43f,
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

                M1_BBGun_Damage = Config.BindAndOptions(
                    sectionPrimaries,
                    nameof(M1_BBGun_Damage),
                    0.65f,
                    0,
                    20,
                    "");

                M1_BBGun_ProcCoefficient = Config.BindAndOptions(
                    sectionPrimaries,
                    nameof(M1_BBGun_ProcCoefficient),
                    0.55f,
                    0,
                    2,
                    "very low because very high firerate. if you increase this, lower fire rate");
                M1_BBGun_Interval = Config.BindAndOptions(
                    sectionPrimaries,
                    nameof(M1_BBGun_Interval),
                    4f,
                    0.01f,
                    1000,
                    "lower interval means fire faster. measured in fixedupdates (frames/ticks). ror2 updates 50 times per frame.\n" +
                    "set 1 to fire 50 times per second. set 5 to fire every 5 updates so 10 times per second, etc\n" +
                    "be careful, this attack scales past framerate");

                M1_BBGun_Spread = Config.BindAndOptions(
                    sectionPrimaries,
                    nameof(M1_BBGun_Spread),
                    10f,
                    10,
                    100,
                    "");

                M1_BBGun_VFXAlways = Config.BindAndOptions(
                    sectionPrimaries,
                    nameof(M1_BBGun_VFXAlways),
                    true,
                    "Set false and some bees will not render to help performance. Damage instances will not be affected",
                    true);

                M1_BBGun_VFXPooled = Config.BindAndOptions(
                    sectionPrimaries,
                    nameof(M1_BBGun_VFXPooled),
                    false,
                    "[WIP] Squeezes out some extra frames by putting bees in an object pool.\nDisable if it is causing issues\nI wrote a bunch of code and it only saved like 8fps but it was an improvement so I'm keeping it.",
                    true);

                M1_BBGunCharged_Damage_Min = Config.BindAndOptions(
                    sectionPrimaries,
                    nameof(M1_BBGunCharged_Damage_Min),
                    0.5f,
                    0,
                    20,
                    "");

                M1_BBGunCharged_Damage_Max = Config.BindAndOptions(
                    sectionPrimaries,
                    nameof(M1_BBGunCharged_Damage_Max),
                    1.8f,
                    0,
                    20,
                    "");

                M1_BBGunCharged_HitInterval = Config.BindAndOptions(
                    sectionPrimaries,
                    nameof(M1_BBGunCharged_HitInterval),
                    0.5f,
                    -1,
                    2,
                    "interval between repeated hits of the big bee. -1 to only hit once. do not set to 0",
                    true);

                BBGunRange = Config.BindAndOptions(
                    sectionDebug,
                    nameof(BBGunRange),
                    42f,
                    0,
                    300,
                    "");
                bbgunMinSpeed = Config.BindAndOptions(
                    sectionDebug,
                    nameof(bbgunMinSpeed),
                    55f,
                    0,
                    1000,
                    "");
                bbgunMaxSpeed = Config.BindAndOptions(
                    sectionDebug,
                    nameof(bbgunMaxSpeed),
                    117f,
                    0,
                    1000,
                    "");
                bbgunSpreadYaw = Config.BindAndOptions(
                    sectionDebug,
                    nameof(bbgunSpreadYaw),
                    2f,
                    0,
                    10,
                    "");
                bbgunBaseBs = Config.BindAndOptions(
                    sectionDebug,
                    nameof(bbgunBaseBs),
                    2,
                    1,
                    10,
                    "");

                bbgunRadius = Config.BindAndOptions(
                    sectionDebug,
                    nameof(bbgunRadius),
                    2,
                    0,
                    10,
                    "");

            #endregion primary

            #region utility
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
            #endregion utility

            #region the rest
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
            #endregion the rest

            #region debug

            #endregion debug
        }
    }
}