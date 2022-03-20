using R2API;
using RoR2;
using UnityEngine;

namespace Modules {
    internal static class DamageTypes {

        public static float stunTime = 2.5f;
        public static float shockTime = 2.5f;
        public static float shockTime2 = 1f;

        public static DamageAPI.ModdedDamageType stunXs;
        public static DamageAPI.ModdedDamageType shockXs;
        public static DamageAPI.ModdedDamageType shockXs2;

        public static DamageAPI.ModdedDamageType conductive;
        public static DamageAPI.ModdedDamageType consumeConductive;

        public static void RegisterDamageTypes() {

            if (FacelessJoePlugin.conductiveMechanic) {

                stunXs = DamageAPI.ReserveDamageType();
                shockXs = DamageAPI.ReserveDamageType();
                shockXs2 = DamageAPI.ReserveDamageType();

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

                if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(stunXs)) {
                    self.SetStun(stunTime);
                    return;
                }

                if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(shockXs)) {
                    self.SetShock(shockTime);
                    return;
                }
                //how scuffed is this?
                if (flag && self.canBeStunned && damageInfo.HasModdedDamageType(shockXs2)) {
                    self.SetShock(shockTime2);
                    return;
                }
            }
        }
    }
}