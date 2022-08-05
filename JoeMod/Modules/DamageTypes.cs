using R2API;
using RoR2;
using UnityEngine;

namespace Modules {
    internal static class DamageTypes {

        public static float stunLongTime = 2.5f;
        public static float shockTimeMed = 2.5f;
        public static float shockTimeShort = 1f;

        public static DamageAPI.ModdedDamageType stunLong;
        public static DamageAPI.ModdedDamageType shockMed;
        public static DamageAPI.ModdedDamageType shockShort;

        public static DamageAPI.ModdedDamageType conductive;
        public static DamageAPI.ModdedDamageType consumeConductive;

        public static void RegisterDamageTypes() {

            if (FacelessJoePlugin.conductiveMechanic) {

                stunLong = DamageAPI.ReserveDamageType();
                shockMed = DamageAPI.ReserveDamageType();
                shockShort = DamageAPI.ReserveDamageType();

                conductive = DamageAPI.ReserveDamageType();
                consumeConductive = DamageAPI.ReserveDamageType();

                SetHooks();
            }
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

                if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(stunLong)) {
                    self.SetStun(stunLongTime);
                    return;
                }

                if ((damageInfo.damageType & DamageType.Shock5s) == damageInfo.damageType) {

                    if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(shockMed)) {
                        self.SetShock(shockTimeMed);
                        return;
                    }
                    //how scuffed is this?
                    if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(shockShort)) {
                        self.SetShock(shockTimeShort);
                        return;
                    }
                }
            }
        }


    }
}