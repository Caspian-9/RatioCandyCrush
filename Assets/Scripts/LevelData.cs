using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class AllLevelsData
{
    public static int level = 0;

    public static LevelData lv0;
    public static LevelData lv1;
    public static LevelData lv2;

    public static Dictionary<int, LevelData> Data =
        new Dictionary<int, LevelData>
        {
            { 0, getLv0() },
            { 1, getLv1() },
            { 2, getLv2() }
        };

    static AllLevelsData()
    {
        tutorialGrid = new List<Vector2Int>
        {
            new Vector2Int(0,0), new Vector2Int(1,1), new Vector2Int(1,1), new Vector2Int(1,2),
            new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(1,1),
            new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0),
            new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0)
        };

        lv1Base = new List<Vector2Int>
        {
            new Vector2Int(1,2), new Vector2Int(1,3), new Vector2Int(2,3)
        };

        lv2Base = new List<Vector2Int>
        {
            new Vector2Int(1,3), new Vector2Int(2,3), new Vector2Int(4,5), new Vector2Int(1,6)
        };
    }


    private static LevelData getLv0()
    {
        if (lv0 == null)
        {
            lv0 = new LevelData(1, CollectibleTypes.GEM, 4, new List<Vector2Int>(), tutorialGrid);
        }
        return lv0;
    }

    private static LevelData getLv1()
    {
        if (lv1 == null)
        {
            lv1 = new LevelData(4, CollectibleTypes.GEM, 4, lv1Base, new List<Vector2Int>());
        }
        return lv1;
    }

    private static LevelData getLv2()
    {
        if (lv2 == null)
        {
            lv2 = new LevelData(4, CollectibleTypes.GOLD, 6, lv2Base, new List<Vector2Int>());
        }
        return lv2;
    }


    //public static LevelData lv0 = new LevelData(1, CollectibleTypes.GEM, 4, new List<Vector2Int>(), tutorialGrid);
    //public static LevelData lv1 = new LevelData(3, CollectibleTypes.GEM, 4, lv1Base, new List<Vector2Int>());
    //public static LevelData lv2 = new LevelData(6, CollectibleTypes.GEM, 6, lv2Base, new List<Vector2Int>());


    public static List<Vector2Int> tutorialGrid =
        new List<Vector2Int>
        {
            new Vector2Int(0,0), new Vector2Int(1,1), new Vector2Int(1,1), new Vector2Int(1,2),
            new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(1,1),
            new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0),
            new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0)
        };

    public static List<Vector2Int> lv1Base =
        new List<Vector2Int>
        {
            new Vector2Int(1,2), new Vector2Int(1,3), new Vector2Int(2,3)
        };

    public static List<Vector2Int> lv2Base =
        new List<Vector2Int>
        {
            new Vector2Int(1,3), new Vector2Int(2,3), new Vector2Int(4,5), new Vector2Int(1,6)
        };


    public static List<string> tutorialMessages =
        new List<string>
        {
            "Click on a tile to select it",
            "Click another adjacent tile to attempt a swap",
            "Swap only works if it results in a match 3",
            "Match 3 tiles to clear them",
            "Goal: collect the treasure"
        };
}



public class LevelData
{

    // stores information about individual levels

    public int NumCollectible;
    public CollectibleTypes AssignedType;

    public int Dimension;

    public List<Vector2Int> BaseFractions;

    public List<Vector2Int> CustomGrid;


    public LevelData( int numCollectible,
                      CollectibleTypes type,
                      int dimension,
                      List<Vector2Int> baseFractions,
                      List<Vector2Int> customGrid)
    {
        this.NumCollectible = numCollectible;
        this.AssignedType = type;
        this.Dimension = dimension;
        this.BaseFractions = baseFractions;
        this.CustomGrid = customGrid;
    }

}
