using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Manager : MonoBehaviour {

    public Inventory inventory;

    public GameObject LevelCube;

    public GameObject CounterPrefab;

    //public Dictionary<CollectibleTypes, int> itemsToCollect = new Dictionary<CollectibleTypes, int>();

    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    // Start is called before the first frame update
    void Start()
    {
        //LoadDictionary();

        inventory.UpdateInventory();
        ShowItemsList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public void LoadDictionary() {
    //    itemsToCollect.Add(CollectibleTypes.GEM, 10);
    //}


    public void ShowItemsList() {

        foreach (CollectibleTypes type in Items.ItemsToCollect.Keys)
        {
            TextMeshProUGUI t = CounterPrefab.GetComponentInChildren<TextMeshProUGUI>();
            t.text = Items.ItemsToCollect[type].ToString();
        }
        
    }


    public void LoadLevel()
    {
        // 0 = 3d platformer scene; 1 = level scene

        SceneManager.LoadScene(1);
    }

}
