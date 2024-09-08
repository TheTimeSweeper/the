using RA2Mod.Modules;

namespace RA2Mod.Survivors.Conscript
{
    public static class ConscriptConfig
    {
        public const string ConfigVersion = " 0.0";
        public const string SectionSkills = "1-5. Conscript Skills" + ConfigVersion;
        public const string SectionBody = "1-5. Conscript Body" + ConfigVersion;

        public static ConfigEntry<float> M1_Rifle_Damage;
        public static ConfigEntry<float> M1_Rifle_Duration;
        public static ConfigEntry<float> M3_Buff_Duration;


        public static void Init()
        {
            Config.DisableSection(SectionSkills);

            #region m1 1 1
            M1_Rifle_Damage = Config.BindAndOptions(
                SectionSkills,
                "M1_Rifle_Damage",
                3.0f,
                0,
                20,
                "");
            M1_Rifle_Duration = Config.BindAndOptions(
                SectionSkills,
                "M1_Rifle_Duration",
                0.3f,
                0,
                20,
                "");

            M3_Buff_Duration = Config.BindAndOptions(
                SectionSkills,
                "M3_Buff_Duration",
                4f,
                0,
                20,
                "");

            #endregion m4
        }
    }
}
