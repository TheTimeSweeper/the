using RTAutoSprintEx;
using System.Collections;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoCompat
    {
        public static void init()
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.johnedwa.RTAutoSprintEx"))
            {
                RA2Plugin.instance.StartCoroutine(DoCompat());
            }
        }

        static IEnumerator DoCompat()
        {
            yield return null;
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.SkillStates.ChronoCharacterMain");
            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Chrono.SkillStates.PhaseState");
        }
    }
}
