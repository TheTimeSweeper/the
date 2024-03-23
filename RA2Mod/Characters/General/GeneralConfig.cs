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

        public static ConfigEntry<bool> ChronoEnabled;
        public static ConfigEntry<bool> GIEnabled;

        public static void Init()
        {
            //0-0. General
                //0-1. Survivors, 0-2 items, etc
            //1-1, 1-2, 1-3, the boys
            //2-1 compats?
            string sectionGeneral = "0-0. General";
            /*
ReflectionTypeLoadException: Exception of type 'System.Reflection.ReflectionTypeLoadException' was thrown.
Stack trace:
System.Reflection.Assembly.GetExportedTypes () (at <44afb4564e9347cf99a1865351ea8f4a>:IL_0000)
RiskOfOptions.ExtensionMethods.GetModMetaData (System.Reflection.Assembly assembly) (at <4f97726794804a5f8d0d102c5af0919e>:IL_0008)
RiskOfOptions.ModSettingsManager.AddOption (RiskOfOptions.Options.BaseOption option) (at <4f97726794804a5f8d0d102c5af0919e>:IL_0005)
RA2Mod.Modules.Config.TryRegisterOption[T] (BepInEx.Configuration.ConfigEntry`1[T] entry, System.Single min, System.Single max, System.Boolean restartRequired) (at <2cc47b8797a54062971e34731797117e>:IL_009C)
RA2Mod.Modules.Config.BindAndOptions[T] (System.String section, System.String name, T defaultValue, System.Single min, System.Single max, System.String description, System.Boolean restartRequired) (at <2cc47b8797a54062971e34731797117e>:IL_0085)
RA2Mod.Modules.Config.BindAndOptions[T] (System.String section, System.String name, T defaultValue, System.String description, System.Boolean restartRequired) (at <2cc47b8797a54062971e34731797117e>:IL_0000)
RA2Mod.General.GeneralConfig.Init () (at <2cc47b8797a54062971e34731797117e>:IL_0032)
RA2Mod.RA2Plugin.Awake () (at <2cc47b8797a54062971e34731797117e>:IL_0013)
UnityEngine.GameObject:AddComponent(Type)
BepInEx.Bootstrap.Chainloader:Start()   
FlashWindow:.cctor()*/
            Debug = RA2Plugin.instance.Config.Bind<bool>(
                sectionGeneral,
                "Debug Logs", 
                false, 
                "In case I forget to remove something");
            Log.Warning("uh");
            NewColor = Config.BindAndOptions(
                sectionGeneral,
                "New Color (s?)",
                false,
                "add black for the wife",
                true);
            Log.Warning("uh2");

            Cursed = Config.BindAndOptions(
                sectionGeneral,
                "Cursed",
                false,
                "Enables extra/wip content\nyes there's a fucking minecraft skin",
                true);
            Log.Warning("uh3");

            RestKeybind = Config.BindAndOptions(
                sectionGeneral,
                "Rest Key",
                new KeyboardShortcut(KeyCode.Alpha1),
                "key to play Rest emote");
            Log.Warning("uh4");

            VoiceKey = Config.BindAndOptions(
                sectionGeneral,
                "Voice Line Key",
                new KeyboardShortcut(KeyCode.CapsLock),
                "key to play voice lines from Red Alert 2");
            Log.Warning("uh5");

            VoiceInLobby = Config.BindAndOptions(
                sectionGeneral,
                "Voice Line In Lobby",
                false,
                "For the Red Alert 2 fans out there");
            Log.Warning("uh6");

            RA2Icon = Config.BindAndOptions(
                sectionGeneral,
                "red alert 2 icon",
                false,
                "Changes character icon to the unit icon from Red Alert 2",
                true);

            string sectionSurvivors = "0-1. Survivors";

            //ChronoEnabled = Config.CharacterEnableConfig(survivorSection, "Chrono Legionnaire");
            GIEnabled = Config.CharacterEnableConfig(sectionSurvivors, "GI", "BETA", true);
        }
    }
}
