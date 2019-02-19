using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleScreen_Script : MonoBehaviour
{
	public GameObject selectionImageGroup;
	private CanvasGroup selectionGroupCanvasGroup;
	public GameObject loadGamePanel;
    public GameObject optionsPanel;
	public RectTransform title;
	private CanvasGroup titleCanvas;
    public RectTransform[] selectionOption;
	private CanvasGroup[] selectionCanvasGroup;
	public RectTransform arrows;
    public int titleSelection;
    public int fileSelection;

	private bool animFinished;

    public int optionSelection;
    public float unselectedAlpha;
    public float inputAxis;
    public float scrollCooldown = 0.25f;
    public float scrollCooldownCounter;
    public int NewGameSceneID;
    private TransitionManager transition;
    public enum TitleState { Title, Load, Options};
    public TitleState state;

    public Text resolutionText;
    public Text qualityText;
    public OptionsManager options;
    public SoundPlayer soundPlayer;

    // Use this for initialization
    void Start ()
    {
		titleCanvas = title.GetComponent<CanvasGroup>();
		selectionGroupCanvasGroup = selectionImageGroup.GetComponent<CanvasGroup>();
		selectionCanvasGroup = new CanvasGroup[selectionOption.Length];
		for (int i = 0; i < selectionOption.Length; i++) selectionCanvasGroup[i] = selectionOption[i].GetComponent<CanvasGroup>();
		transition = GameObject.FindGameObjectWithTag("Manager").GetComponent<TransitionManager>();
        options = transition.GetComponent<OptionsManager>();
        soundPlayer = transition.GetComponent<SoundPlayer>();
        UpdateVideoSettingsText();

        unselectedAlpha = 0.6f;
		animFinished = false;

		state = TitleState.Title;
		CancelSelection();
		SelectOption(0);

		title.DOAnchorPosX(-154.8f, 2).From().SetDelay(0.5f);
		titleCanvas.DOFade(0, 1.5f).From().SetDelay(0.7f);
		selectionGroupCanvasGroup.DOFade(0, 1).From().SetDelay(2.5f).OnComplete(FinishAnim);

        
	}
	
	// Update is called once per frame
	void Update ()
    {
        inputAxis = Input.GetAxisRaw("Vertical");
		if (!animFinished) return;

        if(state == TitleState.Title) //Update for Normal title selection
        {
            if (scrollCooldownCounter >= scrollCooldown)
            {
                if (inputAxis <= -1)
                {
                    if (titleSelection >= selectionOption.Length - 1) SelectOption(0);
                    else SelectOption(titleSelection + 1);
                    scrollCooldownCounter = 0;
                }
                else if (inputAxis >= 1)
                {
                    if (titleSelection <= 0) SelectOption(3);
                    else SelectOption(titleSelection - 1);
                    scrollCooldownCounter = 0;
                }
            }
            else scrollCooldownCounter += Time.deltaTime;
            if (Input.GetKey(KeyCode.Z)) ConfirmSelection(titleSelection);
            if(Input.GetKeyDown(KeyCode.Z))soundPlayer.PlaySound(0, 1, true);
        }
        else if(state == TitleState.Load)
        {
            if (Input.GetKey(KeyCode.X)) 
            {
                CancelSelection();
                soundPlayer.PlaySound(1, 1, true);
            }

            if (inputAxis <= -1)
                {
                    
                }
            else if (inputAxis >= 1)
            {
                
            }
        }
        else if (state == TitleState.Options)
        {
            if (Input.GetKey(KeyCode.X)) 
            {
                CancelSelection();
                soundPlayer.PlaySound(1, 1, true);
            }

            if (inputAxis <= -1)
                {
                    
                }
            else if (inputAxis >= 1)
            {
                
            }
        }

    }

	public void FinishAnim()
	{
		
		animFinished = true;
	}
        
    
    public void UnselectAll()
    {
        for (int i = 0; i < selectionOption.Length; i++)
        {
            selectionCanvasGroup[i].alpha = unselectedAlpha;
            selectionOption[i].DOScale(Vector3.one * 0.75f, 0.15f);
        }
    }

    public void SelectOption(int optionNum)
    {
        titleSelection = optionNum;
        UnselectAll();
		selectionCanvasGroup[optionNum].alpha = 1;
        selectionOption[optionNum].DOScale(Vector3.one, 0.15f);
		arrows.DOAnchorPosY(selectionOption[optionNum].localPosition.y, 0.15f, true);
    }
    public void ConfirmSelection(int optionNum)
    {
        
        if (optionNum == 0) //New Game
        {
            // Set up first character for New game.
            CharacterStats.CreateCharacterStats(0, 1, 70, 35, 15, 10, 5, 20);
            CharacterStats.SetTileOccupied(0, new Vector2(1, 1), 8);

            transition.FadeToSceneChange(false, NewGameSceneID);
        }
        else if (optionNum == 1)
        {
            selectionImageGroup.SetActive(false);
            optionsPanel.SetActive(false);
            loadGamePanel.SetActive(true);
            state = TitleState.Load;
        }
        else if (optionNum == 2)
        {
            
            selectionImageGroup.SetActive(false);
            optionsPanel.SetActive(true);
            loadGamePanel.SetActive(false);
            UpdateVideoSettingsText();
            state = TitleState.Options;
        }
        else if (optionNum == 3) Application.Quit();
    }
    public void CancelSelection()
    {
        
        state = TitleState.Title;
        selectionImageGroup.SetActive(true);
        optionsPanel.SetActive(false);
        loadGamePanel.SetActive(false);
    }
    
    public void UpdateVideoSettingsText()
    {
        resolutionText.text = options.resolutions[options.resolutionOption];
        qualityText.text = options.qualities[options.qualityOption];
    }
}
