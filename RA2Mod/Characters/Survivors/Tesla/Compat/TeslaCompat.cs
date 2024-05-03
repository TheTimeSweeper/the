using RA2Mod.General;
using RA2Mod.Survivors.Tesla.Compat;
using System.Collections;

namespace RA2Mod.Survivors.Tesla
{
    public static class TeslaCompat
    {
        public static void Init()
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.cwmlolzlz.skills"))
            {
                TeslaSkillsPlusCompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.xoxfaby.BetterUI"))
            {
                TeslaBetterUICompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DrBibop.VRAPI"))
            {
                TeslaVRCompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                TeslaMemeCompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.johnedwa.RTAutoSprintEx"))
            {
                RA2Plugin.instance.StartCoroutine(AutoSprintCompat());
            }
            if (GeneralCompat.driverInstalled)
            {
                new TeslaDriverCompat().Init();
            }
        }

        private static IEnumerator AutoSprintCompat()
        {
            //let awake happen
            yield return null;
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Tesla.States.AimBigZap");
        }
    }
}
