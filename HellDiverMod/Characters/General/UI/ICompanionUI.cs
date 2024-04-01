namespace HellDiverMod.General.Components.UI
{
    public interface ICompanionUI<T>
    {
        void OnInitialize(T hasUIComponent);
        void OnUIUpdate();
    }
}
