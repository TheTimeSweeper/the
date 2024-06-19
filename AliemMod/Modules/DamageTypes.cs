using R2API;
using RoR2;
using UnityEngine;

namespace AliemMod.Modules
{

    public static class DamageTypes
    {
        //Aliem
        public static DamageAPI.ModdedDamageType ApplyAliemRiddenBuff;
        public static DamageAPI.ModdedDamageType Decapitating;
        public static DamageAPI.ModdedDamageType FuckinChargedKillAchievementTracking;

        public static void RegisterDamageTypes()
        {
            //Aliem
            ApplyAliemRiddenBuff = DamageAPI.ReserveDamageType();
            Decapitating = DamageAPI.ReserveDamageType();
            FuckinChargedKillAchievementTracking = DamageAPI.ReserveDamageType();

        }
    }
}