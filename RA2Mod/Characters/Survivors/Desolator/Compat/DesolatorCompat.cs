using RA2Mod.General;
using RA2Mod.Survivors.Desolator.Compat;
using System.Collections;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorCompat
    {

        public static void Init()
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.cwmlolzlz.skills"))
            {
                DesolatorSkillsPlusCompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.xoxfaby.BetterUI"))
            {
                DesolatorBetterUICompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                DesolatorMemeCompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.johnedwa.RTAutoSprintEx"))
            {
                RA2Plugin.instance.StartCoroutine(AutoSprintCompat());
            }
            if (GeneralCompat.driverInstalled)
            {
                new DesolatorDriverCompat().Init();
            }
        }

        private static IEnumerator AutoSprintCompat()
        {
            //let awake happen
            yield return null;

            RA2Plugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("RA2Mod.Survivors.Desolator.States.AimBigRadBeam");
        }
    }
}