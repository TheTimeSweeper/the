using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Matchmaker.MatchGrid
{
    public class InteractableTileBomb : InteractableTileBehavior
    {
        public MatchTileType tileToDestroy;

        [SerializeField]
        private Image bombTarget;

        private float _randomizeInterval = 0.5f;
        private float _randomizeTim = 0.5f;
        private int _tileTargetIndex = 0;

        public override void Init(MatchTile matchTile)
        {
            base.Init(matchTile);
            _tileTargetIndex = Random.Range(0, matchGrid.TileTypes.Length);
            IncrementTargetTIle();
        }

        void FixedUpdate()
        {
            _randomizeTim -= Time.fixedDeltaTime;

            if (_randomizeTim < 0)
            {
                _randomizeTim = _randomizeInterval;
                IncrementTargetTIle();
            }
        }

        private void IncrementTargetTIle()
        {
            _tileTargetIndex++;
            _tileTargetIndex %= matchGrid.TileTypes.Length;

            tileToDestroy = matchGrid.TileTypes[_tileTargetIndex];

            bombTarget.color = tileToDestroy.GetColor();
            bombTarget.sprite = tileToDestroy.GetIcon();
        }

        protected override void Activate()
        {
            BreakThisTile();

            for (int x = 0; x < matchGrid.TileGrid.GetLength(0); x++)
            {
                for (int y = 0; y < matchGrid.TileGrid.GetLength(1); y++)
                {
                    if (matchGrid.TileGrid[x, y] != null && matchGrid.TileGrid[x, y].TileType == tileToDestroy)
                    {

                        matchGrid.Award(matchGrid.TileGrid[x, y].TileType, 1, 1);
                        matchGrid.TileGrid[x, y].Break();

                    }
                }
            }
            matchGrid.FillEmptyTiles();
            matchGrid.DelayedProcessAllGridMatches(0.5f);

        }
    }
}