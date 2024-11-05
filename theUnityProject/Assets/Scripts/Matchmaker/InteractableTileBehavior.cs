using UnityEngine.EventSystems;

namespace Matchmaker.MatchGrid
{
    public abstract class InteractableTileBehavior : MatchTileBehavior
    {
        public override void OnTilePointerDown(MatchTile tile, PointerEventData eventData)
        {
            if (!matchGrid.CanActivateInteractable)
                return;
            Activate();
        }

        protected abstract void Activate();

        public override void OnTilePointerUp(MatchTile tile)
        {
            return;
        }
        public override void OnTileDrag(PointerEventData eventData)
        {
            return;
        }
    }
}