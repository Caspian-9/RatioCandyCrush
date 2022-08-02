using UnityEngine;
using System.Collections;

public class InfoPrompt : MonoBehaviour
{

	public GameObject iPrompt;

	// Use this for initialization
	void Start()
	{
		iPrompt.SetActive(true);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void ToggleInfoPrompt()
	{
		iPrompt.SetActive(!iPrompt.activeSelf);
	}
}

