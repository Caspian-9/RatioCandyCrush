using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public GameObject BlockPrefab;
    public GameObject PiePrefab;
    public GameObject TreasurePrefab;

    public GameObject InstantiateTile(TileTypes type)
    {
        switch (type)
        {
            case TileTypes.BLOCK:
                return Instantiate(BlockPrefab);

            case TileTypes.PIE:
                return Instantiate(PiePrefab);

            default:
                // block by default
                return Instantiate(BlockPrefab);
        }
    }

    public GameObject InstantiateCollectible(CollectibleTypes type) {

        switch (type) {
            case CollectibleTypes.GEM:
                return Instantiate(TreasurePrefab);

            default:
                return Instantiate(TreasurePrefab);
        }
    }
}
