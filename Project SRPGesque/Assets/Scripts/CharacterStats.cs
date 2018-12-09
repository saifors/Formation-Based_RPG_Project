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

	public void CreateCharacterStats(int charID, int hp, int mp, int atk, int def, int res, int spd)
	{
		
		PlayerPrefs.SetInt(charID + "Hp", hp);
		PlayerPrefs.SetInt(charID + "Mp", mp);
		PlayerPrefs.SetInt(charID + "Attack", atk);
		PlayerPrefs.SetInt(charID + "Defense", def);
		PlayerPrefs.SetInt(charID + "Resistance", res);
		PlayerPrefs.SetInt(charID + "Speed", spd);
	}
    public void CreateCharacterStats(int charID, int hp, int mp, int atk, int def, int res, int spd, Vector2 tiles)
    {

        PlayerPrefs.SetInt(charID + "Hp", hp);
        PlayerPrefs.SetInt(charID + "Mp", mp);
        PlayerPrefs.SetInt(charID + "Attack", atk);
        PlayerPrefs.SetInt(charID + "Defense", def);
        PlayerPrefs.SetInt(charID + "Resistance", res);
        PlayerPrefs.SetInt(charID + "Speed", spd);
        PlayerPrefs.SetFloat(charID + "_TileX", tiles.x);
        PlayerPrefs.SetFloat(charID + "_TileY", tiles.y);
        //PlayerPrefs.SetInt(charID + "_TileID", Mathf.FloorToInt(tiles.y + tiles.x * rowSize));
    }

    public void SetTileOccupied(int charID, Vector2 tiles, int rowSize)
	{
        PlayerPrefs.SetFloat (charID + "_TileX", tiles.x);
        PlayerPrefs.SetFloat (charID + "_TileY", tiles.y);
        PlayerPrefs.SetInt (charID + "_TileID", Mathf.FloorToInt(tiles.y + tiles.x * rowSize));
    }
}
