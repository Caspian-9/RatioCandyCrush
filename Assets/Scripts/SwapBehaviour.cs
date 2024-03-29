﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwapBehaviour : MonoBehaviour //, IPointerDownHandler
{
    private GridManager manager = null;
    private int dimension;
    private StatusMessage statusMessage;

    private static SwapBehaviour selected;
    
    //private SpriteRenderer Renderer;
    private Color color;
    public Vector2Int GridIndices;

    public GameObject border;

    // used for tutorial only
    public static bool firstClicked = true;


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
        //Renderer = gameObject.GetComponent<SpriteRenderer>();
        //color = Renderer.color;
        //color.a = 0f;
        //gameObject.GetComponent<SpriteRenderer>().material.color = color;
        border.SetActive(false);
    }

    public void setGridManager(GridManager m)
    {
        this.manager = m;
        this.dimension = manager.GridDimension;
        this.statusMessage = manager.statusMessage;
    }

    public void Select(GameObject obj)
    {
        //color.a = 1f;
        //obj.GetComponent<SpriteRenderer>().material.color = color;
        obj.GetComponent<SwapBehaviour>().border.SetActive(true);
    }

    public void Unselect(GameObject obj)
    {
        //color.a = 0f;
        //obj.GetComponent<SpriteRenderer>().material.color = color;
        obj.GetComponent<SwapBehaviour>().border.SetActive(false);
    }


    public void OnMouseDown()
    {
        //SwapBehaviour clicked = eventData.pointerEnter.GetComponent<SwapBehaviour>();
        //Debug.Log(eventData);
        SwapBehaviour clicked = gameObject.GetComponent<SwapBehaviour>();
        Vector2Int clickedFraction = clicked.gameObject.GetComponent<Tile>().GetFraction();
        Debug.Log("Clicked on " + clicked + " type tile with value " + clickedFraction.x + "/" + clickedFraction.y);

        if (!clickable || clicked == null)
        {
            return;
        }

        if (selected == null)
        {
            if (AllLevelsData.level == 0 && firstClicked == true)
            {
                firstClicked = false;
                statusMessage.SetText(AllLevelsData.tutorialMessages[1]);
            } else if (AllLevelsData.level == 0)
            {
                statusMessage.SetText(AllLevelsData.tutorialMessages[3]);
            }



            selected = clicked;
            selected.Select(selected.gameObject);
            return;
        }

        if (selected == clicked)  // clicked on self
        {
            selected.Unselect(selected.gameObject);
            selected = null;
            return;
        }

        //if (AllLevelsData.level == 0)
        //{
        //    statusMessage.SetText(AllLevelsData.tutorialMessages[2]);
        //}

        Vector2Int sf = selected.gameObject.GetComponent<Tile>().GetFraction();
        Vector2Int cf = clicked.gameObject.GetComponent<Tile>().GetFraction();

        string s = sf.x + "/" + sf.y;
        string c = cf.x + "/" + cf.y;

        if (isLegalMove(selected.GridIndices, clicked.GridIndices))
        {
            Debug.Log("Swapped tiles at " + selected.GridIndices + " with value " + s + " and " + clicked.GridIndices + " with value " + c);
            statusMessage.SetText("");
            SwapBlocks(selected.GridIndices, clicked.GridIndices);

        }
        else   // move is not legal
        {
            //statusMessage.gameObject.SetActive(true);
            //statusMessage.gameObject.GetComponent<SpriteRenderer>().material.color.a = 0f;
            if (AllLevelsData.level == 0)
            {
                statusMessage.SetText(AllLevelsData.tutorialMessages[2]);
            }
            else
            {
                statusMessage.SetText("Move doesn't result in match. Try again");
            }
            Debug.Log("Unsuccessfully tried to swap tiles at " + selected.GridIndices + " with value " + s + " and " + clicked.GridIndices + " with value " + c);

            //statusMessage.Show();
        }

        selected.Unselect(selected.gameObject);
        selected = null;
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
        StartCoroutine(Move(tile2, pos1, 230f));
        tile2.transform.SetAsLastSibling();
        tile2.GetComponent<SwapBehaviour>().GridIndices = tile1Inds;

        grid[tile2Inds.x, tile2Inds.y] = tile1;
        //tile1.transform.position = pos2;
        StartCoroutine(Move2(tile1, pos2, 230f));
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

    private IEnumerator Move2(GameObject obj, Vector3 target, float speed)
    {
        // same thing but this needs to be called last so it calls endturn
        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        manager.EndTurn();
        yield break;
        //yield return new WaitForSeconds(2);
    }




    private bool isLegalMove(Vector2Int v1Inds, Vector2Int v2Inds)
    {
        SwapVals(v1Inds, v2Inds);

        if (isMatch3() && isAdjacent(v1Inds, v2Inds))
        {
            return true;
        }
        else
        {
            SwapVals(v2Inds, v1Inds);    // swap back
            return false;
        }

    }

    private bool isAdjacent(Vector2Int v1Inds, Vector2Int v2Inds)
    {
        Vector2Int top = new Vector2Int(v1Inds.x, v1Inds.y + 1);
        Vector2Int bottom = new Vector2Int(v1Inds.x, v1Inds.y - 1);
        Vector2Int left = new Vector2Int(v1Inds.x - 1, v1Inds.y);
        Vector2Int right = new Vector2Int(v1Inds.x + 1, v1Inds.y);

        return v2Inds == top || v2Inds == bottom || v2Inds == left || v2Inds == right;
    }

    private void SwapVals(Vector2Int v1Inds, Vector2Int v2Inds)
    {
        //Vector2Int[] grid = manager.TempValueGrid;

        Vector2Int v1 = manager.TempValueGrid[v1Inds.y * dimension + v1Inds.x];
        Vector2Int v2 = manager.TempValueGrid[v2Inds.y * dimension + v2Inds.x];

        Vector2Int temp = v1;

        manager.TempValueGrid[v1Inds.y * dimension + v1Inds.x] = v2;
        manager.TempValueGrid[v2Inds.y * dimension + v2Inds.x] = temp;

    }

    private bool isMatch3()
    {
        List<Vector2Int> matchedCoordinates = new List<Vector2Int>();

        for (int row = 0; row < dimension; row++)
        {
            for (int column = 0; column < dimension; column++)
            {
                Vector2Int current = manager.TempValueGrid[row * dimension + column];

                List<Vector2Int> horizontalMatches = FindColumnMatchForVec(column, row, current);
                if (horizontalMatches.Count >= 2)
                {
                    matchedCoordinates.AddRange(horizontalMatches);
                    matchedCoordinates.Add(new Vector2Int(column, row));
                }

                List<Vector2Int> verticalMatches = FindRowMatchForVec(column, row, current);
                if (verticalMatches.Count >= 2)
                {
                    matchedCoordinates.AddRange(verticalMatches);
                    matchedCoordinates.Add(new Vector2Int(column, row));
                }
            }
        }
        //Debug.Log(matchedCoordinates.Count);
        return matchedCoordinates.Count >= 3;
    }


    private List<Vector2Int> FindColumnMatchForVec(int col, int row, Vector2Int vec)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = col + 1; i < dimension; i++)
        {
            Vector2Int nextColumn = new Vector2Int(i, row);
            Vector2Int nextColValue = manager.TempValueGrid[nextColumn.y * dimension + nextColumn.x];

            if (nextColumn == null)
                break;

            if (FractionToFloat(nextColValue, 4) != FractionToFloat(vec, 4))
            {
                break;
            }
            else
            {
                result.Add(new Vector2Int(i, row));
            }

        }
        return result;
    }


    private List<Vector2Int> FindRowMatchForVec(int col, int row, Vector2Int vec)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = row + 1; i < dimension; i++)
        {
            Vector2Int nextRow = new Vector2Int(col, i);
            Vector2Int nextRowValue = manager.TempValueGrid[nextRow.y * dimension + nextRow.x];

            if (nextRow == null)
                break;

            if (FractionToFloat(nextRowValue, 4) != FractionToFloat(vec, 4))
            {
                break;
            }
            else
            {
                result.Add(new Vector2Int(col, i));
            }

        }
        return result;
    }



    private float FractionToFloat(Vector2Int fraction, int roundDigits)
    {
        int n = fraction.x;
        int d = fraction.y;
        return (float)Math.Round((double)n / d, roundDigits);   // round to this many decimal places
    }



    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    SwapBehaviour clicked = eventData.pointerEnter.GetComponent<SwapBehaviour>();
    //    Debug.Log(eventData);

    //    if (!clickable || clicked == null)
    //    {
    //        return;
    //    }

    //    if (selected == null)
    //    {
    //        selected = clicked;
    //        selected.Select(selected.gameObject);
    //        return;
    //    }

    //    if (selected == clicked)  // clicked on self
    //    {
    //        selected.Unselect(selected.gameObject);
    //        selected = null;
    //        return;
    //    }
    //    //else  // clicked one that isnt self
    //    //{
    //    //    selected = clicked;
    //    //    selected.Select();
    //    //    Debug.Log(selected.GridIndices);
    //    //}

    //    if (isLegalMove(selected.GridIndices, clicked.GridIndices))
    //    {
    //        statusMessage.SetText("");
    //        SwapBlocks(selected.GridIndices, clicked.GridIndices);

    //    }
    //    else   // move is not legal
    //    {
    //        //statusMessage.gameObject.SetActive(true);
    //        //statusMessage.gameObject.GetComponent<SpriteRenderer>().material.color.a = 0f;
    //        statusMessage.SetText("Move doesn't result in match. Try again");
    //        //statusMessage.Show();
    //    }

    //    selected.Unselect(selected.gameObject);
    //    selected = null;
    //}


    //public void OnMouseDown()
    //{
    //    if (!clickable)
    //    {
    //        return;
    //    }

    //    if (selected != null)
    //    {
    //        Debug.Log("clicked");
    //        selected.Unselect();
    //        if (Vector2.Distance(selected.GridIndices, GridIndices) <= 1)
    //        {
    //            if (isLegalMove(selected.GridIndices, GridIndices))
    //            {
    //                SwapBlocks(selected.GridIndices, GridIndices);
    //                manager.EndTurn();
    //            }
    //            else
    //            {
    //                statusMessage.gameObject.SetActive(true);
    //                statusMessage.SetText("Move doesn't result in match. Try again");
    //                statusMessage.Show();
    //            }

    //            //SwapBlocks(GridIndices, selected.GridIndices);
    //            //if (manager.CheckMatches().Count < 3)
    //            //{
    //            //    StopAllCoroutines();
    //            //    SwapBlocks(selected.GridIndices, GridIndices);
    //            //}
    //            //else
    //            //{
    //            //    manager.EndTurn();
    //            //}
    //            selected = null;
    //        }
    //        else
    //        {
    //            selected = this;
    //            Select();
    //        }
    //    }
    //    else
    //    {
    //        selected = this;
    //        Select();
    //    }
    //}


    //private List<Vector2Int> FindRowMatchForVec(int col, int row, Vector2Int vec)
    //{
    //    List<Vector2Int> result = new List<Vector2Int>();

    //    float currentValue = obj.GetComponent<Tile>().GetValue();

    //    for (int i = row + 1; i < dimension; i++)
    //    {
    //        GameObject nextRow = GetGameObjectAt(col, i);
    //        if (nextRow == null || !nextRow.activeSelf)
    //            break;

    //        float nextRowValue = nextRow.GetComponent<Tile>().GetValue();

    //        if (nextRowValue != currentValue)
    //        {
    //            break;
    //        }
    //        else
    //        {
    //            result.Add(new Vector2Int(col, i));
    //        }
    //    }
    //    return result;
    //}
}
