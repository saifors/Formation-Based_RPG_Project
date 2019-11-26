using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class VictoryPanel : MonoBehaviour
{
	private RectTransform trans;
	public RectTransform vicTrans;
	public BattleUI battleUI;
	public GameManager gameManager;
	public VicLevelUp vicLevel;
	public GameObject vicLevelObject;

	public GameObject vicInfoPrefab;
	private VicMemberInfo[] vicInfo;

	private bool finishedAnim;
	private float timeCounter;

	int expGains;
	int goldGains;
	int[] itemGains;

	public TextMeshProUGUI goldText;
	public TextMeshProUGUI expText;
	public TextMeshProUGUI itemText;

	public TextMeshProUGUI[] textForLanguage;


	CanvasGroup canvas;

	public CanvasGroup[] subCanvases;
	public RectTransform levelTrans;

	public void Init()
	{
		battleUI = GetComponentInParent<BattleUI>();
		trans = GetComponent<RectTransform>();
		canvas = GetComponent<CanvasGroup>();
		gameManager = battleUI.gameManager;
		vicLevel = GetComponentInChildren<VicLevelUp>();
		vicLevelObject = vicLevel.gameObject;
		vicLevel.Init(gameManager);
		vicLevelObject.SetActive(false);
			//--------Language-----------
		//textForLanguage[0].text = LanguageManager.langData.;
	}


	public void DisplayVictory()
	{
		gameManager.CalculateRewards();
		finishedAnim = false;

		expGains = gameManager.totalExp;
		goldGains = gameManager.totalGold;
		itemGains = gameManager.allItems.ToArray();

		gameManager.gameData.Misc.gold += goldGains;
		goldText.text = goldGains.ToString();
		expText.text = expGains.ToString();

		//------- Items gained
		ItemCalc();
		//-------

		vicInfo = new VicMemberInfo[gameManager.partyMembers];

		for (int i = 0; i < gameManager.partyMembers; i++)
		{
			GameObject obj = Instantiate(vicInfoPrefab);
			vicInfo[i] = obj.GetComponent<VicMemberInfo>();
			vicInfo[i].Init(i, gameManager.gameData.Party[i].name, gameManager.gameData.Party[i].level, gameManager.gameData.Party[i].exp, expGains, gameManager, battleUI.portraitSprites[i]);
			vicInfo[i].trans.SetParent(vicTrans);

			float xTemp;
			float yTemp;

			if((i + 1)%2 == 0) xTemp = -20; //even (2nd row)
			else xTemp = -580; //odd (1st row)
			if (i > 1) yTemp = - 165; // 2nd collumn
			else yTemp = 143; //1st collumn

			vicInfo[i].trans.anchoredPosition = new Vector2(xTemp, yTemp);
			vicInfo[i].trans.localScale = Vector2.one;
		}

		trans.DOAnchorPosY(1030, 1.5f, true).SetEase(Ease.OutSine).From().OnComplete(FinishAnim);
		canvas.alpha = 0;
		canvas.DOFade(1, 1);
	}

	public void FinishAnim()
	{
		gameManager.selecting = GameManager.SelectingMenu.victoryScreen;
		finishedAnim = true;
		for (int i = 0; i < vicInfo.Length; i++)
		{
			vicInfo[i].ExpAnimStart();
		}
		
	}

	public void ItemCalc() // For Item Rewards
	{
		itemText.text = "";
		for (int i = 0; i < itemGains.Length; i++)
		{
			itemText.text += LanguageManager.langData.itemName[gameManager.gameData.ItemsCollection[itemGains[i]].name] + '\n';

			bool cont = true;

			for (int e = 0; e < gameManager.gameData.ItemInventory.Count; e++)
			{
				if (cont)
				{
					if (itemGains[i] == gameManager.gameData.ItemInventory[e].itemId) //You already have one of this item in your inventory
					{
						gameManager.gameData.ItemInventory[e].amount++;
						break;
					}
					else if (e == gameManager.gameData.ItemInventory.Count - 1) //final of the loop and still no match with items in inevntory
					{
						string newItem = gameManager.gameData.ItemInventory.Count.ToString() + '\t' + gameManager.gameData.ItemsCollection[itemGains[i]].id.ToString() + '\t' + 1;
						gameManager.gameData.ItemInventory.Add(new InventoryData(newItem));
						cont = false;
					}
				}
			}
		}
	}

	public void SwitchToLevelAnim()
	{
		vicLevelObject.SetActive(true);
		gameManager.selecting = GameManager.SelectingMenu.waiting;
		subCanvases[0].DOFade(0, 0.1f);
		subCanvases[1].DOFade(0, 0.25f).From();
		levelTrans.DOAnchorPosY(-200, 0.75f).SetEase(Ease.OutCirc).From().OnComplete(FinishSwitchToLevel);
	}
	public void FinishSwitchToLevel()
	{
		gameManager.selecting = GameManager.SelectingMenu.victoryScreen;
	}
	public void LevelUpCheck(int chara)
	{
		//Debug.Log("level new " + vicInfo[chara].levelNew + "old" + vicInfo[chara].levelOld);
		for (int i = 0; i < vicInfo.Length; i++)
		{
			vicInfo[i].ExpAnimFinished();
		}


		//Right here officer
		if(chara >= gameManager.partyMembers) //chara don't exist
		{
			gameManager.EndVictory();

			return;
		}

		if (vicInfo[chara].levelNew > vicInfo[chara].levelOld)
		{
			if (!vicLevelObject.activeSelf) SwitchToLevelAnim();
			
			vicLevel.DisplayLevelUp(vicInfo[chara]);
			gameManager.levelUpScreenProgress++;
		}
		else if (chara == gameManager.partyMembers - 1) //if no level up and this is last char
		{
			gameManager.EndVictory();
		}
		else //if no level up but more char after this one to calculate
		{
			gameManager.levelUpScreenProgress++;
			gameManager.VictoryContinuation(gameManager.levelUpScreenProgress);
		}
	}

	public void PanelActiveReset()
	{
		subCanvases[0].alpha = 1;
		subCanvases[1].alpha = 1;
		vicLevelObject.SetActive(false);
	}

	public void DeleteVicInfo()
	{
		for (int i = 0; i < vicInfo.Length; i++)
		{
			Destroy(vicInfo[i].gameObject);
		}
	}
}
