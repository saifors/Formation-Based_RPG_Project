using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using DG.Tweening;

public class TurnAnims : MonoBehaviour
{
	public BattleUI battleUI;

	public GameObject gObject;

	public Image[] images;
	public Color[] allianceColors;
	public TextMeshProUGUI turnText;
	public RectTransform trans;
	public float[] startPositions;
	public Vector2[] endPositions;
	public CanvasGroup canvas;

	GameManager gameManager;
	

	// Start is called before the first frame update
	public void Init(GameManager gM)
    {
		trans = this.GetComponent<RectTransform>();
		battleUI = GetComponentInParent<BattleUI>();
		gObject = this.gameObject;
		canvas = GetComponent<CanvasGroup>();

		gameManager = gM;
    }

    public void StartTurnAnim(string name, CharacterStats.Alliance alliance)
	{
		
		canvas.alpha = 1;

		gameManager.soundPlayer.PlaySound(7, true);

		turnText.text = name + " Turn";
		if(alliance == CharacterStats.Alliance.Player)
		{
			for (int i = 0; i < images.Length; i++)
			{
				images[i].color = allianceColors[0];
			}

			trans.anchoredPosition = endPositions[0];

			trans.DOAnchorPosX(startPositions[0], 0.25f, true).From().OnComplete(FadeAway);
		}
		else if (alliance == CharacterStats.Alliance.Enemy)
		{
			for (int i = 0; i < images.Length; i++)
			{
				images[i].color = allianceColors[1];
			}
			trans.anchoredPosition = endPositions[1];
			trans.DOAnchorPosX(startPositions[1], 0.25f, true).From().OnComplete(FadeAway);
		}
	}

	public void FadeAway()
	{
		canvas.DOFade(0, 0.15f).SetDelay(0.4f).OnComplete(FinishedFade);
	}

	public void FinishedFade()
	{
		battleUI.gameManager.FinishedTurnAnim();
		gObject.SetActive(false);
	}
}
