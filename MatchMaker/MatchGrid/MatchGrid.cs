using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Matchmaker.MatchGrid
{
    public class MatchGrid : MonoBehaviour
    {
        public delegate void MatchAwardedEvent(MatchTileType matchType, int matchCount, int tilesMatched);
        public MatchAwardedEvent OnMatchAwarded;

        [SerializeField]
        private UISelectOnHover UISelectOnHover;

        [SerializeField]
        private MatchTile tilePrefab;

        [SerializeField]
        private Vector2Int gridSize;
        public Vector2Int GridSize => gridSize;

        [SerializeField]
        private Vector2 tileDistance;
        public Vector2 TileDistance => tileDistance;

        [SerializeField]
        private RectTransform anchor;

        [SerializeField]
        private float minimumSwapDistance;

        [SerializeField]
        private float breakTime = 0.3f;

        [SerializeField]
        private float fallTime = 0.3f;

        private MatchTileType[] _tileTypes;
        public MatchTileType[] TileTypes => _tileTypes;

        [SerializeField]
        private MatchGridInteractables matchGridInteractables;
        private MatchTile[] _interactableTileTypes;

        [SerializeField]
        private float _interactableTileChance;

        private Vector3 _gridStartLocalPosition;

        private MatchTile[,] _tileGrid = new MatchTile[0, 0];
        public MatchTile[,] TileGrid => _tileGrid;

        private MatchTile[,] _tempTileGrid = new MatchTile[0, 0];

        private MatchTile _ghostTileHead;
        private MatchTile _ghostTileTail;

        private Vector2Int _selectedGridPosition;
        private Vector2 _pointerDownPosition;

        private bool _awaitingDrag;
        private Vector2Int _awaitingGridPosition;
        private Vector2 _awaitingPointerDownPosition;

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

        private IEnumerator _pausedCoroutine;

        public bool CanActivateInteractable => _manualCoroutine == null && !_isDragging;

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
            _interactableTileTypes = matchGridInteractables.InteractableTileTypes;
            ReGenerateGrid();

            if (_ghostTileHead == null)
            {
                _ghostTileHead = Instantiate(tilePrefab, anchor);
                _ghostTileHead.gameObject.SetActive(false);
            }

            if (_ghostTileTail == null)
            {
                _ghostTileTail = Instantiate(tilePrefab, anchor);
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
            anchor.sizeDelta = Vector2.Scale(tileDistance, gridSize.ToVector2());

            _tileGrid = new MatchTile[gridSize.x, gridSize.y];
            _tempTileGrid  = new MatchTile[gridSize.x, gridSize.y];

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

            int chain = 0;
            List<MatchInfo> matches;
            do
            {
                if (_awaitingDrag)
                {
                    StartDragging();
                    break;
                }

                matches = new List<MatchInfo>();

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

            _manualCoroutine = null;
        }

        private List<MatchInfo> FindAllGridmatches()
        {
            List<MatchInfo> matches = new List<MatchInfo>();
            for (int gridX = 0; gridX < gridSize.x; gridX++)
            {
                for (int gridY = 0; gridY < gridSize.y; gridY++)
                {
                    CheckForMatches(_tileGrid[gridX, gridY], matches);
                }
            }

            return matches;
        }

        private void CheckForMatches(MatchTileDragInfo matchTileDragInfo, List<MatchInfo> matches)
        {
            if (CheckForMatch(matchTileDragInfo, DragDirection.VERTICAL, out MatchInfo matchV))
            {
                if (!matches.Contains(matchV))
                {
                    matches.Add(matchV);
                }
            }

            if (CheckForMatch(matchTileDragInfo, DragDirection.HORIZONTAL, out MatchInfo matchH))
            {
                if (!matches.Contains(matchH))
                {
                    matches.Add(matchH);
                }
            }
        }
        private void CheckForMatches(MatchTile tile, List<MatchInfo> matches)
        {
            if (CheckForMatch(tile, DragDirection.VERTICAL, out MatchInfo matchV))
            {
                if (!matches.Contains(matchV))
                {
                    matches.Add(matchV);
                }
            }

            if (CheckForMatch(tile, DragDirection.HORIZONTAL, out MatchInfo matchH))
            {
                if (!matches.Contains(matchH))
                {
                    matches.Add(matchH);
                }
            }
        }

        private MatchTile GenerateRandomTile(int gridX, int gridY)
        {
            MatchTile matchTile;
            if (UnityEngine.Random.value > _interactableTileChance)
            {

                matchTile = Instantiate(tilePrefab, anchor);
                matchTile.transform.localPosition = GridToWorldPosition(gridX, gridY);
                matchTile.Init(this,
                               _tileTypes[UnityEngine.Random.Range(0, _tileTypes.Length)],
                               gridX,
                               gridY);
            } 
            else
            {
                matchTile = Instantiate(_interactableTileTypes[UnityEngine.Random.Range(0, _interactableTileTypes.Length)], anchor);
                matchTile.transform.localPosition = GridToWorldPosition(gridX, gridY);
                matchTile.Init(this,
                               null,
                               gridX,
                               gridY);
            }
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
                for (int i = 0; i < oldLineup_.Count; i++)
                {
                    TempMoveTile(oldLineup_[i].currentGridPosition, oldLineup_[i].tile);
                }
                List<MatchInfo> matches = new List<MatchInfo>();
                for (int i = 0; i < oldLineup_.Count; i++)
                {
                    CheckForMatches(oldLineup_[i], matches);
                }

                ClearTempGrid();

                if (matches.Count > 0)
                {
                    //set dragged line to new positions
                    for (int i = 0; i < oldLineup_.Count; i++)
                    {
                        MatchTileDragInfo tileInfo = oldLineup_[i];
                        Vector2Int newPosition = tileInfo.currentGridPosition;

                        SetMoveTile(newPosition, tileInfo.tile);
                    }
                }
                else
                {
                    oldLineup_.ResetTiles();
                }

                IEnumerator allMatchesCoroutine = ProcessAllGridMatches();
                while (allMatchesCoroutine.MoveNext())
                {
                    yield return null;
                }
            }
            _manualCoroutine = null;
        }

        private void ClearTempGrid()
        {
            for (int gridX = 0; gridX < gridSize.x; gridX++)
            {
                for (int gridY = 0; gridY < gridSize.y; gridY++)
                {
                    _tempTileGrid[gridX, gridY] = null;
                } 
            }
        }

        private void TempMoveTile(Vector2Int newPosition, MatchTile movingTile)
        {
            _tempTileGrid[newPosition.x, newPosition.y] = movingTile;
        }

        private void SetMoveTile(Vector2Int newPosition, MatchTile movingTile)
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
                Award(matches[i].matchType, matches[i].GetMatchCount(), matches[i].tilesMatched.Length);
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

        public bool FillEmptyTiles()
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
                SetMoveTile(droppingTiles[i].GridPosition + Vector2Int.down * dropAmount, droppingTiles[i]);
            }
        }

        private void Award(MatchTileType matchType, int matchCount, int tilesMatched)
        {
            Debug.LogWarning($"matched {matchCount} {matchType}s!");
            OnMatchAwarded?.Invoke(matchType, matchCount, tilesMatched);
        }

        private bool CheckForMatch(MatchTileDragInfo startTile, DragDirection dragDirection, out MatchInfo foundMatch) => CheckForMatch(startTile.tile, startTile.currentGridPosition, dragDirection, out foundMatch);
        private bool CheckForMatch(MatchTile tile, DragDirection dragDirection, out MatchInfo foundMatch) => CheckForMatch(tile, tile.GridPosition, dragDirection, out foundMatch);
        private bool CheckForMatch(MatchTile tile, Vector2Int currentGridPosition, DragDirection dragDirection, out MatchInfo foundMatch)
        {
            if (tile == null || tile.TileType == null)
            {
                foundMatch = null;
                return false;
            }

            List<MatchTile> matchedTiles = AggregateMatches(tile, currentGridPosition, dragDirection);

            if (matchedTiles.Count >= 3)
            {
                foundMatch = new MatchInfo(matchedTiles);
                return true;
            }

            foundMatch = null;
            return false;
        }

        private List<MatchTile> AggregateMatches(MatchTile tile, Vector2Int currentGridPosition, DragDirection dragDirection)
        {
            List<MatchTile> matchedTiles = new List<MatchTile>
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
                if (dragDirection == DragDirection.HORIZONTAL)
                {
                    CheckTile(matchedTiles, tilePositionQueue, checkedTilePositions, tile.TileType, queueTile + Vector2Int.up);
                    CheckTile(matchedTiles, tilePositionQueue, checkedTilePositions, tile.TileType, queueTile + Vector2Int.down);
                }
                else
                {
                    CheckTile(matchedTiles, tilePositionQueue, checkedTilePositions, tile.TileType, queueTile + Vector2Int.left);
                    CheckTile(matchedTiles, tilePositionQueue, checkedTilePositions, tile.TileType, queueTile + Vector2Int.right);
                }
            }
            return matchedTiles;
        }

        private void CheckTile(List<MatchTile> matchedTiles, Queue<Vector2Int> tileCheckQueue, List<Vector2Int> checkedTilePositions, MatchTileType tileTYpe, Vector2Int checkTilePosition)
        {
            if (!checkedTilePositions.Contains(checkTilePosition) && 
                checkTilePosition.x >= 0 && 
                checkTilePosition.x < gridSize.x && 
                checkTilePosition.y >= 0 && 
                checkTilePosition.y < gridSize.y)
            {
                MatchTile lookTile = GetGridTileToCheck(checkTilePosition.x, checkTilePosition.y);
                if (lookTile != null && lookTile.TileType != null && lookTile.TileType == tileTYpe)
                {
                    matchedTiles.Add(lookTile);
                    checkedTilePositions.Add(checkTilePosition);
                    tileCheckQueue.Enqueue(checkTilePosition);
                }
            }
        }

        private MatchTile GetGridTileToCheck(int x, int y)
        {
            if (_tempTileGrid[x,y] != null)
            {
                return _tempTileGrid[x, y];
            }
            return _tileGrid[x,y];
        }

        #region tile event trigger callbacks

        public void TilePointerDown(MatchTile tile, PointerEventData eventData)
        {
            if (tile.IsMoving)
                return;

            AwaitDragging(tile.GridPosition, eventData.position);

            if (_manualCoroutine == null)
            {
                StartDragging();
            }
        }

        private void AwaitDragging(Vector2Int gridPosition, Vector2 position)
        {
            _awaitingDrag = true;
            _awaitingGridPosition = gridPosition;
            _awaitingPointerDownPosition = position;
        }

        private void StartDragging()
        {
            _awaitingDrag = false;
            _isDragging = true;

            _selectedGridPosition = _awaitingGridPosition;
            _pointerDownPosition = _awaitingPointerDownPosition;

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
            CancelDragging();
            if(_manualCoroutine == null)
            {
                _manualCoroutine = ProcessAllGridMatches();
            }
        }

        private void CancelDragging()
        {
            _awaitingDrag = false;
            _isDragging = false;
            curentDragLineup = null;
            _ghostTileHead.gameObject.SetActive(false);
            _ghostTileTail.gameObject.SetActive(false);
        }

        public void TileDrag(PointerEventData eventData)
        {
            if (_manualCoroutine != null || !_isDragging)
                return;

            if (_currentDragLineup != null)
            {
                for (int i = 0; i < _currentDragLineup.Count; i++)
                {
                    if (_currentDragLineup[i].tile.IsMoving)
                    {
                        _currentDragLineup.ResetTiles();
                        CancelDragging();
                        return;
                    }
                }
            }

            //show the ghost tiles. these will be managed by the current TileDragLineup
            _ghostTileHead.gameObject.SetActive(true);
            _ghostTileTail.gameObject.SetActive(true);

            Vector2 inputDeltaPosition = eventData.position - _pointerDownPosition;
            inputDeltaPosition = inputDeltaPosition.ToVector3();

            Vector2Int gridDelta;

            //get drag direction
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

            //find amount to move tiles and grid spaces that tiles have moved based on pointer delta 
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

            //move tiles
            curentDragLineup.UpdatePositions(inputDeltaPosition, gridDelta);
        }

        public void RemoveTile(MatchTile matchTile)
        {
            _tileGrid[matchTile.GridPosition.x, matchTile.GridPosition.y] = null;
        }

        internal void DelayedProcessAllGridMatches(float delay)
        {
            _manualCoroutine = DelayedCoroutine(delay, ProcessAllGridMatches());
        }
        private IEnumerator DelayedCoroutine(float delay, IEnumerator coroutine)
        {
            _manualWait = delay;
            while (_manualWait > 0)
            {
                yield return null;
            }
            _manualCoroutine = coroutine;
        }

        public void GetSelected()
        {
            UISelectOnHover.SetSelectedGameObject();
        }

        #endregion tile event trigger callbacks
    }
}