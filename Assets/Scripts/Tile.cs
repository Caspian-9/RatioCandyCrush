using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface Tile
{

    //private TextMeshProUGUI Text;

    ////public Vector2Int Position;

    //private int[] fraction;   // [0] is numerator, [1] is denominator
    //private float internalValue;

    public TileTypes GetType();


    public void InitTile();

    public void SetText();


    // getters
    public int[] GetFraction();

    public float GetValue();


    // setters
    public void SetFraction(int[] f);

    public void SetValue(float v);

}

