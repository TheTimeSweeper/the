using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Matchmaker.MatchGrid
{
    public class InteractableTileBomb : InteractableTileBehavior
    {
        public MatchTileType tileToDestroy;

        [SerializeField]
        private Image bombTarget;

        public override void Init(MatchTile matchTile)
        {
            base.Init(matchTile);
            if(tileToDestroy == null)
            {
                tileToDestroy = matchGrid.TileTypes[Random.Range(0,matchGrid.TileTypes.Length)];
            }
            bombTarget.color = tileToDestroy.GetColor();
            bombTarget.sprite = tileToDestroy.GetIcon();
        }

        protected override void Activate()
        {
            _matchTile.Break();
            matchGrid.FillEmptyTiles();
            for (int x = 0; x < matchGrid.TileGrid.GetLength(0); x++)
            {
                for (int y = 0; y < matchGrid.TileGrid.GetLength(1); y++)
                {
                    if (matchGrid.TileGrid[x, y] != null && matchGrid.TileGrid[x, y].TileType == tileToDestroy)
                    {
                        matchGrid.TileGrid[x, y].Break();
                    }
                }
            }
            matchGrid.FillEmptyTiles();
            matchGrid.DelayedProcessAllGridMatches(0.5f);
        }
    }
}