using System;
using BepInEx.Configuration;
using JoeModForReal;
using UnityEngine;

namespace Modules
{
    internal static class Config
    {
        public static bool Debug;

        public static bool Cursed;

        public static ConfigEntry<bool> AlwaysRide;

        public static void ReadConfig()
        {
            Debug = FacelessJoePlugin.instance.Config.Bind(
                "Debug",
                "Debug Logs",
                false,
                "in case I forget to delete them when I upload").Value;

            //string sectionGeneral = "General";

            //Cursed = AliemPlugin.instance.Config.Bind(
            //    sectionGeneral,
            //    "Cursed",
            //    false,
            //    "Enable wip/unused content").Value;

            //AlwaysRide = AliemPlugin.instance.Config.Bind(
            //    sectionGeneral,
            //    "Always Ride",
            //    false,
            //    "While leaping, you will only ride enemies while holding input. Set true to always ride enemy while leaping");
        }

        //Taken from https://github.com/ToastedOven/CustomEmotesAPI/blob/main/CustomEmotesAPI/CustomEmotesAPI/CustomEmotesAPI.cs
        public static bool GetKeyPressed(ConfigEntry<KeyboardShortcut> entry) {
            foreach (var item in entry.Value.Modifiers) {
                if (!Input.GetKey(item)) {
                    return false;
                }
            }
            return Input.GetKeyDown(entry.Value.MainKey);
        }
    }
}