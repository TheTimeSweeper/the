using UnityEngine;

namespace Matchmaker.MatchGrid
{
    [CreateAssetMenu(fileName = "tileNew", menuName = "new matchTileType")]
    public class MatchTileType : ScriptableObject
    {
        public Color color = Color.white;
    }
}