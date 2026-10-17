namespace GorpMod.Modules.Components
{
    public interface IDependentTracker
    {
        ITracker dependentTracker { get; set; }
    }
}