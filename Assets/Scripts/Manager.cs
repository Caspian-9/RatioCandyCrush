using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Manager : MonoBehaviour {

    public Inventory inventory;

    public CutscenePlayer cutscene;

    public GameObject LevelCube;

    //public List<GameObject> itemList;
    public GameObject gemCounter;
    public GameObject goldCounter;
    public GameObject compassCounter;

    public StatusMessage statusMessage;

    //public Dictionary<CollectibleTypes, int> itemsToCollect = new Dictionary<CollectibleTypes, int>();

    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    // Start is called before the first frame update
    void Start()
    {
        //LoadDictionary();
        statusMessage.gameObject.SetActive(true);

        inventory.UpdateInventory();
        ShowItemsList();
        CheckCompletion();
    }

    // Update is called once per frame
    void Update()
    {
        if (statusMessage.gameObject.activeSelf)
        {
            if (CheckIfMoved())
            {
                statusMessage.FadeOut(statusMessage.gameObject);
            }
        }
    }


    //public void LoadDictionary() {
    //    itemsToCollect.Add(CollectibleTypes.GEM, 10);
    //}


    public void ShowItemsList() {

        //foreach (CollectibleTypes type in Items.ItemsToCollect.Keys)
        //{
        //    TextMeshProUGUI t = CounterPrefab.GetComponentInChildren<TextMeshProUGUI>();
        //    t.text = Items.ItemsToCollect[type].ToString();
        //}

        TextMeshProUGUI mt = gemCounter.GetComponentInChildren<TextMeshProUGUI>();
        mt.text = Items.ItemsToCollect[CollectibleTypes.GEM].ToString();

        TextMeshProUGUI gt = goldCounter.GetComponentInChildren<TextMeshProUGUI>();
        gt.text = Items.ItemsToCollect[CollectibleTypes.GOLD].ToString();

        TextMeshProUGUI ct = compassCounter.GetComponentInChildren<TextMeshProUGUI>();
        ct.text = Items.ItemsToCollect[CollectibleTypes.COMPASS].ToString();

    }


    public void LoadLevel()
    {
        // 0 = 3d platformer scene; 1 = level scene

        SceneManager.LoadScene(1);
    }

    public void CheckCompletion()
    {
        if (Items.isComplete())
        {
            cutscene.StartTimeline();
        }
    }

    


    

    private bool CheckIfMoved()
    {
        // bruh
        if (Input.GetKeyUp(KeyCode.W))
        {
            return true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            return true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            return true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            return true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            return true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            return true;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            return true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            return true;
        }

        return false;
    }

    private CollectibleTypes TagToType(string tag)
    {
        if (tag == "Gem")
            return CollectibleTypes.GEM;
        if (tag == "Gold")
            return CollectibleTypes.GOLD;
        if (tag == "Compass")
            return CollectibleTypes.COMPASS;

        return CollectibleTypes.GEM;
    }
}
