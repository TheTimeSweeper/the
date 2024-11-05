namespace MatcherMod.Modules.UI
{
    public interface ICompanionUI<T>
    {
        void OnInitialize(T hasUIComponent);
        void OnUIUpdate();
        void OnBodyUnFocused();
        void OnBodyLost();
    }
}
