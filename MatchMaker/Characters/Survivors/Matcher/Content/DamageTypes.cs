using R2API;

namespace MatcherMod.Survivors.Matcher.MatcherContent
{
    public class DamageTypes
    {
        public static DamageAPI.ModdedDamageType ComboFinisherDebuffDamage;

        public static void Init()
        {
            ComboFinisherDebuffDamage = DamageAPI.ReserveDamageType();
        }
    }
}
