using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Block : MonoBehaviour
{
    
    private static Block selected;
    private SpriteRenderer Renderer;
    private SwapBehaviour swapBehaviour;

    private TextMeshProUGUI Text;
    
    //public Vector2Int Position;

    private int[] fraction;   // [0] is numerator, [1] is denominator
    private float internalValue;


    // Start is called before the first frame update
    void Start()
    {
        //Renderer = GetComponent<SpriteRenderer>();

        //Text = GetComponentInChildren<TextMeshProUGUI>();
        ////Text.text = FracToString();

    }

    public void InitBlock()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Text = GetComponentInChildren<TextMeshProUGUI>();
        swapBehaviour = GetComponent<SwapBehaviour>();
    }



    public void SetText()
    {
        Text.text = fraction[0].ToString() + "/" + fraction[1].ToString();
    }


    // getters
    public int[] GetFraction()
    {
        return fraction;
    }

    public float GetValue()
    {
        return internalValue;
    }


    // setters
    public void SetFraction(int[] f)
    {
        fraction = f;
    }

    public void SetValue(float v)
    {
        internalValue = v;
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
