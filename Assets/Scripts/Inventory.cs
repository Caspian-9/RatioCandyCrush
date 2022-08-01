using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Inventory : MonoBehaviour
{

	public GameObject invBG;
	public GameObject bagIcon;

	public List<GameObject> itemList;

	public Sprite[] bagSprites;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
			
	}

	public void UpdateInventory()
	{
		// syncs the inventory display with inventory contents
		foreach (GameObject x in itemList)
		{
			CollectibleTypes type = x.GetComponent<Counter>().AssignedType;
			x.GetComponentInChildren<TextMeshProUGUI>().text = Items.Inventory[type].ToString();
		}
	}


	public void AddItem(CollectibleTypes type)
	{
		// adds item of specified type to inventory
		Items.Inventory[type] += 1;

		foreach (GameObject x in itemList)
		{
			if (x.GetComponent<Counter>().AssignedType == type)
			{
				x.GetComponentInChildren<TextMeshProUGUI>().text = Items.Inventory[type].ToString();
			}

		}

	}


	public void ToggleBag()
	{
		invBG.SetActive(!invBG.activeSelf);
		int spritesIndex = System.Convert.ToInt32(invBG.activeSelf);
		bagIcon.GetComponent<Image>().sprite = bagSprites[spritesIndex];
	}
}

