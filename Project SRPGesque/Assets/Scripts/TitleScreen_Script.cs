using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen_Script : MonoBehaviour
{
    public GameObject selectionImageGroup;
    public GameObject loadGamePanel;
    public GameObject optionsPanel;
    public Image[] selectionImage;
    public int titleSelection;
    public int fileSelection;

    public int optionSelection;
    public Color unselectedColor;
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
    public CharacterStats charStats;

    // Use this for initialization
    void Start ()
    {
        selectionImageGroup = GameObject.FindGameObjectWithTag("TitleScreenOptions");
        selectionImage = selectionImageGroup.GetComponentsInChildren<Image>();
        transition = GameObject.FindGameObjectWithTag("Manager").GetComponent<TransitionManager>();
        options = transition.GetComponent<OptionsManager>();
        charStats = transition.GetComponent<CharacterStats>();
        UpdateVideoSettingsText();

        unselectedColor = new Color(0.66f, 0.66f, 0.66f, 0.75f);

        state = TitleState.Title;
        CancelSelection();
        SelectOption(0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        inputAxis = Input.GetAxisRaw("Vertical");

        if(state == TitleState.Title) //Update for Normal title selection
        {
            if (scrollCooldownCounter >= scrollCooldown)
            {
                if (inputAxis <= -1)
                {
                    if (titleSelection >= selectionImage.Length - 1) SelectOption(0);
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
        }
        else if(state == TitleState.Load)
        {
            if (Input.GetKey(KeyCode.X)) CancelSelection();

            if (inputAxis <= -1)
                {
                    
                }
            else if (inputAxis >= 1)
            {
                
            }
        }
        else if (state == TitleState.Options)
        {
            if (Input.GetKey(KeyCode.X)) CancelSelection();

            if (inputAxis <= -1)
                {
                    
                }
            else if (inputAxis >= 1)
            {
                
            }
        }

    }
        
    
    public void UnselectAll()
    {
        for (int i = 0; i < selectionImage.Length; i++)
        {
            selectionImage[i].color = unselectedColor;
            selectionImage[i].transform.localScale = Vector3.one;
        }
    }

    public void SelectOption(int optionNum)
    {
        titleSelection = optionNum;
        UnselectAll();
        selectionImage[optionNum].color = Color.white;
        selectionImage[optionNum].transform.localScale = Vector3.one * 1.1f;
    }
    public void ConfirmSelection(int optionNum)
    {
        if (optionNum == 0) //New Game
        {
            // Set up first character for New game.
            charStats.CreateCharacterStats("Player", 0, 70, 35, 15, 10, 5, 20);
            charStats.SetTileOccupied("Player", 0, new Vector2(1, 1), 8);

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
