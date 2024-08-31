using UnityEngine;
using Modules.Survivors;
using AliemMod.Modules;

namespace Modules
{
    public class MemeCompat {
        public static void init() {
            //actually breaks the fucking game somehow
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
        }

        private static void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig) {

            GameObject skele = AliemAssets.LoadAsset<GameObject>("Aliem_Meme");
            EmotesAPI.CustomEmotesAPI.ImportArmature(AliemMod.Content.Survivors.AliemSurvivor.instance.bodyPrefab, skele, false);
            //skele.GetComponentInChildren<BoneMapper>().scale = 1.5f;
            orig();
        }
    }
}