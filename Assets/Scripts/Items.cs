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
}

