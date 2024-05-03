using UnityEngine;

namespace RA2Mod.Survivors.Chrono
{
    public class ChronoMemeCompat
    {
        public static void init()
        {
            Hooks.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
        }

        private static void SurvivorCatalog_Init()
        {
            //todo teslamove async
            GameObject skele = ChronoSurvivor.instance.assetBundle.LoadAsset<GameObject>("Chrono_meme");
            EmotesAPI.CustomEmotesAPI.ImportArmature(ChronoSurvivor.instance.bodyPrefab, skele, false);
        }
    }
}