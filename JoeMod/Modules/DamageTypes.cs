using R2API;
using RoR2;
using UnityEngine;

namespace Modules {

    internal static class DamageTypes {

        public static float StunLongTime = 2.5f;
        public static float ShockTimeMed = 2.5f;
        public static float ShockTimeShort = 1f;

        public static DamageAPI.ModdedDamageType StunLong;
        public static DamageAPI.ModdedDamageType ShockMed;
        public static DamageAPI.ModdedDamageType ShockShort;

        public static DamageAPI.ModdedDamageType Conductive;
        //public static DamageAPI.ModdedDamageType ApplyBlinkCooldown;

        public static DamageAPI.ModdedDamageType DesolatorArmorShred;
        public static DamageAPI.ModdedDamageType DesolatorDot;
        public static DamageAPI.ModdedDamageType DesolatorDot2;
        public static DamageAPI.ModdedDamageType DesolatorDotPrimary;

        public static void RegisterDamageTypes() {
            
            StunLong = DamageAPI.ReserveDamageType();
            ShockMed = DamageAPI.ReserveDamageType();
            ShockShort = DamageAPI.ReserveDamageType();

            Conductive = DamageAPI.ReserveDamageType();
            //ApplyBlinkCooldown = DamageAPI.ReserveDamageType();
            if (TeslaTrooperPlugin.Desolator) {
                DesolatorArmorShred = DamageAPI.ReserveDamageType();
                DesolatorDot = DamageAPI.ReserveDamageType();
                DesolatorDot2 = DamageAPI.ReserveDamageType();
                DesolatorDotPrimary = DamageAPI.ReserveDamageType();
            }
            SetHooks();
        }

        private static void SetHooks() {
            On.RoR2.SetStateOnHurt.OnTakeDamageServer += SetStateOnHurt_OnTakeDamageServer;
        }

        private static void SetStateOnHurt_OnTakeDamageServer(On.RoR2.SetStateOnHurt.orig_OnTakeDamageServer orig, SetStateOnHurt self, DamageReport damageReport) {
            orig(self, damageReport);


            DamageInfo damageInfo = damageReport.damageInfo;
            HealthComponent victim = damageReport.victim;

            bool flag = damageInfo.procCoefficient >= Mathf.Epsilon;

            if (!victim.isInFrozenState) {

                if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(StunLong)) {
                    self.SetStun(StunLongTime);
                    return;
                }

                if ((damageInfo.damageType & DamageType.Shock5s) == damageInfo.damageType) {

                    if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(ShockMed)) {
                        self.SetShock(ShockTimeMed);
                        return;
                    }
                    //how scuffed is this?
                    if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(ShockShort)) {
                        self.SetShock(ShockTimeShort);
                        return;
                    }
                }
            }
        }


    }
}