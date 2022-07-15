using UnityEngine;
using System.Collections;

public class SwapBehaviour : MonoBehaviour
{

    private static SwapBehaviour selected;
    private SpriteRenderer Renderer;
    private Color color;
    public Vector2Int Position;

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

    public void Select()
    {
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;

        //Debug.Log("selected");
    }

    public void Unselect()
    {
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;

        //Debug.Log("unselected");
    }


    private void OnMouseDown()
    {
        //Debug.Log("clicced");

        if (!isInGrid)
        {
            //Debug.Log("not in grid");
            return;
        }

        if (selected != null)
        {
            //Debug.Log("null");

            if (selected == this)
                return;
            selected.Unselect();
            if (Vector2Int.Distance(selected.Position, Position) <= 1)
            {
                GridManager.Instance.SwapBlocks(Position, selected.Position);
                //// todo fix this so ugly
                //GridManager.Instance.score += GridManager.Instance.CalculateScore(GridManager.Instance.CheckMatches());
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
            //Debug.Log("bruh");
            selected = this;
            Select();
        }
    }

    public void SetInGrid(bool inGrid)
    {
        isInGrid = inGrid;
    }
}
