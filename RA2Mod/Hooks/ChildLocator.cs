using RA2Mod.Hooks.RoR2;
using System;
using UnityEngine;

namespace RA2Mod.Hooks
{
    public class ChildLocator
    {
        private static FindChildHook FindChildHookTracker = new FindChildHook();
        public static event Action<string, Transform> FindChild
        {
            add
            {
                FindChildHookTracker.Event_FindChild += value;
                FindChildHookTracker.OnSubscribed();
            }
            remove
            {
                FindChildHookTracker.Event_FindChild -= value;
                FindChildHookTracker.OnUnsubscribed();
            }
        }

        private class FindChildHook : HookTracker
        {
            public event Action<string, Transform> Event_FindChild;

            public override void ApplyHook()
            {
                
                On.ChildLocator.FindChild_string += ChildLocator_FindChild_string;
            }

            public override void RemoveHook()
            {
                On.ChildLocator.FindChild_string -= ChildLocator_FindChild_string;
            }

            private Transform ChildLocator_FindChild_string(On.ChildLocator.orig_FindChild_string orig, global::ChildLocator self, string childName)
            {
                Transform childFound = orig(self, childName);
                Event_FindChild.Invoke(childName, childFound);
                return childFound;
            }
        }
    }
}
