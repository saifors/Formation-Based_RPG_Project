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
        //Debug.Log(PlayerPrefs.GetInt("Player0_TileID"));
	}

	public void CreateCharacterStats(string alliance, int charID, int hp, int mp, int atk, int def, int res, int spd)
	{
		
		PlayerPrefs.SetInt(alliance + charID + "Hp", hp);
		PlayerPrefs.SetInt(alliance + charID + "Mp", mp);
		PlayerPrefs.SetInt(alliance + charID + "Attack", atk);
		PlayerPrefs.SetInt(alliance + charID + "Defense", def);
		PlayerPrefs.SetInt(alliance + charID + "Resistance", res);
		PlayerPrefs.SetInt(alliance + charID + "Speed", spd);

        PlayerPrefs.SetInt(alliance + charID + "Current Hp", hp);
        PlayerPrefs.SetInt(alliance + charID + "Current Mp", mp);
    }
    public void CreateCharacterStats(string alliance, int charID, int hp, int mp, int atk, int def, int res, int spd, Vector2 tiles)
    {

        PlayerPrefs.SetInt(alliance + charID + "Hp", hp);
        PlayerPrefs.SetInt(alliance + charID + "Mp", mp);
        PlayerPrefs.SetInt(alliance + charID + "Attack", atk);
        PlayerPrefs.SetInt(alliance + charID + "Defense", def);
        PlayerPrefs.SetInt(alliance + charID + "Resistance", res);
        PlayerPrefs.SetInt(alliance + charID + "Speed", spd);
        PlayerPrefs.SetFloat(alliance + charID + "_TileX", tiles.x);
        PlayerPrefs.SetFloat(alliance + charID + "_TileY", tiles.y);
        //PlayerPrefs.SetInt(charID + "_TileID", Mathf.FloorToInt(tiles.y + tiles.x * rowSize));
    }

    public void SetTileOccupied(string alliance, int charID, Vector2 tiles, int rowSize)
	{
        PlayerPrefs.SetFloat (alliance + charID + "_TileX", tiles.x);
        PlayerPrefs.SetFloat (alliance + charID + "_TileY", tiles.y);
        PlayerPrefs.SetInt (alliance + charID + "_TileID", Mathf.FloorToInt(tiles.y + tiles.x * rowSize));
    }
}
