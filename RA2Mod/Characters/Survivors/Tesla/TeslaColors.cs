using RoR2;
using R2API;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla
{
    internal class TeslaColors
    {
        internal static DamageColorIndex ChargedColor;

        public static void Init()
        {
            ChargedColor = ColorsAPI.RegisterDamageColor(Color.cyan);
        }
    }
}