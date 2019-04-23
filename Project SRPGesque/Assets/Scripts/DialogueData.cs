using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class DialogueData
{
	public int dialogueID;
	public int[] dialogueTexts;
	public int dialogueSpeakerL;
	public int dialogueSpeakerR;
	public bool[] speakerDirection; // false = L. true = R

	public DialogueData(string diaString)
	{
		string[] cols = diaString.Split('\t');

		dialogueID = int.Parse(cols[0]);

		List<int> diaTemp = new List<int>();
		string[] dia = cols[1].Split(',');
		for (int i = 0; i < dia.Length; i++)
		{
			diaTemp.Add(int.Parse(dia[i]));
		}
		dialogueTexts = diaTemp.ToArray();

		dialogueSpeakerL = int.Parse(cols[2]);
		dialogueSpeakerR = int.Parse(cols[3]);

		string[] speaker = cols[4].Split(',');
		speakerDirection = new bool[speaker.Length];
		if (speaker.Length != dialogueTexts.Length) Debug.LogError("Dialogue ID: " + dialogueID + "'s amount of speakerDirections andTexts don't line up");
		else
		{
			for (int i = 0; i < speakerDirection.Length; i++)
			{
				speakerDirection[i] = bool.Parse(speaker[i]);
			}
		}
		
	}
}
