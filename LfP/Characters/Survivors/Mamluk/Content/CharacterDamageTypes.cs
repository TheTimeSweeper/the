using R2API;

namespace LfPMod.Survivors.Mamluk.MamlukContent
{
    public class CharacterDamageTypes
    {
        public static DamageAPI.ModdedDamageType comboFinisherDebuffDamage;

        public static void Init()
        {
            comboFinisherDebuffDamage = DamageAPI.ReserveDamageType();
        }
    }
}
