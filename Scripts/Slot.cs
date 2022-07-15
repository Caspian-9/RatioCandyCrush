using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public Vector2Int Position;

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        //Debug.Log("dropped");
        if (eventData.pointerDrag != null)
        {
            Vector2 slotPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = slotPosition;
            eventData.pointerDrag.GetComponent<DragDrop>().Lock();

            GridManager.Instance.AddBlockToGrid(eventData.pointerDrag, this.Position);
            GridManager.Instance.EndTurn();
        }
    }
}
