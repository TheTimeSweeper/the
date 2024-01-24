using R2API;

namespace RA2Mod.Survivors.Chrono {
    public static class ChronoDamageTypes {
        public static DamageAPI.ModdedDamageType chronoDamage;
        public static DamageAPI.ModdedDamageType chronoDamageDouble;
        public static DamageAPI.ModdedDamageType vanishingDamage;

        public static void Init() {
            chronoDamage = DamageAPI.ReserveDamageType();
            chronoDamageDouble = DamageAPI.ReserveDamageType();
            vanishingDamage = DamageAPI.ReserveDamageType();
        }
    }
}
