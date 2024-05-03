using R2API;

namespace RA2Mod.Survivors.Desolator
{
    public class DesolatorDamageTypes
    {
        public static DamageAPI.ModdedDamageType DesolatorArmorShred;
        public static DamageAPI.ModdedDamageType DesolatorDot;
        public static DamageAPI.ModdedDamageType DesolatorDot2;
        public static DamageAPI.ModdedDamageType DesolatorDotPrimary;

        public static void Init()
        {
            DesolatorArmorShred = DamageAPI.ReserveDamageType();
            DesolatorDot = DamageAPI.ReserveDamageType();
            DesolatorDot2 = DamageAPI.ReserveDamageType();
            DesolatorDotPrimary = DamageAPI.ReserveDamageType();
        }
    }
}