using System;
using System.Collections.Generic;
using UnityEngine;

namespace Matchmaker.MatchGrid
{
    public class TileDragLineup : List<MatchTileDragInfo>
    {

        public MatchTile ghostTileTail;
        public MatchTile ghostTileHead;
        private MatchGrid grid;

        private MatchTile _tailTile;
        private MatchTile _headTile;
        private Vector3 _ghostTileOFfset;

        private bool orderDirty = true;

        public TileDragLineup(MatchGrid grid, MatchTile ghostTileHead, MatchTile ghostTileTail)
        {
            this.ghostTileHead = ghostTileHead;
            this.ghostTileTail = ghostTileTail;
            this.grid = grid;
        }

        public void ResetTiles()
        {
            for (int i = 0; i < base.Count; i++)
            {
                base[i].ResetTile();
            }
        }

        public void UpdatePositions(Vector3 delta, Vector2Int deltaGridPosition)
        {
            for (int i = 0; i < base.Count; i++)
            {
                orderDirty |= base[i].UpdatePosition(delta, deltaGridPosition);
            }

            if (orderDirty)
            {
                orderDirty = false;
                base.Sort((tile1, tile2) =>
                {
                    return tile1.currentGridPosition.sqrMagnitude > tile2.currentGridPosition.sqrMagnitude ? 1 : -1;
                });

                if (delta.x > 0)
                {
                    SetGhostTiles(base[Count - 1].tile, base[0].tile);
                    _ghostTileOFfset = Vector3.right * grid.TileDistance.x;
                }
                if (delta.x < 0)
                {
                    SetGhostTiles(base[0].tile, base[Count - 1].tile);
                    _ghostTileOFfset = -Vector3.right * grid.TileDistance.x;
                }

                if (delta.y > 0)
                {
                    SetGhostTiles(base[Count - 1].tile, base[0].tile);
                    _ghostTileOFfset = Vector3.up * grid.TileDistance.y;
                }
                if (delta.y < 0)
                {
                    SetGhostTiles(base[0].tile, base[Count - 1].tile);
                    _ghostTileOFfset = -Vector3.up * grid.TileDistance.y;
                }
            }
            ghostTileHead.transform.localPosition = _headTile.transform.localPosition + _ghostTileOFfset;
            ghostTileTail.transform.localPosition = _tailTile.transform.localPosition - _ghostTileOFfset;
        }

        internal void AddRange(List<MatchTile> matchTiles, MatchGrid matchGrid)
        {
            for (int i = 0; i < matchTiles.Count; i++)
            {
                Add(new MatchTileDragInfo(matchTiles[i], matchGrid));
            }
        }

        private void SetGhostTiles(MatchTile headTile, MatchTile tailTile)
        {
            ghostTileTail.InitTileType(headTile.TileType);
            ghostTileHead.InitTileType(tailTile.TileType);
            _tailTile = tailTile;
            _headTile = headTile;
        }
    }
}