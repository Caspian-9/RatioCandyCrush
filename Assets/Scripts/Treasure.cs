using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Treasure : MonoBehaviour, Tile, Collectible
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


    // getters

    public new CollectibleTypes GetType()
    {
        return CollectibleTypes.GEM;
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

    public void SetText()
    {
        TextTop.text = fraction[0].ToString();
        TextBottom.text = fraction[1].ToString();
    }
}
