namespace RA2Mod.General.Components
{
    public interface IDependentTracker
    {
        ITracker dependentTracker { get; set; }
    }
}