using MatcherMod;
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

        private SpecialTileInfo[] _specialTileTypes;

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

        public bool CanInteract => _manualCoroutine == null && !_isDragging;
        private bool isTimeStopped => _timeStoppedTime > 0;
        private float _timeStoppedTime;
        public Action<float> timeStopAction;

        private void FixedUpdate()
        {
            _manualWait -= Time.fixedDeltaTime;
            if (_manualCoroutine != null)
            {
                _manualCoroutine.MoveNext();
            }
            if (_timeStoppedTime > 0)
            {
                _timeStoppedTime -= Time.fixedDeltaTime;

                if (_timeStoppedTime < 0)
                {
                    if (_isDragging)
                    {
                        CancelDragging();
                    }
                    else
                    {
                        _manualCoroutine = ProcessAllGridMatches();
                    }
                }
            }
        }

        public void CompleteCoroutine()
        {
            if (_manualCoroutine == null)
            {
                return;
            }

            while (_manualCoroutine.MoveNext())
            {
                _manualWait = -1;
            }
            _manualCoroutine = null;
        }

        public void Init(MatchTileType[] tileTypes_, SpecialTileInfo[] specialTileTypes_) => Init(tileTypes_, specialTileTypes_, gridSize);
        public void Init(MatchTileType[] tileTypes_, SpecialTileInfo[] specialTileTypes_, Vector2Int gridSize_)
        {
            _tileTypes = tileTypes_;
            _specialTileTypes = specialTileTypes_;
            gridSize = gridSize_;
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
            //CompleteCoroutine();

            for (int x = 0; x < _tileGrid.GetLength(0); x++)
            {
                for (int y = 0; y < _tileGrid.GetLength(1); y++)
                {
                    MatchTile tile = _tileGrid[x, y];

                    if (tile != null)
                    {
                        Destroy(tile.gameObject);
                    }
                    else
                    {
                        Log.Warning($"null at {x}, {y} {Environment.StackTrace}");
                    }
                }
            }

            GenerateGrid();
        }

        private void GenerateGrid()
        {
            anchor.sizeDelta = Vector2.Scale(tileDistance, gridSize.ToVector2());

            _tileGrid = new MatchTile[gridSize.x, gridSize.y];
            _tempTileGrid = new MatchTile[gridSize.x, gridSize.y];

            Vector3 startOffset = Vector3.Scale(tileDistance.ToVector3() * 0.5f, (gridSize - Vector2Int.one).ToVector3());
            _gridStartLocalPosition = -startOffset;

            //for (int gridX = 0; gridX < gridSize.x; gridX++)
            //{
            //    for (int gridY = 0; gridY < gridSize.y; gridY++)
            //    {
            //        _tileGrid[gridX, gridY] = GenerateRandomTile(gridX, gridY);
            //    }
            //}

            FillEmptyTiles();
            _manualCoroutine = DelayedProcessMatches(0.6f);
        }

        private IEnumerator DelayedProcessMatches(float delay)
        {
            _manualWait = delay;
            while (_manualWait > 0)
            {
                yield return null;
            }
            _manualCoroutine = ProcessAllGridMatches();
        }

        private IEnumerator ProcessAllGridMatches()
        {
            Log.Debug("processing matches");
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

            Log.Debug($"match chain {chain}");

            _manualCoroutine = null;
        }

        private List<MatchInfo> FindAllGridmatches()
        {
            List<MatchInfo> matches = new List<MatchInfo>();
            for (int gridX = 0; gridX < gridSize.x; gridX++)
            {
                for (int gridY = 0; gridY < gridSize.y; gridY++)
                {
                    if (_tileGrid[gridX, gridY] == null)
                    {
                        Log.Error($"checking for match and tile at {gridX}, {gridY} was null. is this fine?\n" + Environment.StackTrace);
                    }
                    CheckForMatches(_tileGrid[gridX, gridY], matches);
                }
            }

            return matches;
        }

        private MatchTile GenerateRandomTile(int gridX, int gridY)
        {
            MatchTile matchTile;

            List<float> weights = new List<float>();
            for (int i = 0; i < _specialTileTypes.Length; i++)
            {
                weights.Add(_specialTileTypes[i].SpawnChance);
            }

            if (_specialTileTypes.Length == 0 || UnityEngine.Random.value > weights.Sum())
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
                SpecialTileInfo specialTileInfo = _specialTileTypes[Util.WeightedRandom(weights)];
                matchTile = Instantiate(specialTileInfo.tilePrefab, anchor);
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
                    MatchTileDragInfo matchTileDragInfo = oldLineup_[i];

                    CheckForMatches(matchTileDragInfo, matches);

                    List<MatchTile> tileLine = _dragDirection == DragDirection.VERTICAL ? GetGridRow(matchTileDragInfo.currentGridPosition.y) : GetGridColumn(matchTileDragInfo.currentGridPosition.x);

                    //check the whole dang perpendicular row or column, in case wilds
                    //if every single tile was wild, well god damn.
                    for (int j = 0; j < tileLine.Count; j++)
                    {
                        //alreadyh checked this position, also this tile has been moved
                        if (tileLine[j].GridPosition == matchTileDragInfo.currentGridPosition)
                            continue;

                        CheckForMatches(tileLine[j], matches);
                    }
                }

                ClearTempGrid();

                if (matches.Count > 0 || isTimeStopped)
                {
                    //set dragged line to new positions
                    for (int i = 0; i < oldLineup_.Count; i++)
                    {
                        MatchTileDragInfo tileInfo = oldLineup_[i];
                        Vector2Int newPosition = tileInfo.currentGridPosition;

                        SetMoveTile(newPosition, tileInfo.tile, false);
                    }
                }
                else
                {
                    oldLineup_.ResetTiles();
                }
                if (!isTimeStopped)
                {

                    IEnumerator allMatchesCoroutine = ProcessAllGridMatches();
                    while (allMatchesCoroutine.MoveNext())
                    {
                        yield return null;
                    }
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

        private void SetMoveTile(Vector2Int newPosition, MatchTile movingTile, bool animate = true)
        {
            if (animate)
            {
                movingTile.OnMove();
            }
            movingTile.GridPosition = newPosition;
            movingTile.transform.localPosition = GridToWorldPosition(newPosition);
            _tileGrid[newPosition.x, newPosition.y] = movingTile;
        }

        private IEnumerator ProcessMatches(List<MatchInfo> matches)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                #region log
                if (Util.IsDebug)
                {
                    string log = "";
                    for (int j = 0; j < matches[i].tilesMatched.Length; j++)
                    {
                        log += $"({matches[i].tilesMatched[j].GridPosition.x}, {matches[i].tilesMatched[j].GridPosition.y}) ";
                    }
                    //Log.Debug($"matched {matches[i].GetMatchCount()} {matches[i].matchType}s!\n{log}");
                }
                #endregion log

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


        public void Award(MatchTileType matchType, int matchCount, int tilesMatched)
        {
            //Debug.LogWarning($"matched {matchCount} {matchType}s!");
            OnMatchAwarded?.Invoke(matchType, matchCount, tilesMatched);
        }


        /// <summary>
        /// when checking against a moved tile, check against its current dragged position, not the tile's original position
        /// </summary>
        private void CheckForMatches(MatchTileDragInfo matchTileDragInfo, List<MatchInfo> matches)
        {
            if (CheckForMatch(matchTileDragInfo.tile, matchTileDragInfo.currentGridPosition, DragDirection.VERTICAL, out MatchInfo matchV))
            {
                if (!matches.Contains(matchV))
                {
                    matches.Add(matchV);
                }
            }

            if (CheckForMatch(matchTileDragInfo.tile, matchTileDragInfo.currentGridPosition, DragDirection.HORIZONTAL, out MatchInfo matchH))
            {
                if (!matches.Contains(matchH))
                {
                    matches.Add(matchH);
                }
            }
        }

        /// <summary>
        /// if the tile isn't moving, go ahead and check its tile position
        /// </summary>
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

        private bool CheckForMatch(MatchTile tile, DragDirection dragDirection, out MatchInfo foundMatch)
        {
            if (tile == null)
            {
                foundMatch = null;
                return false;
            }

            return CheckForMatch(tile, tile.GridPosition, dragDirection, out foundMatch);
        }

        private bool CheckForMatch(MatchTile tile, Vector2Int currentGridPosition, DragDirection dragDirection, out MatchInfo foundMatch)
        {
            if (tile == null)
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

            var tileType = tile.TileType;

            int failsafe = 0;
            while (tilePositionQueue.Count > 0)
            {
                if (Util.FAILSAFE(ref failsafe))
                    break;

                Vector2Int queueTile = tilePositionQueue.Dequeue();
                if (dragDirection == DragDirection.HORIZONTAL)
                {
                    CheckTile(matchedTiles, tilePositionQueue, checkedTilePositions, ref tileType, queueTile + Vector2Int.up);
                    CheckTile(matchedTiles, tilePositionQueue, checkedTilePositions, ref tileType, queueTile + Vector2Int.down);
                }
                else
                {
                    CheckTile(matchedTiles, tilePositionQueue, checkedTilePositions, ref tileType, queueTile + Vector2Int.left);
                    CheckTile(matchedTiles, tilePositionQueue, checkedTilePositions, ref tileType, queueTile + Vector2Int.right);
                }
            }

            return matchedTiles;
        }

        private void CheckTile(List<MatchTile> matchedTiles, Queue<Vector2Int> tileCheckQueue, List<Vector2Int> checkedTilePositions, ref MatchTileType tile, Vector2Int checkTilePosition)
        {
            if (!checkedTilePositions.Contains(checkTilePosition) &&
                checkTilePosition.x >= 0 &&
                checkTilePosition.x < gridSize.x &&
                checkTilePosition.y >= 0 &&
                checkTilePosition.y < gridSize.y)
            {
                MatchTile lookTile = GetGridTileToCheck(checkTilePosition);
                if (lookTile != null && lookTile.CheckAgainstThisTileType(tile))
                {
                    matchedTiles.Add(lookTile);
                    checkedTilePositions.Add(checkTilePosition);
                    tileCheckQueue.Enqueue(checkTilePosition);
                }
            }
        }

        private MatchTile GetGridTileToCheck(Vector2Int coordinates) => GetGridTileToCheck(coordinates.x, coordinates.y);
        private MatchTile GetGridTileToCheck(int x, int y)
        {
            if (_tempTileGrid[x, y] != null)
            {
                return _tempTileGrid[x, y];
            }
            return _tileGrid[x, y];
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

            if (!SetSelectedPositionLineups())
            {
                CancelDragging();
            }
        }

        private bool SetSelectedPositionLineups()
        {
            _selectedDragRow = new TileDragLineup(this, _ghostTileHead, _ghostTileTail);

            List<MatchTile> matchTilesRow = GetGridRow(_selectedGridPosition.y);
            for (int i = 0; i < matchTilesRow.Count; i++)
            {
                if (matchTilesRow[i] == null)
                {
                    return false;
                }
            }
            _selectedDragRow.AddRange(matchTilesRow, this);

            _selectedDragCol = new TileDragLineup(this, _ghostTileHead, _ghostTileTail);

            List<MatchTile> matchTilesColumn = GetGridColumn(_selectedGridPosition.x);
            for (int i = 0; i < matchTilesColumn.Count; i++)
            {
                if (matchTilesColumn[i] == null)
                {
                    return false;
                }
            }
            _selectedDragCol.AddRange(matchTilesColumn, this);
            return true;
        }

        private List<MatchTile> GetGridRow(int y)
        {
            List<MatchTile> tilesH = new List<MatchTile>();
            for (int x = 0; x < _tileGrid.GetLength(0); x++)
            {
                tilesH.Add(_tileGrid[x, y]);
            }

            return tilesH;
        }
        private List<MatchTile> GetGridColumn(int x)
        {
            List<MatchTile> tilesH = new List<MatchTile>();
            for (int y = 0; y < _tileGrid.GetLength(1); y++)
            {
                tilesH.Add(_tileGrid[x, y]);
            }

            return tilesH;
        }

        public void TilePointerUp(MatchTile tile)
        {
            CancelDragging();

            if (_manualCoroutine == null && !isTimeStopped)
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
                inputDeltaPosition.x *= 1920f / Screen.width;

                int gridDistance = Mathf.RoundToInt((inputDeltaPosition.x /*- tileDistance.x / 2 * Mathf.Sign(movingDeltaPosition.x)*/) / tileDistance.x);
                gridDelta = new Vector2Int(gridDistance, 0);
            }
            else
            {
                curentDragLineup = _selectedDragCol;
                inputDeltaPosition.x = 0;
                inputDeltaPosition.y *= 1080f / Screen.height;

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

        //killing SOC to save some time I apologize
        public void StopTime()
        {
            _timeStoppedTime = 3;
            timeStopAction?.Invoke(3);
        }

        #endregion tile event trigger callbacks
    }
}