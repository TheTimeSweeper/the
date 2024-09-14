using System;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;
using JoeModForReal;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using UnityEngine;

namespace Modules
{
    internal static class Config
    {
        public static bool Debug;

        public static ConfigEntry<bool> Cursed;

        public static ConfigEntry<bool> jerry;

        //man this is just too much work
        ////primary
        //public static ConfigEntry<float> primaryDamage;
        //public static ConfigEntry<float> primaryDuration;

        //public static ConfigEntry<float> primaryJumpSwingDamage;

        ////secondary
        //public static ConfigEntry<float> secondaryDamage;
        //public static ConfigEntry<int> secondaryStonks;
        //public static ConfigEntry<Bool> secondaryReload;
        //public static ConfigEntry<float> secondaryCooldown;

        public static void ReadConfig()
        {
            Debug = FacelessJoePlugin.instance.Config.Bind(
                "Debug",
                "Debug Logs",
                false,
                "in case I forget to delete them when I upload").Value;

            string sectionGeneral = "General";

            Cursed = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "Cursed",
                false,
                "Enable wip/unused content");

            jerry =
                BindAndOptions(
                    sectionGeneral,
                    "Everyone is Jerry",
                    false,
                    "When anyone dies, the Jerry scream is heard.");

            #region fuck
            /*
            string sectionBETA = "Z_BETA";

            #region primary
            primaryDamage =
                BindAndOptions(
                    sectionBETA,
                    "Primary Swing Damage",
                    1.8f
                );

            primaryDuration =
                BindAndOptions(
                    sectionBETA,
                    "Primary Duration",
                    0.96ff
                );

            primaryJumpSwingDamage =
                BindAndOptions(
                    sectionBETA,
                    "Primary Jump Swing Damage",
                    3.2f
                );
            #endregion primary

            #region secondary

            #endregion secondary
            */
            #endregion fuck

        }

        public static ConfigEntry<T> BindAndOptions<T>(string section, string name, T defaultValue, string description = "", bool restartRequired = false) {
            if (string.IsNullOrEmpty(description)) {
                description = name;
            }

            if (restartRequired) {
                description += " (restart required)";
            }

            ConfigEntry<T> configEntry = FacelessJoePlugin.instance.Config.Bind(section, name, defaultValue, description);

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions")) {
                TryRegisterOption(configEntry, restartRequired);
            }

            return configEntry;
        }
        public static ConfigEntry<float> BindAndOptionsSlider(string section, string name, float defaultValue, string description = "", float min = 0, float max = 20, bool restartRequired = false) {
            if (string.IsNullOrEmpty(description)) {
                description = name;
            }

            if (restartRequired) {
                description += " (restart required)";
            }

            ConfigEntry<float> configEntry = FacelessJoePlugin.instance.Config.Bind(section, name, defaultValue, description);

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions")) {
                TryRegisterOptionSlider(configEntry, min, max, restartRequired);
            }

            return configEntry;
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void TryRegisterOption<T>(ConfigEntry<T> entry, bool restartRequired) {
            if (entry is ConfigEntry<float>) {
                ModSettingsManager.AddOption(new SliderOption(entry as ConfigEntry<float>, new SliderConfig() { min = 0, max = 20, formatString = "{0:0.00}", restartRequired = restartRequired }));
            }
            if (entry is ConfigEntry<int>) {
                ModSettingsManager.AddOption(new IntSliderOption(entry as ConfigEntry<int>, restartRequired));
            }
            if (entry is ConfigEntry<bool>) {
                ModSettingsManager.AddOption(new CheckBoxOption(entry as ConfigEntry<bool>, restartRequired));
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void TryRegisterOptionSlider(ConfigEntry<float> entry, float min, float max, bool restartRequired) {
            ModSettingsManager.AddOption(new SliderOption(entry as ConfigEntry<float>, new SliderConfig() { min = min, max = max, formatString = "{0:0.00}", restartRequired = restartRequired }));
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