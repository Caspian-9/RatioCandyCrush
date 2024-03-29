using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Treasure : MonoBehaviour, Tile, Collectible
{
    public ParticleSystem ps;

    public Image img;

    private Vector2Int fraction;
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

    //public void Destroy()
    //{
    //    Debug.Log("destroy animation not implemented");
    //    Destroy(gameObject, 2f);
    //}

    public void Destroy()
    {
        var em = ps.emission;
        var dur = ps.main.duration;

        em.enabled = true;
        ps.Play();

        //Destroy(img);
        //Destroy(TextTop);
        //Destroy(TextBottom);

        Invoke(nameof(DestroyObj), dur / 2);

    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}
