﻿using R2API;

namespace RA2Mod.Survivors.Tesla
{
    public class TeslaDamageTypes
    {
        //public static float StunLongTime = 2.5f;
        //public static float ShockTimeMed = 2.5f;
        public static float ShockTimeShort = 1f;

        //public static DamageAPI.ModdedDamageType StunLong;
        //public static DamageAPI.ModdedDamageType ShockMed;
        public static DamageAPI.ModdedDamageType ShockShort;

        public static DamageAPI.ModdedDamageType Conductive;

        public static void Init()
        {
            //StunLong = DamageAPI.ReserveDamageType();
            //ShockMed = DamageAPI.ReserveDamageType();
            ShockShort = DamageAPI.ReserveDamageType();

            Conductive = DamageAPI.ReserveDamageType();
        }
    }
}