using RA2Mod.Hooks.RoR2;
using UnityEngine;

namespace RA2Mod.Hooks
{
    public class ChildLocator
    {
        public static FindChildHook FindChildHookTracker = new FindChildHook();
        public delegate void FindChildEvent(string childName, Transform child);
        public static event FindChildEvent FindChild
        {
            add
            {
                FindChildHookTracker.FindChildHookEvent += value;
                FindChildHookTracker.OnSubscribed();
            }
            remove
            {
                FindChildHookTracker.FindChildHookEvent -= value;
                FindChildHookTracker.OnUnsubscribed();
            }
        }

        public class FindChildHook : HookTracker
        {
            public event FindChildEvent FindChildHookEvent;

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
                FindChildHookEvent.Invoke(childName, childFound);
                return childFound;
            }
        }
    }
}
