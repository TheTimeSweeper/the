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
        public static ConfigEntry<bool> UncappedUtility;
        public static ConfigEntry<bool> VoiceInLobby;

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

            voiceKey = FacelessJoePlugin.instance.Config.Bind(
                "General",
                "Voice Line Key",
                KeyCode.CapsLock,
                "key to play Tesla Trooper voice lines from Red Alert 2");

            VoiceInLobby = FacelessJoePlugin.instance.Config.Bind(
                "General",
                "Voice Line In Lobby",
                false,
                "For the Red Alert 2 fans out there");

            TowerTargeting = FacelessJoePlugin.instance.Config.Bind(
                "Gameplay",
                "Tower Targets Reticle",
                false,
                "if false, tower simply targets nearby. If true, tower targets the enemy you're currently targeting.\nWould appreciate feedback on how this feels");

            UncappedUtility = FacelessJoePlugin.instance.Config.Bind(
                "Gameplay",
                "Uncapped utility damage",
                false,
                "Removes the cap on how much damage you can retaliate with.\nIf you want utility to be his main source of damage");
            

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