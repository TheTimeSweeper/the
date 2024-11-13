namespace Matchmaker.MatchGrid
{
    public class InteractableTileTimeStop : InteractableTileBehavior
    {
        protected override void Activate()
        {
            BreakThisTile();
            matchGrid.StopTime();
        }
    }
}