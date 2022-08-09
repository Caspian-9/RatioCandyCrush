using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Manager : MonoBehaviour {

    public Inventory inventory;

    public CutscenePlayer cutscene;

    public GameObject LevelCube;

    public GameObject CounterPrefab;

    public GameObject message;

    //public Dictionary<CollectibleTypes, int> itemsToCollect = new Dictionary<CollectibleTypes, int>();

    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    // Start is called before the first frame update
    void Start()
    {
        //LoadDictionary();
        message.SetActive(true);

        inventory.UpdateInventory();
        ShowItemsList();
        CheckCompletion();
    }

    // Update is called once per frame
    void Update()
    {
        if (message.activeSelf)
        {
            if (CheckIfMoved())
            {
                StartCoroutine(FadeOut(message));
            }
        }
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

    public void CheckCompletion()
    {
        if (Items.isComplete())
        {
            cutscene.StartTimeline();
        }
    }

    


    IEnumerator FadeIn(GameObject obj)
    {
        Renderer r = obj.GetComponent<SpriteRenderer>();
        Color c = r.material.color;

        for (float alpha = 0f; alpha <= 1f; alpha += 0.1f)
        {
            c.a = alpha;
            yield return null;
        }

        yield break;
    }

    IEnumerator FadeOut(GameObject obj)
    {
        Renderer r = obj.GetComponent<SpriteRenderer>();
        Color c = r.material.color;

        for (float alpha = 1f; alpha >= 0f; alpha -= 0.05f)
        {
            c.a = alpha;
            yield return null;
        }
        obj.SetActive(false);
        yield break;
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
}
