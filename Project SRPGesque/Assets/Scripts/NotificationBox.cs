using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class NotificationBox : MonoBehaviour
{
	private GameManager gameManager;
	public TextMeshProUGUI text;
	public CanvasGroup canvas;

	// Start is called before the first frame update
    public void Init(GameManager gM)
    {
		gameManager = gM;
		text = GetComponentInChildren<TextMeshProUGUI>();
		canvas = GetComponent<CanvasGroup>();
		gameObject.SetActive(false);
    }

    public void DisplayNotif(string inputText)
	{
		gameObject.SetActive(true);
		text.text = inputText;
		canvas.alpha = 1;

		canvas.DOFade(0, 0.25f).From().OnComplete(FadeAway);
	}

	public void FadeAway()
	{
		canvas.DOFade(0, 0.25f).SetDelay(0.75f).OnComplete(FadeComplete);
	}

	public void FadeComplete()
	{
		gameObject.SetActive(false);
	}
}
