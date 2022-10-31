using R2API;
using RoR2;
using UnityEngine;

namespace Modules {

    internal static class DamageTypes {
        //Aliem
        public static DamageAPI.ModdedDamageType ApplyAliemRiddenBuff;

        public static void RegisterDamageTypes() {

            //Aliem
            ApplyAliemRiddenBuff = DamageAPI.ReserveDamageType();

            //SetHooks();
        }

        private static void SetHooks() {
            On.RoR2.SetStateOnHurt.OnTakeDamageServer += SetStateOnHurt_OnTakeDamageServer;
        }

        private static void SetStateOnHurt_OnTakeDamageServer(On.RoR2.SetStateOnHurt.orig_OnTakeDamageServer orig, SetStateOnHurt self, DamageReport damageReport) {
            orig(self, damageReport);
        }


    }
}