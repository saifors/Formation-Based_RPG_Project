using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
	public enum Alliance { Neutral, Player, Enemy}

	public static void CreateCharacterStats(int charID, int level, int hp, int mp, int atk, int def, int res, int spd)
	{


		PlayerPrefs.SetInt(charID + "Hp", hp);
		PlayerPrefs.SetInt(charID + "Mp", mp);
		PlayerPrefs.SetInt(charID + "Attack", atk);
		PlayerPrefs.SetInt(charID + "Defense", def);
		PlayerPrefs.SetInt(charID + "Resistance", res);
		PlayerPrefs.SetInt(charID + "Speed", spd);

        PlayerPrefs.SetInt(charID + "Level", level);

        PlayerPrefs.SetInt(charID + "Current Hp", hp);
        PlayerPrefs.SetInt(charID + "Current Mp", mp);
    }
    public static void CreateCharacterStats(int charID, int level, int hp, int mp, int atk, int def, int res, int spd, Vector2 tiles)
    {
		CreateCharacterStats(charID,level,hp,mp,atk,def,res,spd);


		PlayerPrefs.SetFloat(charID + "_TileX", tiles.x);
        PlayerPrefs.SetFloat(charID + "_TileY", tiles.y);
        //PlayerPrefs.SetInt(charID + "_TileID", Mathf.FloorToInt(tiles.y + tiles.x * rowSize));
    }

    public static void SetTileOccupied(int charID, Vector2 tiles, int rowSize)
	{
		PlayerPrefs.SetFloat (charID + "_TileX", tiles.x);
        PlayerPrefs.SetFloat (charID + "_TileY", tiles.y);
        PlayerPrefs.SetInt (charID + "_TileID", Mathf.FloorToInt(tiles.y + tiles.x * rowSize));
    }
}
