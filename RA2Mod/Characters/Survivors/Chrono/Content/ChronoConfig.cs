using BepInEx.Configuration;
using RA2Mod.Modules;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoConfig
    {
        public static ConfigEntry<float> chronoStacksToVanish;

        public static ConfigEntry<float> M1Damage;

        public static ConfigEntry<float> M3Radius;

        public static ConfigEntry<float> M4Interval;
        public static ConfigEntry<float> M4Duration;
        public static ConfigEntry<float> M4Damage;

        public static void Init()
        {
            string section = "Chrono";

            chronoStacksToVanish = Config.BindAndOptionsSlider(
                section,
                "chronoStacksToVanish",
                20,
                "",
                0,
                100);

            M1Damage = Config.BindAndOptionsSlider(
                section,
                "M1Damage",
                0.1f,
                "",
                0,
                10);

            M3Radius = Config.BindAndOptionsSlider(
                section,
                "M3Radius",
                20f,
                "",
                0,
                100);

            M4Interval = Config.BindAndOptionsSlider(
                section,
                "M4Interval",
                0.1f,
                "",
                0,
                1);

            M4Duration = Config.BindAndOptionsSlider(
                section,
                "M4Duration",
                3f,
                "",
                0,
                10);

            M4Damage = Config.BindAndOptionsSlider(
                section,
                "M4Damage",
                3f,
                "",
                0,
                10);
        }
    }
}
