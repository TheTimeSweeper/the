namespace Matchmaker.MatchGrid
{
    public class WildMatchTileBehavior : MatchTileBehavior
    {
        public override void Init(MatchTile matchTile)
        {
            base.Init(matchTile);
        }
        //unsure which one to use. we'll keep both for now
        public override bool CheckAgainstThisTile(MatchTile otherTile)
        {
            return true;
        }
        public override bool CheckAgainstThisTileType(MatchTileType otherTile)
        {
            return true;
        }
    }
}