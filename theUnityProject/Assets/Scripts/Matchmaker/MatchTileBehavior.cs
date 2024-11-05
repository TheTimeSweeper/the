using UnityEngine;
using UnityEngine.EventSystems;

namespace Matchmaker.MatchGrid
{
    public class MatchTileBehavior : MonoBehaviour
    {
        protected MatchTile _matchTile;
        protected MatchGrid matchGrid => _matchTile.MatchGrid;

        public virtual void Init(MatchTile matchTile)
        {
            _matchTile = matchTile;
        }

        public virtual void OnTilePointerDown(MatchTile tile, PointerEventData eventData)
        {
            matchGrid.TilePointerDown(tile, eventData);
        }

        public virtual void OnTilePointerUp(MatchTile tile)
        {
            matchGrid.TilePointerUp(tile);
        }

        public virtual void OnTileDrag(PointerEventData eventData)
        {
            matchGrid.TileDrag(eventData);
        }
    }
}