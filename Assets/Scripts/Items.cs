using System;
using System.Collections.Generic;

public static class Items
{

	public static Dictionary<CollectibleTypes, int> ItemsToCollect
        = new Dictionary<CollectibleTypes, int>()
        {
            { CollectibleTypes.GEM, 10 }

        };

    public static Dictionary<CollectibleTypes, int> Inventory
        = new Dictionary<CollectibleTypes, int>()
        {
            { CollectibleTypes.GEM, 0 }

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

