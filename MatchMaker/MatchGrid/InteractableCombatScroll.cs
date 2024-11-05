using UnityEngine.EventSystems;

namespace Matchmaker.MatchGrid
{
    public class InteractableCombatScroll : InteractableTileBehavior
    {
        protected override void Activate()
        {
            _matchTile.Break();
            matchGrid.FillEmptyTiles();
            for (int x = 0; x < matchGrid.TileGrid.GetLength(0); x++)
            {
                for (int y = 0; y < matchGrid.TileGrid.GetLength(1); y++)
                {
                    for (int i = 2; i < matchGrid.TileTypes.Length; i++)
                    {
                        if (matchGrid.TileGrid[x, y] != null && matchGrid.TileGrid[x, y].TileType == matchGrid.TileTypes[i])
                        {
                            matchGrid.TileGrid[x, y].Transform(matchGrid.TileTypes[UnityEngine.Random.Range(0, 2)]);
                        }
                    }
                }
            }
            matchGrid.DelayedProcessAllGridMatches(0.5f);
        }
    }
}