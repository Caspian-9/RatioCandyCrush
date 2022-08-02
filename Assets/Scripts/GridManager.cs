using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GridManager : MonoBehaviour
{
    //public List<Sprite> Sprites = new List<Sprite>();

    private Canvas canvas;

    public TileFactory factory;

    public GameObject GridContainer;

    public Inventory inventory;

    public InfoPrompt infoPrompt;
    public GameObject EndPrompt;

    public GameObject SlotPrefab;

    //public int GridDimension;
    //public float Distance;
    private int GridDimension = 6;
    private Vector2 positionOffset;

    public int[][] Values;

    // todo: PROBABLY DEPRECATED
    private Dictionary<int[], List<int[]>> Multiples = new Dictionary<int[], List<int[]>>();

    private GameObject[,] SlotGrid;
    public GameObject[,] TileGrid;
    //private bool[] isBlockHere;

    private TreasureCalculator tCalculator;
    public CollectibleTypes treasureType = CollectibleTypes.GEM;

    //public int score = 0;
    public TextMeshProUGUI GemText;

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
        
    }


    // stuff to do at the start of a GAME

    public void NewGame()
    {
        infoPrompt.iPrompt.SetActive(false);

        Values = new int[GridDimension * GridDimension][];
        SlotGrid = new GameObject[GridDimension, GridDimension];
        TileGrid = new GameObject[GridDimension, GridDimension];

        canvas = GetComponentInChildren<Canvas>();

        // todo: make it maybe not gem when more types start existing
        tCalculator = new TreasureCalculator(Items.ItemsToCollect[CollectibleTypes.GEM]);
        stopwatch = stopwatchText.GetComponent<Stopwatch>();
        EndPrompt.SetActive(false);


        RectTransform rt = GridContainer.transform.GetComponent<RectTransform>();
        float edgelength = 0.8f * (2 * Camera.main.orthographicSize);
        rt.sizeDelta = new Vector2(edgelength, edgelength);
        rt.position = new Vector3(0, 0, 0);
        // positionOffset = new Vector2( -(edgelength / 2), -(edgelength / 2)) * canvas.GetComponent<RectTransform>().localScale.x;
        positionOffset = new Vector2(-(edgelength / 2), -(edgelength / 2));
        

        //isBlockHere = PossibleBlockPositions(3);

        GemText.text = "Gems: 0/" + tCalculator.totalTreasures.ToString();

        InitValues();
        InitGrid();

        stopwatch.setTime(0f);
        stopwatch.StartStopwatch();
    }


    void InitValues()
    {

        List<int[]> baseVals = new List<int[]>();

        // generate base fractions
        do {
            int n = rand.Next(1, 3);
            int d = rand.Next(1, 7);

            int[] fraction = new int[2] { n, d };
            if (!baseVals.Contains(fraction) && (FractionToFloat(fraction, RoundDigits) < 1f))
            {
                baseVals.Add(fraction);
                Multiples[fraction] = new List<int[]>();
            }
                

        } while (baseVals.Count < GridDimension);

        // generate the grid
        for (int i = 0; i < (int)Math.Pow(GridDimension, 2); i++)
        {

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
                newSlot.transform.localScale *= 4f / GridDimension;

                Slot slot = newSlot.GetComponent<Slot>();
                slot.setGridManager(this);

                slot.GridIndices = new Vector2Int(column, row);


                if (tCalculator.isTreasureHere()) {
                    InitCollectibleInGrid(treasureType, column, row, Values[row * GridDimension + column]);
                    tCalculator.incrementPlaced();
                    //Debug.Log("***************");
                    //Debug.Log(tCalculator.getCollected());
                    //Debug.Log(tCalculator.getPlaced());
                    //Debug.Log(tCalculator.getTotal());
                }
                else {
                    InitTileInGrid(getRandomType(), column, row, Values[row * GridDimension + column]);
                }


                // newSlot.transform.position = new Vector3(column * Distance, row * Distance, 0);
                newSlot.transform.position = GetXYfromColRow(column, row) + positionOffset;

                SlotGrid[column, row] = newSlot;
            }
        }

    }


    private void InitTileInGrid(TileTypes type, int column, int row, int[] fraction)
    {
        GameObject newTile = factory.InstantiateTile(type);
        Tile tile = newTile.GetComponent<Tile>();
        //DragDrop dragDrop = newTile.GetComponent<DragDrop>();
        SwapBehaviour swapBehaviour = newTile.GetComponent<SwapBehaviour>();

        newTile.transform.SetParent(GridContainer.transform);
        newTile.transform.position = GetXYfromColRow(column, row) + positionOffset;
        newTile.transform.localScale *= 4f / GridDimension;

        TileGrid[column, row] = newTile;

        swapBehaviour.Init();
        swapBehaviour.setGridManager(this);
        swapBehaviour.GridIndices = new Vector2Int(column, row);
        swapBehaviour.SetClickable(true);

        float v = FractionToFloat(fraction, RoundDigits);
        tile.SetFraction(fraction);
        tile.SetValue(v);

        tile.SetText();

        //dragDrop.Lock();

    }

    private void InitCollectibleInGrid(CollectibleTypes type, int column, int row, int[] fraction) {

        GameObject newTile = factory.InstantiateCollectible(type);
        Tile tile = newTile.GetComponent<Tile>();
        SwapBehaviour swapBehaviour = newTile.GetComponent<SwapBehaviour>();

        newTile.transform.SetParent(GridContainer.transform);
        newTile.transform.position = GetXYfromColRow(column, row) + positionOffset;
        newTile.transform.localScale *= 4f / GridDimension;

        TileGrid[column, row] = newTile;

        swapBehaviour.Init();
        swapBehaviour.setGridManager(this);
        swapBehaviour.GridIndices = new Vector2Int(column, row);
        swapBehaviour.SetClickable(true);

        float v = FractionToFloat(fraction, RoundDigits);
        tile.SetFraction(fraction);
        tile.SetValue(v);

        tile.SetText();
    }


    // stuff to do at the end of a GAME

    private void EndGame()
    {
        stopwatch.StopStopwatch();
        DisableGrid();
        EndPrompt.SetActive(true);

        //score += CalculateTimeBonus(stopwatch.getTime());
        //ScoreText.text = "Score: " + score.ToString();
        //GemText.text = "Gems: " + treasuresCollected.ToString() + "/" + totalTreasures.ToString();

        //Debug.Log("level cleared");
    }

    private void DisableGrid()
    {
        foreach (GameObject tile in TileGrid)
        {
            tile.GetComponent<SwapBehaviour>().SetClickable(false);
        }
    }

    public void ReturnToLobby() {
        // 0 = 3d platformer scene; 1 = level scene
        SceneManager.LoadScene(0);
    }



    // stuff to do at the end of a TURN

    public void EndTurn()
    {
        //score += CalculateScore(CheckMatches());
        int matches = CheckMatches();
        if (matches >= 3) {
            tCalculator.calculateChance(matches);
            //Debug.Log(tCalculator.getChance());
        }
        //ScoreText.text = "Score: " + score.ToString();
        //GemText.text = "Gems: " + tCalculator.getCollected().ToString() + "/" + tCalculator.getTotal().ToString();

        if (tCalculator.isAllCollected())
        {
            EndGame();
        }
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

    
    // todo: do i still need this?
    private int CalculateTimeBonus(float timeElapsed)
    {
        return (int)(20 / Math.Sqrt(timeElapsed));
    }


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

                GameObject tile = TileGrid[col, row];

                if (tile != null)
                {
                    tile.GetComponent<SwapBehaviour>().Select();

                    //if (tile.GetComponent<Collectible>() != null  && tile.GetComponent<Collectible>().GetType() == CollectibleTypes.GEM) {
                    if (tile.GetComponent<Collectible>() != null) {

                        tCalculator.incrementCollected();
                        inventory.AddItem(tile.GetComponent<Collectible>().GetType());
                        GemText.text = "Gems: " + tCalculator.treasuresCollected.ToString() + "/" + tCalculator.totalTreasures.ToString();

                        if (tCalculator.isAllCollected()) {
                            EndGame();
                        }

                    }
                    
                }

                Destroy(tile, 1f);
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

                        //Debug.Log(column.ToString() + "," + filler.ToString() + ": " + next);

                        next.transform.position = GetXYfromColRow(column, filler) + positionOffset;
                        next.GetComponent<SwapBehaviour>().GridIndices = new Vector2Int(column, filler);
                        
                    }

                    if (tCalculator.isTreasureHere()) {
                        InitCollectibleInGrid(treasureType, column, GridDimension - 1, Values[rand.Next(0, Values.Length)]);
                        tCalculator.incrementPlaced();
                    }
                    else {
                        InitTileInGrid(getRandomType(), column, GridDimension - 1, Values[rand.Next(0, Values.Length)]);
                    }
                }
            }
        }
    }





    // helpers

    private GameObject GetGameObjectAt(int column, int row)
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
        int enumCountNoTreasure = Enum.GetNames(typeof(TileTypes)).Length;
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











    // old stuff no longer used



    //public void AddTileToGrid(GameObject obj, Vector2Int indices)
    //{
    //    SwapBehaviour swapBehaviour = obj.GetComponent<SwapBehaviour>();
    //    swapBehaviour.SetClickable(true);
    //    swapBehaviour.GridIndices = indices;
    //    TileGrid[indices.x, indices.y] = obj;
    //}



    // placeholder to test treasure collection
    //bool[] PossibleBlockPositions(int NumOfTreasures)
    //{
    //    //tCalculator.setTotal(NumOfTreasures);

    //    bool[] blockPositions = new bool[GridDimension * GridDimension];

    //    // treasures
    //    for (int i = 0; i < NumOfTreasures; i++)
    //    {
    //        blockPositions[i] = true;
    //    }

    //    System.Random rand = new System.Random();
    //    blockPositions = blockPositions.OrderBy(x => rand.Next()).ToArray();
    //    return blockPositions;
    //}



}
