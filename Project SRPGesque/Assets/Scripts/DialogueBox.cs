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
	public TextMeshProUGUI[] options;
	public RectTransform[] optionsTrans;
	// Start is called before the first frame update
    void Start()
    {
		optionsTrans = new RectTransform[options.Length];
		for(int i = 0; i < optionsTrans.Length; i++)
		{
			optionsTrans[i] = options[i].GetComponent<RectTransform>();
		}
    }

}
