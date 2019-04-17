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
	public int deltaRemainingExp;
	public int oldRemainingExp;
	public int newRemainingExp;

	public TextMeshProUGUI[] languageText;

	public void Init(string name, int level, int oldGivenExp, int gainedExp, GameManager gM, Sprite portSprite)
	{
		gameManager = gM;
		trans = GetComponent<RectTransform>();
		levelsGained = 0;
		levelCounter = 0;

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
		oldRemainingExp = gameManager.gameData.LevelRequirement[levelOld].exp - (oldGivenExp - subtractExp);
		//See how much exp will be required
//-----------------------------------------------------------------------
		if(oldRemainingExp - gainedExp <= 0)
		{
			int expSurpassed = oldRemainingExp - gainedExp;
			while (expSurpassed < 0)
			{
				if (expSurpassed + gameManager.gameData.LevelRequirement[levelNew].exp > 0)
				{
					levelsGained++;
					expSurpassed += gameManager.gameData.LevelRequirement[levelNew].exp;
					levelNew++;
				}
				else break;
				
			}

			newRemainingExp = gameManager.gameData.LevelRequirement[levelNew].exp + (expSurpassed);
		}
		else newRemainingExp = oldRemainingExp - gainedExp;
		//After the previous batch we now know: The new level, how many levels have been gained and the new remaining experience for the next level.
//-----------------------------------------------------------------------


		deltaRemainingExp = oldRemainingExp;
		remainingExp.text = deltaRemainingExp.ToString();
		finishedCounting = false;
		

		//Tween for the visual feedback.
		if(levelsGained > 0)
		{
			ExpAnimLevelCheck();
		}
		else DOTween.To(() => deltaExp, x => deltaExp = x, oldGivenExp + gainedExp, 5);


		/*languageText[0].text = LanguageManager.langData.
		languageText[1].text = LanguageManager.langData. */
	}

	private void Update()
	{
		if (!finishedCounting)
		{
			timeCounter += Time.deltaTime;
			if (timeCounter >= 0.2f)
			{
				
				remainingExp.text = deltaRemainingExp.ToString();
					timeCounter = 0;
			}
		}
	}

	public void ExpAnimStart()
	{

	}

	public void ExpAnimLevelCheck()
	{
		if (levelCounter < levelsGained)
		{
			deltaExp = gameManager.gameData.LevelRequirement[levelOld + levelCounter].exp;
			DOTween.To(() => deltaExp, x => deltaExp = x, 0, 5 / levelsGained).OnComplete(ExpAnimLevelCheck);
			levelCounter++;
			levelText.text = (levelOld + levelCounter).ToString();
		}
		else if (levelCounter == levelsGained)
		{
			deltaExp = gameManager.gameData.LevelRequirement[levelNew].exp;
			DOTween.To(() => deltaExp, x => deltaExp = x, newRemainingExp, 5 / levelsGained).OnComplete(ExpAnimFinished);
			levelText.text = levelNew.ToString();
		}
	}

	public void ExpAnimFinished()
	{
		finishedCounting = true;
		remainingExp.text = newRemainingExp.ToString();
	}
}
