using UnityEngine.EventSystems;

namespace Matchmaker.MatchGrid
{
    public class MatchTileTrigger : EventTrigger
    {
        private MatchGrid _matchGrid;
        private MatchTile _tile;

        public void Init(MatchGrid matchGrid, MatchTile tile)
        {
            _matchGrid = matchGrid;
            _tile = tile;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _matchGrid.TilePointerDown(_tile, eventData);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _matchGrid.TilePointerUp(_tile);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            _matchGrid.TileDrag(eventData);
        }
    }
}