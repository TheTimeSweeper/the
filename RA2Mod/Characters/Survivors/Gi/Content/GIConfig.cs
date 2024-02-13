using RA2Mod.Modules;

namespace RA2Mod.Survivors.GI
{
    public static class GIConfig
    {
        public static ConfigEntry<float> M1PistolDamage;
        public static ConfigEntry<bool> M1PistolFalloff;
        public static ConfigEntry<float> M1PistolInterval;
        public static ConfigEntry<float> M1PistolFinalInterval;
        public static ConfigEntry<int> M1PistolShots;

        public static ConfigEntry<float> M1HeavyFireDamage;
        public static ConfigEntry<float> M1HeavyFireInterval;
        public static ConfigEntry<float> M1HeavyFireFinalInterval;
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
        public static ConfigEntry<float> M2CaltropsDotDamage;
        public static ConfigEntry<float> M2CaltropsDotDuration;
        public static ConfigEntry<float> M2CaltropsScale;
        public static ConfigEntry<float> M2CaltropsPitch;

        public static ConfigEntry<float> M2MineThrowDuration;
        public static ConfigEntry<float> M2MinePitch;
        public static ConfigEntry<float> M2MineDamage;
        public static ConfigEntry<float> M2MineForce;
        public static ConfigEntry<float> M2MineTriggerRadius;
        public static ConfigEntry<float> M2MineBlastRadius;

        public static ConfigEntry<float> M3SlideDuration;
        public static ConfigEntry<float> M3SlideAirDuration;
        public static ConfigEntry<float> M3SlideAirJumpMultiplier;

        public static ConfigEntry<float> M4TransformInDuration;
        public static ConfigEntry<float> M4TransformOutDuration;
        public static ConfigEntry<float> M4TransformArmor;

        public const string ConfigVersion = " 0.0.0";
        public const string GISectionSkills = "1-4. G.I. Skills" + ConfigVersion;
        public const string GISectionBody = "1-4. G.I. Body" + ConfigVersion;
        public const string GISectionStocks = "1-4. G.I. Body" + ConfigVersion;

