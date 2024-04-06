using RoR2;
using UnityEngine;
using KatamariMod.Modules;
using System;
using RoR2.Projectile;

namespace KatamariMod.Survivors.Katamari
{
    public static class KatamariAssets
    {
        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {
            _assetBundle = assetBundle;
        }
    }
}
