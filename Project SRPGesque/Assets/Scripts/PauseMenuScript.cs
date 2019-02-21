using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenuScript : MonoBehaviour
{
    public Image bg;
    public RectTransform header;
    public RectTransform bottomBar;
    public RectTransform selectionBox;
    public RectTransform partyInfoBoxes;
    public RectTransform locationNameTrans;

    public bool isAnimating;

    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<Image>();
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

    }
    public void FinishedAnim()
    {
        isAnimating = false;
    }
}
