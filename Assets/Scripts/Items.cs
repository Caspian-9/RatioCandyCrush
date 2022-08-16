using System;
using System.Collections.Generic;

public static class Items
{

	public static Dictionary<CollectibleTypes, int> ItemsToCollect
        = new Dictionary<CollectibleTypes, int>()
        {
            { CollectibleTypes.GEM, 5 },
            { CollectibleTypes.GOLD, 4 },
            { CollectibleTypes.COMPASS, 2 }
        };

    public static Dictionary<CollectibleTypes, int> Inventory
        = new Dictionary<CollectibleTypes, int>()
        {
            { CollectibleTypes.GEM, 0 },
            { CollectibleTypes.GOLD, 0 },
            { CollectibleTypes.COMPASS, 0 }
        };

    public static bool isComplete()
    {
        foreach (CollectibleTypes type in ItemsToCollect.Keys)
        {
            if (!Inventory.ContainsKey(type) || Inventory[type] < ItemsToCollect[type])
            {
                return false;
            }
        }
        return true;
    }
}

