using BepInEx.Configuration;
using UnityEngine;

namespace Modules
{
    internal static class Config
    {
        public static bool Debug;
        public static bool NewColor;
        public static bool Cursed;

        public static ConfigEntry<bool> TowerTargeting;

        public static ConfigEntry<KeyCode> voiceKey;

        public static void ReadConfig()
        {
            Debug = FacelessJoePlugin.instance.Config.Bind(
                "Debug",
                "Debug",
                false,
                "in case I forget to delete them when I upload").Value;

            NewColor = FacelessJoePlugin.instance.Config.Bind(
                "General",
                "New Color (s?)",
                false,
                "add black for the wife").Value;

            Cursed = FacelessJoePlugin.instance.Config.Bind(
                "General",
                "Cursed",
                false,
                "yes there's a fucking minecraft skin").Value;

            TowerTargeting = FacelessJoePlugin.instance.Config.Bind(
                "General",
                "Tower Targets Reticle",
                false,
                "if false, tower simply targets nearby. If true, tower targets the enemy you're currently targeting.\nWould appreciate feedback on how this feels");

            voiceKey = FacelessJoePlugin.instance.Config.Bind(
                "General",
                "Voice Line Key",
                KeyCode.CapsLock,
                "key to play Tesla Trooper voice lines from Red Alert 2");
        }

        // this helper automatically makes config entries for disabling survivors
        public static ConfigEntry<bool> CharacterEnableConfig(string characterName, bool enabledDefault = true, string description = "")
        {
            return FacelessJoePlugin.instance.Config.Bind<bool>("General",
                                                                "Enable "+ characterName,
                                                                enabledDefault,
                                                                !string.IsNullOrEmpty(description) ? description : "Set to false to disable this character");
        }

        public static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return FacelessJoePlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }
    }
}