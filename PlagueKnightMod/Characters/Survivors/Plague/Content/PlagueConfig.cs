using BepInEx.Configuration;

namespace PlagueMod.Survivors.Plague
{
    public static class PlagueConfig
    {
        public static ConfigEntry<float> blastJumpForward;
        public static ConfigEntry<float> blastJumpUpward;

        public static void Init()
        {
            string section = "Plague";
            blastJumpForward = Modules.Config.BindAndOptionsSlider(
                section,
                "blastJumpForward",
                10,
                "",
                0,
                100);
            blastJumpUpward = Modules.Config.BindAndOptionsSlider(
                section,
                "blastJumpUpward",
                10,
                "",
                0,
                100);
        }
    }
}
