using RA2Mod.Modules;

namespace RA2Mod.Survivors.GI
{
    public static class GIConfig
    {
        public static ConfigEntry<float> M1_Pistol_Damage;
        public static ConfigEntry<bool> M1_Pistol_Falloff;
        public static ConfigEntry<float> M1_Pistol_Interval;
        public static ConfigEntry<float> M1_Pistol_FinalInterval;
        public static ConfigEntry<int> M1_Pistol_Shots;

        public static ConfigEntry<float> M1_HeavyFire_Damage;
        public static ConfigEntry<float> M1_HeavyFire_Interval;
        public static ConfigEntry<float> M1_HeavyFire_FinalInterval;
        public static ConfigEntry<float> M1_HeavyFire_Radius;
        public static ConfigEntry<float> M1_HeavyFire_Force;
        public static ConfigEntry<float> M1_HeavyFire_Recoil;

        public static ConfigEntry<float> M1_Missile_Duration;
        public static ConfigEntry<float> M1_Missile_Damage;
        public static ConfigEntry<float> M1_Missile_ExplosionRadius;

        public static ConfigEntry<float> M1_HeavyMissile_Interval;
        public static ConfigEntry<float> M1_HeavyMissile_FinalInterval;
        public static ConfigEntry<float> M1_HeavyMissile_Damage;
        public static ConfigEntry<float> M1_HeavyMissile_ExplosionRadius;

        public static ConfigEntry<float> M2_Caltrops_ThrowDuration;
        public static ConfigEntry<float> M2_Caltrops_DotDamage;
        public static ConfigEntry<bool> M2_Caltrops_Optimized;
        //public static ConfigEntry<float> M2_Caltrops_DotDuration;
        //public static ConfigEntry<float> M2_Caltrops_Scale;
        //public static ConfigEntry<float> M2_Caltrops_Pitch;

        public static ConfigEntry<float> M2_Mine_ThrowDuration;
        public static ConfigEntry<float> M2_Mine_Pitch;
        public static ConfigEntry<float> M2_Mine_Damage;
        public static ConfigEntry<float> M2_Mine_Force;
        public static ConfigEntry<float> M2_Mine_TriggerRadius;
        public static ConfigEntry<float> M2_Mine_BlastRadius;

        public static ConfigEntry<float> M3_Slide_Duration;
        public static ConfigEntry<float> M3_Slide_AirDuration;
        public static ConfigEntry<float> M3_Slide_AirJumpMultiplier;

        public static ConfigEntry<float> M4_Transform_InDuration;
        public static ConfigEntry<float> M4_Transform_OutDuration;
        public static ConfigEntry<float> M4_Transform_Armor;

        public const string ConfigVersion = " 0.0";
        public const string SectionSkills = "1-4. G.I. Skills" + ConfigVersion;
        public const string SectionBody = "1-4. G.I. Body" + ConfigVersion;

