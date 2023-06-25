using System;
using BepInEx.Configuration;
using UnityEngine;
using Modules;

namespace AliemMod.Content
{
    internal static class AliemConfig
    {
        public static ConfigEntry<bool> Debug;

        public static ConfigEntry<bool> Cursed;
        public static ConfigEntry<bool> AlwaysRide;

        //examples
        public static ConfigEntry<float> SomeFloatConfig;
        public static ConfigEntry<float> SomeFloatConfigWithSlider;

        public static void ReadConfig()
        {
            Debug = Config.BindAndOptions(
                "Debug",
                "Debug Logs",
                false,
                "in case I forget to delete them when I upload");

            string sectionGeneral = "General";

            Cursed = Config.BindAndOptions(
                sectionGeneral,
                "Cursed",
                false,
                "Enable wip/unused content",
                true);

            AlwaysRide = Config.BindAndOptions(
                sectionGeneral,
                "Always Ride",
                false,
                "While leaping, you will only ride enemies while holding input. Set true to always ride enemy while leaping");

            //creates a float option in RiskOfOptions using a default slider with range 0-20
            SomeFloatConfig = Config.BindAndOptions(
                sectionGeneral,
                "Name of Float Config",
                2f,
                "description of float config");

            //creates a float option in RiskOfOptions using a slider with custom range
            SomeFloatConfigWithSlider = Config.BindAndOptionsSlider(
                sectionGeneral,
                "Name of Float Config Slider",
                0f,
                "description of float config slider",
                -10f,
                10f);

        }
    }
}