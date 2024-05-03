using UnityEngine;

namespace RA2Mod.Survivors.Desolator.Compat
{
    public class DesolatorMemeCompat
    {
        public static void init()
        {
            Hooks.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
        }

        private static void SurvivorCatalog_Init()
        {
            //todo teslamove async
            GameObject skele2 = DesolatorSurvivor.instance.assetBundle.LoadAsset<GameObject>("Desolator_meme");
            if (skele2)
            {
                EmotesAPI.CustomEmotesAPI.ImportArmature(DesolatorSurvivor.instance.bodyPrefab, skele2, false);
            }
        }
    }
}