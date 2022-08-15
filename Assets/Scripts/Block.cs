using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Block : MonoBehaviour, Tile, RegularTile
{

    public TextMeshProUGUI TextTop;
    public TextMeshProUGUI TextBottom;
    private Vector2Int fraction;   // x is numerator, y is denominator
    private float internalValue;

    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {

    }


    // getters

    public new TileTypes GetType()   // error if i dont use new
    {
        return TileTypes.BLOCK;
    }

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
    }

    public void SetText()
    {
        TextTop.text = fraction.x.ToString();
        TextBottom.text = fraction.y.ToString();
    }

    public void Destroy()
    {
        Debug.Log("play anim");
        animator.Play("BlockDestroyAnim");
        Destroy(gameObject, 2f);
    }
}
