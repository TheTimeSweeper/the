using R2API;

namespace JoeModForReal.Content.Survivors {
    public static class GenjiDamageTypes {
        public static DamageAPI.ModdedDamageType GolemLaser;

        public static void Init() {
            GolemLaser = DamageAPI.ReserveDamageType();
        }
    }
}