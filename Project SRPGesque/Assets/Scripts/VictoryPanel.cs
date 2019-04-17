using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class VictoryPanel : MonoBehaviour
{
	private RectTransform trans;
	public BattleUI battleUI;
	public GameManager gameManager;

	public GameObject vicInfoPrefab;
	private VicMemberInfo[] vicInfo;

	int expGains;
	int goldGains;

	public TextMeshProUGUI goldText;
	public TextMeshProUGUI expText;

	public TextMeshProUGUI[] textForLanguage;

	CanvasGroup canvas;

	public void Init()
	{
		battleUI = GetComponentInParent<BattleUI>();
		trans = GetComponent<RectTransform>();
		canvas = GetComponent<CanvasGroup>();
		gameManager = battleUI.gameManager;
			//--------Language-----------
		//textForLanguage[0].text = LanguageManager.langData.;
	}

	public void DisplayVictory()
	{
		gameManager.CalculateRewards();

		expGains = gameManager.totalExp;
		goldGains = gameManager.totalGold;

		goldText.text = goldGains.ToString();
		expText.text = expGains.ToString();

		vicInfo = new VicMemberInfo[gameManager.partyMembers];

		for (int i = 0; i < gameManager.partyMembers; i++)
		{
			GameObject obj = Instantiate(vicInfoPrefab);
			vicInfo[i] = obj.GetComponent<VicMemberInfo>();
			vicInfo[i].Init(gameManager.gameData.Party[i].name, gameManager.gameData.Party[i].level, gameManager.gameData.Party[i].exp, expGains, gameManager, battleUI.portraitSprites[i]);
			vicInfo[i].trans.SetParent(trans);
			vicInfo[i].trans.anchoredPosition = new Vector2(-580, 143);
			vicInfo[i].trans.localScale = Vector2.one;
		}

		trans.DOAnchorPosY(1030, 1.5f, true).SetEase(Ease.OutSine).From().OnComplete(FinishAnim);
		canvas.alpha = 0;
		canvas.DOFade(1, 1);
	}

	public void FinishAnim()
	{
		gameManager.selecting = GameManager.SelectingMenu.victoryScreen;
	}
}
