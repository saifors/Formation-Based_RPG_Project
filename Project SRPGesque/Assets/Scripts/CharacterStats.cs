using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void CreateCharacterStats(char charID, int hp, int mp, int atk, int def, int spd)
	{
		PlayerPrefs.SetInt(charID + "Hp", hp);
		PlayerPrefs.SetInt(charID + "Mp", mp);
		PlayerPrefs.SetInt(charID + "Attack", atk);
		PlayerPrefs.SetInt(charID + "Defense", def);
		PlayerPrefs.SetInt(charID + "Speed", spd);
	}
}
