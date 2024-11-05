using UnityEngine;

namespace Matchmaker.MatchGrid
{
    public class MatchGridInteractables : MonoBehaviour
    {
        [SerializeField]
        private MatchTile[] interactableTileTypes;
        public MatchTile[] InteractableTileTypes => interactableTileTypes;
    }
}