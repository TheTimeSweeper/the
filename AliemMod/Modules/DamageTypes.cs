using R2API;
using RoR2;
using UnityEngine;

namespace AliemMod.Modules
{

    public static class DamageTypes
    {
        //Aliem
        public static DamageAPI.ModdedDamageType Decapitating;
        public static DamageAPI.ModdedDamageType FuckinChargedKillAchievementTracking;

        public static void RegisterDamageTypes()
        {
            //Aliem
            Decapitating = DamageAPI.ReserveDamageType();
            FuckinChargedKillAchievementTracking = DamageAPI.ReserveDamageType();
        }
    }
}