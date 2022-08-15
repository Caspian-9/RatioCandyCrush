using UnityEngine;

public interface Tile
{
    // tile that has a value

    // getters

    public Vector2Int GetFraction();

    public float GetValue();


    // setters

    public void SetFraction(Vector2Int f);

    public void SetValue(float v);

    public void SetText();

    public void Destroy();

}

