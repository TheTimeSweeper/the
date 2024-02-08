using RA2Mod.Modules;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoConfig
    {
        public static ConfigEntry<float> M0SprintTeleportDistTimeMulti;
        public static ConfigEntry<float> M0SprintTeleportTimeTimeMulti;
        public static ConfigEntry<float> M0CameraLerpTime;

        public static ConfigEntry<bool> M0TeleportOnRelese;

        public static ConfigEntry<float> M1Damage;
        public static ConfigEntry<float> M1Duration;
        public static ConfigEntry<float> M1Radius;
        public static ConfigEntry<float> M1Screenshake;

        public static ConfigEntry<float> M2Damage;

        public static ConfigEntry<float> M3Radius;
        public static ConfigEntry<float> M3Delay;
        public static ConfigEntry<float> M3ChronosphereCameraLerpTime;

        public static ConfigEntry<float> M4Interval;
        public static ConfigEntry<float> M4Duration;
        public static ConfigEntry<float> M4Damage;
        public static ConfigEntry<float> M4ChronoStacksToVanish;

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
                0.169f,
                "Phase out penalty multiplier based on distance teleported.",
                0,
                1);

            M0SprintTeleportTimeTimeMulti = Config.BindAndOptionsSlider(
                section,
                "M0SprintblinkTimeTimeMulti",
                0.5f,
                "Phase out penalty multiplier based on time spent in teleporting state. Only comes into play after 1 second, and only replaces distance penalty if larger (does not add)",
                0,
                1);
            M0CameraLerpTime = Config.BindAndOptionsSlider(
                section,
                "M0CameraLerpTime",
                0.5f,
                "",
                0,
                3);
            //
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
            M1Radius = Config.BindAndOptionsSlider(
                section,
                "M1Radius",
                6f,
                "",
                0,
                100);

            M1Screenshake = Config.BindAndOptionsSlider(
                section,
                "M1Screenshake",
                2f,
                "",
                0,
                10);
            //
            M2Damage = Config.BindAndOptionsSlider(
                section,
                "M2Damage",
                5f,
                "",
                0,
                10);
            //
            M3Radius = Config.BindAndOptionsSlider(
                section,
                "M3Radius",
                20f,
                "",
                0,
                100);
            M3Delay = Config.BindAndOptionsSlider(
                section,
                "M3Delay",
                0.3f,
                "",
                0,
                1);
            M3ChronosphereCameraLerpTime = Config.BindAndOptionsSlider(
                section,
                "M3ChronosphereCameraLerpTime",
                0.5f,
                "",
                0,
                3);
            //
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

        }
    }
}
