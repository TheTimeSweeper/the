using UnityEngine;

namespace Matchmaker.MatchGrid
{
    public class MatchTileDragInfo
    {
        public MatchTile tile;
        private MatchGrid grid;
        private Vector3 initialPosition;
        private Vector2Int initialGridPosition;

        public Vector2Int currentGridPosition;

        public MatchTileDragInfo(MatchTile tile, MatchGrid grid)
        {
            this.tile = tile;
            this.grid = grid;
            initialGridPosition = tile.GridPosition;
            initialPosition = tile.transform.localPosition;
        }

        public void ResetTile()
        {
            tile.transform.localPosition = initialPosition;
        }

        public bool UpdatePosition(Vector3 delta, Vector2Int deltaGridPosition)
        {
            bool orderDirty = false;

            currentGridPosition = initialGridPosition + deltaGridPosition;

            Vector3 deltaSwapOffset = Vector3.zero;
            while (currentGridPosition.x >= grid.GridSize.x)
            {
                currentGridPosition.x -= grid.GridSize.x;
                deltaSwapOffset.x -= grid.GridSize.x * grid.TileDistance.x;
                orderDirty = true;
            }
            while (currentGridPosition.x < 0)
            {
                currentGridPosition.x += grid.GridSize.x;
                deltaSwapOffset.x += grid.GridSize.x * grid.TileDistance.x;
                orderDirty = true;
            }

            while (currentGridPosition.y >= grid.GridSize.y)
            {
                currentGridPosition.y -= grid.GridSize.y;
                deltaSwapOffset.y -= grid.GridSize.y * grid.TileDistance.y;
                orderDirty = true;
            }
            while (currentGridPosition.y < 0)
            {
                currentGridPosition.y += grid.GridSize.y;
                deltaSwapOffset.y += grid.GridSize.y * grid.TileDistance.y;
                orderDirty = true;
            }

            tile.transform.localPosition = initialPosition + delta + deltaSwapOffset;

            return orderDirty;
        }
    }
}