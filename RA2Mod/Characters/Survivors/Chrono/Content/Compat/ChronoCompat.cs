using RA2Mod.General;
using RTAutoSprintEx;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoCompat
    {
        public static bool AutoSprintInstalled;

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void Init()
        {
            if (GeneralCompat.driverInstalled)
            {
                new ChronoDriverCompat().Init();
            }
            if (AutoSprintInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.johnedwa.RTAutoSprintEx"))
            {
                RA2Plugin.instance.StartCoroutine(AutoSprintCompat());
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                ChronoMemeCompat.init();
            }
        }

        private static IEnumerator AutoSprintCompat()
        {
            //let awake happen
            yield return null;
            Log.WarningNull("auto auto", RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>());
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.States.ChronoCharacterMain");
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.States.PhaseState");
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.States.AimChronosphere1");
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.States.AimChronosphere2");
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.States.PlaceChronosphere2");
        }
    }
}
