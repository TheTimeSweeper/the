namespace HellDiverMod.General.Components.UI
{
    public interface IHasCompanionUI<T>
    {
        bool allowUIUpdate { get; set; }
        T CompanionUI { get; set; }
    }
}
