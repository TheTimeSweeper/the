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
                //SkillsPlusCompat.init();
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
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.johnedwa.RTAutoSprintEx"))
            {
                RA2Plugin.instance.StartCoroutine(AutoSprintCompat());
            }
            if (GeneralCompat.driverInstalled)
            {
                new DriverCompat().Init();
            }
        }

        private static IEnumerator AutoSprintCompat()
        {
            //let awake happen
            yield return null;
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("ModdedEntityStates.TeslaTrooper.AimBigZap");
        }
    }
}
