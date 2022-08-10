using UnityEngine;
using System.Collections;
using System.IO;

public class WriteDebugToFile : MonoBehaviour
{

    // writes all debug messages to a log file

    string filename = "";

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }


    // Use this for initialization
    void Start()
	{
        filename = Application.dataPath + "Log.txt";
        Debug.Log(filename);
	}

	// Update is called once per frame
	void Update()
	{
			
	}

    public void Log(string logString, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(filename, true);
        tw.WriteLine("[" + System.DateTime.Now + "] " + logString);
        tw.Close();
    }
}

