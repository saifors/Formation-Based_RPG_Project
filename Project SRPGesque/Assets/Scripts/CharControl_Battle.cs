using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControl_Battle : MonoBehaviour 
{
	public int charID;
	public int hp;
	public int mp;
	public int atk;
	public int def;
	public int res;
	public int spd;

    public int[] attacks;
    public int attacksAmount;
    public int maxAttacks;

    public Vector2 tile;
	public int tileID;
    public int rowSize;

	// Use this for initialization
	void Start () 
	{
        attacks = new int[attacksAmount];
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
		tile.x = PlayerPrefs.GetFloat(charID + "_TileX");
		tile.y = PlayerPrefs.GetFloat(charID + "_TileY");
        tileID = Mathf.FloorToInt(tile.y + tile.x* rowSize);
        //transform.position = 
    }
    public void UpdateTileID()
    {
        tile.x = PlayerPrefs.GetFloat(charID + "_TileX");
        tile.y = PlayerPrefs.GetFloat(charID + "_TileY");
        tileID = Mathf.FloorToInt(tile.y + tile.x * rowSize);
    }
}
