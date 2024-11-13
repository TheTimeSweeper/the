using UnityEngine.EventSystems;

namespace Matchmaker.MatchGrid
{
    public abstract class InteractableTileBehavior : MatchTileBehavior
    {
        public override void OnTilePointerDown(MatchTile tile, PointerEventData eventData)
        {
            matchGrid.GetSelected();
            if (!matchGrid.CanInteract)
                return;
            Activate();
        }

        protected abstract void Activate();

        public override bool CheckAgainstThisTile(MatchTile otherTile)
        {
            return false;
        }
        public override bool CheckAgainstThisTileType(MatchTileType otherTile)
        {
            return false;
        }

        public override void OnTilePointerUp(MatchTile tile)
        {
            return;
        }
        public override void OnTileDrag(PointerEventData eventData)
        {
            return;
        }

        protected virtual void BreakThisTile()
        {
            _matchTile.Break();
            matchGrid.FillEmptyTiles();
        }
    }
}