using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.Minions.TeslaTower
{
    internal class TeslaTowerAssets
    {
        internal static Material ChainLightningMaterial;

        internal static List<IEnumerator> GetAssetBundleInitializedCoroutines(AssetBundle assetBundle)
        {
            //todo teslamove async fun
            List<IEnumerator> coroutines = Modules.Assets.LoadAssetsAsync<Sprite>(assetBundle,
                "texIconTeslaTower",
                "texTeslaSkillSecondaryThunderclap");

            coroutines.Add(Modules.Assets.LoadAddressableAssetCoroutine<Material>("RoR2/Base/Common/VFX/matLightningLongBlue.mat", (result) =>
            {
                ChainLightningMaterial = result;
            }));



            return coroutines;
        }
    }
}