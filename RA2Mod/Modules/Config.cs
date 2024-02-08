using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RA2Mod.Modules
{
    public class ConfigEntry<T>
    {
        public ConfigEntry(BepInEx.Configuration.ConfigEntry<T> actualConfigEntry, T defaultValue)
        {
            ActualConfigEntry = actualConfigEntry;
            DefaultValue = defaultValue;
        }

        public BepInEx.Configuration.ConfigEntry<T> ActualConfigEntry;
        public T DefaultValue;

        public T Value
        {
            get
            {
                if (ActualConfigEntry != null)
                {
                    return ActualConfigEntry.Value;
                }
                return DefaultValue;
            }
        }
    }

    public static class Config
    {
        public static ConfigFile MyConfig = RA2Plugin.instance.Config;

        private static List<string> disabledSections = new List<string>();

        private static bool enableAll = true;

        public static void DisableSection(string section)
        {
            disabledSections.Add(section);
        }
        private static bool SectionDisabled(string section)
        {
            return disabledSections.Contains(section);
        }

        public static void ConfigureBody(CharacterBody body, string section, string bodyInfoTitle = "")
        {
            if (string.IsNullOrEmpty(bodyInfoTitle))
            {
                bodyInfoTitle = body.name;
            }


        }

        public static void ConfigureSkillDef(SkillDef skillDef, string section, string skillTitle, bool cooldown = true, bool maxStock = true, bool rechargeStock = false)
        {
            if (cooldown)
            {
                skillDef.baseRechargeInterval = Config.BindAndOptionsSlider(
                    section,
                    $"{skillTitle} cooldown",
                    skillDef.baseRechargeInterval,
                    "",
                    0,
                    20).Value;
            }
            if (maxStock)
            {
                skillDef.baseMaxStock = Config.BindAndOptions(
                    section,
                    $"{skillTitle} stocks",
                    skillDef.baseMaxStock,
                    "").Value;
            }
            if (rechargeStock)
            {
                skillDef.rechargeStock = Config.BindAndOptions(
                    section,
                    $"{skillTitle} recharge stocks",
                    skillDef.baseMaxStock,
                    "").Value;
            }
        }

        /// <summary>
        /// automatically makes config entries for disabling survivors
        /// </summary>
        /// <param name="section"></param>
        /// <param name="characterName"></param>
        /// <param name="description"></param>
        /// <param name="enabledByDefault"></param>
        public static ConfigEntry<bool> CharacterEnableConfig(string section, string characterName, string description = "", bool enabledByDefault = true)
        {

            if (string.IsNullOrEmpty(description))
            {
                description = "Set to false to disable this character and as much of its code and content as possible";
            }
            return BindAndOptions<bool>(section,
                                        "Enable " + characterName,
                                        enabledByDefault,
                                        description,
                                        true);
        }

        public static ConfigEntry<T> BindAndOptions<T>(string section, string name, T defaultValue, string description = "", bool restartRequired = false)
        {
            if (string.IsNullOrEmpty(description))
            {
                description = name;
            }

            if (restartRequired)
            {
                description += " (restart required)";
            }

            if(!enableAll && SectionDisabled(section))
                return new ConfigEntry<T>(null, defaultValue);

            BepInEx.Configuration.ConfigEntry<T> configEntry = MyConfig.Bind(section, name, defaultValue, description);

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                TryRegisterOption(configEntry, restartRequired);
            }

            return new ConfigEntry<T>(configEntry, defaultValue);
        }

        public static ConfigEntry<float> BindAndOptionsSlider(string section, string name, float defaultValue, string description = "", float min = 0, float max = 20, bool restartRequired = false)
        {
            if (string.IsNullOrEmpty(description))
            {
                description = name;
            }

            if (restartRequired)
            {
                description += " (restart required)";
            }

            if (!enableAll && SectionDisabled(section))
                return new ConfigEntry<float>(null, defaultValue);

            BepInEx.Configuration.ConfigEntry<float> configEntry = MyConfig.Bind(section, name, defaultValue, description);

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                TryRegisterOptionSlider(configEntry, min, max, restartRequired);
            }

            return new ConfigEntry<float>(configEntry, defaultValue);
        }

        //add risk of options dll to your project libs and uncomment this for a soft dependency
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void TryRegisterOption<T>(BepInEx.Configuration.ConfigEntry<T> entry, bool restartRequired)
        {
            if (entry is ConfigEntry<float>)
            {
                ModSettingsManager.AddOption(new SliderOption(entry as BepInEx.Configuration.ConfigEntry<float>, new SliderConfig() { min = 0, max = 20, formatString = "{0:0.00}", restartRequired = restartRequired }));
            }
            if (entry is ConfigEntry<int>)
            {
                ModSettingsManager.AddOption(new IntSliderOption(entry as BepInEx.Configuration.ConfigEntry<int>, restartRequired));
            }
            if (entry is ConfigEntry<bool>)
            {
                ModSettingsManager.AddOption(new CheckBoxOption(entry as BepInEx.Configuration.ConfigEntry<bool>, restartRequired));
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void TryRegisterOptionSlider(BepInEx.Configuration.ConfigEntry<float> entry, float min, float max, bool restartRequired)
        {
            ModSettingsManager.AddOption(new SliderOption(entry as BepInEx.Configuration.ConfigEntry<float>, new SliderConfig() { min = min, max = max, formatString = "{0:0.00}", restartRequired = restartRequired }));
        }

        //Taken from https://github.com/ToastedOven/CustomEmotesAPI/blob/main/CustomEmotesAPI/CustomEmotesAPI/CustomEmotesAPI.cs
        public static bool GetKeyPressed(ConfigEntry<KeyboardShortcut> entry)
        {
            foreach (var item in entry.Value.Modifiers)
            {
                if (!Input.GetKey(item))
                {
                    return false;
                }
            }
            return Input.GetKeyDown(entry.Value.MainKey);
        }
    }
}
