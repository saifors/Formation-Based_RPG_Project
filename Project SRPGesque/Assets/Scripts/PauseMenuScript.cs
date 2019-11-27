using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum PauseState { Main, Party, Item, Quest, Save, Load, Quit,  Exit};

public class PauseMenuScript : MonoBehaviour
{
	private GameManager gameManager;
	public Image bg;
    public RectTransform header;
    public RectTransform bottomBar;
    public RectTransform selectionBox;
    public RectTransform partyInfoBoxes;
    public CanvasGroup partyInfoBoxesC;
	public RectTransform locationNameTrans;

	public RectTransform[] options;
	public int selectedOption;

    public bool isAnimating;

	public TextMeshProUGUI locationText;
	public TextMeshProUGUI goldText;
	public TextMeshProUGUI playtimeText;
	
	public RectTransform selectionContainer;
	public RectTransform selectionGraphic;

	private Vector2 axis;
	public float scrollCooldownCounter;
	public float scrollCooldown;

	public Sprite[] portraitSprites;
	public GameObject cInfoPrefab;
	public CharMenuInfo[] charInfo;
	public Transform cInfoBox;
	
	public PauseState state;

	public GameObject saveBox;
	public GameObject LoadBox;

	// Start is called before the first frame update
	public void Init(GameManager gM)
    {
        bg = GetComponent<Image>();
		partyInfoBoxesC = partyInfoBoxes.GetComponent<CanvasGroup>();
		gameManager = gM;
		scrollCooldown = 0.3f;
	}

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isPaused)
		{
			float pS = gameManager.gameData.Misc.playtimeSeconds;
			int hours = Mathf.FloorToInt(pS / 3600);
			int minutes = Mathf.FloorToInt((pS % 3600) / 60);
			int seconds = Mathf.FloorToInt((pS % 3600) % 60);
			playtimeText.text = "Time: " + hours + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
		}

		//if (scrollCooldownCounter <= scrollCooldown + 0.5f)
			scrollCooldownCounter += Time.unscaledDeltaTime;
		if (axis.y < 0 && scrollCooldownCounter >= scrollCooldown)
		{
			
			if(state == PauseState.Main)SelectOption(selectedOption + 1);
			scrollCooldownCounter = 0;

		}
		else if (axis.y > 0 && scrollCooldownCounter >= scrollCooldown)
		{
			if (state == PauseState.Main) SelectOption(selectedOption - 1);
			scrollCooldownCounter = 0;
		}
	}

	public void SetAxis(Vector2 inputAxis)
	{
		axis = inputAxis;
	}

	public void UpdateInfo()
	{
		if (gameManager.gameData.Misc.partyMembers.Count < 3)
		{
			charInfo = new CharMenuInfo[gameManager.gameData.Misc.partyMembers.Count];
			for (int i = 0; i < gameManager.gameData.Misc.partyMembers.Count; i++)
			{
				GameObject obj = Instantiate(cInfoPrefab);
				obj.transform.SetParent(cInfoBox);
				obj.transform.localScale = new Vector3(1, 1, 1);
				obj.transform.localPosition = new Vector3(0, -i * (180 + 5), 0);
				charInfo[i] = obj.GetComponent<CharMenuInfo>();
				charInfo[i].SetUp(gameManager.gameData.Party[gameManager.gameData.Misc.partyMembers[i]], portraitSprites, gameManager);

				//pos.y = -i*(180+5);
			}
		}
		else
		{
			charInfo = new CharMenuInfo[3];
			for (int i = 0; i < 3; i++)
			{
				GameObject obj = Instantiate(cInfoPrefab);
				obj.transform.SetParent(cInfoBox);
				obj.transform.localScale = new Vector3(1, 1, 1);
				RectTransform objTrans = obj.GetComponent<RectTransform>();
				objTrans.anchoredPosition = new Vector3(0, -i * (180 + 5), 0);
				charInfo[i] = obj.GetComponent<CharMenuInfo>();
				charInfo[i].SetUp(gameManager.gameData.Party[gameManager.gameData.Misc.partyMembers[i]], portraitSprites, gameManager);
			}
		}

		for (int i = 0; i < charInfo.Length; i++)
		{
			charInfo[i].SetUp(gameManager.gameData.Party[gameManager.gameData.Misc.partyMembers[i]], portraitSprites, gameManager);
		}

		locationText.text = LanguageManager.langData.mapNames[gameManager.gameData.MapEncounterCollection[gameManager.gameData.Misc.mapID].nameKey];

		goldText.text = "Gold: " + gameManager.gameData.Misc.gold;

		float pS = gameManager.gameData.Misc.playtimeSeconds;
		int hours = Mathf.FloorToInt(pS / 3600);
		int minutes = Mathf.FloorToInt((pS % 3600) / 60);
		int seconds = Mathf.FloorToInt((pS % 3600) % 60);
		playtimeText.text = "Time: " + hours + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
	}

    public void PauseLoadAnimation()
    {
		UpdateInfo();
		selectionGraphic.anchoredPosition = new Vector2(-300, 0);
		isAnimating = true;
        bg.DOFade(0, 0.5f).From();
        header.DOAnchorPosY(180, 0.2f).From();
        bottomBar.DOAnchorPosY(-230, 0.4f).From().SetDelay(0.15f);
        selectionBox.DOAnchorPosX(-352.5f, 0.3f).From().SetDelay(0.15f);
		partyInfoBoxesC.DOFade(0, 0.5f).SetEase(Ease.InSine).From().SetDelay(0.10f);
        partyInfoBoxes.DOAnchorPosX(800, 0.7f).From().SetDelay(0.15f).OnComplete(FinishedAnim);
        locationNameTrans.DOAnchorPosY(603, 0.2f).From().SetDelay(0.15f);

		selectedOption = 0;
    }
    public void FinishedAnim()
    {
		SelectOption(0);
		isAnimating = false;
    }

	public void SelectOption(int optionNum)
	{
		if (optionNum >= options.Length) optionNum = 0;
		else if (optionNum < 0) optionNum = options.Length - 1;
		selectedOption = optionNum;
		selectionGraphic.anchoredPosition = new Vector2(-300,0);

		selectionContainer.DOAnchorPosY(options[optionNum].anchoredPosition.y, 0.1f);
		selectionGraphic.DOAnchorPosX(0, 0.1f).SetDelay(0.05f);
	}

	public void ConfirmOption()
	{
		
		if (state == PauseState.Main)
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
					Quest();
					break;
				case 3:
					Save();
					break;
				case 4:
					Load();
					break;
				case 5:
					Quit();
					break;
				case 6:
					Exit();
					break;
				default:
					break;
			}
		}
	}

	public void Party()
	{

	}
	
	public void Item()
	{

	}
	public void Quest()
	{

	}

	public void Save()
	{
		state = PauseState.Save;
		saveBox.SetActive(true);
	}

	public void Load()
	{

	}

	public void Quit()
	{
		
	}

	public void Exit()
	{
		gameManager.PauseToggle();
	}

	public void Cancel()
	{
		state = PauseState.Main;
		saveBox.SetActive(false);
	}
}
