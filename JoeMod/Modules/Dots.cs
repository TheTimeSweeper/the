using R2API;
using RoR2;

namespace Modules {
    internal static class Dots {
        public static DotController.DotIndex DesolatorDot;

        public static void RegisterDots() {
            DesolatorDot = DotAPI.RegisterDotDef(new DotController.DotDef {
                interval = 0.5f,
                damageCoefficient = 0.2f,
                damageColorIndex = DamageColorIndex.Poison,
                associatedBuff = Modules.Buffs.desolatorDotDeBuff
            });
        }
    }
}