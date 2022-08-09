using UnityEngine;
using System.Collections;

public class SwapBehaviour : MonoBehaviour
{
    private GridManager manager = null;

    private static SwapBehaviour selected;
    private SpriteRenderer Renderer;
    private Color color;
    public Vector2Int GridIndices;

    private bool clickable = false;

    //private bool moving = false;


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
        if (!clickable)
        {
            return;
        }

        if (selected != null)
        {
                
            selected.Unselect();
            if (Vector2.Distance(selected.GridIndices, GridIndices) <= 1)
            {
                SwapBlocks(GridIndices, selected.GridIndices);
                if (manager.CheckMatches().Count < 3)
                {
                    SwapBlocks(selected.GridIndices, GridIndices);
                }
                else
                {
                    manager.EndTurn();
                }
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

    public void SetClickable(bool c)
    {
        clickable = c;
    }


    public void SwapBlocks(Vector2Int tile1Inds, Vector2Int tile2Inds)
    {
        GameObject[,] grid = manager.TileGrid;

        GameObject tile1 = grid[tile1Inds.x, tile1Inds.y];
        GameObject tile2 = grid[tile2Inds.x, tile2Inds.y];

        Vector2 pos1 = tile1.transform.position;
        Vector2 pos2 = tile2.transform.position;

        grid[tile1Inds.x, tile1Inds.y] = tile2;
        //tile2.transform.position = pos1;
        StartCoroutine(Move(tile2, pos1, 5f));
        tile2.transform.SetAsLastSibling();
        tile2.GetComponent<SwapBehaviour>().GridIndices = tile1Inds;

        grid[tile2Inds.x, tile2Inds.y] = tile1;
        //tile1.transform.position = pos2;
        StartCoroutine(Move(tile1, pos2, 5f));
        tile1.transform.SetAsLastSibling();
        tile1.GetComponent<SwapBehaviour>().GridIndices = tile2Inds;

    }

    private IEnumerator Move(GameObject obj, Vector3 target, float speed)
    {

        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        yield break;
        //yield return new WaitForSeconds(2);
    }
}
