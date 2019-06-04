using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenuScript : MonoBehaviour
{
	private GameManager gameManager;
	public Image bg;
    public RectTransform header;
    public RectTransform bottomBar;
    public RectTransform selectionBox;
    public RectTransform partyInfoBoxes;
    public RectTransform locationNameTrans;

	public RectTransform[] options;
	public int selectedOption;

    public bool isAnimating;

    // Start is called before the first frame update
    public void Init(GameManager gM)
    {
        bg = GetComponent<Image>();
		gameManager = gM;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseLoadAnimation()
    {
        isAnimating = true;
        bg.DOFade(0, 0.7f).From();
        header.DOAnchorPosY(180, 0.5f).From();
        bottomBar.DOAnchorPosY(-230, 1).From().SetDelay(0.2f);
        selectionBox.DOAnchorPosX(-352.5f, 0.5f).From().SetDelay(0.3f);
        partyInfoBoxes.DOAnchorPosX(1450, 0.7f).From().SetDelay(0.5f).OnComplete(FinishedAnim);
        locationNameTrans.DOAnchorPosY(603, 0.3f).From().SetDelay(0.2f);

		selectedOption = 0;
    }
    public void FinishedAnim()
    {
        isAnimating = false;
    }



	public void ConfirmOption()
	{
		switch (selectedOption)
		{
			case 0:
				Party();
				break;
			case 1:
				Item();
				break;
			case 2:
				Save();
				break;
			case 3:
				Load();
				break;
			case 4:
				Exit();
				break;
			case 5:
				break;
			default:
				break;
		}
	}

	public void Party()
	{

	}
	
	public void Item()
	{

	}

	public void Save()
	{

	}

	public void Load()
	{

	}

	public void Exit()
	{
		gameManager.PauseToggle();
	}
}
