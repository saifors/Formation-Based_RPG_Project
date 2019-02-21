using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
	public TextMeshProUGUI gameOverText;
	public TextMeshProUGUI pressText;
	public RectTransform pressTrans;
	public float separationValue;
	public bool finishedScroll;
	public TransitionManager transition;

    public Image bgElementChar;
    public Image bg;

    //815
    // Start is called before the first frame update
    void Start()
    {
		pressTrans = pressText.GetComponent<RectTransform>();
		finishedScroll = false;
		DOTween.To(() => separationValue, x => separationValue = x, 815, 5).From().OnComplete(FinishedGOverScroll);

        bgElementChar.DOFade(0, 3).From().SetDelay(3).SetEase(Ease.InOutSine);
        bg.DOColor(new Color(0.5f, 0.5f, 0.5f, 1), 4).SetEase(Ease.InOutSine).From().SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
		if(!finishedScroll) gameOverText.wordSpacing = separationValue;
		else
		{
			if(Input.GetKeyDown(KeyCode.Z))
			{
				transition.FadeToSceneChange(false, 1);
			}
		}
    }
	public void FinishedGOverScroll()
	{
		gameOverText.wordSpacing = 0;
		finishedScroll = true;
		pressText.DOFade(1, 2).OnComplete(TurnOnFlicker);
		pressTrans.DOAnchorPosY(-33, 2, true).From();
	}
	public void TurnOnFlicker()
	{
		pressText.DOFade(0.3f, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
	}
}