        public static void Init()
        {
            //Config.DisableSection(SectionSkills);
            //Log.Warning(GISurvivor.instance.bodyName);
            #region m1 1 1
            M1_Pistol_Damage = Config.BindAndOptions(
                SectionSkills,
                "M1_Pistol_Damage",
                1.0f,
                0,
                20,
                "");
            M1_Pistol_Falloff = Config.BindAndOptions(
                SectionSkills,
                "M1_Pistol_Falloff",
                true,
                "");

            M1_Pistol_Interval = Config.BindAndOptions(
                SectionSkills,
                "M1_Pistol_Interval",
                0.12f,
                0,
                1,
                "");

            M1_Pistol_FinalInterval = Config.BindAndOptions(
                SectionSkills,
                "M1_Pistol_FinalInterval",
                0.4f,
                0,
                1,
                "");

            M1_Pistol_Shots = Config.BindAndOptions(
                SectionSkills,
                "M1_Pistol_Shots",
                3,
                "");
            #endregion m1 1 1

            #region m1 1 2
            M1_HeavyFire_Damage = Config.BindAndOptions(
                SectionSkills,
                "M1_HeavyFire_Damage",
                2f,
                0,
                20,
                "");

            M1_HeavyFire_Interval = Config.BindAndOptions(
                SectionSkills,
                "M1_HeavyFire_Interval",
                0.13F,
                0,
                20,
                "");

            M1_HeavyFire_FinalInterval = Config.BindAndOptions(
                SectionSkills,
                "M1_HeavyFire_FinalInterval",
                0.15F,
                0,
                20,
                "");
            M1_HeavyFire_Radius = Config.BindAndOptions(
                SectionSkills,
                "M1_HeavyFire_Radius",
                3f,
                0,
                20,
                "");
            
            M1_HeavyFire_Force = Config.BindAndOptions(
                SectionSkills,
                "M1_HeavyFire_Force",
                200f,
                0,
                10000,
                "");
            M1_HeavyFire_Recoil = Config.BindAndOptions(
                SectionSkills,
                "M1_HeavyFire_Recoil",
                0.5f,
                0,
                2,
                "");
            #endregion m1 1 2

            #region m1 2 1
            M1_Missile_Duration = Config.BindAndOptions(
                SectionSkills,
                "M1_Missile_Duration",
                0.5f,
                0,
                20,
                "");
            M1_Missile_Damage = Config.BindAndOptions(
                SectionSkills,
                "M1_Missile_Damage",
                2.2f,
                0,
                20,
                "");

            M1_Missile_ExplosionRadius = Config.BindAndOptions(
                SectionSkills,
                "M1_Missile_ExplosionRadius",
                7f,
                0,
                20,
                "",
                true);
            #endregion m1 2 1

            #region m1 2 2
            M1_HeavyMissile_Interval = Config.BindAndOptions(
                SectionSkills,
                "M1_HeavyMissile_Interval",
                0.3f,
                0,
                20,
                "");
            M1_HeavyMissile_FinalInterval = Config.BindAndOptions(
                SectionSkills,
                "M1_HeavyMissile_FinalInterval",
                0.4f,
                0,
                20,
                "");
            M1_HeavyMissile_Damage = Config.BindAndOptions(
                SectionSkills,
                "M1_HeavyMissile_Damage",
                3f,
                0,
                20,
                "");
            M1_HeavyMissile_ExplosionRadius = Config.BindAndOptions(
                SectionSkills,
                "M1_HeavyMissile_ExplosionRadius",
                12f,
                0,
                20,
                "",
                true);
            #endregion m1 2 2

            #region m2 1 1
            M2_Caltrops_ThrowDuration = Config.BindAndOptions(
                SectionSkills,
                "M2_Caltrops_ThrowDuration",
                0.5f,
                0,
                10,
                "");

            //M2_Caltrops_Pitch = Config.BindAndOptions(
            //    GISectionSkills,
            //    "M2_Caltrops_Pitch",
            //    -8f,
            //    -90,
            //    90,
            //    "");

            //M2_Caltrops_Scale = Config.BindAndOptions(
            //    GISectionSkills,
            //    "M2_Caltrops_Scale",
            //    30f,
            //    0,
            //    100,
            //    "",
            //    true);
            
            M2_Caltrops_DotDamage = Config.BindAndOptions(
                SectionSkills,
                "M2_Caltrops_DotDamage",
                0.25f,
                0,
                20,
                "");

            M2_Caltrops_Optimized = Config.BindAndOptions(
                SectionSkills,
                "M2_Caltrops_Optimized",
                false,
                "reduces caltrops from 25 projectiles to 9, but looks more jank");

            //M2_Caltrops_DotDuration = Config.BindAndOptions(
            //    GISectionSkills,
            //    "M2_Caltrops_DotDuration",
            //    3f,
            //    0,
            //    20,
            //    "");
            #endregion m2 1 1

            #region m2 1 2

            M2_Mine_ThrowDuration = Config.BindAndOptions(
                SectionSkills,
                "M2_Mine_ThrowDuration",
                0.5f,
                0,
                10,
                "");
            M2_Mine_Pitch = Config.BindAndOptions(
                SectionSkills,
                "M2_Mine_Pitch",
                -8f,
                -90,
                90,
                "");

            M2_Mine_Damage = Config.BindAndOptions(
                SectionSkills,
                "M2_Mine_Damage",
                5f,
                0,
                20,
                "");
            M2_Mine_Force = Config.BindAndOptions(
                SectionSkills,
                "M2_Mine_Force",
                4000f,
                0,
                10000,
                "");
            M2_Mine_TriggerRadius = Config.BindAndOptions(
                SectionSkills,
                "M2_Mine_TriggerRadius",
                8f,
                0,
                20,
                "");
            M2_Mine_BlastRadius = Config.BindAndOptions(
                SectionSkills,
                "M2_Mine_BlastRadius",
                12f,
                0,
                20,
                "");
            #endregion m2 1 2

            #region m3

            M3_Slide_Duration = Config.BindAndOptions(
                SectionSkills,
                "M3_Slide_Duration",
                1f,
                0,
                20,
                "");

            M3_Slide_AirDuration = Config.BindAndOptions(
                SectionSkills,
                "M3_Slide_AirDuration",
                0.8f,
                0,
                20,
                "");

            M3_Slide_AirJumpMultiplier = Config.BindAndOptions(
                SectionSkills,
                "M3_Slide_AirJumpMultiplier",
                1.3f,
                0,
                20,
                "");
            #endregion m3

            #region m4
            M4_Transform_InDuration = Config.BindAndOptions(
                SectionSkills,
                "M4_Transform_Duration",
                0.3f,
                0,
                20,
                "");

            M4_Transform_OutDuration = Config.BindAndOptions(
                SectionSkills,
                "M4_Transform_OutDuration",
                0.2f,
                0,
                20,
                "");

            M4_Transform_Armor = Config.BindAndOptions(
                SectionSkills,
                "M4_Transform_Armor",
                50f,
                0,
                1000,
                "");
            #endregion m4
        }
    }
}
