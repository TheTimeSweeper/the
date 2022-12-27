using R2API;
using RoR2;
using UnityEngine;

namespace Modules {
    internal static class Colors {
        public static DamageColorIndex ChargedColor;
        public static void Init() {
            ChargedColor = ColorsAPI.RegisterDamageColor(Color.cyan);
        }
    }
}