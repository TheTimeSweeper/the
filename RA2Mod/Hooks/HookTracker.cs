namespace RA2Mod.Hooks
{
    public abstract class HookTracker
    {
        public static int initHooks;
        public void OnSubscribed()
        {
            if (initHooks == 0)
            {
                ApplyHook();
            }
            initHooks++;
        }
        public void OnUnsubscribed()
        {
            initHooks--;
            if (initHooks == 0)
            {
                RemoveHook();
            }
        }

        public abstract void RemoveHook();

        public abstract void ApplyHook();
    }
}
