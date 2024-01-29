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
            //1-1, 1-2, 1-3, the boys
            //2-1 compats?
            string section = "0-0. General";

            Debug = RA2Plugin.instance.Config.Bind<bool>(
                section,
                "Debug Logs", 
                false, 
                "In case I forget to remove something");

            string survivorSection = "0-1. Survivors";

            //ChronoEnabled = Config.CharacterEnableConfig(survivorSection, "Chrono Legionnaire");

        }
    }
}
