using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGPlacer : MonoBehaviour
{

    //[SerializeField]
    public GameObject cloudBGPrefab;

    [SerializeField]
    private List<GameObject> cloudBGs;

    [SerializeField]
    private float scaleWidth, scaleHeight, distance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [ContextMenu("CreateBG")]

    private void CreateBG()
    {
        // breaks at runtime
        foreach (var c in cloudBGs)
        {
            DestroyImmediate(c);
        }

        cloudBGs = new List<GameObject>();

        float angle = 0f;

        for (int i = 0; i < 8; i++)
        {
            cloudBGs.Add(Instantiate(cloudBGPrefab, transform));
            cloudBGs[i].transform.Rotate(new Vector3(0f, angle, 0f));
            angle += 45f;
        }
    }


    [ContextMenu("SetBGScale")]

    private void SetBGScale()
    {
        foreach (var c in cloudBGs)
        {
            c.transform.localScale = new Vector3(scaleWidth, scaleHeight, 1f);
            float spriteLength = cloudBGs[0].GetComponent<SpriteRenderer>().bounds.size.x;
            distance = spriteLength / 2f + (Mathf.Sqrt(2) / 2) * spriteLength;
            c.transform.position = distance * c.transform.forward;
        }
    }

    private void OnValidate()
    {
        if (cloudBGs.Count == 8)
        {
            SetBGScale();
        }
    }
}
