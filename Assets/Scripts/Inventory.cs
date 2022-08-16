using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

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

			CollectibleTypes type = TagToType(x.tag);
			x.GetComponentInChildren<TextMeshProUGUI>().text = Items.Inventory[type].ToString();
		}
	}


	public void AddItem(CollectibleTypes type)
	{
		// adds item of specified type to inventory
		Items.Inventory[type] += 1;

		foreach (GameObject x in itemList)
		{
			if (TagToType(x.tag) == type)
			{
				x.GetComponentInChildren<TextMeshProUGUI>().text = Items.Inventory[type].ToString();
			}
		}

		// shake the bag and the gem for VISUAL EFFECTS AND UX MMMMMMMM MYES
		StartCoroutine(SpinShake(bagIcon));
	}


	public void ToggleBag()
	{
		invBG.SetActive(!invBG.activeSelf);
		int spritesIndex = Convert.ToInt32(invBG.activeSelf);
		bagIcon.GetComponent<Image>().sprite = bagSprites[spritesIndex];
	}


    // shake obj coroutine
    IEnumerator SpinShake(GameObject obj)
    {
		for (float t = 0; t <= 1; t += Time.deltaTime)
		{
			obj.transform.Rotate(0, 0, (float)(0.4f * Mathf.Sin(4 * Mathf.PI * t)), Space.World);
			yield return null;
		}

		yield break;
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

