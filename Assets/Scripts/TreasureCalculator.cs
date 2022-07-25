using System;


public class TreasureCalculator
{

    private float chance;

    private int treasuresCollected;
    private int treasuresPlaced;
    private int totalTreasures;

    private Random random = new Random();

    public TreasureCalculator(int totalTreasures)
	{
        this.totalTreasures = totalTreasures;
	}


    public float getChance()
    {
        return this.chance;
    }

    public void setChance(float chance)
    {
        this.chance = chance;
    }


    public bool isAllCollected()
    {
        //return treasuresCollected == totalTreasures;

        // placeholder to test if end game works
        return true;
    }


    // change these to setters if clearing multiple gems at a time starts causing problems
    public void incrementCollected()
    {
        treasuresCollected += 1;

    }

    public void incrementPlaced()
    {
        treasuresPlaced += 1;
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

