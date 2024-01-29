using BepInEx.Configuration;
using RA2Mod.Modules;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoConfig
    {
        public static ConfigEntry<float> M0SprintTeleportDistTimeMulti;
        public static ConfigEntry<float> M0SprintTeleportTimeTimeMulti;

        public static ConfigEntry<bool> M0TeleportOnRelese;

        public static ConfigEntry<float> M1Damage;
        public static ConfigEntry<float> M1Duration;

        public static ConfigEntry<float> M2Damage;

        public static ConfigEntry<float> M3Radius;

        public static ConfigEntry<float> M4Interval;
        public static ConfigEntry<float> M4Duration;
        public static ConfigEntry<float> M4Damage;
        public static ConfigEntry<float> M4ChronoStacksToVanish;


        public static ConfigEntry<float> y;
        public static ConfigEntry<float> yy;
        public static ConfigEntry<float> z;
        public static ConfigEntry<float> fov;
        public static ConfigEntry<float> t;
        public static ConfigEntry<float> y2;
        public static ConfigEntry<float> z2;
        public static ConfigEntry<float> fov2;
        public static ConfigEntry<float> t2;

        public static void Init()
        {
            string section = "1-3. Chrono Legionnaire 0.0.0";

            M0TeleportOnRelese = Config.BindAndOptions(
                section,
                "M0TeleportOnRelese",
                false,
                "Should sprinting teleport on release, or should it require a second press of sprint");

            M0SprintTeleportDistTimeMulti = Config.BindAndOptionsSlider(
                section,
                "M0SprintBlinkTimeMulti",
                0.16f,
                "Phase out penalty multiplier based on distance teleported.",
                0,
                1);

            M0SprintTeleportTimeTimeMulti = Config.BindAndOptionsSlider(
                section,
                "M0SprintblinkTimeTimeMulti",
                0.5f,
                "Phase out penalty multiplier based on time spent in teleporting state. Only comes into play after 1 second, and replaces distance penalty if larger (does not add)",
                0,
                1);

            M1Damage = Config.BindAndOptionsSlider(
                section,
                "M1Damage",
                2f,
                "",
                0,
                10);

            M1Duration = Config.BindAndOptionsSlider(
                section,
                "M1Duration",
                0.8f,
                "",
                0,
                10);

            M2Damage = Config.BindAndOptionsSlider(
                section,
                "M2Damage",
                5f,
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
                3f,
                "",
                0,
                10);

            M4Damage = Config.BindAndOptionsSlider(
                section,
                "M4Damage",
                0.8f,
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

            y = Config.BindAndOptionsSlider(
                section,
                "y",
                9,
                "",
                0,
                100);

            yy = Config.BindAndOptionsSlider(
                section,
                "yy",
                10,
                "",
                0,
                100);
            z = Config.BindAndOptionsSlider(
                section,
                "z",
                -17,
                "",
                -100,
                0);
            fov = Config.BindAndOptionsSlider(
                section,
                "fov",
                80,
                "",
                0,
                200);
            t = Config.BindAndOptionsSlider(
                section,
                "t",
                0.5f,
                "",
                0,
                3);

            y2 = Config.BindAndOptionsSlider(
                section,
                "y2",
                0,
                "",
                0,
                100);
            z2 = Config.BindAndOptionsSlider(
                section,
                "z2",
                -15,
                "",
                -100,
                0);
            fov2 = Config.BindAndOptionsSlider(
                section,
                "fov2",
                66,
                "",
                0,
                200);
            t2 = Config.BindAndOptionsSlider(
                section,
                "t2",
                0.5f,
                "",
                0,
                3);
        }
    }
}
