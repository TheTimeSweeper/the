using BepInEx.Configuration;

namespace PlagueMod.Survivors.Plague
{
    public static class PlagueConfig
    {
        public static ConfigEntry<float> blastJumpForward;
        public static ConfigEntry<float> blastJumpUpward;
        public static ConfigEntry<float> blastJumpAirControl;
        public static ConfigEntry<float> blastJumpGravity;
        public static ConfigEntry<float> bombAirControl;
        public static ConfigEntry<float> bombSmallHop;

        public static void Init()
        {
            string section = "Plague";
            blastJumpForward = Modules.Config.BindAndOptionsSlider(
                section,
                "blastJumpForward",
                10,
                "",
                0,
                20);
            blastJumpUpward = Modules.Config.BindAndOptionsSlider(
                section,
                "blastJumpUpward",
                10,
                "",
                0,
                100);
            blastJumpAirControl = Modules.Config.BindAndOptionsSlider(
                section,
                "blastJumpAirControl",
                0.1f,
                "",
                0,
                1);
            blastJumpGravity = Modules.Config.BindAndOptionsSlider(
                section,
                "blastJumpGravity",
                1f,
                "",
                0,
                100);        

            bombAirControl = Modules.Config.BindAndOptionsSlider(
                section,
                "bombAirControl",
                0.25f,
                "",
                0,
                1);

            bombSmallHop = Modules.Config.BindAndOptionsSlider(
                section,
                "bombSmallHop",
                3f,
                "",
                0,
                10);
        }
    }
}
