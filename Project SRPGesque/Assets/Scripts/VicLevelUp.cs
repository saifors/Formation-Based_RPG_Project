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
	
	public void Init(GameManager gM)
    {
		gameManager = gM;

		for (int i = 0; i < statsText.Length; i++)
		{
			//levelUpText.text = LanguageManager.langData. ;
			//statsText[i].text = LanguageManager.langData.stats["Level"] + '\n' + LanguageManager.langData.stats["HP"] + '\n' + LanguageManager.langData.stats["MP"] + '\n' + LanguageManager.langData.stats["Attack"] + '\n' + LanguageManager.langData.stats["Defense"] + '\n' + LanguageManager.langData.stats["Resistance"] + '\n' + LanguageManager.langData.stats["Speed"];
		}
    }
	
	public void DisplayLevelUp(VicMemberInfo vicInfo)
	{
		nameText.text = vicInfo.nameText.text;

		level[0].text = vicInfo.levelOld.ToString();
		level[1].text = vicInfo.levelNew.ToString();

		hp[0].text = gameManager.gameData.Party[vicInfo.ID].hp.ToString(); 
		mp[0].text = gameManager.gameData.Party[vicInfo.ID].mp.ToString(); 
		atk[0].text = gameManager.gameData.Party[vicInfo.ID].attack.ToString(); 
		def[0].text = gameManager.gameData.Party[vicInfo.ID].defense.ToString(); 
		res[0].text = gameManager.gameData.Party[vicInfo.ID].resistance.ToString(); 
		spd[0].text = gameManager.gameData.Party[vicInfo.ID].speed.ToString();

		gameManager.UpdateStats(vicInfo.ID);

		hp[1].text = gameManager.gameData.Party[vicInfo.ID].hp.ToString();
		mp[1].text = gameManager.gameData.Party[vicInfo.ID].mp.ToString();
		atk[1].text = gameManager.gameData.Party[vicInfo.ID].attack.ToString();
		def[1].text = gameManager.gameData.Party[vicInfo.ID].defense.ToString();
		res[1].text = gameManager.gameData.Party[vicInfo.ID].resistance.ToString();
		spd[1].text = gameManager.gameData.Party[vicInfo.ID].speed.ToString();
	}
}
