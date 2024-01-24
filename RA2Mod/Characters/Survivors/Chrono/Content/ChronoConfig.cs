using BepInEx.Configuration;
using RA2Mod.Modules;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoConfig
    {
        public static ConfigEntry<float> M4ChronoStacksToVanish;

        public static ConfigEntry<float> M0SprintBlinkTimeMulti;

        public static ConfigEntry<float> M1Damage;

        public static ConfigEntry<float> M3Radius;

        public static ConfigEntry<float> M4Interval;
        public static ConfigEntry<float> M4Duration;
        public static ConfigEntry<float> M4Damage;

        public static void Init()
        {
            string section = "4. Chrono Legionnaire";

            M0SprintBlinkTimeMulti = Config.BindAndOptionsSlider(
                section,
                "M0SprintBlinkTimeMulti",
                0.15f,
                "",
                0,
                1);

            M1Damage = Config.BindAndOptionsSlider(
                section,
                "M1Damage",
                1f,
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
                0.3f,
                "",
                0,
                1);

            M4Duration = Config.BindAndOptionsSlider(
                section,
                "M4Duration",
                2f,
                "",
                0,
                10);

            M4Damage = Config.BindAndOptionsSlider(
                section,
                "M4Damage",
                0.4f,
                "",
                0,
                10);

            M4ChronoStacksToVanish = Config.BindAndOptionsSlider(
                section,
                "chronoStacksToVanish",
                100,
                "",
                0,
                200);
        }
    }
}
