using BepInEx.Configuration;

namespace KatamariMod.Survivors.Katamari
{
    public static class KatamariConfig
    {
        public static ConfigEntry<float> passive_speedAirControlMultiplier;
        public static ConfigEntry<float> passive_speedAttackThreshold;
        public static ConfigEntry<float> passive_speedAttackMultiplier;

        public static ConfigEntry<float> ChargeRoll_Multiplier;

        public static void Init()
        {
            string section = "Skills";
            passive_speedAirControlMultiplier = Modules.Config.BindAndOptionsSlider(
                section,
                "passive_speedAirControlMultiplier",
                10,
                "lower air controll means more slipperiness, and more influence of momentum",
                -100,
                100);

            passive_speedAttackThreshold = Modules.Config.BindAndOptionsSlider(
                section,
                "passive_velocityAttackThreshold",
                10,
                "",
                0,
                100);

            passive_speedAttackMultiplier = Modules.Config.BindAndOptionsSlider(
                section,
                "passive_velocityAttackMultiplier",
                10,
                "",
                0,
                100);

            ChargeRoll_Multiplier = Modules.Config.BindAndOptionsSlider(
                section,
                "ChargeRoll_Multiplier",
                0.1f,
                "",
                0,
                10);
        }
    }
}
