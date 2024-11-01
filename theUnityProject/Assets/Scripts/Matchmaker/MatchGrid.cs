using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Matchmaker.MatchGrid
{
    public class MatchGrid : MonoBehaviour
    {
        public delegate void MatchAwardedEvent(int matchCount, MatchTileType matchType);
        private MatchAwardedEvent OnMatchAwarded;

        [SerializeField]
        private MatchTile tilePrefab;

        [SerializeField]
        private Vector2Int gridSize;
        public Vector2Int GridSize => gridSize;

        [SerializeField]
        private Vector2 tileDistance;
        public Vector2 TileDistance => tileDistance;

        [SerializeField]
        private float minimumSwapDistance;

        [SerializeField]
        private float breakTime = 0.3f;

        [SerializeField]
        private float fallTime = 0.3f;

        private MatchTileType[] _tileTypes;

        private Vector3 _gridStartLocalPosition;

        private MatchTile[,] _tileGrid = new MatchTile[0, 0];
        private MatchTile _ghostTileHead;
        private MatchTile _ghostTileTail;

        private Vector2Int _selectedGridPosition;
        private Vector2 _pointerDownPosition;

        private TileDragLineup _selectedDragRow;
        private TileDragLineup _selectedDragCol;

        private TileDragLineup _currentDragLineup;
        private TileDragLineup curentDragLineup
        {
            get => _currentDragLineup;
            set
            {
                if (_currentDragLineup == value)
                    return;
                _manualCoroutine = OnDraggedLineChanged(_currentDragLineup, value);
                _currentDragLineup = value;
            }
        }

        private DragDirection _dragDirection;
        private bool _isDragging;

        private IEnumerator _manualCoroutine;
        private float _manualWait;

        private void FixedUpdate()
        {
            _manualWait -= Time.fixedDeltaTime;
            if (_manualCoroutine != null)
            {
                _manualCoroutine.MoveNext();
            }
        }

        public void Init(MatchTileType[] tileTypes)
        {
            _tileTypes = tileTypes;
            ReGenerateGrid();

            if (_ghostTileHead == null)
            {
                _ghostTileHead = Instantiate(tilePrefab, transform);
                _ghostTileHead.gameObject.SetActive(false);
            }

            if (_ghostTileTail == null)
            {
                _ghostTileTail = Instantiate(tilePrefab, transform);
                _ghostTileTail.gameObject.SetActive(false);
            }
        }

        private void ReGenerateGrid()
        {
            foreach (var tile in _tileGrid)
            {
                Destroy(tile.gameObject);
            }
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            GetComponent<RectTransform>().sizeDelta = Vector2.Scale(tileDistance, gridSize.ToVector2());

            _tileGrid = new MatchTile[gridSize.x, gridSize.y];

            Vector3 startOffset = Vector3.Scale(tileDistance.ToVector3() * 0.5f, (gridSize - Vector2Int.one).ToVector3());
            _gridStartLocalPosition = -startOffset;

            for (int gridX = 0; gridX < gridSize.x; gridX++)
            {
                for (int gridY = 0; gridY < gridSize.y; gridY++)
                {
                    _tileGrid[gridX, gridY] = GenerateRandomTile(gridX, gridY);
                }
            }

            _manualCoroutine = ProcessAllGridMatches();

            StartCoroutine(_manualCoroutine);
        }

        private IEnumerator ProcessAllGridMatches()
        {
            int failsafe = 0;

            var originalDrag = _dragDirection;

            int chain = 0;
            List<MatchInfo> matches;
            do
            {
                matches = new List<MatchInfo>();

                _dragDirection = DragDirection.HORIZONTAL;
                matches.AddRange(FindAllGridmatches());

                _dragDirection = DragDirection.VERTICAL;
                matches.AddRange(FindAllGridmatches());

                chain += matches.Count;
                IEnumerator enumerator = ProcessMatches(matches);
                while (enumerator.MoveNext())
                {
                    yield return null;
                }

                Util.FAILSAFE(ref failsafe);
            }
            while (matches.Count > 0);

            Debug.Log($"match chain {chain}");
            _dragDirection = originalDrag;

            _manualCoroutine = null;
        }

        private List<MatchInfo> FindAllGridmatches()
        {
            List<MatchInfo> matches = new List<MatchInfo>();
            for (int gridX = 0; gridX < gridSize.x; gridX++)
            {
                for (int gridY = 0; gridY < gridSize.y; gridY++)
                {
                    if (CheckForMatch(_tileGrid[gridX, gridY], out MatchInfo match))
                    {
                        if (!matches.Contains(match))
                        {
                            matches.Add(match);
                        }
                    }
                }
            }

            return matches;
        }

        private MatchTile GenerateRandomTile(int gridX, int gridY)
        {
            MatchTile matchTile = Instantiate(tilePrefab, transform);
            matchTile.transform.localPosition = GridToWorldPosition(gridX, gridY);
            matchTile.Init(this,
                           _tileTypes[UnityEngine.Random.Range(0, _tileTypes.Length)],
                           gridX,
                           gridY);
            return matchTile;
        }

        private Vector3 GridToWorldPosition(Vector2Int gridPoint) => GridToWorldPosition(gridPoint.x, gridPoint.y);
        private Vector3 GridToWorldPosition(int gridX, int gridY)
        {
            return _gridStartLocalPosition + new Vector3(gridX * tileDistance.x, gridY * tileDistance.y, 0);
        }

        private IEnumerator OnDraggedLineChanged(TileDragLineup oldLineup_, TileDragLineup newLineup_)
        {
            if (oldLineup_ != null && newLineup_ != null)
            {
                oldLineup_.ResetTiles();
            }
            if (newLineup_ == null)
            {
                List<MatchInfo> matches = new List<MatchInfo>();
                for (int i = 0; i < oldLineup_.Count; i++)
                {
                    if (CheckForMatch(oldLineup_[i], out MatchInfo match))
                    {
                        matches.Add(match);
                    }
                }
                if (matches.Count > 0)
                {
                    //set dragged line to new positions
                    for (int i = 0; i < oldLineup_.Count; i++)
                    {
                        MatchTileDragInfo tileInfo = oldLineup_[i];
                        Vector2Int newPosition = tileInfo.currentGridPosition;

                        MoveTile(newPosition, tileInfo.tile);
                    }

                    IEnumerator matchCoroutine = ProcessMatches(matches);
                    while (matchCoroutine.MoveNext())
                    {
                        yield return null;
                    }

                    IEnumerator allMatchesCoroutine2 = ProcessAllGridMatches();
                    while (allMatchesCoroutine2.MoveNext())
                    {
                        yield return null;
                    }
                }
                else
                {
                    oldLineup_.ResetTiles();
                }
            }
            _manualCoroutine = null;
        }

        private void MoveTile(Vector2Int newPosition, MatchTile movingTile)
        {
            movingTile.OnMove();
            movingTile.GridPosition = newPosition;
            movingTile.transform.localPosition = GridToWorldPosition(newPosition);
            _tileGrid[newPosition.x, newPosition.y] = movingTile;
        }

        private IEnumerator ProcessMatches(List<MatchInfo> matches)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                Award(matches[i].matchCount, matches[i].matchType);
                matches[i].Break();
            }
            if (matches.Count > 0)
            {
                _manualWait = breakTime;
                while (_manualWait > 0)
                {
                    yield return null;
                }
                //yield return new WaitForSeconds(0.5f);
            }

            if (FillEmptyTiles())
            {
                _manualWait = fallTime;
                while (_manualWait > 0)
                {
                    yield return null;
                }
                //yield return new WaitForSeconds(0.5f);
            }
        }

        private bool FillEmptyTiles()
        {
            bool dropped = false;
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = gridSize.y - 1; y >= 0; y--)
                {
                    List<MatchTile> droppingTiles = new List<MatchTile>();
                    if (_tileGrid[x, y] == null)
                    {
                        int searchY = y;
                        int emptyTiles = 0;
                        while (searchY < gridSize.y)
                        {
                            if (_tileGrid[x, searchY] != null)
                            {
                                droppingTiles.Add(_tileGrid[x, searchY]);
                            }
                            else
                            {
                                emptyTiles++;
                            }
                            searchY++;
                        }

                        dropped = true;
                        DropTiles(x, y + 1, emptyTiles, droppingTiles);
                    }
                }
            }

            return dropped;
        }

        private void DropTiles(int x, int y, int dropAmount, List<MatchTile> droppingTiles)
        {
            for (int i = 0; i < dropAmount; i++)
            {
                droppingTiles.Add(GenerateRandomTile(x, gridSize.y + i));
            }
            for (int i = 0; i < droppingTiles.Count; i++)
            {
                MoveTile(droppingTiles[i].GridPosition + Vector2Int.down * dropAmount, droppingTiles[i]);
            }
        }

        private void Award(int matchCount, MatchTileType matchType)
        {
            Debug.LogWarning($"matched {matchCount} {matchType}s!");
            OnMatchAwarded?.Invoke(matchCount, matchType);
        }

        private bool CheckForMatch(MatchTileDragInfo startTile, out MatchInfo foundMatch) => CheckForMatch(startTile.tile, startTile.currentGridPosition, out foundMatch);
        private bool CheckForMatch(MatchTile tile, out MatchInfo foundMatch) => CheckForMatch(tile, tile.GridPosition, out foundMatch);
        private bool CheckForMatch(MatchTile tile, Vector2Int currentGridPosition, out MatchInfo foundMatch)
        {
            List<MatchTile> matchedTilesVertical = new List<MatchTile>
        {
            tile
        };
            List<Vector2Int> checkedTilePositions = new List<Vector2Int>()
        {
            currentGridPosition
        };

            Queue<Vector2Int> tilePositionQueue = new Queue<Vector2Int>();
            tilePositionQueue.Enqueue(currentGridPosition);

            int failsafe = 0;
            while (tilePositionQueue.Count > 0)
            {
                if (Util.FAILSAFE(ref failsafe))
                    break;

                Vector2Int queueTile = tilePositionQueue.Dequeue();
                if (_dragDirection == DragDirection.HORIZONTAL)
                {
                    CheckTile(matchedTilesVertical, tilePositionQueue, checkedTilePositions, tile.TileType, queueTile + Vector2Int.up);
                    CheckTile(matchedTilesVertical, tilePositionQueue, checkedTilePositions, tile.TileType, queueTile + Vector2Int.down);
                }
                else
                {
                    CheckTile(matchedTilesVertical, tilePositionQueue, checkedTilePositions, tile.TileType, queueTile + Vector2Int.left);
                    CheckTile(matchedTilesVertical, tilePositionQueue, checkedTilePositions, tile.TileType, queueTile + Vector2Int.right);
                }
            }

            if (matchedTilesVertical.Count >= 3)
            {
                foundMatch = new MatchInfo(matchedTilesVertical);
                return true;
            }

            foundMatch = null;
            return false;
        }

        private void CheckTile(List<MatchTile> matchedTiles, Queue<Vector2Int> tileCheckQueue, List<Vector2Int> checkedTilePositions, MatchTileType tileTYpe, Vector2Int checkTilePosition)
        {
            if (!checkedTilePositions.Contains(checkTilePosition) && checkTilePosition.x >= 0 && checkTilePosition.x < gridSize.x && checkTilePosition.y >= 0 && checkTilePosition.y < gridSize.y)
            {
                MatchTile lookTile = _tileGrid[checkTilePosition.x, checkTilePosition.y];
                if (lookTile.TileType == tileTYpe)
                {
                    matchedTiles.Add(lookTile);
                    checkedTilePositions.Add(checkTilePosition);
                    tileCheckQueue.Enqueue(checkTilePosition);
                }
            }
        }

        #region tile drag callbacks

        public void TilePointerDown(MatchTile tile, PointerEventData eventData)
        {
            if (_manualCoroutine != null)
                return;
            _isDragging = true;

            _selectedGridPosition = tile.GridPosition;
            _pointerDownPosition = eventData.position;

            SetSelectedPositionLineups();
        }

        private void SetSelectedPositionLineups()
        {
            _selectedDragRow = new TileDragLineup(this, _ghostTileHead, _ghostTileTail);
            for (int x = 0; x < _tileGrid.GetLength(0); x++)
            {
                _selectedDragRow.Add(new MatchTileDragInfo(_tileGrid[x, _selectedGridPosition.y], this));
            }

            _selectedDragCol = new TileDragLineup(this, _ghostTileHead, _ghostTileTail);
            for (int y = 0; y < _tileGrid.GetLength(1); y++)
            {
                _selectedDragCol.Add(new MatchTileDragInfo(_tileGrid[_selectedGridPosition.x, y], this));
            }
        }

        public void TilePointerUp(MatchTile tile)
        {
            _isDragging = false;
            curentDragLineup = null;
            _ghostTileHead.gameObject.SetActive(false);
            _ghostTileTail.gameObject.SetActive(false);
        }

        public void TileDrag(PointerEventData eventData)
        {
            if (_manualCoroutine != null || !_isDragging)
                return;

            _ghostTileHead.gameObject.SetActive(true);
            _ghostTileTail.gameObject.SetActive(true);

            Vector2 inputDeltaPosition = eventData.position - _pointerDownPosition;
            inputDeltaPosition = inputDeltaPosition.ToVector3();

            Vector2Int gridDelta;

            if (MathF.Abs(inputDeltaPosition.x) < minimumSwapDistance && MathF.Abs(inputDeltaPosition.y) < minimumSwapDistance)
            {
                if (MathF.Abs(inputDeltaPosition.x) > MathF.Abs(inputDeltaPosition.y))
                {
                    _dragDirection = DragDirection.HORIZONTAL;
                }
                else
                {
                    _dragDirection = DragDirection.VERTICAL;
                }
            }

            if (_dragDirection == DragDirection.HORIZONTAL)
            {
                curentDragLineup = _selectedDragRow;
                inputDeltaPosition.y = 0;

                int gridDistance = Mathf.RoundToInt((inputDeltaPosition.x /*- tileDistance.x / 2 * Mathf.Sign(movingDeltaPosition.x)*/) / tileDistance.x);
                gridDelta = new Vector2Int(gridDistance, 0);
            }
            else
            {
                curentDragLineup = _selectedDragCol;
                inputDeltaPosition.x = 0;

                int gridDistance = Mathf.RoundToInt((inputDeltaPosition.y /*- tileDistance.y / 2 * Mathf.Sign(movingDeltaPosition.y)*/) / tileDistance.y);
                gridDelta = new Vector2Int(0, gridDistance);
            }

            curentDragLineup.UpdatePositions(inputDeltaPosition, gridDelta);
        }

        public void RemoveTile(MatchTile matchTile)
        {
            _tileGrid[matchTile.GridPosition.x, matchTile.GridPosition.y] = null;
        }
        #endregion tile drag callbacks
    }
}