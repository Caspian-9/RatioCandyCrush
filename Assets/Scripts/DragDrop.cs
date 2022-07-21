using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private bool isLocked = false;


    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }


    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("clicked");
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("start drag");

        if (isLocked)
        {
            eventData.pointerDrag = null;
            return;
        }

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        //Debug.Log("drag");

        if (isLocked)
            eventData.pointerDrag = null;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("end drag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

}
