using GorpMod.Modules;
using R2API.Utils;
using System;
using System.Runtime.CompilerServices;

namespace GorpMod.Enemies.Gorp.Content
{
    public class CharacterConfig
    {
        public const string SectionGeneral = "Hi";
        public const string SectionBody = "Body";
        public const string SectionSkills = "Skills";
        public const string SectionMisc = "Misc";

        public static BepInEx.Configuration.ConfigEntry<bool> Debug;

        [Configure(SectionMisc, 8f, max = 30f)]
        public static ConfigEntry<float> HealthThresholds;

        [Configure(SectionMisc, 20f, max = 30f)]
        public static ConfigEntry<float> Size_MaxHealth;
        [Configure(SectionMisc, 9f, max = 30f)]
        public static ConfigEntry<float> Size_MinHealth;
        [Configure(SectionMisc, 2f, max = 30f)]
        public static ConfigEntry<float> EarScale;
        [Configure(SectionMisc, 0.3f)]
        public static ConfigEntry<float> SpikeStateRepeatInterval;

        [Configure(SectionMisc, 0.5f)]
        public static ConfigEntry<float> Mortar_baseDuration;
        [Configure(SectionMisc, 1f)]
        public static ConfigEntry<float> Mortar_minimumTime;
        [Configure(SectionMisc, 10f, max = 100)]
        public static ConfigEntry<float> Mortar_projectileHorizontalSpeed;
        [Configure(SectionMisc, 1f)]
        public static ConfigEntry<float> Mortar_animationSpeed;

        //[Configure(SectionMisc, 0f, max = 30f)]
        //public static ConfigEntry<float> Damage_MaxHealth;
        //[Configure(SectionMisc, -1f, max = 30f, min = -20f)]
        //public static ConfigEntry<float> Damage_MinHealth;

        //[Configure(SectionMisc, 0f, max = 30f)]
        //public static ConfigEntry<float> Speed_MaxHealth;
        //[Configure(SectionMisc, 2f, max = 30f)]
        //public static ConfigEntry<float> Speed_MinHealth;

        //[Configure(SectionMisc, 0f, max = 30f)]
        //public static ConfigEntry<float> ASpeed_MaxHealth;
        //[Configure(SectionMisc, 3f, max = 30f)]
        //public static ConfigEntry<float> ASpeed_MinHealth;

        //[Configure(SectionMisc, 0f, max = 30f)]
        //public static ConfigEntry<float> CDR_MaxHealth;
        //[Configure(SectionMisc, 0.5f, max = 30f)]
        //public static ConfigEntry<float> CDR_MinHealth;

        public void Init()
        {
            Debug = GorpPlugin.instance.Config.Bind(SectionGeneral, "Debug", false, "Debug");
            
            Config.InitConfigAttributes(GetType());
        }
    }
}
