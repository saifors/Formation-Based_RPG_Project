using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VicLevelUp : MonoBehaviour
{
	public GameManager gameManager;

	public TextMeshProUGUI levelUpText;
	public TextMeshProUGUI[] statsText;

	public TextMeshProUGUI nameText;

	public TextMeshProUGUI[] level;
	public TextMeshProUGUI[] hp;
	public TextMeshProUGUI[] mp;
	public TextMeshProUGUI[] atk;
	public TextMeshProUGUI[] def;
	public TextMeshProUGUI[] res;
	public TextMeshProUGUI[] spd;

	public RectTransform burstTrans;
	CanvasGroup burstCanvas;
	Animator burstAnim;

	bool statAnimStarted;
	Vector3 burstMargin;

	RectTransform[] newStatTrans;
	int statUpCounter;
	VicMemberInfo vInfo;

	public TextMeshProUGUI attackLearned;
	
	public void Init(GameManager gM)
    {
		gameManager = gM;
		statUpCounter = 0;
		statAnimStarted = false;
		burstMargin = new Vector3(100,-40);

		burstAnim = burstTrans.GetComponent<Animator>();
		burstCanvas = burstTrans.GetComponent<CanvasGroup>();

		burstAnim.enabled = false;
		burstCanvas.alpha = 1;

		List<RectTransform> nSTList = new List<RectTransform>();
		nSTList.Add(level[1].GetComponent<RectTransform>());
		nSTList.Add(hp[1].GetComponent<RectTransform>());
		nSTList.Add(mp[1].GetComponent<RectTransform>());
		nSTList.Add(atk[1].GetComponent<RectTransform>());
		nSTList.Add(def[1].GetComponent<RectTransform>());
		nSTList.Add(res[1].GetComponent<RectTransform>());
		nSTList.Add(spd[1].GetComponent<RectTransform>());
		newStatTrans = nSTList.ToArray();

		for (int i = 0; i < statsText.Length; i++)
		{
			//levelUpText.text = LanguageManager.langData. ;
			//statsText[i].text = LanguageManager.langData.stats["Level"] + '\n' + LanguageManager.langData.stats["HP"] + '\n' + LanguageManager.langData.stats["MP"] + '\n' + LanguageManager.langData.stats["Attack"] + '\n' + LanguageManager.langData.stats["Defense"] + '\n' + LanguageManager.langData.stats["Resistance"] + '\n' + LanguageManager.langData.stats["Speed"];
		}
    }

	private void Update()
	{
		if (statAnimStarted)
		{
			if (burstAnim.GetCurrentAnimatorStateInfo(0).IsName("AnimEnded"))
			{
				StatUpAnimFinished();
			}
		}
	}

	public void DisplayLevelUp(VicMemberInfo vicInfo)
	{
		nameText.text = vicInfo.nameText.text;

		level[0].text = vicInfo.levelOld.ToString();

		hp[0].text = gameManager.gameData.Party[vicInfo.ID].hp.ToString(); 
		mp[0].text = gameManager.gameData.Party[vicInfo.ID].mp.ToString(); 
		atk[0].text = gameManager.gameData.Party[vicInfo.ID].attack.ToString(); 
		def[0].text = gameManager.gameData.Party[vicInfo.ID].defense.ToString(); 
		res[0].text = gameManager.gameData.Party[vicInfo.ID].resistance.ToString(); 
		spd[0].text = gameManager.gameData.Party[vicInfo.ID].speed.ToString();

		vInfo = vicInfo;
		attackLearned.text = "";
		LearnAttacks();

		gameManager.UpdateStats(vicInfo.ID);

		

		//Full recovery on level up
		gameManager.FullHeal(vInfo.ID);

		gameManager.selecting = GameManager.SelectingMenu.waiting;
		StatUpCalc();
	}

	public void StatUpCalc()
	{
		switch (statUpCounter)
		{
			case 0:
				level[1].text = vInfo.levelNew.ToString();
				break;
			case 1:
				hp[1].text = gameManager.gameData.Party[vInfo.ID].hp.ToString();		
				break;
			case 2:
				mp[1].text = gameManager.gameData.Party[vInfo.ID].mp.ToString();		
				break;
			case 3:
				atk[1].text = gameManager.gameData.Party[vInfo.ID].attack.ToString();		
				break;
			case 4:
				def[1].text = gameManager.gameData.Party[vInfo.ID].defense.ToString();		
				break;
			case 5:
				res[1].text = gameManager.gameData.Party[vInfo.ID].resistance.ToString();		
				break;
			case 6:
				spd[1].text = gameManager.gameData.Party[vInfo.ID].speed.ToString();
				break;
			default:
				break;
		}

		BurstAnim();
		
	}

	public void BurstAnim()
	{
		burstTrans.localPosition = newStatTrans[statUpCounter].localPosition + burstMargin;
		gameManager.soundPlayer.PlaySound(4, true);
		statUpCounter++;
		StatUpAnim();
	}

	public void StatUpAnim()
	{
		burstCanvas.alpha = 1;
		statAnimStarted = true;
		burstAnim.enabled = true;
		burstAnim.Play("LevelBurstAnim");
	}
	public void StatUpAnimFinished()
	{
		statAnimStarted = false;
		burstCanvas.alpha = 0;
		burstAnim.enabled = false;
		if (statUpCounter < 7) StatUpCalc();
		else
		{
			statUpCounter = 0;
			gameManager.selecting = GameManager.SelectingMenu.victoryScreen;
		}
	}

	public void LearnAttacks()
	{

		for (int i = 0; i < gameManager.gameData.CharStats[vInfo.ID].learnAttacks.Count; i++)
		{
			Debug.Log("Yo1");
			if (vInfo.levelOld < gameManager.gameData.CharStats[vInfo.ID].levelAttacks[i] && vInfo.levelNew >= gameManager.gameData.CharStats[vInfo.ID].levelAttacks[i])
			{
				Debug.Log("Yo2");

				//Gained attack i with this level
				if (attackLearned.text == "")
				{
					attackLearned.text = "Learned: " + LanguageManager.langData.attackName[gameManager.gameData.AttackList[gameManager.gameData.CharStats[vInfo.ID].learnAttacks[i]].nameKey];
				}
				else
				{
					attackLearned.text += ", " + LanguageManager.langData.attackName[gameManager.gameData.AttackList[gameManager.gameData.CharStats[vInfo.ID].learnAttacks[i]].nameKey];
				}

				gameManager.gameData.Party[vInfo.ID].attacksLearned.Add(gameManager.gameData.CharStats[vInfo.ID].learnAttacks[i]);
				gameManager.gameData.Party[vInfo.ID].attackAmount++;
			}
		}
	}
}
