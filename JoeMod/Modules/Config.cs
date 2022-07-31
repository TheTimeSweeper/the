using BepInEx.Configuration;
using UnityEngine;

namespace Modules
{
    internal static class Config
    {
        public static bool Debug;

        public static bool NewColor;
        public static bool Cursed;
        public static bool TowerItemDisplays;
        public static ConfigEntry<KeyCode> voiceKey;
        public static ConfigEntry<bool> VoiceInLobby;
        public static bool RA2Icon;

        public static ConfigEntry<bool> TowerTargeting;
        public static int LysateLimit;
        public static ConfigEntry<bool> UncappedUtility;



        public static void ReadConfig()
        {
            Debug = FacelessJoePlugin.instance.Config.Bind(
                "Debug",
                "Debug Logs",
                false,
                "in case I forget to delete them when I upload").Value;

            string sectionGeneral = "General";

            NewColor = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "New Color (s?)",
                false,
                "add black for the wife").Value;

            Cursed = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "Cursed",
                false,
                "yes there's a fucking minecraft skin").Value;

            TowerItemDisplays = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "Tower Item Displays",
                true,
                "Set false to disable tower item displays if you find them too silly").Value;

            voiceKey = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "Voice Line Key",
                KeyCode.CapsLock,
                "key to play Tesla Trooper voice lines from Red Alert 2");

            VoiceInLobby = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "Voice Line In Lobby",
                false,
                "For the Red Alert 2 fans out there");

            RA2Icon = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "red alert 2 icon",
                false,
                "Changes character icon to the unit icon from Red Alert 2").Value;

            string sectionGameplay= "Gameplay";

            TowerTargeting = FacelessJoePlugin.instance.Config.Bind(
                sectionGameplay,
                "Tower Targets Reticle",
                false,
                "if false, tower simply targets nearby. If true, tower targets the enemy you're currently targeting.\nWould appreciate feedback on how this feels");

            LysateLimit = FacelessJoePlugin.instance.Config.Bind(
                sectionGameplay,
                "Lysate Cell Additional Tower Limit",
                1,
                "With additional towers, lysate cell is way too strong for a green item. Default is 1, but for proper balance I would suggest 0\n-1 for unlimited. have fun").Value;

            UncappedUtility = FacelessJoePlugin.instance.Config.Bind(
                sectionGameplay,
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