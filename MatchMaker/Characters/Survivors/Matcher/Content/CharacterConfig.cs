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

        [Configure(SectionSkills, 3f, max = 10f)]
        public static ConfigEntry<float> M1_Sword_Damage;
        [Configure(SectionSkills, 2.5f, max = 10f)]
        public static ConfigEntry<float> M1_Sword_MatchMultiplier;
        [Configure(SectionSkills, 0.9f, max = 10f)]
        public static ConfigEntry<float> M1_Sword_Duration;
        [Configure(SectionSkills, 1.1f, max = 10f, description = "stronger so a bit slower")]
        public static ConfigEntry<float> M1_Sword_DurationBoostedMultiplier;

        [Configure(SectionSkills, 3.0f, max = 10f)]
        public static ConfigEntry<float> M2_Staff_Damage;
        [Configure(SectionSkills, 2.0f, max = 10f)]
        public static ConfigEntry<float> M2_Staff_Damage_Match;
        [Configure(SectionSkills, 30.0f, max = 100f, restartRequired = true)]
        public static ConfigEntry<float> M2_Staff_Radius;
        [Configure(SectionSkills, 30.0f, max = 100f, restartRequired = true)]
        public static ConfigEntry<float> M2_Staff_Radius_Boosted;
        [Configure(SectionSkills, 4.0f, max = 10f)]
        public static ConfigEntry<float> M2_Staff2_Damage;
        [Configure(SectionSkills, 2.0f, max = 10f)]
        public static ConfigEntry<float> M2_Staff2_Damage_Match;
        [Configure(SectionSkills, 30.0f, max = 100f)]
        public static ConfigEntry<float> M2_Staff2_Radius;
        [Configure(SectionSkills, 5.0f, max = 10f)]
        public static ConfigEntry<float> M2_Staff2_MatchRadius;

        [Configure(SectionSkills, 7f, max = 50f)]
        public static ConfigEntry<float> M3_Shield_RollInitialSpeed;
        [Configure(SectionSkills, 0.4f, max = 10f)]
        public static ConfigEntry<float> M3_Shield_RollMatchSpeedMultiplier;

        [Configure(SectionSkills, 10f, max = 10f)]
        public static ConfigEntry<float> M3_Shield_BuffArmor;
        [Configure(SectionSkills, 100f, max = 10f)]
        public static ConfigEntry<float> M3_Shield_BuffArmorMax;

        [Configure(SectionSkills, 1.5f, max = 20f)]
        public static ConfigEntry<float> M4_Key_UnlockBaseValue;
        [Configure(SectionSkills, 1f, max = 100f)]
        public static ConfigEntry<float> M4_Key_UnlockPercentValue;
        [Configure(SectionSkills, 3f, max = 20f)]
        public static ConfigEntry<float> M4_Crate_PercentChance;
        [Configure(SectionSkills, 2f, max = 20f)]
        public static ConfigEntry<float> M4_Brain_Experience;
        //[Configure(SectionSkills, 20f, max = 10f)]
        //public static ConfigEntry<float> M4_Brain_NearDistance;//kill
        [Configure(SectionSkills, 1, max = 20)]
        public static ConfigEntry<int> M4_Chicken_HealthPerLevel;

        [Configure(SectionSkills, 10f)]
        public static ConfigEntry<float> M5_MatchGrid_Duration;

        [Configure(SectionMisc, 3f, max = 100f)] 
        public static ConfigEntry<float> Special_2X_PercentChance;
        [Configure(SectionMisc, 2f, max = 100f)]
        public static ConfigEntry<float> Special_3X_PercentChance;
        [Configure(SectionMisc, 0.8f, max = 100f)]  
        public static ConfigEntry<float> Special_Bomb_PercentChance;
        [Configure(SectionMisc, 0.69f, max = 100f)]
        public static ConfigEntry<float> Special_Scroll_PercentChance;
        [Configure(SectionMisc, 4f, max = 100f)]
        public static ConfigEntry<float> Special_Wild_PercentChance;
        [Configure(SectionMisc, 0.8f, max = 100f)]
        public static ConfigEntry<float> Special_TimeStop_PercentChance;

        public void Init()
        {
            Debug = MatcherPlugin.instance.Config.Bind(SectionGeneral, "Debug", false, "Debug");

            Config.InitConfigAttributes(GetType());
        }
    }
}
