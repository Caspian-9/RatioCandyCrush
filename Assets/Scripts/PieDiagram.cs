using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieDiagram : MonoBehaviour, Tile
{
    //private static PieDiagram selected;
    //private SpriteRenderer Renderer;
    //private SwapBehaviour swapBehaviour;
    //public Vector2Int Position;

    public Image PieSector;
    private int[] fraction;   // [0] is numerator, [1] is denominator
    private float internalValue;

    // Start is called before the first frame update
    void Start()
    {
        //SetValue(0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public new TileTypes GetType() { 
        return TileTypes.PIE;
    }

    public void InitTile()
    {
        //Renderer = GetComponent<SpriteRenderer>();
        //swapBehaviour = GetComponent<SwapBehaviour>();
        //Debug.Log(swapBehaviour == null);
    }

    public void SetText()
    {
        // no text to set. don't do anything
        return;
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
        PieSector.fillAmount = v;
    }

}
