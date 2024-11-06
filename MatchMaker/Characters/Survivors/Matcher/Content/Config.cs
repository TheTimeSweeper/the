using MatcherMod.Modules;

namespace MatcherMod.Survivors.Matcher.MatcherContent
{
    public static class Config
    {
        public const string SectionGeneral = "Hi";
        public const string SectionBody = "Body";
        public const string SectionSkills = "Skills";

        public static ConfigEntry<float> M1_Sword_Damage;
        public static ConfigEntry<float> M1_Sword_Duration;
        public static ConfigEntry<float> M1_Sword_Multiplier;

        public static ConfigEntry<float> M2_Staff_Damage;
        public static ConfigEntry<float> M2_Staff_Radius;
        public static ConfigEntry<float> M2_Staff2_Damage;
        public static ConfigEntry<float> M2_Staff2_Radius;
        public static ConfigEntry<float> M2_Staff2_MatchRadius;
        public static ConfigEntry<float> M2_Staff2_SmallHop;
        public static ConfigEntry<float> M2_Staff2_AntiGrav;

        public static ConfigEntry<float> M3_Shield_RollDuration;
        public static ConfigEntry<float> M3_Shield_RollMatchSpeedMultiplier;
        public static ConfigEntry<float> M3_Shield_RollInitialSpeed;
        public static ConfigEntry<float> M3_Shield_RollFinalSpeed;

        public static ConfigEntry<float> M3_Shield_BuffArmor;

        public static ConfigEntry<int> M4_Key_UnlockBaseValue;
        public static ConfigEntry<float> M4_Crate_PercentChance;
        public static ConfigEntry<float> M4_Brain_Experience;
        public static ConfigEntry<float> M4_Brain_NearDistance;

        public static ConfigEntry<float> nipper;

        public static void Init()
        {
            //someConfigBool = Modules.Config.BindAndOptions(
            //    section,
            //    "someConfigBool",
            //    true,
            //    "this creates a bool config, and a checkbox option in risk of options");

            nipper = Modules.Config.BindAndOptions(
                SectionBody,
                nameof(nipper),
                6f, -2000, 2000);

            M1_Sword_Damage = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M1_Sword_Damage),
                2.2f);

            M1_Sword_Multiplier = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M1_Sword_Multiplier),
                1.5f);

            M1_Sword_Duration = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M1_Sword_Duration),
                0.9f);

            M2_Staff_Damage = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff_Damage),
                2.0f);

            M2_Staff_Radius = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff_Radius),
                30.0f,
                0,
                100,
                "",
                true);

            M2_Staff2_Damage = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff2_Damage),
                2.0f);

            M2_Staff2_Radius = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff2_Radius),
                30.0f,
                0,
                100,
                "",
                true);
            M2_Staff2_MatchRadius = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff2_MatchRadius),
                5.0f);

            M2_Staff2_SmallHop = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff2_SmallHop),
                10.0f);
            M2_Staff2_AntiGrav = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M2_Staff2_AntiGrav),
                5.0f);

            M3_Shield_BuffArmor = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_BuffArmor),
                100f,
                0,
                1000);

            M3_Shield_RollDuration = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_RollDuration),
                0.4f);

            M3_Shield_RollMatchSpeedMultiplier = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_RollMatchSpeedMultiplier),
                0.5f);

            M3_Shield_RollInitialSpeed = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_RollInitialSpeed),
                5f);
            M3_Shield_RollFinalSpeed = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M3_Shield_RollFinalSpeed),
                2.5f,
                0,
                50,
                "");

            M4_Key_UnlockBaseValue = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M4_Key_UnlockBaseValue),
                3);
            M4_Crate_PercentChance = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M4_Crate_PercentChance),
                7f,
                0,
                100);
            M4_Brain_Experience = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M4_Brain_Experience),
                3f);

            M4_Brain_NearDistance = Modules.Config.BindAndOptions(
                SectionSkills,
                nameof(M4_Brain_NearDistance),
                15f);
        }
    }
}
