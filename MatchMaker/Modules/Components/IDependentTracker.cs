namespace MatcherMod.Modules.Components
{
    public interface IDependentTracker
    {
        ITracker dependentTracker { get; set; }
    }
}