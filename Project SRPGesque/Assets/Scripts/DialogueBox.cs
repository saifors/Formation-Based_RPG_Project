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
	}
}
