using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleTransition : MonoBehaviour
{
	public BattleUI battleUI;
	public RectTransform left;
	public RectTransform right;
	float xDestination = 2400;
	float duration = 1;
	int step;

	public void TransitionTo()
	{
		step = 0;
		right.DOAnchorPosX(xDestination, duration, true).SetEase(Ease.InOutQuart).OnComplete(TransitionFrom);
		left.DOAnchorPosX(-xDestination, duration, true).SetEase(Ease.InOutQuart).OnComplete(TransitionFrom);
	}

	public void TransitionFrom()
	{
		step++;
		if(step == 2)
		{
			StartCoroutine(WaitForTransition());
		}
	}

	IEnumerator WaitForTransition()
	{
		yield return new WaitForSeconds(0.1f);
		battleUI.gameManager.InitializeEncounter();
		right.DOAnchorPosX(0, duration, true).SetEase(Ease.InOutQuart);
		left.DOAnchorPosX(0, duration, true).SetEase(Ease.InOutQuart);
	}
}
