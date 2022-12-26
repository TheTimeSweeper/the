using Modules.Survivors;
using R2API;
using RoR2;

namespace Modules {
    internal static class Dots {
        public static DotController.DotIndex DesolatorDot;

        public static void RegisterDots() {
            if (FacelessJoePlugin.Desolator) {
                DesolatorDot = DotAPI.RegisterDotDef(new DotController.DotDef {
                    interval = DesolatorSurvivor.DotInterval,
                    damageCoefficient = DesolatorSurvivor.DotDamage,
                    damageColorIndex = DamageColorIndex.Poison,
                    associatedBuff = Modules.Buffs.desolatorDotDeBuff
                });
            }
        }
    }
}