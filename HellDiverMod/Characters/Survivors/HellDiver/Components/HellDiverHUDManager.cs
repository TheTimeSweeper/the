using HellDiverMod.General.Components.UI;
using UnityEngine;

namespace HellDiverMod.Survivors.HellDiver.Components.UI
{
    public class HellDiverHUDManager : CompoanionHUDManager<StratagemInputController, HellDiverUI>
    {
        protected override GameObject UIPrefab => HellDiverAssets.hellDiverUI;

        protected override string transformPath => "MainContainer/MainUIArea/SpringCanvas/LeftCluster";
    }
}
