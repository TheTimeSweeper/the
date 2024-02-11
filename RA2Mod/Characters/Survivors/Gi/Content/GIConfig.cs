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
        public static ConfigEntry<float> M2CaltropsDamage;
        public static ConfigEntry<float> M2CaltropsPitch;

        public static ConfigEntry<float> M2MineThrowDuration;
        public static ConfigEntry<float> M2MinePitch;
        public static ConfigEntry<float> M2MineDamage;
        public static ConfigEntry<float> M2MineForce;
        public static ConfigEntry<float> M2MineTriggerRadius;
        public static ConfigEntry<float> M2MineBlastRadius;

        public static ConfigEntry<float> M3SlideDuration;
        public static ConfigEntry<float> M3SlideAirDuration;

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
            M1PistolDamage = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1PistolDamage",
                1.2f,
                0,
                20,
                "");

            M1PistolInterval = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1PistolInterval",
                0.12f,
                0,
                1,
                "");

            M1PistolFinalInterval = Config.BindAndOptionsSlider(
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
            M1HeavyFireDamage = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1HeavyFireDamage",
                2f,
                0,
                20,
                "");

            M1HeavyFireInterval = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1HeavyFireInterval",
                0.11F,
                0,
                20,
                "");
            string logbaseduration, loginterval, logbaseminusint, logfinal;
            int iterations = 50;

            logbaseduration = "\nlogbaseduration"; loginterval = "\nloginterval"; logbaseminusint = "\nlogbaseminusint"; logfinal = "\nlogfinal"; 
            for (int i = 0; i < iterations; i++)
            {
                float baseDuration = M1HeavyFireInterval.Value * 3f;
                float interval = M1HeavyFireInterval.Value / (1 + i *0.15f);
                logbaseduration += $"\n{baseDuration}";
                loginterval += $"\n{interval}";
                logbaseminusint += $"\n{baseDuration / interval}";
                logfinal += $"\n{(int)(baseDuration / interval)}";
            }
            Log.Warning("(int)");
            //Log.Warning(logbaseduration);
            //Log.Warning(loginterval);
            Log.Warning(logbaseminusint);
            Log.Warning(logfinal);

            logbaseduration = "\nlogbaseduration"; loginterval = "\nloginterval"; logbaseminusint = "\nlogbaseminusint"; logfinal = "\nlogfinal";
            for (int i = 0; i < iterations; i++)
            {
                float baseDuration = M1HeavyFireInterval.Value * 3.1f;
                float interval = M1HeavyFireInterval.Value / (1 + i * 0.15f);
                logbaseduration += $"\n{baseDuration}";
                loginterval += $"\n{interval}";
                logbaseminusint += $"\n{baseDuration / interval}";
                logfinal += $"\n{(int)(baseDuration / interval)}";
            }
            Log.Warning("(int) 3.1");
            //Log.Warning(logbaseduration);
            //Log.Warning(loginterval);
            //Log.Warning(logbaseminusint);
            Log.Warning(logfinal);

            logbaseduration = "\nlogbaseduration"; loginterval = "\nloginterval"; logbaseminusint = "\nlogbaseminusint"; logfinal = "\nlogfinal";
            for (int i = 0; i < iterations; i++)
            {
                float baseDuration = M1HeavyFireInterval.Value * 3f;
                float interval = M1HeavyFireInterval.Value / (1 + i * 0.15f);
                logbaseduration += $"\n{baseDuration}";
                loginterval += $"\n{interval}";
                logbaseminusint += $"\n{baseDuration / interval}";
                logfinal += $"\n{UnityEngine.Mathf.RoundToInt(baseDuration / interval)}";
            }
            Log.Warning("round");
            //Log.Warning(logbaseduration);
            //Log.Warning(loginterval);
            //Log.Warning(logbaseminusint);
            Log.Warning(logfinal);

            logbaseduration = "\nlogbaseduration"; loginterval = "\nloginterval"; logbaseminusint = "\nlogbaseminusint"; logfinal = "\nlogfinal";
            for (int i = 0; i < iterations; i++)
            {
                float baseDuration = M1HeavyFireInterval.Value * 3.1f;
                float interval = M1HeavyFireInterval.Value / (1 + i * 0.15f);
                logbaseduration += $"\n{baseDuration}";
                loginterval += $"\n{interval}";
                logbaseminusint += $"\n{baseDuration / interval}";
                logfinal += $"\n{UnityEngine.Mathf.RoundToInt(baseDuration / interval)}";
            }
            Log.Warning("round 3.1");
            //Log.Warning(logbaseduration);
            //Log.Warning(loginterval);
            //Log.Warning(logbaseminusint);
            Log.Warning(logfinal);

            M1HeavyFireFinalInterval = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1HeavyFireFinalInterval",
                0.14F,
                0,
                20,
                "");
            M1HeavyFireRadius = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1HeavyFireRadius",
                3f,
                0,
                20,
                "");

            M1HeavyFireForce = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1HeavyFireForce",
                200f,
                0,
                10000,
                "");
            M1HeavyFireRecoil = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1HeavyFireRecoil",
                0.5f,
                0,
                2,
                "");
            #endregion m1 1 2

            #region m1 2 1
            M1MissileDuration = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1MissileDuration",
                0.8f,
                0,
                20,
                "");
            M1MissileDamage = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1MissileDamage",
                3f,
                0,
                20,
                "");

            M1MissileExplosionRadius = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1MissileExplosionRadius",
                6f,
                0,
                20,
                "",
                true);
            #endregion m1 2 1

            #region m1 2 2
            M1HeavyMissileDuration = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1HeavyMissileDuration",
                0.4f,
                0,
                20,
                "");
            M1HeavyMissileDamage = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1HeavyMissileDamage",
                5f,
                0,
                20,
                "");
            M1HeavyMissileExplosionRadius = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M1HeavyMissileExplosionRadius",
                12f,
                0,
                20,
                "",
                true);
            #endregion m1 2 2

            #region m2 1 1
            M2CaltropsThrowDuration = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M2CaltropsThrowDuration",
                0.5f,
                0,
                10,
                "");

            M2CaltropsPitch = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M2CaltropsPitch",
                -8f,
                -90,
                90,
                "");

            M2CaltropsDamage = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M2CaltropsDamage",
                0.25f,
                0,
                20,
                "");
            #endregion m2 1 1

            #region m2 1 2

            M2MineThrowDuration = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M2MineThrowDuration",
                0.5f,
                0,
                10,
                "");
            M2MinePitch = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M2MinePitch",
                -8f,
                -90,
                90,
                "");

            M2MineDamage = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M2MineDamage",
                5f,
                0,
                20,
                "");
            M2MineForce = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M2MineForce",
                4000f,
                0,
                10000,
                "");
            M2MineTriggerRadius = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M2MineTriggerRadius",
                8f,
                0,
                20,
                "");
            M2MineBlastRadius = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M2MineBlastRadius",
                12f,
                0,
                20,
                "");
            #endregion m2 1 2

            #region m3

            M3SlideDuration = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M3SlideDuration",
                0.8f,
                0,
                20,
                "");

            M3SlideAirDuration = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M3SlideAirDuration",
                0.69f,
                0,
                20,
                "");
            #endregion m3

            #region m4
            M4TransformInDuration = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M4TransformDuration",
                0.3f,
                0,
                20,
                "");

            M4TransformOutDuration = Config.BindAndOptionsSlider(
                GISectionSkills,
                "M4TransformOutDuration",
                0.2f,
                0,
                20,
                "");

            M4TransformArmor = Config.BindAndOptionsSlider(
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
