using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Block : MonoBehaviour, Tile
{

    //private static Block selected;
    //private SpriteRenderer Renderer;
    //private SwapBehaviour swapBehaviour;
    //public Vector2Int Position;

    public TextMeshProUGUI TextTop;
    public TextMeshProUGUI TextBottom;
    private int[] fraction;   // [0] is numerator, [1] is denominator
    private float internalValue;


    // Start is called before the first frame update
    void Start()
    {
        //Renderer = GetComponent<SpriteRenderer>();

        //Text = GetComponentInChildren<TextMeshProUGUI>();
        ////Text.text = FracToString();

    }

    public new TileTypes GetType()   // error if i dont use new
    {
        return TileTypes.BLOCK;
    }

    public void InitTile()
    {
        //Renderer = GetComponent<SpriteRenderer>();
        //Text = GetComponentInChildren<TextMeshProUGUI>();
        //swapBehaviour = GetComponent<SwapBehaviour>();
    }



    public void SetText()
    {
        TextTop.text = fraction[0].ToString();
        TextBottom.text = fraction[1].ToString();
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

}
