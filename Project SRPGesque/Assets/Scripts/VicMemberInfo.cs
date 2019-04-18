using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class VicMemberInfo : MonoBehaviour
{
	private GameManager gameManager;

	public RectTransform trans;

	public TextMeshProUGUI nameText;
	public TextMeshProUGUI levelText;
	public TextMeshProUGUI remainingExp;

	public Image portrait;

	private float timeCounter;
	private bool finishedCounting;

	public int levelOld;
	public int levelNew;
	public int levelsGained;

	private int levelCounter;

	public int deltaExp;
	public int oldExp;
	public int newExp;
	public int deltaRemainingExp;
	public int oldRemainingExp;
	public int newRemainingExp;

	public TextMeshProUGUI[] languageText;

	public void Init(int id, string name, int level, int oldGivenExp, int gainedExp, GameManager gM, Sprite portSprite)
	{
		gameManager = gM;
		trans = GetComponent<RectTransform>();
		levelsGained = 0;
		levelCounter = 0;
		oldExp = oldGivenExp;
		newExp = oldExp + gainedExp;

		nameText.text = name;
		levelOld = level;
		levelNew = levelOld;
		levelText.text = levelOld.ToString();
		portrait.sprite = portSprite;

		//Subtract the exp from previous levels to see how much exp has been gained in the current level.
		int subtractExp = 0;
		for (int i = 1; i < level; i++)
		{
			subtractExp += gameManager.gameData.LevelRequirement[i].exp;
		}
		
		//In brackets is the exp from this level which is subtracted from how much exp is required to see the remainder of necessary exp.
		oldRemainingExp = gameManager.gameData.LevelRequirement[levelOld - 1].exp - (oldGivenExp - subtractExp);
		//See how much exp will be required. This works like a miracle, the gods themselves have descended to bless this chuck of code and say "it gud". 
//-----------------------------------------------------------------------
		if(oldRemainingExp - gainedExp <= 0)
		{
			int expSurpassed = oldRemainingExp - gainedExp;
			while (expSurpassed < 0)
			{
				if (expSurpassed + gameManager.gameData.LevelRequirement[levelNew - 1].exp > 0)
				{
					levelsGained++;
					expSurpassed += gameManager.gameData.LevelRequirement[levelNew - 1].exp;
					levelNew++;
				}
				else break;
				
			}

			newRemainingExp = gameManager.gameData.LevelRequirement[levelNew - 1].exp - (expSurpassed);
		}
		else newRemainingExp = oldRemainingExp - gainedExp;
		Debug.Log("[OLD] Level " + levelOld + " with remaining exp " + oldRemainingExp + ". [NEW] level " + levelNew + " with remainging exp " + newRemainingExp + " gaining the following amount of levels " + levelsGained);
		//After the previous batch we now know: The new level, how many levels have been gained and the new remaining experience for the next level.
//-----------------------------------------------------------------------


		deltaRemainingExp = oldRemainingExp;
		remainingExp.text = deltaRemainingExp.ToString();
		finishedCounting = false;

		
		gameManager.gameData.Party[id].exp += newExp;
		gameManager.gameData.Party[id].level += levelNew - 1;



		/*languageText[0].text = LanguageManager.langData.
		languageText[1].text = LanguageManager.langData. */
	}

	private void Update()
	{
		if (!finishedCounting)
		{
			timeCounter += Time.deltaTime;
			if (timeCounter >= 0.1f)
			{
				//number rising sound.
				//gameManager.soundPlayer.PlaySound();
			}
				if (timeCounter >= 0.05f)
			{
			// deltaRemainingExp = deltaExp - ;
				remainingExp.text = deltaRemainingExp.ToString();
					timeCounter = 0;
			}
		}
	}

	public void ExpAnimStart()
	{
		//Tween for the visual feedback.
		if(levelsGained > 0)
		{
			ExpAnimLevelCheck();
		}
		else DOTween.To(() => deltaRemainingExp, x => deltaRemainingExp = x, newRemainingExp, 3).SetEase(Ease.InOutQuad);
	}

	public void ExpAnimLevelCheck()
	{
		if (levelCounter < levelsGained)
		{
			//------Level Up------
			levelText.text = (levelOld + levelCounter).ToString();
			//-------
			deltaRemainingExp = gameManager.gameData.LevelRequirement[levelOld - 1 + levelCounter].exp;
			DOTween.To(() => deltaRemainingExp, x => deltaRemainingExp = x, 0, 3 / levelsGained).SetEase(Ease.InOutQuad).OnComplete(ExpAnimLevelCheck);
			levelCounter++;
			
		}
		else if (levelCounter == levelsGained)
		{
			levelText.text = levelNew.ToString();
			deltaRemainingExp = gameManager.gameData.LevelRequirement[levelNew - 1].exp;
			DOTween.To(() => deltaRemainingExp, x => deltaRemainingExp = x, newRemainingExp, 3 / levelsGained).SetEase(Ease.InOutQuad).OnComplete(ExpAnimFinished);
			
		}
	}

	public void ExpAnimFinished()
	{
		
		finishedCounting = true;
		remainingExp.text = newRemainingExp.ToString();
	}
}
