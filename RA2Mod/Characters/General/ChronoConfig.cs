using BepInEx.Configuration;
using RA2Mod.Modules;

namespace RA2Mod.General
{
    public static class GeneralConfig
    {
        public static ConfigEntry<bool> Debug;

        public static ConfigEntry<bool> ChronoEnabled;

        public static void Init()
        {
            //0-0. General
            //0-1. Survivors
            //1-0, 1-1, 1-2, characters
            //2-0 compats?
            string section = "0-0. General";

            Debug = Config.BindAndOptions<bool>(
            section,
            "Debug Logs",
            true);

            string survivorSection = "0-1. Survivors";

            //ChronoEnabled = Config.CharacterEnableConfig(survivorSection, "Chrono Legionnaire");

        }
    }
}
