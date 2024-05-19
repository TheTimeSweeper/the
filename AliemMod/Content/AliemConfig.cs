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
        public static ConfigEntry<float> rideOffset;
        public static ConfigEntry<float> rideLerpSpeed;
        public static ConfigEntry<float> rideClimbAnimTime;

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

            rideOffset = Config.BindAndOptions(
                sectionGeneral,
                "RideOFfset",
                0.5f,
                "");

            rideLerpSpeed = Config.BindAndOptionsSlider(
                sectionGeneral,
                "rideLerpSpeed",
                2f,
                "",
                0,
                1000);

            rideClimbAnimTime = Config.BindAndOptions(
                sectionGeneral,
                "rideClimbAnimTime",
                0.2f,
                "");

        }
    }
}