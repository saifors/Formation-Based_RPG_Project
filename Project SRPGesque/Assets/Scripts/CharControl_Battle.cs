using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControl_Battle : MonoBehaviour 
{
	public string charID;
	public int hp;
	public int mp;
	public int atk;
	public int def;
	public int res;
	public int spd;
	public int tileID;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(string charID)
	{
		hp = PlayerPrefs.GetInt(charID + "Hp");
		mp = PlayerPrefs.GetInt(charID + "Mp");
		atk = PlayerPrefs.GetInt(charID + "Attack");
		def = PlayerPrefs.GetInt(charID + "Defense");
		res = PlayerPrefs.GetInt(charID + "Resistance");
		spd = PlayerPrefs.GetInt(charID + "Speed");
		tileID =  PlayerPrefs.GetInt(charID + "TileID");
		//transform.position = 
	}
}
