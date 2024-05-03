using RoR2;
using System;
using UnityEngine;

namespace RA2Mod.Hooks.RoR2
{
    public static class SurvivorCatalog
    {
        #region Init
        public static event Action Init
        {
            add
            {
                InitHookTracker.InitHookEvent += value;
                InitHookTracker.OnSubscribed();
            }
            remove
            {
                InitHookTracker.InitHookEvent -= value;
                InitHookTracker.OnUnsubscribed();
            }
        }
        private static InitHook InitHookTracker = new InitHook();
        private class InitHook : HookTracker
        {
            public event Action InitHookEvent;

            public override void ApplyHook()
            {
                On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            }

            public override void RemoveHook()
            {
                On.RoR2.SurvivorCatalog.Init -= SurvivorCatalog_Init;
            }

            private void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
            {
                InitHookEvent?.Invoke();
                orig();
            }
        }
        #endregion init

        #region setsurvivordefs
        public static event Action<SurvivorDef[]> SetSurvivorDefs
        {
            add
            {
                SetSurvivorDefsHookTracker.SetSurvivorDefsHookEvent += value;
                SetSurvivorDefsHookTracker.OnSubscribed();
            }
            remove
            {
                SetSurvivorDefsHookTracker.SetSurvivorDefsHookEvent -= value;
                SetSurvivorDefsHookTracker.OnUnsubscribed();
            }
        }
        public static event Action<GameObject> SetSurvivorDefs_Driver
        {
            add
            {
                SetSurvivorDefsHookTracker.SetSurvivorDefsHookEvent_Driver += value;
                SetSurvivorDefsHookTracker.OnSubscribed();
            }
            remove
            {
                SetSurvivorDefsHookTracker.SetSurvivorDefsHookEvent_Driver -= value;
                SetSurvivorDefsHookTracker.OnUnsubscribed();
            }
        }
        private static SetSurvivorDefsHook SetSurvivorDefsHookTracker = new SetSurvivorDefsHook();
        private class SetSurvivorDefsHook : HookTracker
        {
            public event Action<SurvivorDef[]> SetSurvivorDefsHookEvent;
            public event Action<GameObject> SetSurvivorDefsHookEvent_Driver;

            public override void ApplyHook()
            {
                On.RoR2.SurvivorCatalog.SetSurvivorDefs += SurvivorCatalog_SetSurvivorDefs;
            }

            public override void RemoveHook()
            {
                On.RoR2.SurvivorCatalog.SetSurvivorDefs -= SurvivorCatalog_SetSurvivorDefs;
            }

            private void SurvivorCatalog_SetSurvivorDefs(On.RoR2.SurvivorCatalog.orig_SetSurvivorDefs orig, SurvivorDef[] newSurvivorDefs)
            {
                orig(newSurvivorDefs);
                SetSurvivorDefsHookEvent?.Invoke(newSurvivorDefs);

                for (int i = 0; i < newSurvivorDefs.Length; i++)
                {
                    if (newSurvivorDefs[i].bodyPrefab.name == "RobDriverBody")
                    {
                        SetSurvivorDefsHookEvent_Driver?.Invoke(newSurvivorDefs[i].bodyPrefab);
                        return;
                    }
                }

                Log.Debug("no driver. ra2 compat failed");
            }
        }
        #endregion setsurvivordefs
    }

    #region dun work but maybe I should have learned something
    //public abstract class GenericHook<T>
    //{
    //    public int hooksAdded;
    //    public event T hookEvent;
    //    public event T Hook
    //    {
    //        add
    //        {
    //            hookEvent += value;
    //            if (hooksAdded == 0)
    //            {
    //                ApplyHook();
    //            }
    //            hooksAdded++;
    //        }
    //        remove
    //        {
    //            hookEvent -= value;
    //            hooksAdded--;
    //            if (hooksAdded == 0)
    //            {
    //                RemoveHook();
    //            }
    //        }
    //    }

    //    public abstract void RemoveHook();

    //    public abstract void ApplyHook();
    //}

    //public class SurvivorCatalog1
    //{
    //    public static SurvivorCatalog_InitHook Init = new SurvivorCatalog_InitHook();

    //    public delegate void InitEvent();

    //    public class SurvivorCatalog_InitHook : GenericHook<InitEvent>
    //    {
    //        public override void ApplyHook()
    //        {
    //            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
    //        }

    //        public override void RemoveHook()
    //        {
    //            On.RoR2.SurvivorCatalog.Init -= SurvivorCatalog_Init;
    //        }

    //        private void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
    //        {
    //            hookEvent?.Invoke();
    //            orig();
    //        }
    //    }
    //}
    #endregion dun work

    #region work but COPY PASTE
    public class SurvivorCatalog0
    {
        public static int initHooks;
        public delegate void InitEvent();
        public static event InitEvent initHook;
        public static event InitEvent Init
        {
            add
            {
                initHook += value;
                if (initHooks == 0)
                {
                    ApplyHook();
                }
                initHooks++;
            }
            remove
            {
                initHook -= value;
                initHooks--;
                if (initHooks == 0)
                {
                    RemoveHook();
                }
            }
        }

        public static void ApplyHook()
        {
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
        }

        private static void RemoveHook()
        {
            On.RoR2.SurvivorCatalog.Init -= SurvivorCatalog_Init;
        }

        private static void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            initHook?.Invoke();
            orig();
        }
    }
    #endregion COPY PASTE
}