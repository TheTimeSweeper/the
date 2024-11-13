using System;
using UnityEngine;

namespace Matchmaker.MatchGrid
{
    [CreateAssetMenu(fileName = "tileNew", menuName = "new matchTileType")]
    public class MatchTileType : ScriptableObject
    {
        public Color color = Color.white;

        internal Color GetColor()
        {
            return color;
        }

        internal Sprite GetIcon()
        {
            return null;
        }
    }
}