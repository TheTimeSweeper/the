namespace MatcherMod.Modules.UI
{
    public interface ICompanionUI<T>
    {
        void OnInitialize(T hasUIComponent, RoR2.UI.HUD hud);
        void OnUIUpdate();
        void OnBodyUnFocused();
        void OnBodyLost();
    }
}
