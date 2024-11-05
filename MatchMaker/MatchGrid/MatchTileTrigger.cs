using UnityEngine.EventSystems;

namespace Matchmaker.MatchGrid
{
    public class MatchTileTrigger : EventTrigger
    {
        private MatchGrid _matchGrid;
        private MatchTile _tile;

        public void Init(MatchTile tile)
        {
            _tile = tile;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _tile.OnTilePointerDown(_tile, eventData);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _tile.OnTilePointerUp(_tile);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            _tile.OnTileDrag(eventData);
        }
    }
}