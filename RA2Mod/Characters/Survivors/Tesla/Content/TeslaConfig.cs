using RA2Mod.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace RA2Mod.Survivors.Tesla
{
    //todo teslamove ugh
    public class TeslaConfig
    {
        public static ConfigEntry<bool> M1_Zap_TargetingReticleCenter;
        public static ConfigEntry<float> M1_Zap_ConductiveAllyBoost;

        public static ConfigEntry<float> M3_ChargingUp_DamageAbsorption;
        public static float UtilityDamageAbsorption => UnityEngine.Mathf.Clamp01(M3_ChargingUp_DamageAbsorption.Value);
        public static ConfigEntry<bool> M3_ChargingUp_Uncapped;

        public static ConfigEntry<int> M4_Tower_LysateLimit;
        public static ConfigEntry<bool> M4_Tower_Targeting;
        public static ConfigEntry<bool> M4_Tower_ItemDisplays;

        public const string ConfigVersion = " 0.1";
        public const string SectionSkills = "1-1. Tesla Trooper Skills" + ConfigVersion;
        public const string SectionBody = "1-1. Tesla Trooper Body" + ConfigVersion;

        public static void Init()
        {
            //M1_Zap_TargetingReticleCenter = Config.BindAndOptions(
            //    SectionSkills,
            //    "M1_Zap_TargetingReticleCenter",
            //    false,
            //    "Keeps the targeting reticle at the center of the body.\nDoes not change gameplay. Damage is still based on the part of the body that is closest to the crosshair",
            //    false);
            M1_Zap_ConductiveAllyBoost = Config.BindAndOptions(
                SectionSkills,
                "M1_Zap_ConductiveAllyBoost",
                1.1f,
                0,
                20,
                "Damage Multiplier added when you zap an ally");

            M3_ChargingUp_DamageAbsorption = Config.BindAndOptions(
                SectionSkills,
                "M3_ChargingUp_DamageAbsorption",
                1.0f,
                0,
                1,
                "How much damage (as a multiplier from 0 to 1) is completely blocked while charging up. If set to 0, no damage would be blocked." +
                "\nNote that this does not affect how much damage is reflected after the buff expires." +
                "\nPeople have felt that 100% damage absorption is too strong, but in practice anything lower is still very risky in ror2. I suggest trying other means of balancing if you still feel that way (duration, cooldown, etc)");

            M3_ChargingUp_Uncapped = Config.BindAndOptions(
                SectionSkills,
                "M3_ChargingUp_Uncapped",
                false,
                "Removes the cap on how much damage you can retaliate with.\nIf you want utility to be his main source of damage");

            M4_Tower_LysateLimit = Config.BindAndOptions(
                SectionSkills,
                "M4_Tower Lysate Cell Additional Tower Limit",
                1,
                -1,
                20,
                "With additional towers, lysate cell is way too strong for a green item. Default is 1, but for proper balance I would suggest 0\n-1 for unlimited. have fun");

            M4_Tower_ItemDisplays = Config.BindAndOptions(
                SectionSkills,
                "M4_Tower_ItemDisplays",
                true,
                "Set false to disable tower item displays if you find them too silly");

            M4_Tower_Targeting = Config.BindAndOptions(
                SectionSkills,
                "M4_Tower Targets Reticle",
                false,
                "if false, tower simply targets nearby. If true, tower targets the enemy you're currently targeting.\nWould appreciate feedback on how this feels");
        }
    }
}
