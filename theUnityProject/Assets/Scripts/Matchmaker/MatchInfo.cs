using System;
using System.Collections.Generic;

namespace Matchmaker.MatchGrid
{
    public class MatchInfo : IEquatable<MatchInfo>
    {
        public MatchTile[] tilesMatched;
        public int GetMatchCount()
        {
            switch (tilesMatched.Length)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return 1;
                case 4:
                    return 2;
                default:
                case 5:
                    return 5;
            }
        }
        public MatchTileType matchType => tilesMatched[0].TileType;

        public MatchInfo(List<MatchTile> tilesMatched)
        {
            this.tilesMatched = tilesMatched.ToArray();
        }
        public MatchInfo(params MatchTile[] tilesMatched)
        {
            this.tilesMatched = tilesMatched;
        }

        public void Break()
        {
            for (int i = 0; i < tilesMatched.Length; i++)
            {
                MatchTile matchedTile = tilesMatched[i];
                if (matchedTile != null && !matchedTile.Broken)
                {
                    matchedTile.Break();
                }
            }
        }

        public bool Equals(MatchInfo other)
        {
            if (other.tilesMatched.Length != tilesMatched.Length)
                return false;

            int equalCount = 0;
            for (int i = 0; i < tilesMatched.Length; i++)
            {
                for (int j = 0; j < other.tilesMatched.Length; j++)
                {
                    if (tilesMatched[i] == other.tilesMatched[j])
                    {
                        equalCount++;
                        continue;
                    }
                }
            }

            return equalCount == tilesMatched.Length;
        }
    }
}