using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Board: MonoBehaviour
{
    [SerializeField] private Transform _cellHolder;
    [SerializeField] private Transform _itemHolder;
    [SerializeField] private GameObject _cellPrefab;

    private float BoardFrameHeightOffset = 0.2f;
    private float BoardFrameWidthOffset = 0.15f;
    private float padding  = 0.01f;
    private Vector3 animationDestination = new(0,-1.38f,0);
    private float animationTime = 1f;
    private SpriteRenderer _borderSpriteRenderer;
    private IMatchManager _matchManager;
    private MoveManager _moveManager;
    private FallManager _fallManager;
    private FallAnimationManager _animationManager;
    private HashSet<Vector2Int> largeMatchItems = new HashSet<Vector2Int>();

    public Cell[,] Grid  { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Transform ItemHolder { get => _itemHolder; }


    void Awake()
    {
        _borderSpriteRenderer = GetComponent<SpriteRenderer>();
        _animationManager = GetComponent<FallAnimationManager>();
        gameObject.transform.DOMove(animationDestination,animationTime).SetEase(Ease.InOutQuad);
    }
    public void Initialize(int width, int height, MoveManager moveManager,ItemFactory factory)
    {
        _matchManager = new MatchManager(GetCell,CheckNeighbours);
        _fallManager = new FallManager(this,factory,_animationManager);
        _moveManager = moveManager;
        InitializeBoard(width,height);
    }
    public bool CheckPositionIsValid(int x,int y) => x >= 0 && x<Width && y >= 0 && y <Height;

    public Cell GetCell(Vector2Int index)
    {
        if(CheckPositionIsValid(index.x,index.y)) {
            return Grid[index.x,index.y];
        }
        return null;
    }
    private void InitializeBoard(int width, int height)
    {
        Width = width;
        Height = height;

        float availableScreenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2 * 0.9f;
        float availableScreenHeight = Camera.main.orthographicSize * 2 * 0.65f;

        float totalPaddingX = padding * (Width - 1);
        float totalPaddingY = padding * (Height - 1);

        float availableWidth = availableScreenWidth - totalPaddingX;
        float availableHeight = availableScreenHeight - totalPaddingY;

        float cellSize = Mathf.Min(availableWidth / Width, availableHeight / Height);

        float boardWidth = cellSize * Width + totalPaddingX + 2 * BoardFrameWidthOffset;
        float boardHeight = cellSize * Height + totalPaddingY + 2 * BoardFrameHeightOffset;

        AdjustBoardSprite(boardWidth, boardHeight);

        Grid = new Cell[width, height];

        InitializeBoardWithCells(cellSize);



        void InitializeBoardWithCells(float cellSize) {
            Vector2Int cellIndex = new Vector2Int();

            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {

                    float localXPosition = (-boardWidth / 2) + BoardFrameWidthOffset + (j * (cellSize + padding)) + (cellSize / 2);
                    float localYPosition = (-boardHeight / 2) + BoardFrameHeightOffset + (i * (cellSize + padding)) + (cellSize / 2);

                    Vector3 worldPosition = transform.TransformPoint(new Vector3(localXPosition, localYPosition, 0));

                    var go = Instantiate(_cellPrefab, worldPosition, Quaternion.identity);
                    var cell = go.GetComponent<Cell>();

                    if (cell == null) {
                        Debug.LogWarning("Cell script is null");
                    }

                    go.transform.SetParent(_cellHolder, true);

                    var spriteRenderer = go.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null) {
                        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
                        float scale = cellSize / Mathf.Min(spriteSize.x, spriteSize.y);
                        go.transform.localScale = new Vector3(scale, scale, 1);

                    }
                    cellIndex.x = j;
                    cellIndex.y = i;
                    cell.Initialize(cellIndex,
                                    _matchManager.GetMatchGroup,
                                    _moveManager.CanMakeMove,
                                    AfterExplode);

                    Grid[j, i] = cell;
                }
            }
        }
    }
    private void AdjustBoardSprite(float boardWidth, float boardHeight)
    {
        if (_borderSpriteRenderer != null) {
            _borderSpriteRenderer.size = new Vector2(boardWidth, boardHeight);
        } else {
            Debug.LogWarning("Board sprite renderer is missing!");
        }
    }
    public void FixSpriteOrders() {
        int initialOrder = 400;

        for(int i = 0 ; i<Height ; i++) {
            for(int j = 0 ; j<Width  ; j++) {
                ItemBase item = Grid[j,i].GetItem();
                if(item == null) {
                    return;
                }

                item.GetComponent<SpriteRenderer>().sortingOrder = initialOrder;
            }
            initialOrder++;
        }
    }
    public void AfterExplode()
    {
        _moveManager.MakeMove();
        _fallManager.ProcessFalling();
        CheckAndChangeSpritesForLargeMatches();
        FixSpriteOrders();

    }


    public List<Cell> CheckNeighbours(Vector2Int index)
    {
        List<Cell> neighbours = new List<Cell>();
        CheckNeighbour(index.x+1,index.y);
        CheckNeighbour(index.x-1,index.y);
        CheckNeighbour(index.x,index.y+1);
        CheckNeighbour(index.x,index.y-1);

        return neighbours;

            void CheckNeighbour(int x, int y) {
                if(CheckPositionIsValid(x,y)) {
                    Cell neighbor =  Grid[x,y];
                    if(neighbor.IsItemPresent()) {
                        neighbours.Add(neighbor);
                }
            }
        }
    }

    public void AddToBoard(ItemBase item,int index)
    {
            Vector2Int position = CalculateIndex(index);
            Cell cell = Grid[position.x,position.y];
            if(cell == null) { Debug.LogWarning("Cell is null"); return; }
            cell.SetItem(item);
            return;
    }
    public Vector2Int CalculateIndex(int index)
    {
        Vector2Int indexAtBoard = new Vector2Int();
        indexAtBoard.x = index %  Width;
        indexAtBoard.y = index / Width;
        return indexAtBoard;
    }
    public List<Cell> GetCellsInRange(int range, Vector2Int index)
    {
        List<Cell> cellsInRange = new List<Cell>();
        int halfRange = range / 2;

        for (int x = index.x - halfRange; x <= index.x + halfRange; x++) {
            for (int y = index.y - halfRange; y <= index.y + halfRange; y++) {
                if (CheckPositionIsValid(x, y) && Grid[x, y].IsItemPresent()  && !CellVisitTracker.HasVisited(new Vector2Int(x,y))) {
                        cellsInRange.Add(Grid[x, y]);
                        CellVisitTracker.AddAsVisited(new Vector2Int(x,y),Grid[x,y]);
                }
            }
        }
        return cellsInRange;
    }

 public void CheckAndChangeSpritesForLargeMatches()
    {
        HashSet<Vector2Int> currentLargeMatchItems = new HashSet<Vector2Int>();
        HashSet<Vector2Int> processedCells = new HashSet<Vector2Int>();

        foreach (Vector2Int index in largeMatchItems)
        {
            var cell = Grid[index.x, index.y];
            var item = cell?.GetItem() as SolidColorItem;
            if (item != null)
            {
                item.ChangeSpriteToNormal();
            }
        }
        CellVisitTracker.Reset();

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Vector2Int cellIndex = new Vector2Int(j, i);
                if (processedCells.Contains(cellIndex)) continue;

                var cell = Grid[j, i];
                if (cell == null || cell.GetItem() == null) continue;

                CellVisitTracker.Reset();

                List<Cell> matchGroup = _matchManager.GetMatchGroup(cellIndex);

                if (matchGroup.Count < 5)
                {
                    foreach (var matchCell in matchGroup)
                    {
                        var matchItem = matchCell.GetItem() as SolidColorItem;
                        if (matchItem != null)
                        {
                            matchItem.ChangeSpriteToNormal();
                        }
                    }
                }
                else
                {
                    foreach (var matchCell in matchGroup)
                    {
                        var matchItem = matchCell.GetItem() as SolidColorItem;
                        if (matchItem != null)
                        {
                            matchItem.ChangeSpriteToTnt();
                            currentLargeMatchItems.Add(matchCell.GetIndex());
                        }
                    }
                }

                foreach (var matchCell in matchGroup)
                {
                    processedCells.Add(matchCell.GetIndex());
                }
            }
        }

        largeMatchItems = currentLargeMatchItems;
    }



}




