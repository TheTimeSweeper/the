using RoR2;
using R2API;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla
{
    public class TeslaColors
    {
        internal static DamageColorIndex ChargedColor;

        public static void Init()
        {
            ChargedColor = ColorsAPI.RegisterDamageColor(Color.cyan);
        }
    }
}