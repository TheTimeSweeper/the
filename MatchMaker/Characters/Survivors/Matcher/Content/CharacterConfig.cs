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
        public const string SectionMisc = "Misc";

        public static BepInEx.Configuration.ConfigEntry<bool> Debug;

        [Configure(SectionBody, 10f, max = 20f)]
        public static ConfigEntry<float> M1_Sword_Hop;
        [Configure(SectionBody, 0.1f, max = 1f)]
        public static ConfigEntry<float> M1_Sword_HitStun;
        [Configure(SectionBody, 0.15f, max = 1f)]
        public static ConfigEntry<float> M1_Sword_HitStun2;

        [Configure(SectionSkills, 10f, max = 20f)]
        public static ConfigEntry<float> M2_Staff2_SmallHop;//kill
        [Configure(SectionSkills, 5f, max = 20f)]
        public static ConfigEntry<float> M2_Staff2_AntiGrav;//kill

        [Configure(SectionSkills, 7f, max = 50f)]
        public static ConfigEntry<float> M3_Shield_RollInitialSpeed;//kill
        [Configure(SectionSkills, 2f, max = 50f)]
        public static ConfigEntry<float> M3_Shield_RollFinalSpeed;//kill
        [Configure(SectionSkills, 0.3f, max = 10f)]
        public static ConfigEntry<float> M3_Shield_RollDuration;//killv
        [Configure(SectionSkills, 15f, max = 10f)]
        public static ConfigEntry<float> M4_Brain_NearDistance;//kill

        [Configure(SectionSkills, 6f, max = 10f)]
        public static ConfigEntry<float> nipper;

        [Configure(SectionSkills, 3f, max = 10f)]
        public static ConfigEntry<float> M1_Sword_Damage;
        [Configure(SectionSkills, 2.5f, max = 10f)]
        public static ConfigEntry<float> M1_Sword_MatchMultiplier;
        [Configure(SectionSkills, 0.9f, max = 10f)]
        public static ConfigEntry<float> M1_Sword_Duration;
        [Configure(SectionSkills, 1.1f, max = 10f, description = "stronger so a bit slower")]
        public static ConfigEntry<float> M1_Sword_DurationBoostedMultiplier;

        [Configure(SectionSkills, 2.0f, max = 10f)]
        public static ConfigEntry<float> M2_Staff_Damage;
        [Configure(SectionSkills, 30.0f, max = 100f)]
        public static ConfigEntry<float> M2_Staff_Radius;
        [Configure(SectionSkills, 2.0f, max = 10f)]
        public static ConfigEntry<float> M2_Staff2_Damage;
        [Configure(SectionSkills, 30.0f, max = 100f)]
        public static ConfigEntry<float> M2_Staff2_Radius;
        [Configure(SectionSkills, 5.0f, max = 10f)]
        public static ConfigEntry<float> M2_Staff2_MatchRadius;
        [Configure(SectionSkills, 0.4f, max = 10f)]
        public static ConfigEntry<float> M3_Shield_RollMatchSpeedMultiplier;

        [Configure(SectionSkills, 10f, max = 10f)]
        public static ConfigEntry<float> M3_Shield_BuffArmor;
        [Configure(SectionSkills, 100f, max = 10f)]
        public static ConfigEntry<float> M3_Shield_BuffArmorMax;

        [Configure(SectionSkills, 1.5f, max = 20f)]
        public static ConfigEntry<float> M4_Key_UnlockBaseValue;
        [Configure(SectionSkills, 98.5f, max = 100f)]
        public static ConfigEntry<float> M4_Key_UnlockFractionValue;
        [Configure(SectionSkills, 7f, max = 20f)]
        public static ConfigEntry<float> M4_Crate_PercentChance;
        [Configure(SectionSkills, 3f, max = 20f)]
        public static ConfigEntry<float> M4_Brain_Experience;
        [Configure(SectionSkills, 1, max = 20)]
        public static ConfigEntry<int> M4_Chicken_HealthPerLevel;

        [Configure(SectionMisc, 2f, max = 100f)] 
        public static ConfigEntry<float> Special_2X_PercentChance;
        [Configure(SectionMisc, 1f, max = 100f)]
        public static ConfigEntry<float> Special_3X_PercentChance;
        [Configure(SectionMisc, 0.5f, max = 100f)]  
        public static ConfigEntry<float> Special_Bomb_PercentChance;
        [Configure(SectionMisc, 0.2f, max = 100f)]
        public static ConfigEntry<float> Special_Scroll_PercentChance;
        [Configure(SectionMisc, 2f, max = 100f)]
        public static ConfigEntry<float> Special_Wild_PercentChance;
        [Configure(SectionMisc, 0.8f, max = 100f)]
        public static ConfigEntry<float> Special_TimeStop_PercentChance;

        [Configure(SectionSkills, 10f)]
        public static ConfigEntry<float> M5_MatchGrid_Duration;

        
        public static ConfigEntry<float> M5_MatchGrid_Damage;

        public void Init()
        {
            Debug = MatcherPlugin.instance.Config.Bind(SectionGeneral, "Debug", false, "Debug");

            Config.InitConfigAttributes(GetType());
        }
    }
}
