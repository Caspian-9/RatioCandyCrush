using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject LevelButton;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        //mainCamera.transform.position = new Vector3(0, 15, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewLevel()
    {
        // GridManager gm = new GridManager(4, 2f, 3);
        //mainCamera.transform.position = new Vector3(0, 0, -10);
        //gm.NewGame();
    }
}
