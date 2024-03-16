using RA2Mod.Survivors.Tesla.Compat;

namespace RA2Mod.Survivors.Tesla
{
    public class TeslaCompat
    {
        public static void Init()
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.cwmlolzlz.skills"))
            {
                SkillsPlusCompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.xoxfaby.BetterUI"))
            {
                BetterUICompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DrBibop.VRAPI"))
            {
                VRCompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                MemeCompat.init();
            }
        }
    }
}
