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
	private float expTimeCounter;
	public bool finishedCounting;
	public bool lastScroll;
	public bool startedCounting;

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
	
	public CanvasGroup levelUpCanvas;
	public Animator levelUpAnim;
	private bool levelAnimStarted;

	public int ID;

	public void Init(int id, string name, int level, int oldGivenExp, int gainedExp, GameManager gM, Sprite portSprite)
	{
		gameManager = gM;
		ID = id;
		levelAnimStarted = false;
		trans = GetComponent<RectTransform>();
		lastScroll = false;
		levelsGained = 0;
		levelCounter = 0;
		oldExp = oldGivenExp;
		newExp = oldExp + gainedExp;

		levelUpCanvas.alpha = 0;
		levelUpAnim.enabled = false;

		nameText.text = name;
		levelOld = level;
		levelNew = levelOld;
		levelText.text = levelOld.ToString();
		portrait.sprite = portSprite;

		//Subtract the exp from previous levels to see how much exp has been gained in the current level.
		int subtractExp = 0;
		for (int i = 1; i < level; i++)
		{
			subtractExp += gameManager.gameData.LevelRequirement[i - 1].exp;
		}
		int totalSub = oldExp - subtractExp;
		//Debug.Log("total sub" + totalSub + "from subtract" + subtractExp);
		//In brackets is the exp from this level which is subtracted from how much exp is required to see the remainder of necessary exp.
		oldRemainingExp = gameManager.gameData.LevelRequirement[levelOld - 1].exp - totalSub;
		//Debug.Log("Old remain exp" + oldRemainingExp);
		//See how much exp will be required. 
//-----------------------------------------------------------------------
		if(oldRemainingExp - gainedExp <= 0)//If levels up
		{
			//Standard, if this if is triggered it means it at the very least has leveled up once
			levelNew++;
			levelsGained++;

			int expSurpassed = oldRemainingExp - gainedExp; //Guaranteed negative number
			
			while (expSurpassed < 0) //As long as its still positive, keep looping
			{

				//Is the exp surpassed enough to level up a second or 'n'th time?
				if (gameManager.gameData.LevelRequirement[levelNew - 1].exp + expSurpassed < 0) // The excess EXP was enough for the level after this so it levels up again
				{
					// Even after the following Exp surpassed will still be negative
					expSurpassed += gameManager.gameData.LevelRequirement[levelNew - 1].exp;
					levelsGained++;
					levelNew++;
					Debug.Log(levelsGained + "fuck" + expSurpassed);
				}
				else //Not enough exp to break through another level, end of the line.
				{

					break;
				}
				
			}
			
			

			//Debug.Log("exp surpassed when you take aqaye the other level ups" + expSurpassed);

			newRemainingExp = gameManager.gameData.LevelRequirement[levelNew - 1].exp + (expSurpassed);
		}
		else newRemainingExp = oldRemainingExp - gainedExp; // if it doesn't level up
		//Debug.Log("[OLD] Level " + levelOld + " with remaining exp " + oldRemainingExp + ". [NEW] level " + levelNew + " with remainging exp " + newRemainingExp + " gaining the following amount of levels " + levelsGained);
		//After the previous batch we now know: The new level, how many levels have been gained and the new remaining experience for the next level.
//-----------------------------------------------------------------------


		deltaRemainingExp = oldRemainingExp;
		remainingExp.text = deltaRemainingExp.ToString();
		finishedCounting = false;
		startedCounting = false;

		//Debug.Log("old exp"+ oldExp + "new" + newExp);
		gameManager.gameData.Party[id].exp = newExp;
		gameManager.gameData.Party[id].level = levelNew;



		/*languageText[0].text = LanguageManager.langData.
		languageText[1].text = LanguageManager.langData. */
	}

	private void Update()
	{
		if (!finishedCounting && startedCounting)
		{
			timeCounter += Time.deltaTime;
				if (timeCounter >= 0.05f)
				{
				// deltaRemainingExp = deltaExp - ;
					remainingExp.text = deltaRemainingExp.ToString();
					
					timeCounter = 0;
				}

			expTimeCounter += Time.deltaTime;
			if(expTimeCounter >= 0.5f)
			{
				gameManager.soundPlayer.PlaySound(5, true);
				expTimeCounter = 0;
			}
			
		}

		if(levelAnimStarted)
		{
			if(levelUpAnim.GetCurrentAnimatorStateInfo(0).IsName("AnimEnded"))
			{
				LevelUpAnimFinished();
			}
		}
	}

	public void ExpAnimStart()
	{
		//Tween for the visual feedback.
		startedCounting = true;


		if (levelsGained > 0)
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
			if(levelCounter > 0) LevelUpAnim();
			//-------
			if (deltaRemainingExp == 0) deltaRemainingExp = gameManager.gameData.LevelRequirement[levelOld - 1 + levelCounter].exp;
			DOTween.To(() => deltaRemainingExp, x => deltaRemainingExp = x, 0, 3 / levelsGained).SetEase(Ease.InOutQuad).OnComplete(ExpAnimLevelCheck);
			levelCounter++;
			
		}
		else if (levelCounter == levelsGained)
		{
			levelText.text = levelNew.ToString();
			LevelUpAnim();
			deltaRemainingExp = gameManager.gameData.LevelRequirement[levelNew - 1].exp;
			DOTween.To(() => deltaRemainingExp, x => deltaRemainingExp = x, newRemainingExp, 3 / levelsGained).SetEase(Ease.InOutQuad).OnComplete(ExpAnimFinished);
			lastScroll = true;
		}
	}

	public void ExpAnimFinished()
	{
		//Debug.Log("FinishExpAnim");
		finishedCounting = true;
		startedCounting = false;
		remainingExp.text = newRemainingExp.ToString();
	}

	public void LevelUpAnim()
	{
		levelUpCanvas.alpha = 1;
		levelAnimStarted = true;
		levelUpAnim.enabled = true;
		gameManager.soundPlayer.PlaySound(4, true);
		levelUpAnim.Play("LevelBurstAnim");
	}
	public void LevelUpAnimFinished()
	{
		levelAnimStarted = false;
		levelUpCanvas.alpha = 0;
		levelUpAnim.enabled = false;
	}
	
}
