using R2API;

namespace MatcherMod.Survivors.Matcher.Content
{
    public class CharacterDamageTypes
    {
        public static DamageAPI.ModdedDamageType ComboFinisherDebuffDamage;

        public static void Init()
        {
            ComboFinisherDebuffDamage = DamageAPI.ReserveDamageType();
        }
    }
}
