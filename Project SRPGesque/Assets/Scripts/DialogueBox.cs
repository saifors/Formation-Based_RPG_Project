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
	private TextMeshProUGUI speakerName;

	public TextMeshProUGUI[] options;
	private RectTransform[] optionsTrans;

	private GameManager gameManager;

	private DialogueData diaData;

	private int diaProgress;

	public void Init(GameManager gM)
	{
		optionsTrans = new RectTransform[options.Length];
		for (int i = 0; i < optionsTrans.Length; i++)
		{
			optionsTrans[i] = options[i].GetComponent<RectTransform>();
		}

		speakerName = speakerBox.GetComponentInChildren<TextMeshProUGUI>();

		gameManager = gM;
	}

	public void StartDialogue(int diaID)
	{
		diaData = gameManager.gameData.DialogueCollection[diaID];
		diaProgress = 0;
		DialogueLoop();
	}

	public void DialogueLoop()
	{
		textBoxText.text = LanguageManager.langData.dialogue[ diaData.dialogueTexts[diaProgress].ToString() ];
		diaProgress = 0;
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
		//gameManager.eventManager.
	}
}
