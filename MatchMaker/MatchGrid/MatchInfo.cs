using MatcherMod;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Matchmaker.MatchGrid
{
    public class MatchInfo : IEquatable<MatchInfo>
    {
        public MatchTile[] tilesMatched;
        public MatchTileType matchType => tilesMatched[0].TileType;

        public MatchInfo(List<MatchTile> tilesMatched)
        {
            this.tilesMatched = tilesMatched.ToArray();
        }
        public MatchInfo(params MatchTile[] tilesMatched)
        {
            this.tilesMatched = tilesMatched;
        }

        public int GetMatchCount()
        {
            int baseMatches;
            switch (tilesMatched.Length)                                           
            {
                case 0: case 1: case 2: //not possible bu they    
                    baseMatches = 0;
                    Log.Error($"matched only {tilesMatched.Length} of {matchType}. was this intended?");
                    break;
                case 3: // single match                                            
                case 4: // double                                                  
                case 5: // triple                                                  
                    baseMatches = tilesMatched.Length - 2;                         
                    break;                                                         
                default: //bonus for crazy long                                    
                    baseMatches = Mathf.RoundToInt((tilesMatched.Length - 3) * 2f);
                    break;
            }

            for (int i = 0; i < tilesMatched.Length; i++)
            {
                tilesMatched[i].ApplyMatchModifier(ref baseMatches);
            }

            return baseMatches;
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