using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Matchmaker.MatchGrid
{
    public class MatchTile : MonoBehaviour
    {
        [SerializeField]
        private MatchTileTrigger trigger;
        [SerializeField]
        private MatchTileBehavior behavior;

        [SerializeField]
        private Image image;
        [SerializeField]
        private Image flashImage;

        [SerializeField]
        private float moveLerp = 10;

        private Vector2Int _gridPosition;
        public Vector2Int GridPosition { get => _gridPosition; set => _gridPosition = value; }

        public MatchTileType TileType { get; set; }
        public bool Broken { get; internal set; }

        private MatchGrid _matchGrid;
        public MatchGrid MatchGrid => _matchGrid;

        private Vector3 _moveLerpPosition;

        public bool IsMoving { get; private set; }

        private IEnumerator _manualCoroutine;

        public virtual void Init(MatchGrid matchGrid, MatchTileType tileType, int x, int y)
        {
            _matchGrid = matchGrid;
            _gridPosition = new Vector2Int(x, y);

            trigger.Init(this);
            behavior.Init(this);

            InitTileType(tileType);
        }

        public void InitTileType(MatchTileType tileType)
        {
            TileType = tileType;

            InitTIleTypeView(tileType);
        }

        private void InitTIleTypeView(MatchTileType tileType)
        {
            if (TileType == null)
                return;

            image.color = tileType.GetColor();
            image.sprite = tileType.GetIcon();
        }

        void Update()
        {
            if (_manualCoroutine != null)
            {
                if (!_manualCoroutine.MoveNext())
                {
                    _manualCoroutine = null;
                }
            }
        }

        internal void Break()
        {
            Broken = true;
            _matchGrid.RemoveTile(this);

            image.transform.localScale = Vector3.one * 1.3f;

            _manualCoroutine = DestroyCoroutine();
        }

        private IEnumerator DestroyCoroutine()
        {
            float destroyTimer = 0.5f;
            while (destroyTimer > 0)
            {
                destroyTimer -= Time.deltaTime;

                Color color = image.color;
                color.a = destroyTimer / 0.3f;
                image.color = color;

                yield return null;
            }
            Destroy(gameObject);
        }

        public void OnMove()
        {
            IsMoving = true;
            _moveLerpPosition = image.transform.position;
            _manualCoroutine = MoveCoroutine();
        }

        private IEnumerator MoveCoroutine()
        {
            float moveTimer = 0.3f;
            while (moveTimer > 0)
            {
                moveTimer -= Time.deltaTime;
                _moveLerpPosition = Util.ExpDecayLerp(_moveLerpPosition, transform.position, moveLerp, Time.deltaTime);
                image.transform.position = _moveLerpPosition;

                yield return null;
            }
            image.transform.localPosition = Vector3.zero;
            IsMoving = false;
        }

        public virtual void OnTilePointerDown(MatchTile tile, PointerEventData eventData)
        {
            behavior.OnTilePointerDown(tile, eventData);
        }

        public virtual void OnTilePointerUp(MatchTile tile)
        {
            behavior.OnTilePointerUp(tile);
        }

        public virtual void OnTileDrag(PointerEventData eventData)
        {
            behavior.OnTileDrag(eventData);
        }

        public void Transform(MatchTileType matchTileType)
        {
            InitTileType(matchTileType);

            _manualCoroutine = TransformCoroutine();
        }

        private IEnumerator TransformCoroutine()
        {
            flashImage.color = Color.white;
            image.transform.localScale = Vector3.one * 1.1f;

            float transformTimer = 0.4f;
            while (transformTimer > 0)
            {
                transformTimer -= Time.deltaTime;

                flashImage.color = Util.ExpDecayLerp(flashImage.color, Color.clear, 3, Time.deltaTime);

                image.transform.localScale = Util.ExpDecayLerp(image.transform.localScale, Vector3.one, 3, Time.deltaTime);

                yield return null;
            }
            flashImage.color = Color.clear;
        }
    }
}