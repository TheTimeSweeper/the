using LfPMod.Modules;

namespace LfPMod.Survivors.Mamluk.MamlukContent
{
    public static class CharacterConfig
    {

        [Configure(SectionSkills,
            2f//,
            //min = 0, max = 1,
            //name = "Name",
            //description = "dscrep",
            //restartRequired = true
            )]
        public static ConfigEntry<float> Shield_Primary_DashSpeed;


        public const string ConfigVersion = " 0.0";
        public const string SectionBody = "1-1. Mamluk Body" + ConfigVersion;
        public const string SectionSkills = "1-1. Mamluk Skills" + ConfigVersion;
        public static void Init()
        {
            string section = "Henry";

            Config.InitConfigAttributes(typeof(CharacterConfig));
        }
    }
}
