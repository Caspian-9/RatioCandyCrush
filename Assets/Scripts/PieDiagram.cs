using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieDiagram : MonoBehaviour, Tile, RegularTile
{

    public Image PieSector;
    private Vector2Int fraction;   // [0] is numerator, [1] is denominator
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


    // getters
    public Vector2Int GetFraction()
    {
        return fraction;
    }

    public float GetValue()
    {
        return internalValue;
    }


    // setters
    public void SetFraction(Vector2Int f)
    {
        fraction = f;
    }

    public void SetValue(float v)
    {
        internalValue = v;
        PieSector.fillAmount = v;
    }

    public void SetText()
    {
        // no text to set. don't do anything
        return;
    }

}
