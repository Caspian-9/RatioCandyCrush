public interface Tile
{


    // getters

    public TileTypes GetType();

    public int[] GetFraction();

    public float GetValue();


    // setters

    public void SetFraction(int[] f);

    public void SetValue(float v);

    public void SetText();

}

