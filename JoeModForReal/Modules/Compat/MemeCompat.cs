using UnityEngine;
using Modules.Survivors;
using JoeModForReal.Content.Survivors;

namespace Modules {
    public class MemeCompat {
        public static void init() {
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
        }

        private static void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig) {

            //GameObject skele = Asset.mainAssetBundle.LoadAsset<GameObject>("joe_meme");
            //EmotesAPI.CustomEmotesAPI.ImportArmature(JoeSurivor.instance.bodyPrefab, skele, false);
            //skele.GetComponentInChildren<BoneMapper>().scale = 1.5f;
            orig();
        }
    }
}