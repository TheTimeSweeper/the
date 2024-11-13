using UnityEngine;

namespace Matchmaker.MatchGrid
{
    public class MultiplierMatchTileBehavior : MatchTileBehavior
    {
        [SerializeField]
        private int matchMultiplier;

        public override void Init(MatchTile matchTile)
        {
            base.Init(matchTile);
            matchTile.InitTileType(matchTile.MatchGrid.TileTypes[Random.Range(0, matchTile.MatchGrid.TileTypes.Length - 1)]);
        }

        public override void ApplyMatchModifier(ref int baseMatches)
        {
            baseMatches *= matchMultiplier;
        }
    }
}