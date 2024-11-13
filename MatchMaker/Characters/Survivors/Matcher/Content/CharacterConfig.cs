using MatcherMod.Modules;
using R2API.Utils;
using System;
using System.Runtime.CompilerServices;

namespace MatcherMod.Survivors.Matcher.Content
{
    public class CharacterConfig
    {
        public const string SectionGeneral = "Hi";
        public const string SectionBody = "Body";
        public const string SectionSkills = "Skills";

        public static BepInEx.Configuration.ConfigEntry<bool> Debug;

        public static ConfigEntry<float> M1_Sword_Hop;
        public static ConfigEntry<float> M1_Sword_HitStun;
        public static ConfigEntry<float> M1_Sword_HitStun2;

        public static ConfigEntry<float> M2_Staff2_SmallHop;//kill
        public static ConfigEntry<float> M2_Staff2_AntiGrav;//kill

        public static ConfigEntry<float> M3_Shield_RollInitialSpeed;//kill
        public static ConfigEntry<float> M3_Shield_RollFinalSpeed;//kill
        public static ConfigEntry<float> M3_Shield_RollDuration;//killv

        public static ConfigEntry<float> M4_Brain_NearDistance;//kill

        public static ConfigEntry<float> nipper;

        public static ConfigEntry<float> M1_Sword_Damage;
        public static ConfigEntry<float> M1_Sword_MatchMultiplier;
        public static ConfigEntry<float> M1_Sword_Duration;

        public static ConfigEntry<float> M2_Staff_Damage;
        public static ConfigEntry<float> M2_Staff_Radius;
        public static ConfigEntry<float> M2_Staff2_Damage;
        public static ConfigEntry<float> M2_Staff2_Radius;
        public static ConfigEntry<float> M2_Staff2_MatchRadius;

        public static ConfigEntry<float> M3_Shield_RollMatchSpeedMultiplier;

        public static ConfigEntry<float> M3_Shield_BuffArmor;
        public static ConfigEntry<float> M3_Shield_BuffArmorMax;

        public static ConfigEntry<int> M4_Key_UnlockBaseValue;
        public static ConfigEntry<float> M4_Crate_PercentChance;
        public static ConfigEntry<float> M4_Brain_Experience;
        public static ConfigEntry<int> M4_Chicken_HealthPerLevel;

        [Configure(SectionBody, 2f, max = 100f)] 
        public static ConfigEntry<float> Special_2X_PercentChance;

        [Configure(SectionBody, 1f, max = 100f)]
        public static ConfigEntry<float> Special_3X_PercentChance;

        [Configure(SectionBody, 0.5f, max = 100f)]  
        public static ConfigEntry<float> Special_Bomb_PercentChance;

        [Configure(SectionBody, 0.2f, max = 100f)]
        public static ConfigEntry<float> Special_Scroll_PercentChance;

        [Configure(SectionBody, 2f, max = 100f)]
        public static ConfigEntry<float> Special_Wild_PercentChance;

        public void Init()
        {
            Config.InitConfigAttributes(GetType());

            Debug = MatcherPlugin.instance.Config.Bind(SectionGeneral, "Debug", false, "Debug");

            //someConfigBool = Modules.Config.BindAndOptions(
            //    section,
            //    "someConfigBool",
            //    true,
            //    "this creates a bool config, and a checkbox option in risk of options");

            M1_Sword_Hop = Config.BindAndOptions(
                SectionBody,
                nameof(M1_Sword_Hop),
                10f, 0f, 20f);
            M1_Sword_HitStun = Config.BindAndOptions(
                SectionBody,
                nameof(M1_Sword_HitStun),
                0.1f, 0f, 1);
            M1_Sword_HitStun2 = Config.BindAndOptions(
                SectionBody,
                nameof(M1_Sword_HitStun2),
                0.15f, 0f, 1);

            nipper = Config.BindAndOptions(
                SectionBody,
                nameof(nipper),
                6f, -2000, 2000);

            //that's a big ol LEGACY BAYBEE
            M1_Sword_Damage = Config.BindAndOptions(
                SectionSkills,
                nameof(M1_Sword_Damage),
                3f);
            M1_Sword_MatchMultiplier = Config.BindAndOptions(
                SectionSkills,
                nameof(M1_Sword_MatchMultiplier),
                2.5f);

            M1_Sword_Duration = Config.BindAndOptions(
                SectionSkills,
                nameof(M1_Sword_Duration),
                1f);

            M2_Staff_Damage = Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff_Damage),
                2.0f);

            M2_Staff_Radius = Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff_Radius),
                30.0f,
                0,
                100,
                "",
                true);

            M2_Staff2_Damage = Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff2_Damage),
                2.0f);

            M2_Staff2_Radius = Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff2_Radius),
                30.0f,
                0,
                100,
                "",
                true);
            M2_Staff2_MatchRadius = Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff2_MatchRadius),
                5.0f);

            M2_Staff2_SmallHop = Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff2_SmallHop),
                10.0f);
            M2_Staff2_AntiGrav = Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff2_AntiGrav),
                5.0f);

            M3_Shield_BuffArmor = Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_BuffArmor),
                10f,
                0,
                1000);
            M3_Shield_BuffArmorMax = Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_BuffArmorMax),
                100f,
                0,
                1000);

            M3_Shield_RollDuration = Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_RollDuration),
                0.3f);

            M3_Shield_RollMatchSpeedMultiplier = Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_RollMatchSpeedMultiplier),
                0.4f);

            M3_Shield_RollInitialSpeed = Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_RollInitialSpeed),
                7f);
            M3_Shield_RollFinalSpeed = Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_RollFinalSpeed),
                2f,
                0,
                50,
                "");

            M4_Key_UnlockBaseValue = Config.BindAndOptions(
                SectionSkills,
                nameof(M4_Key_UnlockBaseValue),
                3);
            M4_Crate_PercentChance = Config.BindAndOptions(
                SectionSkills,
                nameof(M4_Crate_PercentChance),
                7f,
                0,
                100);
            M4_Brain_Experience = Config.BindAndOptions(
                SectionSkills,
                nameof(M4_Brain_Experience),
                3f);

            M4_Brain_NearDistance = Config.BindAndOptions(
                SectionSkills,
                nameof(M4_Brain_NearDistance),
                15f);

            M4_Chicken_HealthPerLevel = Config.BindAndOptions(
                SectionSkills,
                nameof(M4_Chicken_HealthPerLevel),
                1, 0, 20);
        }
    }
}
