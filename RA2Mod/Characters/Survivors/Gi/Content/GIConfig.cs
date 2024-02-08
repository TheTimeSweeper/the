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

        public static ConfigEntry<float> M2CaltropsThrowDuration;
        public static ConfigEntry<float> M2CaltropsDamage;
        public static ConfigEntry<float> M2CaltropsPitch;

        public static ConfigEntry<float> M2MineThrowDuration;
        public static ConfigEntry<float> M2MinePitch;
        public static ConfigEntry<float> M2MineDamage;
        public static ConfigEntry<float> M2MineForce;
        public static ConfigEntry<float> M2MineTriggerRadius;
        public static ConfigEntry<float> M2MineBlastRadius;

        public static ConfigEntry<float> M3SlideDuration;

        public static ConfigEntry<float> M4TransformInDuration;
        public static ConfigEntry<float> M4TransformOutDuration;
        public static ConfigEntry<float> M4TransformArmor;

        public static string GISection = "1-4. G.I. 0.0.1";

        public static void Init()
        {
            Config.DisableSection(GISection);

            Config.ConfigureBody(GISurvivor.instance.prefabCharacterBody, GIConfig.GISection);

            #region m1 1 1
            M1PistolDamage = Config.BindAndOptionsSlider(
                GISection,
                "M1PistolDamage",
                1f,
                "",
                0,
                20);

            M1PistolInterval = Config.BindAndOptionsSlider(
                GISection,
                "M0PistolInterval",
                0.13f,
                "",
                0,
                1);

            M1PistolFinalInterval = Config.BindAndOptionsSlider(
                GISection,
                "M0PistolFinalInterval",
                0.4f,
                "",
                0,
                1);

            M1PistolShots = Config.BindAndOptions(
                GISection,
                "M0PistolShots",
                3,
                "");
            #endregion m1 1 1

            #region m1 1 2
            M1HeavyFireDamage = Config.BindAndOptionsSlider(
                GISection,
                "M1HeavyFireDamage",
                2f,
                "",
                0,
                20);

            M1HeavyFireDuration = Config.BindAndOptionsSlider(
                GISection,
                "M1HeavyFireDuration",
                0.11F,
                "",
                0,
                20);
            M1HeavyFireRadius = Config.BindAndOptionsSlider(
                GISection,
                "M1HeavyFireRadius",
                3f,
                "",
                0,
                20);

            M1HeavyFireForce = Config.BindAndOptionsSlider(
                GISection,
                "M1HeavyFireForce",
                1000f,
                "",
                0,
                10000);
            M1HeavyFireRecoil = Config.BindAndOptionsSlider(
                GISection,
                "M1HeavyFireRecoil",
                0.5f,
                "",
                0,
                2);
            #endregion m1 1 2

            #region m1 2 1
            M1MissileDuration = Config.BindAndOptionsSlider(
                GISection,
                "M1MissileDuration",
                0.6f,
                "",
                0,
                20);
            M1MissileDamage = Config.BindAndOptionsSlider(
                GISection,
                "M1MissileDamage",
                4f,
                "",
                0,
                20);

            M1MissileExplosionRadius = Config.BindAndOptionsSlider(
                GISection,
                "M1MissileExplosionRadius",
                7f,
                "",
                0,
                20,
                true);
            #endregion m1 2 1

            #region m1 2 2
            M1HeavyMissileDuration = Config.BindAndOptionsSlider(
                GISection,
                "M1HeavyMissileDuration",
                0.4f,
                "",
                0,
                20);
            M1HeavyMissileDamage = Config.BindAndOptionsSlider(
                GISection,
                "M1HeavyMissileDamage",
                6f,
                "",
                0,
                20);
            M1HeavyMissileExplosionRadius = Config.BindAndOptionsSlider(
                GISection,
                "M1HeavyMissileExplosionRadius",
                10f,
                "",
                0,
                20,
                true);
            #endregion m1 2 2

            #region m2 1 1
            M2CaltropsThrowDuration = Config.BindAndOptionsSlider(
                GISection,
                "M2CaltropsThrowDuration",
                0.5f,
                "",
                0,
                10);

            M2CaltropsPitch = Config.BindAndOptionsSlider(
                GISection,
                "M2CaltropsPitch",
                -8f,
                "",
                -90,
                90);

            M2CaltropsDamage = Config.BindAndOptionsSlider(
                GISection,
                "M2CaltropsDamage",
                0.3f,
                "",
                0,
                20);
            #endregion m2 1 1

            #region m2 1 2

            M2MineThrowDuration = Config.BindAndOptionsSlider(
                GISection,
                "M2MineThrowDuration",
                0.5f,
                "",
                0,
                10);
            M2MinePitch = Config.BindAndOptionsSlider(
                GISection,
                "M2MinePitch",
                -8f,
                "",
                -90,
                90);

            M2MineDamage = Config.BindAndOptionsSlider(
                GISection,
                "M2MineDamage",
                3f,
                "",
                0,
                20);
            M2MineForce = Config.BindAndOptionsSlider(
                GISection,
                "M2MineForce",
                1000f,
                "",
                0,
                10000);
            M2MineTriggerRadius = Config.BindAndOptionsSlider(
                GISection,
                "M2MineTriggerRadius",
                8f,
                "",
                0,
                20);
            M2MineBlastRadius = Config.BindAndOptionsSlider(
                GISection,
                "M2MineBlastRadius",
                12f,
                "",
                0,
                20);
            #endregion m2 1 2

            #region m3

            M3SlideDuration = Config.BindAndOptionsSlider(
                GISection,
                "M3SlideDuration",
                0.5f,
                "",
                0,
                20);
            #endregion m3

            #region m4
            M4TransformInDuration = Config.BindAndOptionsSlider(
                GISection,
                "M4TransformDuration",
                0.3f,
                "",
                0,
                20);

            M4TransformOutDuration = Config.BindAndOptionsSlider(
                GISection,
                "M4TransformOutDuration",
                0.5f,
                "",
                0,
                20);

            M4TransformArmor = Config.BindAndOptionsSlider(
                GISection,
                "M4TransformArmor",
                50f,
                "",
                0,
                1000);
            #endregion m4
        }
    }
}
