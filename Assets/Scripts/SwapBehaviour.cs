using UnityEngine;
using System.Collections;

public class SwapBehaviour : MonoBehaviour
{
    private GridManager manager = null;

    private static SwapBehaviour selected;
    private SpriteRenderer Renderer;
    private Color color;
    public Vector2Int GridIndices;

    private bool isInGrid = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        Renderer = GetComponent<SpriteRenderer>();
        color = Renderer.color;
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
    }

    public void setGridManager(GridManager m)
    {
        this.manager = m;
    }

    public void Select()
    {
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }

    public void Unselect()
    {
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
    }


    private void OnMouseDown()
    {
        if (!isInGrid)
        {
            return;
        }

        if (selected != null)
        {
            if (selected == this)
                return;
            selected.Unselect();
            if (Vector2.Distance(selected.GridIndices, GridIndices) <= 1)
            {
                manager.SwapBlocks(GridIndices, selected.GridIndices);
                selected = null;
            }
            else
            {
                selected = this;
                Select();
            }
        }
        else
        {
            selected = this;
            Select();
        }
    }

    public void SetInGrid(bool inGrid)
    {
        isInGrid = inGrid;
    }
}
