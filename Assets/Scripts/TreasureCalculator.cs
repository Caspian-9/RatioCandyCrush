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



    public void incrementCollected()
    {
        treasuresCollected += 1;
    }






    public bool isAllCollected()
    {
        return treasuresCollected == totalTreasures;

        // placeholder to test if end game works
        //return true;
    }


    public bool isGemHere()
    {
        int index = random.Next(0, 100);
        return chanceToArray()[index];
    }

    private void calculateGemChance()
    {
        // todo finish this lmao
        if (treasuresPlaced == 0)
        {
            chance = 0.9f;

        } else if (treasuresPlaced >= 1)
        {
            chance = 0.7f;
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

