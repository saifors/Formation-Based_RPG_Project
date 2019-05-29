using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditsScript : MonoBehaviour
{
	public RectTransform credits;
	public CanvasGroup end;

	public TransitionManager transition;

	private void Start()
	{
		credits.DOAnchorPosY(0, 15, true).SetEase(Ease.Linear).SetDelay(1).OnComplete(Fin);
		transition = GameObject.FindGameObjectWithTag("Manager").GetComponent<TransitionManager>();
	}

	public void Fin()
	{
		end.DOFade(1, 1.5f).OnComplete(BackToTitle);
	}

	public void BackToTitle()
	{
		StartCoroutine(ReturnTitle());
	}

	IEnumerator ReturnTitle()
	{
		yield return new WaitForSeconds(2);
		transition.FadeToSceneChange(false, 1);
	}
}
