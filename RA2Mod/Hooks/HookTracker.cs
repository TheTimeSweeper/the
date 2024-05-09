namespace RA2Mod.Hooks
{
    public abstract class HookTracker
    {
        private int addedHooks;
        public void OnSubscribed()
        {
            if (addedHooks == 0)
            {
                ApplyHook();
            }
            addedHooks++;
        }
        public void OnUnsubscribed()
        {
            addedHooks--;
            if (addedHooks == 0)
            {
                RemoveHook();
            }
        }

        public abstract void RemoveHook();

        public abstract void ApplyHook();
    }
}
