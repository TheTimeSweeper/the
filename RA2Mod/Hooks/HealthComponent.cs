using RoR2;
using System;

namespace RA2Mod.Hooks.RoR2
{
    public static class HealthComponent
    {
        #region TakeDamage
        public static event Action<global::RoR2.HealthComponent, DamageInfo> TakeDamage_Pre
        {
            add
            {
                takeDamageHookTracker.Event_TakeDamage_Pre += value;
                takeDamageHookTracker.OnSubscribed();
            }
            remove
            {
                takeDamageHookTracker.Event_TakeDamage_Pre -= value;
                takeDamageHookTracker.OnUnsubscribed();
            }
        }
        public static event Action<global::RoR2.HealthComponent, DamageInfo> TakeDamage_Post
        {
            add
            {
                takeDamageHookTracker.Event_TakeDamage_Post += value;
                takeDamageHookTracker.OnSubscribed();
            }
            remove
            {
                takeDamageHookTracker.Event_TakeDamage_Post -= value;
                takeDamageHookTracker.OnUnsubscribed();
            }
        }
        private static TakeDamageHook takeDamageHookTracker = new TakeDamageHook();
        private class TakeDamageHook : HookTracker
        { 
            public event Action<global::RoR2.HealthComponent, DamageInfo> Event_TakeDamage_Pre;
            public event Action<global::RoR2.HealthComponent, DamageInfo> Event_TakeDamage_Post;

            public override void ApplyHook()
            {
                On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            }

            public override void RemoveHook()
            {
                On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            }

            private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, global::RoR2.HealthComponent self, DamageInfo damageInfo)
            {
                Event_TakeDamage_Pre?.Invoke(self, damageInfo);
                orig(self, damageInfo);
                Event_TakeDamage_Post?.Invoke(self, damageInfo);
            }
        }
        #endregion TakeDamage
    }
}