using UnityEngine;

namespace RA2Mod.Survivors.Tesla.Compat
{
    public class TeslaMemeCompat
    {
        public static void init()
        {
            Hooks.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
        }

        private static void SurvivorCatalog_Init()
        {
            //todo teslamove async
            GameObject skele = TeslaTrooperSurvivor.instance.assetBundle.LoadAsset<GameObject>("TeslaTrooper_meme");
            EmotesAPI.CustomEmotesAPI.ImportArmature(TeslaTrooperSurvivor.instance.bodyPrefab, skele, true);
            //skele.GetComponentInChildren<BoneMapper>().scale = 1.5f;
        }
    }
}
