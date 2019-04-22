using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBoxBattle : MonoBehaviour
{
	private GameManager gameManager;
	public GameObject itemTextPrefab;

	public TextMeshProUGUI description;

	public RectTransform inventoryPanel;
	public ItemTextContents[] itemTexts;
	private GameObject[] itemObj;
	private RectTransform[] itemTrans;

	public RectTransform selection;
	public int selected;
	private Vector2 selectionMargin;

	public void Init(GameManager gM)
    {
		gameManager = gM;
		selectionMargin = new Vector2(0, -50);

		CalculateItemBox();
		selected = 0;
		PlaceSelection();
	}

    public void CalculateItemBox()
	{
		itemTexts = new ItemTextContents[gameManager.gameData.ItemInventory.Count];
		itemTrans = new RectTransform[itemTexts.Length];

		//Calculate size of the inventoryPanel
		float height = (Mathf.FloorToInt(itemTexts.Length / 2) +1) * 100;
		inventoryPanel.sizeDelta = new Vector2(1270, height);
		inventoryPanel.anchoredPosition = Vector2.zero;

		List<GameObject> itemTemp = new List<GameObject>();
		for (int i = 0; i < itemTexts.Length; i++)
		{
			GameObject obj = Instantiate(itemTextPrefab);
			itemTrans[i] = obj.GetComponent<RectTransform>();
			itemTexts[i] = obj.GetComponent<ItemTextContents>();
			itemTemp.Add(obj);
			itemTrans[i].SetParent(inventoryPanel);

			float xT;
			float yT;
			if ((i + 1) % 2 == 0)
			{
				xT = 640;
			}
			else xT = 0;
			if (i > 1)
			{
				yT = Mathf.FloorToInt(i / 2) * -100;
			}
			else yT = 0;

			itemTrans[i].anchoredPosition = new Vector2(xT, yT);
			itemTrans[i].localScale = Vector3.one;

			itemTexts[i].name.text = gameManager.gameData.ItemsCollection[gameManager.gameData.ItemInventory[i].itemId].name;
			itemTexts[i].amount.text = "x" + gameManager.gameData.ItemInventory[i].amount.ToString();
	
		}
		itemObj = itemTemp.ToArray();
	}

	public void PlaceSelection()
	{

		if (itemTrans.Length > 0)
		{
			selection.anchoredPosition = itemTrans[selected].anchoredPosition + selectionMargin;
		}
	}
}
