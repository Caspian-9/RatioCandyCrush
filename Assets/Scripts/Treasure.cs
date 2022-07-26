using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Treasure : MonoBehaviour, Tile
{
    private int[] fraction;
    private float internalValue;

    public TextMeshProUGUI TextTop;
    public TextMeshProUGUI TextBottom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public new TileTypes GetType()
    {
        return TileTypes.TREASURE;
    }

    public void InitTile()
    {
        //Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText()
    {
        TextTop.text = fraction[0].ToString();
        TextBottom.text = fraction[1].ToString();
        //Text.text = fraction[0].ToString() + "/" + fraction[1].ToString();
    }

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
