using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TitleScreen_Script : MonoBehaviour
{
	public GameObject selectionImageGroup;
	private CanvasGroup selectionGroupCanvasGroup;
	public GameObject loadGamePanel;
    public GameObject optionsPanel;
	public RectTransform optionsTrans;
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
    public enum TitleState { Title, Load, Options, Wait};
    public TitleState state;

    public Text resolutionText;
    public Text qualityText;
    public OptionsManager options;
    public SoundPlayer soundPlayer;
	Tween arrowTween;

	float timeCounter;
	bool musicStarted;

	public GraphicRaycaster rCaster;
	private Vector3 lastMouseCoordinate;

	public GameData gameData;

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
		optionsTrans = optionsPanel.GetComponent<RectTransform>();

		rCaster = GetComponent<GraphicRaycaster>();

		//CancelSelection();
		selectionImageGroup.SetActive(true);
		optionsPanel.SetActive(false);
		optionsTrans.DOAnchorPosX(1500, 1, true).WaitForCompletion();
		loadGamePanel.SetActive(false);
		state = TitleState.Title;
		
		SelectOption(0);
		arrowTween = arrows.DORotate(new Vector3(180, 0, 0), 1).SetDelay(1).SetLoops(-1, LoopType.Incremental);
		arrowTween.Pause();

		title.DOAnchorPosX(-154.8f, 2).From().SetDelay(0.5f);
		titleCanvas.DOFade(0, 1.5f).From().SetDelay(0.7f);
		selectionGroupCanvasGroup.DOFade(0, 1).From().SetDelay(2.5f).OnComplete(FinishAnim);

		soundPlayer.Init(null, false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (timeCounter <= 2 && !musicStarted)timeCounter += Time.deltaTime;
		else if (!musicStarted)
		{
			musicStarted = true;
			soundPlayer.PlayMusic(0);
		}

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
            if(Input.GetKeyDown(KeyCode.Z))soundPlayer.PlaySound(0, true);

			Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
			// Check whther has moved
			if (mouseDelta != Vector3.zero) 
				{
				GetMouseOverOption();
			}
			// Then we store our mousePosition so that we can check it again next frame.
			lastMouseCoordinate = Input.mousePosition;

			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				if(GetMouseOverOption())
				{
					ConfirmSelection(titleSelection);
				}
			}
			
        }
        else if(state == TitleState.Load)
        {
            if (Input.GetKey(KeyCode.X)) 
            {
                CancelSelection();
                soundPlayer.PlaySound(1, true);
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
                soundPlayer.PlaySound(1, true);
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
		arrowTween.Play();
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
			NewGame();

            transition.FadeToSceneChange(false, NewGameSceneID);
        }
        else if (optionNum == 1)
        {
			state = TitleState.Wait;
			
			selectionImageGroup.SetActive(false);
            optionsPanel.SetActive(false);
            loadGamePanel.SetActive(true);
			if (titleCanvas.alpha == 1) titleCanvas.DOFade(0, 0.5f).OnComplete(CompleteTransition);
            //state = TitleState.Load;
        }
        else if (optionNum == 2)
        {
			state = TitleState.Wait;
			
			selectionImageGroup.SetActive(false);
            optionsPanel.SetActive(true);
			optionsTrans.DOAnchorPosX(0, 1, true).WaitForCompletion();
            loadGamePanel.SetActive(false);
            UpdateVideoSettingsText();
			if (titleCanvas.alpha == 1) titleCanvas.DOFade(0, 0.5f).OnComplete(CompleteTransition);
            //state = TitleState.Options;
        }
        else if (optionNum == 3) Application.Quit();
    }

	public void NewGame()
	{
		gameData = GameDataManager.NewGame();
		transition.gameData = gameData;
		//gameData = GameDataManager.Load("spelQuick.od");
		NewGameSceneID = gameData.Misc.mapID + 3;
		/*gameData.Misc.pos = new Vector3(0,0.5f,0);
		Debug.Log("New Game" + gameData.Misc.pos);*/
		//gameData.Misc.pos.y = 0.5f;
		gameData.Misc.partyMembers = new List<int>();
		gameData.Misc.partyMembers.Add(0);
		gameData.Misc.partyMembers.Add(1);
		//Debug.Log(gameData.Misc.partyMembers.Count);
		gameData.Misc.partyMembers.Add(2);
		gameData.Misc.gold = 2000;
		PlayerPrefs.SetString("CurrentFile", "spelQuick.od");
		GameDataManager.Save(gameData, PlayerPrefs.GetString("CurrentFile"));
		//gameData.Misc.rot = Vector3.zero;
	}

	public void CompleteTransition()
	{
		if (loadGamePanel.activeSelf) state = TitleState.Load;
		else if (optionsPanel.activeSelf) state = TitleState.Options;
		else state = TitleState.Title;
	}

    public void CancelSelection()
    {
		
		state = TitleState.Wait;
        selectionImageGroup.SetActive(true);
        optionsPanel.SetActive(false);
		optionsTrans.DOAnchorPosX(1500, 1, true).WaitForCompletion();
        loadGamePanel.SetActive(false);
		if (titleCanvas.alpha == 0) titleCanvas.DOFade(1, 0.5f).OnComplete(CompleteTransition);
	}

	
    
    public void UpdateVideoSettingsText()
    {
        resolutionText.text = options.resolutions[options.resolutionOption];
        qualityText.text = options.qualities[options.qualityOption];
    }

	public bool GetMouseOverOption()
	{
		
		//Set up the new Pointer Event
		PointerEventData pointerData = new PointerEventData(EventSystem.current);
		List<RaycastResult> results = new List<RaycastResult>();

		//Raycast using the Graphics Raycaster and mouse click position
		pointerData.position = Input.mousePosition;
		this.rCaster.Raycast(pointerData, results);

		bool floatingOverOption = false;
		//For every result returned, output the name of the GameObject on the Canvas hit by the Ray
		foreach (RaycastResult result in results)
		{
			for (int i = 0; i < selectionOption.Length; i++)
			{
				if (result.gameObject == selectionOption[i].gameObject)
				{
					SelectOption(i);
					floatingOverOption = true;
					break;
				}
			}
		}
		return floatingOverOption;
	}
}
