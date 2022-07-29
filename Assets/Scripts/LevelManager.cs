using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public GameObject LevelCube;
    //public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        //mainCamera.transform.position = new Vector3(0, 15, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {

        // 0 = 3d platformer scene; 1 = level scene

        SceneManager.LoadScene(1);
    }
}
