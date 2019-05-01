using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
	public Image[] portraits;
	public RectTransform textBox;
	public TextMeshProUGUI textBoxText;

	public RectTransform speakerBox;
	public Vector2[] speakerPositions;
	private TextMeshProUGUI speakerName;

	public TextMeshProUGUI[] options;
	private RectTransform[] optionsTrans;

	private GameManager gameManager;

	private DialogueData diaData;

	private int diaProgress;

	public Sprite[] portSprites;
	public Color[] activityColors;

	public void Init(GameManager gM)
	{
		gameManager = gM;
		optionsTrans = new RectTransform[options.Length];
		for (int i = 0; i < optionsTrans.Length; i++)
		{
			optionsTrans[i] = options[i].GetComponent<RectTransform>();
		}
		speakerPositions = new Vector2[2];
		speakerPositions[0] = new Vector2(-615, 20);
		speakerPositions[1] = new Vector2(615, 20);
		
		portSprites = new Sprite[gameManager.gameData.speakerPortrait.Length];
		for (int i = 0; i < portSprites.Length; i++)
		{
			portSprites[i] = gameManager.gameData.speakerPortrait[i];
		}

		speakerName = speakerBox.GetComponentInChildren<TextMeshProUGUI>();

		
	}

	public void StartDialogue(int diaID)
	{

		//Debug.Log("test2");
		gameObject.SetActive(true);
		//Debug.Log("test3");
		diaData = gameManager.gameData.DialogueCollection[diaID];
		

		
		if(portSprites.Length > gameManager.gameData.SpeakerCol[diaData.dialogueSpeakerL].portraitID) portraits[0].sprite = portSprites[ gameManager.gameData.SpeakerCol[diaData.dialogueSpeakerL].portraitID ];
		
		if (portSprites.Length > gameManager.gameData.SpeakerCol[diaData.dialogueSpeakerR].portraitID) portraits[1].sprite = portSprites[ gameManager.gameData.SpeakerCol[diaData.dialogueSpeakerR].portraitID ];
		
		gameManager.gameState = GameManager.GameState.Text;
		diaProgress = 0;
		DialogueLoop();
	}

	public void DialogueLoop()
	{
		//Debug.Log(diaData.dialogueTexts[diaProgress]);
		textBoxText.text = LanguageManager.langData.dialogue[ diaData.dialogueTexts[diaProgress] ];

		if (diaData.speakerDirection[diaProgress] == false)
		{
			speakerBox.anchoredPosition = speakerPositions[0];
			
			portraits[0].color = activityColors[1];
			portraits[1].color = activityColors[0];
			speakerName.text = gameManager.gameData.SpeakerCol[ diaData.dialogueSpeakerL ].nameKey;
		}
		else if (diaData.speakerDirection[diaProgress] == true)
		{
			speakerBox.anchoredPosition = speakerPositions[1];
			portraits[0].color = activityColors[0];
			portraits[1].color = activityColors[1];
			speakerName.text = gameManager.gameData.SpeakerCol[ diaData.dialogueSpeakerR ].nameKey;
		}
		diaProgress++;
	}

	public void DialogueNext()
	{
		if (diaProgress >= diaData.dialogueTexts.Length) //has reached the end of the dialogue.
		{
			//end
			DialogueEnd();
		}
		else
		{
			DialogueLoop();
		}
	}

	public void DialogueEnd()
	{
		gameObject.SetActive(false);
		gameManager.eventManager.ContinueEvent();

		
	}
}
