using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieDiagram : MonoBehaviour
{
    private static PieDiagram selected;
    private SpriteRenderer Renderer;
    private SwapBehaviour swapBehaviour;

    public Image PieSector;
    public float InternalValue;
    //public Vector2Int Position;

    // Start is called before the first frame update
    void Start()
    {
        //SetValue(0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitPie()
    {
        Renderer = GetComponent<SpriteRenderer>();
        swapBehaviour = GetComponent<SwapBehaviour>();
        //Debug.Log(swapBehaviour == null);
    }

    public float GetValue()
    {
        return InternalValue;
    }

    public void SetValue(float val)
    {
        InternalValue = val;
        PieSector.fillAmount = val;
    }

    //public void Select()
    //{
    //    Renderer.color = Color.grey;
    //}

    //public void Unselect()
    //{
    //    Renderer.color = Color.white;
    //}


    //private void OnMouseDown()
    //{

    //    if (selected != null)
    //    {
    //        Debug.Log("clicced");
    //        if (selected == this)
    //            return;
    //        selected.Unselect();
    //        if (Vector2Int.Distance(selected.Position, Position) <= 2)
    //        {
    //            GridManager.Instance.SwapBlocks(Position, selected.Position);
    //            selected = null;
    //        }
    //        else
    //        {
    //            selected = this;
    //            Select();
    //        }
    //    }
    //    else
    //    {
    //        selected = this;
    //        Select();
    //    }
    //}

}
