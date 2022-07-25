using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridManager : MonoBehaviour
{
    //public List<Sprite> Sprites = new List<Sprite>();

    private Canvas canvas;

    public TileFactory factory;

    public GameObject GridContainer;

    public GameObject SlotPrefab;

    //public int GridDimension;
    //public float Distance;
    private int GridDimension = 4;
    private Vector2 positionOffset;

    public int[][] Values;

    // todo: PROBABLY DEPRECATED
    private Dictionary<int[], List<int[]>> Multiples = new Dictionary<int[], List<int[]>>();

    private GameObject[,] SlotGrid;
    public GameObject[,] TileGrid;
    //private int[] isBlockHere;

    private TreasureCalculator treasureCalculator;

    public int score = 0;
    public TextMeshProUGUI ScoreText;

    private Stopwatch stopwatch;
    public TextMeshProUGUI stopwatchText;

    private int RoundDigits = 4;

    private System.Random rand = new System.Random();


    // todo: REMOVE SINGLETON PATTERN
    public static GridManager Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        Instance = this;

        Values = new int[GridDimension * GridDimension][];
        canvas = GetComponentInChildren<Canvas>();
        treasureCalculator = new TreasureCalculator(3);

        RectTransform rt = GridContainer.transform.GetComponent<RectTransform>();
        float edgelength = 0.8f * (2 * Camera.main.orthographicSize);
        rt.sizeDelta = new Vector2(edgelength, edgelength);
        rt.position = new Vector3(0, 0, 0);
        // positionOffset = new Vector2( -(edgelength / 2), -(edgelength / 2)) * canvas.GetComponent<RectTransform>().localScale.x;
        positionOffset = new Vector2(-(edgelength / 2), -(edgelength / 2));

        stopwatch = stopwatchText.GetComponent<Stopwatch>();

        NewGame();
    }


    //public GridManager(int dimension, float distance, int numOfTreasures)
    //{
    //    this.GridDimension = dimension;
    //    this.Distance = distance;
    //    this.numOfTreasures = numOfTreasures;

    //    this.Values = new int[GridDimension * GridDimension][];
    //    this.canvas = GetComponentInChildren<Canvas>();
    //    this.stopwatch = stopwatchText.GetComponent<Stopwatch>();
    //}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // make grid and tile sizes always scale with screen size
    }


    // stuff to do at the start of a GAME

    public void NewGame()
    {
        SlotGrid = new GameObject[GridDimension, GridDimension];
        TileGrid = new GameObject[GridDimension, GridDimension];
        //TreasureLocations = new List<Vector2Int>();

        //isBlockHere = PossibleBlockPositions(4, 3, 3);

        ScoreText.text = "Score: 0";

        InitValues();
        InitGrid();
        NewTurn();

        stopwatch.StartStopwatch();
    }


    void InitValues()
    {

        List<int[]> baseVals = new List<int[]>();

        // generate base fractions
        do {
            //int n = rand.Next(1, 5);
            //int d = rand.Next(1, 11);
            int n = 1;
            int d = rand.Next(1, 7);
            int[] fraction = new int[2] { n, d };
            if (!baseVals.Contains(fraction))
            {
                baseVals.Add(fraction);
                Multiples[fraction] = new List<int[]>();
            }
                

        } while (baseVals.Count < GridDimension);

        // generate the grid
        for (int i = 0; i < (int)Math.Pow(GridDimension, 2); i++)
        {

            // this only applies to dr mario style incomplete grid
            //if (isBlockHere[i] == 0)
            //{
            //    continue;
            //}
            

            List<int[]> possibleVals = new List<int[]>(baseVals);

            // remove unwanted values from possibleVals
            List<int[]> toRemove = new List<int[]>();

            float left = 0f;
            float down = 0f;

            if (i - 1 >= 0 && Values[i - 1] != null)
                left = FractionToFloat(Values[i - 1], RoundDigits);
            if (i - GridDimension >= 0 && Values[i - GridDimension] != null)
                down = FractionToFloat(Values[i - GridDimension], RoundDigits);

            foreach (int[] val in possibleVals)
            {
                if (FractionToFloat(val, RoundDigits) == left || FractionToFloat(val, RoundDigits) == down)
                    toRemove.Add(val);
            }
            possibleVals = possibleVals.Except(toRemove).ToList();

            // add fraction
            int m = rand.Next(1, 4);    // multiplier
            int[] selectedFraction = possibleVals[rand.Next(0, possibleVals.Count)];
            int[] multipliedFraction = new int[2] { selectedFraction[0] * m, selectedFraction[1] * m };
            Values[i] = multipliedFraction;

            Multiples[selectedFraction].Add(multipliedFraction);

        }

    }


    void InitGrid()
    {
        //Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.5f, GridDimension * Distance / 2.5f, 0);

        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                
                GameObject newSlot = Instantiate(SlotPrefab);
                newSlot.transform.SetParent(GridContainer.transform);

                Slot slot = newSlot.GetComponent<Slot>();
                slot.setGridManager(this);

                slot.GridIndices = new Vector2Int(column, row);


                InitTileInGrid(getRandomType(), column, row, Values[row * GridDimension + column]);

                // newSlot.transform.position = new Vector3(column * Distance, row * Distance, 0);
                newSlot.transform.position = GetXYfromColRow(column, row) + positionOffset;

                SlotGrid[column, row] = newSlot;
            }
        }

    }


    //int[] PossibleBlockPositions(int NumOfBlocks, int NumOfPies, int NumOfTreasures)
    //{
    //    int[] blockPositions = new int[GridDimension * GridDimension];

    //    // blocks
    //    for (int i = 0; i < NumOfBlocks; i++)
    //    {
    //        blockPositions[i] = 1;
    //    }
    //    // pies
    //    for (int i = 0; i < NumOfPies; i++)
    //    {
    //        blockPositions[i + NumOfBlocks] = 2;
    //    }
    //    //treasures
    //    for (int i = 0; i < NumOfTreasures; i++)
    //    {
    //        blockPositions[i + NumOfBlocks * 2] = 3;
    //    }

    //    System.Random rand = new System.Random();
    //    blockPositions = blockPositions.OrderBy(x => rand.Next()).ToArray();
    //    return blockPositions;
    //}

    private void InitTileInGrid(TileTypes type, int column, int row, int[] fraction)
    {
        GameObject newTile = factory.InstantiateTile(type);
        Tile tile = newTile.GetComponent<Tile>();
        DragDrop dragDrop = newTile.GetComponent<DragDrop>();
        SwapBehaviour swapBehaviour = newTile.GetComponent<SwapBehaviour>();

        newTile.transform.SetParent(GridContainer.transform);
        newTile.transform.position = GetXYfromColRow(column, row) + positionOffset;
        TileGrid[column, row] = newTile;

        tile.InitTile();
        swapBehaviour.Init();
        swapBehaviour.setGridManager(this);
        swapBehaviour.GridIndices = new Vector2Int(column, row);
        swapBehaviour.SetInGrid(true);

        float v = FractionToFloat(fraction, RoundDigits);
        tile.SetFraction(fraction);
        tile.SetValue(v);

        tile.SetText();

        dragDrop.Lock();

    }


    // TURNS (do not use)

    void NewTurn()
    {
        List<int[]> keyList = new List<int[]>(Multiples.Keys.ToList());
        int[] val = keyList[rand.Next(0, Multiples.Count)];
        //GenerateDraggableBlock(TileTypes.BLOCK, val);
    }

    public void EndTurn()
    {
        score += CalculateScore(CheckMatches());
        ScoreText.text = "Score: " + score.ToString();

        if (CheckFullClear())
        {
            stopwatch.StopStopwatch();
            score += CalculateTimeBonus(stopwatch.getTime());
            ScoreText.text = "Score: " + score.ToString();

            Debug.Log("level cleared");
        }
        else
        {
            NewTurn();
        }

    }


    // stuff to do at the end of a TURN

    public void AddTileToGrid(GameObject obj, Vector2Int indices)
    {
        SwapBehaviour swapBehaviour = obj.GetComponent<SwapBehaviour>();
        swapBehaviour.SetInGrid(true);
        swapBehaviour.GridIndices = indices;
        TileGrid[indices.x, indices.y] = obj;

    }


    public int CalculateScore(int numOfBlocks)
    {
        if (numOfBlocks <= 1)
        {
            return 0;
        }

        int baseNum = 1;
        return (int) Math.Max(0.5f * Math.Pow(numOfBlocks * baseNum, 2), numOfBlocks * baseNum);
    }

    private int CalculateTimeBonus(float timeElapsed)
    {
        return (int)(20 / Math.Sqrt(timeElapsed));
    }


    private bool CheckFullClear()
    {
        for (int i = 0; i < GridDimension; i++)
        {
            for (int j = 0; j < GridDimension; j++)
            {
                if (TileGrid[i, j] == null)
                {
                    continue;
                }
                if (TileGrid[i, j].activeSelf)
                {
                    return false;
                }
            }
        }
        return true;
    }




    public void SwapBlocks(Vector2Int tile1Inds, Vector2Int tile2Inds)
    {
        //Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.5f, GridDimension * Distance / 2.5f, 0);

        GameObject tile1 = TileGrid[tile1Inds.x, tile1Inds.y];
        GameObject tile2 = TileGrid[tile2Inds.x, tile2Inds.y];

        //if (tile1.GetComponent<Treasure>() != null || tile2.GetComponent<Treasure>() != null)    // tile 1 is a treasure
        //{
        //    SwapTreasureLocations(tile1Position, tile2Position);
        //}

        Vector2 pos1 = tile1.transform.position;
        Vector2 pos2 = tile2.transform.position;

        TileGrid[tile1Inds.x, tile1Inds.y] = tile2;
        tile2.transform.position = pos1;
        tile2.transform.SetAsLastSibling();
        tile2.GetComponent<SwapBehaviour>().GridIndices = tile1Inds;

        TileGrid[tile2Inds.x, tile2Inds.y] = tile1;
        tile1.transform.position = pos2;
        tile1.transform.SetAsLastSibling();
        tile1.GetComponent<SwapBehaviour>().GridIndices = tile2Inds;

        //bool changesOccurs = CheckMatches();
        //if (!changesOccurs)
        //{
        //    temp = block2;
        //    Grid[block1Position.x, block1Position.y] = block1;
        //    block1.transform.position = new Vector3(block1Position.x * (int)Distance, block1Position.y * (int)Distance, 0) + positionOffset;
        //    block1.GetComponent<Block>().Position = block1Position;

        //    Grid[block2Position.x, block2Position.y] = temp;
        //    temp.transform.position = new Vector3(block2Position.x * (int)Distance, block2Position.y * (int)Distance, 0) + positionOffset;
        //    temp.GetComponent<Block>().Position = block2Position;
        //}
        score += CalculateScore(CheckMatches());
        ScoreText.text = "Score: " + score.ToString();
    }

    //private void SwapTreasureLocations(Vector2Int pos1, Vector2Int pos2)
    //{
    //    for (int i = 0; i < TreasureLocations.Count; i++)
    //    {
    //        if (TreasureLocations[i] == pos1)
    //        {
    //            TreasureLocations[i] = pos2;
    //        }
    //        else if (TreasureLocations[i] == pos2)
    //        {
    //            TreasureLocations[i] = pos1;
    //        }
    //    }
    //}

    public int CheckMatches()
    {
        //List<GameObject> matchedTiles = new List<GameObject>();
        List<int[]> matchedCoordinates = new List<int[]>();


        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                GameObject current = TileGrid[column, row];
                if (current == null || !current.activeSelf)
                {
                    continue;
                }

                List<int[]> horizontalMatches = FindColumnMatchForTile(column, row, current);
                if (horizontalMatches.Count >= 2)
                {
                    matchedCoordinates.AddRange(horizontalMatches);
                    matchedCoordinates.Add(new int[2] { column, row });
                }

                List<int[]> verticalMatches = FindRowMatchForTile(column, row, current);
                if (verticalMatches.Count >= 2)
                {
                    matchedCoordinates.AddRange(verticalMatches);
                    matchedCoordinates.Add(new int[2] { column, row });
                }
            }
        }

        if (matchedCoordinates.Count > 2)
        {
            for (int i = 0; i < matchedCoordinates.Count; i++)
            {
                int col = matchedCoordinates[i][0];
                int row = matchedCoordinates[i][1];

                if (TileGrid[col, row] != null)
                {
                    TileGrid[col, row].GetComponent<SwapBehaviour>().Select();
                }
                Destroy(TileGrid[col, row], 1f);
                TileGrid[col, row] = null;
            }

            Invoke("FillHoles", 1f);
        }

        return matchedCoordinates.Count;
    }


    List<int[]> FindColumnMatchForTile(int col, int row, GameObject obj)
    {
        //List<GameObject> result = new List<GameObject>();
        List<int[]> result = new List<int[]>();

        float currentValue = obj.GetComponent<Tile>().GetValue();

        for (int i = col + 1; i < GridDimension; i++)
        {
            GameObject nextColumn = GetGameObjectAt(i, row);
            if (nextColumn == null || !nextColumn.activeSelf)
                break;

            float nextColValue = nextColumn.GetComponent<Tile>().GetValue(); ;

            if (nextColValue != currentValue) {
                break;
            } else {
                result.Add(new int[2] {i, row});
            }
            
        }
        return result;
    }

    List<int[]> FindRowMatchForTile(int col, int row, GameObject obj)
    {
        //List<GameObject> result = new List<GameObject>();
        List<int[]> result = new List<int[]>();

        float currentValue = obj.GetComponent<Tile>().GetValue();

        for (int i = row + 1; i < GridDimension; i++)
        {
            GameObject nextRow = GetGameObjectAt(col, i);
            if (nextRow == null || !nextRow.activeSelf)
                break;

            float nextRowValue = nextRow.GetComponent<Tile>().GetValue();

            if (nextRowValue != currentValue) {
                break;
            } else {
                result.Add(new int[2] { col, i });
            }
        }
        return result;
    }


    void FillHoles()
    {
        for (int column = 0; column < GridDimension; column++)
        {
            for (int row = 0; row < GridDimension; row++)
            {
                while (GetGameObjectAt(column, row) == null)
                {
                    for (int filler = row; filler < GridDimension - 1; filler++)
                    {
                        // gameobject at column and row doesn't exist (look at while loop condition)
                        GameObject next = GetGameObjectAt(column, filler + 1);

                        if (next == null)
                            continue;

                        //current.sprite = next.sprite;

                        TileGrid[column, filler] = next;

                        Debug.Log(column.ToString() + "," + filler.ToString() + ": " + next);

                        next.transform.position = GetXYfromColRow(column, filler) + positionOffset;
                        next.GetComponent<SwapBehaviour>().GridIndices = new Vector2Int(column, filler);
                        
                    }
                    InitTileInGrid(TileTypes.BLOCK, column, GridDimension - 1, Values[rand.Next(0, Values.Length)]);
                }
            }
        }
    }





    // general helpers

    GameObject GetGameObjectAt(int column, int row)
    {
        if (column < 0 || column >= GridDimension
             || row < 0 || row >= GridDimension)
            return null;
        return TileGrid[column, row];
    }

    public float FractionToFloat(int[] fraction, int roundDigits)
    {
        int n = fraction[0];
        int d = fraction[1];
        return (float)Math.Round((double)n / d, roundDigits);   // round to this many decimal places
    }

    private TileTypes getRandomType()
    {
        // todo: CURRENTLY HAS NO TREASURE GENERATION
        int enumCountNoTreasure = Enum.GetNames(typeof(TileTypes)).Length - 1;
        TileTypes type = (TileTypes)rand.Next(0, enumCountNoTreasure);
        return type;
    }

    private Vector2 GetXYfromColRow(int column, int row)
    {
        RectTransform rt = GridContainer.transform.GetComponent<RectTransform>();
        //float gridsize = rt.sizeDelta.y * canvas.GetComponent<RectTransform>().localScale.x;
        float gridsize = rt.sizeDelta.y;
        float tilesize = gridsize / GridDimension;

        //Debug.Log("gridsize: " + gridsize + "; tilesize: " + tilesize);

        float x = column * tilesize + (tilesize / 2);
        float y = row * tilesize + (tilesize / 2);

        return new Vector2(x, y);
    }











    // slated for removal


    //private void InitBlockInGrid(int column, int row, Vector3 positionOffset, int[] fraction)
    //{
    //    GameObject newBlock = Instantiate(BlockPrefab);
    //    newBlock.transform.SetParent(canvas.transform);
    //    newBlock.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;
    //    TileGrid[column, row] = newBlock;

    //    Block block = newBlock.GetComponent<Block>();
    //    DragDrop dragDrop = newBlock.GetComponent<DragDrop>();
    //    SwapBehaviour swapBehaviour = newBlock.GetComponent<SwapBehaviour>();

    //    block.InitBlock();
    //    swapBehaviour.Init();
    //    swapBehaviour.Position = new Vector2Int(column, row);
    //    swapBehaviour.SetInGrid(true);

    //    float v = FractionToFloat(fraction, RoundDigits);
    //    block.SetFraction(fraction);
    //    block.SetValue(v);

    //    block.SetText();

    //    dragDrop.Lock();
    //}

    //private void InitPieInGrid(int column, int row, Vector3 positionOffset, int[] fraction)
    //{
    //    GameObject newPie = Instantiate(PiePrefab);
    //    newPie.transform.SetParent(canvas.transform);
    //    newPie.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;
    //    TileGrid[column, row] = newPie;

    //    PieDiagram pie = newPie.GetComponent<PieDiagram>();
    //    DragDrop dragDrop = newPie.GetComponent<DragDrop>();
    //    SwapBehaviour swapBehaviour = newPie.GetComponent<SwapBehaviour>();

    //    pie.InitPie();
    //    swapBehaviour.Init();
    //    swapBehaviour.Position = new Vector2Int(column, row);
    //    swapBehaviour.SetInGrid(true);

    //    float v = FractionToFloat(fraction, RoundDigits);
    //    pie.SetValue(v);

    //    dragDrop.Lock();
    //}

    //private void InitTreasureInGrid(int column, int row, Vector3 positionOffset, int[] fraction)
    //{
    //    GameObject newTreasure = Instantiate(TreasurePrefab);
    //    newTreasure.transform.SetParent(canvas.transform);
    //    newTreasure.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;
    //    TileGrid[column, row] = newTreasure;

    //    Treasure treasure = newTreasure.GetComponent<Treasure>();
    //    //DragDrop dragDrop = newTreasure.GetComponent<DragDrop>();
    //    SwapBehaviour swapBehaviour = newTreasure.GetComponent<SwapBehaviour>();

    //    treasure.InitTreasure();
    //    swapBehaviour.Init();
    //    swapBehaviour.Position = new Vector2Int(column, row);
    //    swapBehaviour.SetInGrid(true);

    //    float v = FractionToFloat(fraction, RoundDigits);
    //    treasure.SetFraction(fraction);
    //    treasure.SetValue(v);

    //    treasure.SetText();

    //    //dragDrop.Lock();
    //}


    //void GenerateDraggableBlock(TileTypes type, int[] fraction)
    //{

    //    // GameObject newBlock = Instantiate(BlockPrefab);
    //    GameObject newBlock = factory.InstantiateTile(type);

    //    Block block = newBlock.GetComponent<Block>();
    //    DragDrop dragDrop = newBlock.GetComponent<DragDrop>();
    //    SwapBehaviour swapBehaviour = newBlock.GetComponent<SwapBehaviour>();

    //    block.InitTile();
    //    swapBehaviour.Init();

    //    newBlock.transform.SetParent(canvas.transform);
    //    newBlock.transform.position = new Vector3(-7, -3, 0);
    //    swapBehaviour.SetInGrid(false);

    //    float v = FractionToFloat(fraction, RoundDigits);
    //    block.SetFraction(fraction);
    //    block.SetValue(v);

    //    block.SetText();

    //    dragDrop.Unlock();
    //}










    // old code from CandyCrushTM era do not use pl0x



    //public bool CheckMatchesOld()
    //{
    //    HashSet<GameObject> matchedTiles = new HashSet<GameObject>();
    //    for (int row = 0; row < GridDimension; row++)
    //    {
    //        for (int column = 0; column < GridDimension; column++)
    //        {
    //            GameObject current = Grid[column, row];

    //            List<GameObject> horizontalMatches = FindColumnMatchForTile(column, row, current);
    //            if (horizontalMatches.Count >= 1)
    //            {
    //                matchedTiles.UnionWith(horizontalMatches);
    //                matchedTiles.Add(current);
    //            }

    //            List<GameObject> verticalMatches = FindRowMatchForTile(column, row, current);
    //            if (verticalMatches.Count >= 1)
    //            {
    //                matchedTiles.UnionWith(verticalMatches);
    //                matchedTiles.Add(current);
    //            }
    //        }
    //    }

    //    foreach (GameObject obj in matchedTiles)
    //    {
    //        obj.SetActive(false);
    //    }
    //    return matchedTiles.Count > 0;
    //}

    //double GetInternalValueAt(int column, int row)
    //{
    //    if (column < 0 || column >= GridDimension
    //         || row < 0 || row >= GridDimension)
    //        return 0;
    //    GameObject tile = Grid[column, row];
    //    double value = tile.GetComponent<Tile>().internalValue;
    //    return value;
    //}


}
