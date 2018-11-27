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

	public void CreateCharacterStats(string charID, int hp, int mp, int atk, int def, int res, int spd)
	{
		
		PlayerPrefs.SetInt(charID + "Hp", hp);
		PlayerPrefs.SetInt(charID + "Mp", mp);
		PlayerPrefs.SetInt(charID + "Attack", atk);
		PlayerPrefs.SetInt(charID + "Defense", def);
		PlayerPrefs.SetInt(charID + "Resistance", res);
		PlayerPrefs.SetInt(charID + "Speed", spd);
	}

	public void setTileOccupied(string charID, int tileID)
	{
		PlayerPrefs.SetInt(charID + "_TileID", tileID);
	}
}
