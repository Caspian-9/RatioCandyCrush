using System;


public class TreasureCalculator
{

    private float chance;

    //private int treasuresCollected;
    //private int treasuresPlaced;
    //private int totalTreasures;

    public int treasuresCollected;
    public int treasuresPlaced;
    public int totalTreasures;

    private Random random = new Random();

    public TreasureCalculator(int totalTreasures)
	{
        this.chance = 0f;
        this.treasuresCollected = 0;
        this.treasuresPlaced = 0;
        this.totalTreasures = totalTreasures;

        calculateChance();
	}


    // getters

    public float getChance()
    {
        return chance;
    }

    public int getCollected()
    {
        return treasuresCollected;
    }

    public int getPlaced()
    {
        return treasuresPlaced;
    }

    public int getTotal()
    {
        return totalTreasures;
    }


    // setters

    public void setChance(float chance)
    {
        this.chance = chance;
    }

    public void setCollected(int collected)
    {
        this.treasuresCollected = collected;
    }

    public void setPlaced(int placed)
    {
        this.treasuresPlaced = placed;
    }

    public void setTotal(int total)
    {
        this.totalTreasures = total;
    }



    public void incrementPlaced()
    {
        treasuresPlaced += 1;
        calculateChance();
    }

    public void incrementCollected() {
        treasuresCollected += 1;
        calculateChance();
    }



    public bool isAllCollected()
    {
        return treasuresCollected == totalTreasures;

        // placeholder to test if end game works
        //return true;
    }


    public bool isTreasureHere()
    {
        int index = random.Next(0, 100);
        return chanceToArray()[index];
    }


    public void calculateChance(int numOfMatches = 0)
    {
        if (treasuresPlaced == 0) {    // nothing is placed yet
            chance = 1f / AllLevelsData.Data[AllLevelsData.level].NumCollectible;
        }

        else if (treasuresCollected < treasuresPlaced) {    // gems present on the grid aren't collected
            chance = 0f;
        }

        else if (treasuresCollected == treasuresPlaced) {   // all current gems on grid collected
            chance += 0.05f + (0.002f * numOfMatches);
        }

    }


    private bool[] chanceToArray()
    {
        // converts a specified chance (between 0 and 1) to an array of bools for random to choose from

        bool[] arr = new bool[100];
        int c = (int)(chance * 100);

        for (int i = 0; i < c; i++)
        {
            arr[i] = true;
        }

        return arr;

    }


}

