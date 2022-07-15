using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stopwatch : MonoBehaviour
{

    private float currentTime;
    private bool swActive = false;

    public TextMeshProUGUI StopwatchText;
    
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (swActive)
        {
            currentTime += Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        StopwatchText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
    }

    public void StartStopwatch()
    {
        swActive = true;
    }

    public void StopStopwatch()
    {
        swActive = false;
    }

    public float getTime()
    {
        return currentTime;
    }

}
