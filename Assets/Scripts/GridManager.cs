using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GridManager : MonoBehaviour
{

    public Canvas canvas;

    private LevelData data;

    public TileFactory factory;

    public GameObject GridContainer;

    public Inventory inventory;

    public StatusMessage statusMessage;

    public InfoPrompt infoPrompt;
    public GameObject EndPrompt;

    public GameObject SlotPrefab;

    public int GridDimension;
    private Vector2 positionOffset;

    public List<Vector2Int> Values;

    private GameObject[,] SlotGrid;
    public GameObject[,] TileGrid;
    public Vector2Int[] TempValueGrid;

    private TreasureCalculator tCalculator;
    public CollectibleTypes treasureType;
    
    public TextMeshProUGUI GemText;

    private int RoundDigits = 4;

    private System.Random rand = new System.Random();



    public static GridManager Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        Instance = this;
        

        //Debug.Log(AllLevelsData.level);
        //Debug.Log(data);
        //Debug.Log(data.Dimension);
        //Debug.Log(AllLevelsData.tutorialGrid);
        //Debug.Log(data.CustomGrid);

        
        //Values = data.CustomGrid;

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

    public void NextLevel()
    {
        AllLevelsData.level += 1;
        EndPrompt.SetActive(false);
        NewGame();
    }


    // stuff to do at the start of a GAME

    public void NewGame()
    {
        data = AllLevelsData.Data[AllLevelsData.level];
        GridDimension = data.Dimension;

        infoPrompt.iPrompt.SetActive(false);

        statusMessage.gameObject.SetActive(true);
        statusMessage.SetText("");

        SlotGrid = new GameObject[GridDimension, GridDimension];
        TileGrid = new GameObject[GridDimension, GridDimension];

        //canvas = GetComponentInChildren<Canvas>();

        //tCalculator = new TreasureCalculator(Items.ItemsToCollect[CollectibleTypes.GEM]);
        tCalculator = new TreasureCalculator(data.NumCollectible);
        //stopwatch = stopwatchText.GetComponent<Stopwatch>();
        EndPrompt.SetActive(false);

        treasureType = data.AssignedType;
        inventory.UpdateInventory();

        RectTransform rt = GridContainer.transform.GetComponent<RectTransform>();
        
        float edgelength = 1f * (canvas.GetComponent<RectTransform>().rect.height);

        Debug.Log(canvas.GetComponent<RectTransform>().rect.height + "  ----  " + edgelength);
        Debug.Log(canvas.GetComponent<RectTransform>().sizeDelta.y);
        //Debug.Log(2 * Camera.main.orthographicSize);

        rt.sizeDelta = new Vector2(edgelength, edgelength);
        //rt.localPosition = new Vector3(0, (0.08f * canvas.GetComponent<RectTransform>().rect.height), 0);
        rt.localPosition = new Vector3(0, 0, 0);

        positionOffset = new Vector2(-(edgelength / 2), -(edgelength / 2));

        //float edgelength = 0.72f * (2 * Camera.main.orthographicSize);
        //rt.localPosition = new Vector3(0, 0.16f * Camera.main.orthographicSize, 0);
        // positionOffset = new Vector2( -(edgelength / 2), -(edgelength / 2)) * canvas.GetComponent<RectTransform>().localScale.x;


        //isBlockHere = PossibleBlockPositions(3);

        GemText.text = "Treasures: 0/" + tCalculator.totalTreasures.ToString();

        //Debug.Log(AllLevelsData.level);
        //Debug.Log(data);
        //Debug.Log(data.CustomGrid);
        //if (data.CustomGrid.Count == 0)
        //{
        //    InitGridValues();
        //}

        Values = new List<Vector2Int>();
        TempValueGrid = new Vector2Int[GridDimension * GridDimension];

        if (AllLevelsData.level == 0)
        {
            Values = AllLevelsData.tutorialGrid;
            TempValueGrid = AllLevelsData.tutorialGrid.ToArray();

            statusMessage.SetText(AllLevelsData.tutorialMessages[0]);
        }
        else if (AllLevelsData.level == 1)
        {
            InitGridValues(AllLevelsData.lv1Base);
        }
        else if (AllLevelsData.level == 2)
        {
            InitGridValues(AllLevelsData.lv2Base);
        }
        else if (AllLevelsData.level == 3)
        {
            InitGridValues(AllLevelsData.lv3Base);
        }

        InitGrid();

        //stopwatch.setTime(0f);
        //stopwatch.StartStopwatch();
    }


    void InitGridValues(List<Vector2Int> baseFractions)
    {

        // generate the grid
        for (int row = 0; row < GridDimension; row++)
        {
            for (int col = 0; col < GridDimension; col++)
            {
                //List<Vector2Int> possibleVals = new List<Vector2Int>(data.BaseFractions);
                List<Vector2Int> possibleVals = new List<Vector2Int>(baseFractions);

                // remove unwanted values from possibleVals
                List<Vector2Int> toRemove = new List<Vector2Int>();

                float left = 0f;
                float down = 0f;

                int i = row * GridDimension + col;

                if (i - 1 >= 0 && Values[i - 1] != null)
                    left = FractionToFloat(Values[i - 1], RoundDigits);
                if (i - GridDimension >= 0 && Values[i - GridDimension] != null)
                    down = FractionToFloat(Values[i - GridDimension], RoundDigits);

                foreach (Vector2Int val in possibleVals)
                {
                    if (FractionToFloat(val, RoundDigits) == left || FractionToFloat(val, RoundDigits) == down)
                        toRemove.Add(val);
                }
                possibleVals = possibleVals.Except(toRemove).ToList();

                // add fraction
                int m = rand.Next(1, 4);    // multiplier
                Vector2Int selectedFraction = possibleVals[rand.Next(0, possibleVals.Count)];
                Vector2Int multipliedFraction = new Vector2Int(selectedFraction[0] * m, selectedFraction[1] * m);
                Values.Add(multipliedFraction);
                TempValueGrid[i] = multipliedFraction;

                //Multiples[selectedFraction].Add(multipliedFraction);
            }

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

                RectTransform rt = GridContainer.transform.GetComponent<RectTransform>();
                float scale = canvas.GetComponent<RectTransform>().localScale.x * rt.sizeDelta.y / (GridDimension * 92);
                newSlot.transform.localScale *= scale;

                //newSlot.transform.localScale = canvas.GetComponent<RectTransform>().localScale;
                //newSlot.transform.localScale *= 4f * canvas.GetComponent<RectTransform>().localScale.x / GridDimension;
                newSlot.transform.localPosition = GetXYfromColRow(column, row) + positionOffset;

                Slot slot = newSlot.GetComponent<Slot>();
                slot.setGridManager(this);

                slot.GridIndices = new Vector2Int(column, row);
                SlotGrid[column, row] = newSlot;


                if (Values[row * GridDimension + column] == Vector2Int.zero)
                {
                    TileGrid[column, row] = null;
                    continue;
                }


                if (tCalculator.isTreasureHere())
                {
                    InitCollectibleInGrid(treasureType, column, row, Values[row * GridDimension + column]);
                    tCalculator.incrementPlaced();
                    //Debug.Log("***************");
                    //Debug.Log(tCalculator.getCollected());
                    //Debug.Log(tCalculator.getPlaced());
                    //Debug.Log(tCalculator.getTotal());
                }
                else
                {
                    InitTileInGrid(getRandomType(), column, row, Values[row * GridDimension + column]);
                }
            }
        }
    }


    private void InitTileInGrid(TileTypes type, int column, int row, Vector2Int fraction)
    {
        GameObject newTile = factory.InstantiateTile(type);
        Tile tile = newTile.GetComponent<Tile>();
        //DragDrop dragDrop = newTile.GetComponent<DragDrop>();
        SwapBehaviour swapBehaviour = newTile.GetComponent<SwapBehaviour>();

        newTile.transform.SetParent(GridContainer.transform);
        newTile.transform.localPosition = GetXYfromColRow(column, row) + positionOffset;

        RectTransform rt = GridContainer.transform.GetComponent<RectTransform>();
        float scale = canvas.GetComponent<RectTransform>().localScale.x * rt.sizeDelta.y / (GridDimension * 92);
        newTile.transform.localScale *= scale;

        //newTile.transform.localScale *= 4f * canvas.GetComponent<RectTransform>().localScale.x / GridDimension;
        //newTile.transform.localScale *= (4f / GridDimension);

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

    private void InitCollectibleInGrid(CollectibleTypes type, int column, int row, Vector2Int fraction) {

        GameObject newTile = factory.InstantiateCollectible(type);
        Tile tile = newTile.GetComponent<Tile>();
        SwapBehaviour swapBehaviour = newTile.GetComponent<SwapBehaviour>();

        newTile.transform.SetParent(GridContainer.transform);
        newTile.transform.localPosition = GetXYfromColRow(column, row) + positionOffset;

        RectTransform rt = GridContainer.transform.GetComponent<RectTransform>();
        float scale = canvas.GetComponent<RectTransform>().localScale.x * rt.sizeDelta.y / (GridDimension * 92);
        newTile.transform.localScale *= scale;

        //newTile.transform.localScale *= 4f * canvas.GetComponent<RectTransform>().localScale.x / GridDimension;
        //newTile.transform.localScale *= 4f / GridDimension;

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
        //stopwatch.StopStopwatch();
        DisableGrid();
        
        EndPrompt.SetActive(true);
        if (AllLevelsData.level + 1 >= AllLevelsData.Data.Count)
        {
            EndPrompt.transform.Find("NextButton").gameObject.SetActive(false);
        }

        //score += CalculateTimeBonus(stopwatch.getTime());
        //ScoreText.text = "Score: " + score.ToString();
        //GemText.text = "Gems: " + treasuresCollected.ToString() + "/" + totalTreasures.ToString();

        //Debug.Log("level cleared");
    }

    private void DisableGrid()
    {
        foreach (GameObject tile in TileGrid)
        {
            if (tile == null)
                continue;
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
        List<Vector2Int> matches = CheckMatches();
        if (matches.Count >= 3)
        {
            ClearMatchedTiles(matches);
            tCalculator.calculateChance(matches.Count);
            //Debug.Log(tCalculator.getChance());
        }

        //ScoreText.text = "Score: " + score.ToString();
        GemText.text = "Treasures: " + tCalculator.getCollected().ToString() + "/" + tCalculator.getTotal().ToString();

        if (tCalculator.isAllCollected())
        {
            EndGame();
        }
    }


    public List<Vector2Int> CheckMatches()
    {
        //List<GameObject> matchedTiles = new List<GameObject>();
        List<Vector2Int> matchedCoordinates = new List<Vector2Int>();


        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                GameObject current = TileGrid[column, row];
                if (current == null || !current.activeSelf)
                {
                    continue;
                }

                List<Vector2Int> horizontalMatches = FindColumnMatchForTile(column, row, current);
                if (horizontalMatches.Count >= 2)
                {
                    matchedCoordinates.AddRange(horizontalMatches);
                    matchedCoordinates.Add(new Vector2Int (column, row));
                }

                List<Vector2Int> verticalMatches = FindRowMatchForTile(column, row, current);
                if (verticalMatches.Count >= 2)
                {
                    matchedCoordinates.AddRange(verticalMatches);
                    matchedCoordinates.Add(new Vector2Int (column, row));
                }
            }
        }

        return matchedCoordinates;
    }

    public void ClearMatchedTiles(List<Vector2Int> tiles)
    {

        Debug.Log("================");
        Debug.Log("Matched " + tiles.Count + " tiles: ");

        for (int i = 0; i < tiles.Count; i++)
        {
            int col = tiles[i].x;
            int row = tiles[i].y;

            GameObject tile = TileGrid[col, row];

            if (tile != null)
            {

                Vector2Int f = tile.GetComponent<Tile>().GetFraction();
                Vector2Int pos = tile.GetComponent<SwapBehaviour>().GridIndices;

                Debug.Log("Tile with value " + f.x + "/" + f.y + " at position " + pos);

                tile.GetComponent<SwapBehaviour>().Select(tile);

                //if (tile.GetComponent<Collectible>() != null  && tile.GetComponent<Collectible>().GetType() == CollectibleTypes.GEM) {
                if (tile.GetComponent<Collectible>() != null)
                {
                    Debug.Log("Collected a treasure");
                    tCalculator.incrementCollected();
                    inventory.AddItem(tile.GetComponent<Collectible>().GetType());
                    GemText.text = "Gems: " + tCalculator.treasuresCollected.ToString() + "/" + tCalculator.totalTreasures.ToString();

                    if (tCalculator.isAllCollected())
                    {
                        EndGame();
                    }

                }

            }
            //Destroy(tile, 1.3f);
            tile.GetComponent<Tile>().Destroy();
        }
        Debug.Log("================");

        if (AllLevelsData.level != 0)
            //Debug.Log("filling holes");
            //FillHoles();
            Invoke("FillHoles", 1.8f);
    }



    private List<Vector2Int> FindColumnMatchForTile(int col, int row, GameObject obj)
    {
        //List<GameObject> result = new List<GameObject>();
        List<Vector2Int> result = new List<Vector2Int>();

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
                result.Add(new Vector2Int ( i, row));
            }
            
        }
        return result;
    }


    private List<Vector2Int> FindRowMatchForTile(int col, int row, GameObject obj)
    {
        //List<GameObject> result = new List<GameObject>();
        List<Vector2Int> result = new List<Vector2Int>();

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
                result.Add(new Vector2Int ( col, i ));
            }
        }
        return result;
    }


    private void FillHoles()
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

                        next.transform.localPosition = GetXYfromColRow(column, filler) + positionOffset;
                        next.GetComponent<SwapBehaviour>().GridIndices = new Vector2Int(column, filler);
                        
                    }
                    Vector2Int val = Values[rand.Next(0, Values.Count)];
                    if (tCalculator.isTreasureHere()) {
                        InitCollectibleInGrid(treasureType, column, GridDimension - 1, val);
                        tCalculator.incrementPlaced();
                    }
                    else {
                        InitTileInGrid(getRandomType(), column, GridDimension - 1, val);
                    }

                    UpdateTempValues();
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


    public float FractionToFloat(Vector2Int fraction, int roundDigits)
    {
        int n = fraction.x;
        int d = fraction.y;
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


    private void UpdateTempValues()
    {
        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                GameObject current = TileGrid[column, row];
                if (current == null || !current.activeSelf)
                {
                    continue;
                }

                TempValueGrid[row * GridDimension + column] = current.GetComponent<Tile>().GetFraction();
            }
        }
    }











    // old stuff no longer used


    //void InitBaseValues()
    //{

    //    List<Vector2Int> baseVals = data.BaseFractions;

    //    // generate base fractions
    //    do
    //    {
    //        int n = rand.Next(1, 3);
    //        int d = rand.Next(1, 7);

    //        Vector2Int fraction = new Vector2Int(n, d);
    //        if (!baseVals.Contains(fraction) && (FractionToFloat(fraction, RoundDigits) < 1f))
    //        {
    //            baseVals.Add(fraction);
    //            //Multiples[fraction] = new List<int[]>();
    //        }


    //    } while (baseVals.Count < GridDimension);

    //}


    //public int CalculateScore(int numOfBlocks)
    //{
    //    if (numOfBlocks <= 1)
    //    {
    //        return 0;
    //    }

    //    int baseNum = 1;
    //    return (int) Math.Max(0.5f * Math.Pow(numOfBlocks * baseNum, 2), numOfBlocks * baseNum);
    //}

    // part of checkmatches
    //if (matchedCoordinates.Count > 2)
    //{
    //    for (int i = 0; i < matchedCoordinates.Count; i++)
    //    {
    //        int col = matchedCoordinates[i][0];
    //        int row = matchedCoordinates[i][1];

    //        GameObject tile = TileGrid[col, row];

    //        if (tile != null)
    //        {
    //            tile.GetComponent<SwapBehaviour>().Select();

    //            //if (tile.GetComponent<Collectible>() != null  && tile.GetComponent<Collectible>().GetType() == CollectibleTypes.GEM) {
    //            if (tile.GetComponent<Collectible>() != null)
    //            {

    //                tCalculator.incrementCollected();
    //                inventory.AddItem(tile.GetComponent<Collectible>().GetType());
    //                GemText.text = "Gems: " + tCalculator.treasuresCollected.ToString() + "/" + tCalculator.totalTreasures.ToString();

    //                if (tCalculator.isAllCollected())
    //                {
    //                    EndGame();
    //                }

    //            }

    //        }

    //        Destroy(tile, 1f);
    //    }

    //    if (AllLevelsData.level != 0)
    //        Invoke("FillHoles", 1f);
    //}

    // todo: do i still need this?
    //private int CalculateTimeBonus(float timeElapsed)
    //{
    //    return (int)(20 / Math.Sqrt(timeElapsed));
    //}


    //public List<Sprite> Sprites = new List<Sprite>();
    //private Canvas canvas;
    // todo: PROBABLY DEPRECATED
    //private Dictionary<int[], List<int[]>> Multiples = new Dictionary<int[], List<int[]>>();
    //public int GridDimension;
    //public float Distance;
    //private bool[] isBlockHere;
    //public int score = 0;
    //private Stopwatch stopwatch;
    //public TextMeshProUGUI stopwatchText;



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
