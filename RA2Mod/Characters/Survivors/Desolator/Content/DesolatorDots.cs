using R2API;
using RoR2;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorDots
    {
        public static DotController.DotIndex DesolatorDot;

        public static void Init()
        {
            DesolatorDot = DotAPI.RegisterDotDef(new DotController.DotDef
            {
                interval = DesolatorSurvivor.DotInterval,
                damageCoefficient = DesolatorSurvivor.DotDamage,
                damageColorIndex = DamageColorIndex.Poison,
                associatedBuff = DesolatorBuffs.desolatorDotDeBuff
            });
        }
    }
}