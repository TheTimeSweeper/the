using BepInEx.Configuration;
using UnityEngine;

namespace HenryMod.Modules
{
    public static class Config
    {
        public static bool Debug;

        public static void ReadConfig()
        {
            Debug = FacelessJoePlugin.instance.Config.Bind(
                "Debug",
                "Debug",
                false,
                "in case I forget to delete them when I upload").Value;
        }

        // this helper automatically makes config entries for disabling survivors
        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName, bool enabledDefault = true, string description = "")
        {
            return FacelessJoePlugin.instance.Config.Bind<bool>(characterName,
                                                                "Enable "+ characterName,
                                                                enabledDefault,
                                                                !string.IsNullOrEmpty(description) ? description : "Set to false to disable this character");
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return FacelessJoePlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }
    }
}