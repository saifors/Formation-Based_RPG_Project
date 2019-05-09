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

		for (int i = 0; i < 2; i++)
		{
			level[i].text = vicInfo.levelOld.ToString();
			hp[i].text = gameManager.gameData.Party[vicInfo.ID].hp.ToString(); 
			mp[i].text = gameManager.gameData.Party[vicInfo.ID].mp.ToString(); 
			atk[i].text = gameManager.gameData.Party[vicInfo.ID].attack.ToString(); 
			def[i].text = gameManager.gameData.Party[vicInfo.ID].defense.ToString(); 
			res[i].text = gameManager.gameData.Party[vicInfo.ID].resistance.ToString(); 
			spd[i].text = gameManager.gameData.Party[vicInfo.ID].speed.ToString();
		}
		

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
				if(hp[1].text != gameManager.gameData.Party[vInfo.ID].hp.ToString()) hp[1].text = gameManager.gameData.Party[vInfo.ID].hp.ToString();	
				else
				{
					statUpCounter++;
					StatUpAnimFinished();
					return;
				}
				break;
			case 2:
				if(mp[1].text != gameManager.gameData.Party[vInfo.ID].mp.ToString()) mp[1].text = gameManager.gameData.Party[vInfo.ID].mp.ToString();	
				else
				{
					statUpCounter++;
					StatUpAnimFinished();
					return;
				}
				break;
			case 3:
				if(atk[1].text != gameManager.gameData.Party[vInfo.ID].attack.ToString()) atk[1].text = gameManager.gameData.Party[vInfo.ID].attack.ToString();	
				else
				{
					statUpCounter++;
					StatUpAnimFinished();
					return;
				}
				break;
			case 4:
				if(def[1].text != gameManager.gameData.Party[vInfo.ID].defense.ToString()) def[1].text = gameManager.gameData.Party[vInfo.ID].defense.ToString();	
				else
				{
					statUpCounter++;
					StatUpAnimFinished();
					return;
				}
				break;
			case 5:
				if(res[1].text != gameManager.gameData.Party[vInfo.ID].resistance.ToString()) res[1].text = gameManager.gameData.Party[vInfo.ID].resistance.ToString();	
				else
				{
					statUpCounter++;
					StatUpAnimFinished();
					return;
				}
				break;
			case 6:
				if(spd[1].text != gameManager.gameData.Party[vInfo.ID].speed.ToString()) spd[1].text = gameManager.gameData.Party[vInfo.ID].speed.ToString();
				else
				{
					statUpCounter++;
					StatUpAnimFinished();
					return;
				}
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
			//Debug.Log("Yo1");
			if (vInfo.levelOld < gameManager.gameData.CharStats[vInfo.ID].levelAttacks[i] && vInfo.levelNew >= gameManager.gameData.CharStats[vInfo.ID].levelAttacks[i])
			{
				//Debug.Log("Yo2");

				//Gained attack i with this level
				if (attackLearned.text == "")
				{
					attackLearned.text = "Learned: " + LanguageManager.langData.attackName[gameManager.gameData.AttackList[gameManager.gameData.CharStats[vInfo.ID].learnAttacks[i]].nameKey];
				}
				else
				{
					attackLearned.text += ", " + LanguageManager.langData.attackName[gameManager.gameData.AttackList[gameManager.gameData.CharStats[vInfo.ID].learnAttacks[i]].nameKey];
				}
				//Debug.Log(vInfo.ID + "is" + gameManager.gameData.Party[vInfo.ID].name + "attack learned" + gameManager.gameData.CharStats[vInfo.ID].learnAttacks[i]);
				gameManager.gameData.Party[vInfo.ID].attacksLearned.Add(gameManager.gameData.CharStats[vInfo.ID].learnAttacks[i]);
				gameManager.gameData.Party[vInfo.ID].attackAmount++;
			}
		}
	}
}
