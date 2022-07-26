using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    public Vector2Int GridIndices;
    public Vector2 sPosition;
    private GridManager manager = null;

    public void setGridManager(GridManager m)
    {
        this.manager = m;
    }

}
