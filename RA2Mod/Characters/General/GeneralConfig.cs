using RA2Mod.Modules;
using UnityEngine;
using KeyboardShortcut = BepInEx.Configuration.KeyboardShortcut;

namespace RA2Mod.General
{
    public static class GeneralConfig
    {
        public static BepInEx.Configuration.ConfigEntry<bool> Debug;

        public static ConfigEntry<bool> NewColor;
        public static ConfigEntry<bool> Cursed;
        public static ConfigEntry<KeyboardShortcut> RestKeybind { get; private set; }
        public static ConfigEntry<KeyboardShortcut> VoiceKey { get; private set; }

        public static ConfigEntry<bool> VoiceInLobby;
        public static ConfigEntry<bool> VoiceOnSpawn;
        public static ConfigEntry<bool> VoiceOnDeath;
        public static ConfigEntry<bool> RA2Icon;

        public static ConfigEntry<bool> TeslaEnabled;
        public static ConfigEntry<bool> DesolatorEnabled;
        public static ConfigEntry<bool> ChronoEnabled;
        public static ConfigEntry<bool> GIEnabled;
        public static ConfigEntry<bool> ConscriptEnabled;
        public static ConfigEntry<bool> MCVEnabled;

        public static void Init()
        {
            //0-0. General
                //0-1. Survivors, 0-2 items, etc
            //1-1, 1-2, 1-3, the boys
            //2-1 compats?
            string sectionGeneral = "0-0. General";

            Debug = RA2Plugin.instance.Config.Bind<bool>(
                sectionGeneral,
                "Debug Logs", 
                false, 
                "In case I forget to remove something");

            NewColor = Config.BindAndOptions(
                sectionGeneral,
                "New Color (s?)",
                false,
                "add black for the wife",
                true);

            Cursed = Config.BindAndOptions(
                sectionGeneral,
                "Cursed",
                false,
                "Enables extra/wip content\nyes there's a fucking minecraft skin",
                true);

            RestKeybind = Config.BindAndOptions(
                sectionGeneral,
                "Rest Key",
                new KeyboardShortcut(KeyCode.Alpha1),
                "key to play Rest emote");

            VoiceKey = Config.BindAndOptions(
                sectionGeneral,
                "Voice Line Key",
                new KeyboardShortcut(KeyCode.CapsLock),
                "key to play voice lines from Red Alert 2");

            VoiceInLobby = Config.BindAndOptions(
                sectionGeneral,
                "Voice Line In Lobby",
                false,
                "For the Red Alert 2 fans out there");

            RA2Icon = Config.BindAndOptions(
                sectionGeneral,
                "red alert 2 icon",
                false,
                "Changes character icon to the unit icon from Red Alert 2",
                true);

            string sectionSurvivors = "0-1. Survivors";

            TeslaEnabled = Config.CharacterEnableConfig(sectionSurvivors, "Tesla Trooper", "", true);
            DesolatorEnabled = Config.CharacterEnableConfig(sectionSurvivors, "Desolator", "", true);
            ChronoEnabled = Config.CharacterEnableConfig(sectionSurvivors, "Chrono Legionnaire BETA", "multiplayer ready and everything. just needs polish, and some maybe kit reworks. feedback welcome!", true);
            GIEnabled = Config.CharacterEnableConfig(sectionSurvivors, "GI ALPHA", "all skills perfactly functional in multiplayer and everything, just needs visuals. reach out and give feedback!", false);
            ConscriptEnabled = Config.CharacterEnableConfig(sectionSurvivors, "Conscript NOT EVEN ALPHA", "seriously just barely a proof of concept that probably won't even go anywhere for a long long time. but hey if you're curious", false);
            MCVEnabled = Config.CharacterEnableConfig(sectionSurvivors, "MCV Prototype", "we're getting stupid now", true);
        }
    }
}
