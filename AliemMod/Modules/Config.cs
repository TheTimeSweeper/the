using System;
using BepInEx.Configuration;
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
            Debug = AliemPlugin.instance.Config.Bind(
                "Debug",
                "Debug Logs",
                false,
                "in case I forget to delete them when I upload").Value;

            string sectionGeneral = "General";

            Cursed = AliemPlugin.instance.Config.Bind(
                sectionGeneral,
                "Cursed",
                false,
                "Enable wip/unused content").Value;

            AlwaysRide = AliemPlugin.instance.Config.Bind(
                sectionGeneral,
                "Always Ride",
                false,
                "While leaping, you will only ride enemies while holding input. Set true to always ride enemy while leaping");
        }

        // this helper automatically makes config entries for disabling survivors
        public static ConfigEntry<bool> CharacterEnableConfig(string characterName, bool enabledDefault = true, string description = "")
        {
            return AliemPlugin.instance.Config.Bind<bool>("General",
                                                                "Enable "+ characterName,
                                                                enabledDefault,
                                                                !string.IsNullOrEmpty(description) ? description : "Set to false to disable this character");
        }

        public static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return AliemPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }
    }
}