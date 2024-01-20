using RoR2;
using UnityEngine;
using RA2Mod.Modules;
using System;
using RoR2.Projectile;
using RA2Mod.Survivors.Chrono.Components;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoAssets {

        public static ChronoProjectionMotor markerPrefab;

        public static void Init(AssetBundle assetBundle) {

            GameObject markerPrefabe = assetBundle.LoadAsset<GameObject>("ChronoProjection");
            R2API.PrefabAPI.RegisterNetworkPrefab(markerPrefabe);
            markerPrefab = markerPrefabe.GetComponent<ChronoProjectionMotor>();
        }
    }
}
