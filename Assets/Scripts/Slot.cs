using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour  //, IDropHandler
{
    public Vector2Int GridIndices;
    public Vector2 sPosition;
    private GridManager manager = null;

    public void setGridManager(GridManager m)
    {
        this.manager = m;
    }


    // old dr mario era code. do not use

    //void IDropHandler.OnDrop(PointerEventData eventData)
    //{
    //    //Debug.Log("dropped");
    //    if (eventData.pointerDrag != null)
    //    {
    //        Vector2 slotPosition = GetComponent<RectTransform>().anchoredPosition;
    //        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = slotPosition;
    //        eventData.pointerDrag.GetComponent<DragDrop>().Lock();

    //        manager.AddTileToGrid(eventData.pointerDrag, this.GridIndices);
    //        manager.EndTurn();
    //    }
    //}
}
