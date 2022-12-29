using R2API;
using RoR2;
using UnityEngine;

namespace Modules {

    internal static class DamageTypes {
        public static DamageAPI.ModdedDamageType TenticleLifeStealing;

        public static void RegisterDamageTypes() {
            TenticleLifeStealing = DamageAPI.ReserveDamageType();
        }
    }
}