﻿using UnityEngine;
using BepInEx.Configuration;
using System.Runtime.CompilerServices;
using RiskOfOptions;
using RiskOfOptions.Options;
using RiskOfOptions.OptionConfigs;
using RoR2;
using RoR2.Skills;

namespace AliemMod.Modules
{
    internal static class Config
    {
        public static ConfigFile MyConfig;

        private static bool loadedIcon;

        public static ConfigEntry<T> BindAndOptions<T>(string section, string name, T defaultValue, string description = "", bool restartRequired = false) =>
            BindAndOptions(section, name, defaultValue, 0, 20, description, restartRequired);
        public static ConfigEntry<T> BindAndOptions<T>(string section, string name, T defaultValue, float min, float max, string description = "", bool restartRequired = false)
        {
            if (string.IsNullOrEmpty(description))
            {
                description = name;
            }
            description += $"\nDefault: {defaultValue}";
            if (restartRequired)
            {
                description += " (restart required)";
            }
            ConfigEntry<T> configEntry = MyConfig.Bind(section, name, defaultValue, description);

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                TryRegisterOption(configEntry, min, max, restartRequired);
            }

            return configEntry;
        }

        //back compat
        public static ConfigEntry<float> BindAndOptionsSlider(string section, string name, float defaultValue, string description, float min = 0, float max = 20, bool restartRequired = false) =>
            BindAndOptions(section, name, defaultValue, min, max, description, restartRequired);

        //add risk of options dll to your project libs and uncomment this for a soft dependency
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void TryRegisterOption<T>(ConfigEntry<T> entry, float min, float max, bool restartRequired)
        {
            if (entry is ConfigEntry<float>)
            {
                ModSettingsManager.AddOption(new SliderOption(entry as ConfigEntry<float>, new SliderConfig() { min = min, max = max, formatString = "{0:0.00}", restartRequired = restartRequired }));
            }
            if (entry is ConfigEntry<int>)
            {
                ModSettingsManager.AddOption(new IntSliderOption(entry as ConfigEntry<int>, new IntSliderConfig() { min = (int)min, max = (int)max, restartRequired = restartRequired }));
            }
            if (entry is ConfigEntry<bool>)
            {
                ModSettingsManager.AddOption(new CheckBoxOption(entry as ConfigEntry<bool>, restartRequired));
            }
            if (entry is ConfigEntry<KeyboardShortcut>)
            {
                ModSettingsManager.AddOption(new KeyBindOption(entry as ConfigEntry<KeyboardShortcut>, restartRequired));
            }
            if (!loadedIcon)
            {
                loadedIcon = true;
                try
                {
                    ModSettingsManager.SetModIcon(ImgHandler.LoadSpriteFromModFolder("icon.png"));
                }
                catch (System.Exception e)
                {
                    Debug.LogError("error adding ROO mod icon for aliem\n" + e);
                }
            }
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


        public static void ConfigureBody(CharacterBody bodyComponent, string section, string bodyInfoTitle = "")
        {
            if (string.IsNullOrEmpty(bodyInfoTitle))
            {
                bodyInfoTitle = bodyComponent.name;
            }

            bodyComponent.baseMaxHealth = BindAndOptions(
                    section,
                    $"{bodyInfoTitle} Base Max Health",
                    bodyComponent.baseMaxHealth,
                    0,
                    1000,
                    "levelMaxHealth will be adjusted accordingly (baseMaxHealth * 0.3)",
                    true).Value;
            bodyComponent.levelMaxHealth = Mathf.Round(bodyComponent.baseMaxHealth * 0.3f);

            bodyComponent.baseRegen = BindAndOptions(
                    section,
                    $"{bodyInfoTitle} Base Regen",
                    bodyComponent.baseRegen,
                    "levelRegen will be adjusted accordingly (baseRegen * 0.2)",
                    true).Value;
            bodyComponent.levelRegen = bodyComponent.baseRegen * 0.2f;

            bodyComponent.baseArmor = BindAndOptions(
                    section,
                    $"{bodyInfoTitle} Armor",
                    bodyComponent.baseArmor,
                    "",
                    true).Value;

            bodyComponent.baseDamage = BindAndOptions(
                    section,
                    $"{bodyInfoTitle} Base Damage",
                    bodyComponent.baseDamage,
                    "pretty much all survivors are 12. If you want to change damage, change damage of the moves instead.\nlevelDamage will be adjusted accordingly (baseDamage * 0.2)",
                    true).Value;
            bodyComponent.levelDamage = bodyComponent.baseDamage * 0.2f;

            bodyComponent.baseJumpCount = BindAndOptions(
                    section,
                    $"{bodyInfoTitle} Jump Count",
                    bodyComponent.baseJumpCount,
                    "",
                    true).Value;
        }

        public static void ConfigureSkillDef(SkillDef skillDef, string section, string skillTitle, bool cooldown = true, bool maxStock = true, bool rechargeStock = false)
        {
            if (cooldown)
            {
                skillDef.baseRechargeInterval = BindAndOptions(
                    section,
                    $"{skillTitle} cooldown",
                    skillDef.baseRechargeInterval,
                    0,
                    20,
                    "",
                    true).Value;
            }
            if (maxStock)
            {
                skillDef.baseMaxStock = BindAndOptions(
                    section,
                    $"{skillTitle} stocks",
                    skillDef.baseMaxStock,
                    0,
                    100,
                    "",
                    true).Value;
            }
            if (rechargeStock)
            {
                skillDef.rechargeStock = BindAndOptions(
                    section,
                    $"{skillTitle} recharge stocks",
                    skillDef.baseMaxStock,
                    0,
                    100,
                    "",
                    true).Value;
            }
        }
    }
}