        public static void Init()
        {
            Config.DisableSection(GISectionSkills);

            Config.ConfigureBody(GISurvivor.instance.prefabCharacterBody, GIConfig.GISectionBody);
            #region m1 1 1
            M1PistolDamage = Config.BindAndOptions(
                GISectionSkills,
                "M1PistolDamage",
                1.0f,
                0,
                20,
                "");
            M1PistolFalloff = Config.BindAndOptions(
                GISectionSkills,
                "M1PistolFalloff",
                true,
                "");

            M1PistolInterval = Config.BindAndOptions(
                GISectionSkills,
                "M1PistolInterval",
                0.12f,
                0,
                1,
                "");

            M1PistolFinalInterval = Config.BindAndOptions(
                GISectionSkills,
                "M1PistolFinalInterval",
                0.4f,
                0,
                1,
                "");

            M1PistolShots = Config.BindAndOptions(
                GISectionSkills,
                "M1PistolShots",
                3,
                "");
            #endregion m1 1 1

            #region m1 1 2
            M1HeavyFireDamage = Config.BindAndOptions(
                GISectionSkills,
                "M1HeavyFireDamage",
                2f,
                0,
                20,
                "");

            M1HeavyFireInterval = Config.BindAndOptions(
                GISectionSkills,
                "M1HeavyFireInterval",
                0.13F,
                0,
                20,
                "");

            M1HeavyFireFinalInterval = Config.BindAndOptions(
                GISectionSkills,
                "M1HeavyFireFinalInterval",
                0.15F,
                0,
                20,
                "");
            M1HeavyFireRadius = Config.BindAndOptions(
                GISectionSkills,
                "M1HeavyFireRadius",
                3f,
                0,
                20,
                "");
            
            M1HeavyFireForce = Config.BindAndOptions(
                GISectionSkills,
                "M1HeavyFireForce",
                200f,
                0,
                10000,
                "");
            M1HeavyFireRecoil = Config.BindAndOptions(
                GISectionSkills,
                "M1HeavyFireRecoil",
                0.5f,
                0,
                2,
                "");
            #endregion m1 1 2

            #region m1 2 1
            M1MissileDuration = Config.BindAndOptions(
                GISectionSkills,
                "M1MissileDuration",
                0.8f,
                0,
                20,
                "");
            M1MissileDamage = Config.BindAndOptions(
                GISectionSkills,
                "M1MissileDamage",
                3f,
                0,
                20,
                "");

            M1MissileExplosionRadius = Config.BindAndOptions(
                GISectionSkills,
                "M1MissileExplosionRadius",
                6f,
                0,
                20,
                "",
                true);
            #endregion m1 2 1

            #region m1 2 2
            M1HeavyMissileDuration = Config.BindAndOptions(
                GISectionSkills,
                "M1HeavyMissileDuration",
                0.4f,
                0,
                20,
                "");
            M1HeavyMissileDamage = Config.BindAndOptions(
                GISectionSkills,
                "M1HeavyMissileDamage",
                5f,
                0,
                20,
                "");
            M1HeavyMissileExplosionRadius = Config.BindAndOptions(
                GISectionSkills,
                "M1HeavyMissileExplosionRadius",
                12f,
                0,
                20,
                "",
                true);
            #endregion m1 2 2

            #region m2 1 1
            M2CaltropsThrowDuration = Config.BindAndOptions(
                GISectionSkills,
                "M2CaltropsThrowDuration",
                0.5f,
                0,
                10,
                "");

            M2CaltropsPitch = Config.BindAndOptions(
                GISectionSkills,
                "M2CaltropsPitch",
                -8f,
                -90,
                90,
                "");

            M2CaltropsScale = Config.BindAndOptions(
                GISectionSkills,
                "M2CaltropsScale",
                30f,
                0,
                100,
                "",
                true);

            M2CaltropsDotDamage = Config.BindAndOptions(
                GISectionSkills,
                "M2CaltropsDotDamage",
                0.25f,
                0,
                20,
                "");

            M2CaltropsDotDuration = Config.BindAndOptions(
                GISectionSkills,
                "M2CaltropsDotDuration",
                3f,
                0,
                20,
                "");
            #endregion m2 1 1

            #region m2 1 2

            M2MineThrowDuration = Config.BindAndOptions(
                GISectionSkills,
                "M2MineThrowDuration",
                0.5f,
                0,
                10,
                "");
            M2MinePitch = Config.BindAndOptions(
                GISectionSkills,
                "M2MinePitch",
                -8f,
                -90,
                90,
                "");

            M2MineDamage = Config.BindAndOptions(
                GISectionSkills,
                "M2MineDamage",
                5f,
                0,
                20,
                "");
            M2MineForce = Config.BindAndOptions(
                GISectionSkills,
                "M2MineForce",
                4000f,
                0,
                10000,
                "");
            M2MineTriggerRadius = Config.BindAndOptions(
                GISectionSkills,
                "M2MineTriggerRadius",
                8f,
                0,
                20,
                "");
            M2MineBlastRadius = Config.BindAndOptions(
                GISectionSkills,
                "M2MineBlastRadius",
                12f,
                0,
                20,
                "");
            #endregion m2 1 2

            #region m3

            M3SlideDuration = Config.BindAndOptions(
                GISectionSkills,
                "M3SlideDuration",
                1f,
                0,
                20,
                "");

            M3SlideAirDuration = Config.BindAndOptions(
                GISectionSkills,
                "M3SlideAirDuration",
                0.8f,
                0,
                20,
                "");

            M3SlideAirJumpMultiplier = Config.BindAndOptions(
                GISectionSkills,
                "M3SlideAirJumpMultiplier",
                1.3f,
                0,
                20,
                "");
            #endregion m3

            #region m4
            M4TransformInDuration = Config.BindAndOptions(
                GISectionSkills,
                "M4TransformDuration",
                0.3f,
                0,
                20,
                "");

            M4TransformOutDuration = Config.BindAndOptions(
                GISectionSkills,
                "M4TransformOutDuration",
                0.2f,
                0,
                20,
                "");

            M4TransformArmor = Config.BindAndOptions(
                GISectionSkills,
                "M4TransformArmor",
                50f,
                0,
                1000,
                "");
            #endregion m4
        }
    }
}
