using BepInEx.Configuration;
using RA2Mod.Modules;

namespace RA2Mod.Survivors.GI
{
    public static class GIConfig
    {
        public static ConfigEntry<float> M1PistolDamage;
        public static ConfigEntry<float> M1PistolInterval;
        public static ConfigEntry<float> M1PistolFinalInterval;
        public static ConfigEntry<int> M1PistolShots;

        public static ConfigEntry<float> M1HeavyFireDamage;
        public static ConfigEntry<float> M1HeavyFireDuration;
        public static ConfigEntry<float> M1HeavyFireRadius;
        public static ConfigEntry<float> M1HeavyFireForce;
        public static ConfigEntry<float> M1HeavyFireRecoil;

        public static ConfigEntry<float> M1MissileDuration;
        public static ConfigEntry<float> M1MissileDamage;
        public static ConfigEntry<float> M1MissileExplosionRadius;

        public static ConfigEntry<float> M1HeavyMissileDuration;
        public static ConfigEntry<float> M1HeavyMissileDamage;
        public static ConfigEntry<float> M1HeavyMissileExplosionRadius;

        public static ConfigEntry<float> M2CaltropsDamage;
        public static ConfigEntry<float> M2CaltropsDuration;
        public static ConfigEntry<float> M2CaltropsPitch;

        public static ConfigEntry<float> M3SlideDuration;


        public static ConfigEntry<float> M4TransformInDuration;
        public static ConfigEntry<float> M4TransformOutDuration;
        public static ConfigEntry<float> M4TransformArmor;

        public static void Init()
        {
            string section = "1-4. G.I. 0.0.1";
            #region m1 1 1
            M1PistolDamage = Config.BindAndOptionsSlider(
                section,
                "M1PistolDamage",
                1f,
                "",
                0,
                20);

            M1PistolInterval = Config.BindAndOptionsSlider(
                section,
                "M0PistolInterval",
                0.13f,
                "",
                0,
                1);

            M1PistolFinalInterval = Config.BindAndOptionsSlider(
                section,
                "M0PistolFinalInterval",
                0.4f,
                "",
                0,
                1);

            M1PistolShots = Config.BindAndOptions(
                section,
                "M0PistolShots",
                3,
                "");
            #endregion m1 1 1

            #region m1 1 2
            M1HeavyFireDamage = Config.BindAndOptionsSlider(
                section,
                "M1HeavyFireDamage",
                2f,
                "",
                0,
                20);

            M1HeavyFireDuration = Config.BindAndOptionsSlider(
                section,
                "M1HeavyFireDuration",
                0.11F,
                "",
                0,
                20);
            M1HeavyFireRadius = Config.BindAndOptionsSlider(
                section,
                "M1HeavyFireRadius",
                3f,
                "",
                0,
                20);

            M1HeavyFireForce = Config.BindAndOptionsSlider(
                section,
                "M1HeavyFireForce",
                1000f,
                "",
                0,
                10000);
            M1HeavyFireRecoil = Config.BindAndOptionsSlider(
                section,
                "M1HeavyFireRecoil",
                0.5f,
                "",
                0,
                2);
            #endregion m1 1 2

            #region m1 2 1
            M1MissileDuration = Config.BindAndOptionsSlider(
                section,
                "M1MissileDuration",
                0.6f,
                "",
                0,
                20);
            M1MissileDamage = Config.BindAndOptionsSlider(
                section,
                "M1MissileDamage",
                4f,
                "",
                0,
                20);

            M1MissileExplosionRadius = Config.BindAndOptionsSlider(
                section,
                "M1MissileExplosionRadius",
                7f,
                "",
                0,
                20,
                true);
            #endregion m1 2 1

            #region m1 2 2
            M1HeavyMissileDuration = Config.BindAndOptionsSlider(
                section,
                "M1HeavyMissileDuration",
                0.4f,
                "",
                0,
                20);
            M1HeavyMissileDamage = Config.BindAndOptionsSlider(
                section,
                "M1HeavyMissileDamage",
                6f,
                "",
                0,
                20);
            M1HeavyMissileExplosionRadius = Config.BindAndOptionsSlider(
                section,
                "M1HeavyMissileExplosionRadius",
                10f,
                "",
                0,
                20,
                true);
            #endregion m1 2 2

            #region m2
            M2CaltropsDamage = Config.BindAndOptionsSlider(
                section,
                "M2CaltropsDamage",
                0.3f,
                "",
                0,
                20);

            M2CaltropsDuration = Config.BindAndOptionsSlider(
                section,
                "M2CaltropsDuration",
                0.5f,
                "",
                0,
                10);

            M2CaltropsPitch = Config.BindAndOptionsSlider(
                section,
                "M2CaltropsPitch",
                -8f,
                "",
                -90,
                90);
            #endregion m2

            #region m3

            M3SlideDuration = Config.BindAndOptionsSlider(
                section,
                "M3SlideDuration",
                2f,
                "",
                0,
                20);
            #endregion m3

            M4TransformInDuration = Config.BindAndOptionsSlider(
                section,
                "M4TransformDuration",
                0.3f,
                "",
                0,
                20);

            M4TransformOutDuration = Config.BindAndOptionsSlider(
                section,
                "M4TransformOutDuration",
                0.0f,
                "",
                0,
                20);

            M4TransformArmor = Config.BindAndOptionsSlider(
                section,
                "M4TransformArmor",
                50f,
                "",
                0,
                1000);
        }
    }
}
