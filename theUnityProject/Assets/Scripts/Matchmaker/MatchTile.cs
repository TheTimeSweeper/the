using System;
using UnityEngine;
using UnityEngine.UI;

namespace Matchmaker.MatchGrid
{
    public class MatchTile : MonoBehaviour
    {
        [SerializeField]
        private MatchTileTrigger trigger;

        [SerializeField]
        private Image image;

        private Vector2Int _gridPosition;
        public Vector2Int GridPosition { get => _gridPosition; set => _gridPosition = value; }

        public MatchTileType TileType { get; set; }
        public bool Broken { get; internal set; }

        private MatchGrid _matchGrid;

        private float _destroyTimer = -1;
        private float _moveTimer = -1;
        private Vector3 _moveLerpPosition;

        internal void Init(MatchGrid matchGrid, MatchTileType tileType, int x, int y)
        {
            _matchGrid = matchGrid;
            _gridPosition = new Vector2Int(x, y);

            trigger.Init(matchGrid, this);

            InitTileType(tileType);
        }

        public void InitTileType(MatchTileType tileType)
        {
            TileType = tileType;

            //image.sprite = tileType.skillDef.icon;
        }

        internal void Break()
        {
            Broken = true;
            _matchGrid.RemoveTile(this);

            image.transform.localScale = Vector3.one * 1.1f;

            _destroyTimer = 0.5f;
        }

        void Update()
        {
            if (_destroyTimer > 0)
            {
                Color color = image.color;
                color.a = _destroyTimer / 0.3f;
                image.color = color;

                _destroyTimer -= Time.deltaTime;
                if (_destroyTimer < 0)
                {
                    Destroy(gameObject);
                }
            }

            if (_moveTimer > 0)
            {
                _moveLerpPosition = Util.ExpDecayLerp(_moveLerpPosition, transform.position, 10, Time.deltaTime);
                image.transform.position = _moveLerpPosition;

                _moveTimer -= Time.deltaTime;
                if (_moveTimer < 0)
                {
                    image.transform.localPosition = Vector3.zero;
                }
            }
        }

        internal void OnMove()
        {
            _moveLerpPosition = image.transform.position;
            _moveTimer = 0.5f;
        }
    }
}