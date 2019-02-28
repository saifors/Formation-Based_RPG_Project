using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class CharacterData
{
	[XmlAttribute("PlayerID")]
	public int id;
	[XmlElement("Name")]
	public string name;
	[XmlElement("Level")]
	public int level;
	[XmlElement("Exp")]
	public int exp;
	[XmlElement("HP")]
	public int hp;
	[XmlElement("MP")]
	public int mp;
	[XmlElement("Attack")]
	public int attack;
	[XmlElement("Defense")]
	public int defense;
	[XmlElement("Resistance")]
	public int resistance;
	[XmlElement("Speed")]
	public int speed;
	[XmlElement("CurrentHP")]
	public int currentHp;
	[XmlElement("CurrentMP")]
	public int currentMp;

	public CharacterData(int createID)
	{
		id = createID;

		switch (id)
		{
			case 0:
				name = "A'en";
				level = 1;
				exp = 0;

				hp = 10;
				mp = 10;
				attack = 10;
				defense = 10;
				resistance = 10;
				speed = 10;

				currentHp = hp;
				currentMp = mp;
				break;
			case 1:
				name = "Leech";
				level = 2;
				exp = 0;

				hp = 10;
				mp = 11;
				attack = 10;
				defense = 10;
				resistance = 10;
				speed = 10;

				currentHp = hp;
				currentMp = mp;
				break;
			case 2:
				name = "Fenia";
				level = 5;
				exp = 0;

				hp = 10;
				mp = 10;
				attack = 11;
				defense = 10;
				resistance = 10;
				speed = 10;

				currentHp = hp;
				currentMp = mp;
				break;
			case 3:
				name = "Entkid";
				level = 1;
				exp = 0;

				hp = 10;
				mp = 10;
				attack = 10;
				defense = 11;
				resistance = 10;
				speed = 10;

				currentHp = hp;
				currentMp = mp;
				break;
			default:
				break;
		}

	}

}


public class EnemyData
{
	[XmlAttribute("EnemyID")]
	public int id;
	[XmlElement("Name")]
	public string name;
	[XmlElement("Level")]
	public int level;	
	[XmlElement("HP")]
	public int hp;
	[XmlElement("MP")]
	public int mp;
	[XmlElement("Attack")]
	public int attack;
	[XmlElement("Defense")]
	public int defense;
	[XmlElement("Resistance")]
	public int resistance;
	[XmlElement("Speed")]
	public int speed;
	[XmlElement("ExpGain")]
	public int expGain;
	[XmlElement("GoldGain")]
	public int goldGain;

	public EnemyData(int createID)
	{
		id = createID;

		switch (id)
		{
			case 0:
				name = "Slime";
				level = 1;
				hp = 10;
				mp = 13;
				attack = 10;
				defense = 10;
				resistance = 10;
				speed = 10;
				expGain = 20;
				goldGain = 50;
				break;
			default:
				name = "MissingNo.";
				level = 1;
				hp = 5;
				mp = 5;
				attack = 5;
				defense = 5;
				resistance = 5;
				speed = 5;
				expGain = 250;
				goldGain = 550;
				break;
		}
	}
}

public class AttackData
{
	[XmlAttribute("AttackID")]
	public int id;
	[XmlElement("Name")]
	public string name;
	[XmlElement("Description")]
	public string description;
	[XmlElement("IsMagic")]
	public bool isMagic;
	[XmlElement("Strength")]
	public int strength;
	[XmlElement("MPCost")]
	public int mpCost;
	[XmlElement("RangeSize")]
	public Vector2 rangeSize;
	[XmlElement("RangeActive")]
	public int[] rangeActive;

	public AttackData(int createID)
	{
		id = createID;
		switch (id)
		{
			case 0:
				name = "Fire Bolt";
				description = "Magic: Summons an arrow of fire from above to pierce through the foe's body.";
				isMagic = true;
				strength = 20;
				mpCost = 11;
				rangeSize = new Vector2(3, 2);
				rangeActive = new int[] { 1, 0, 1, 0, 1, 0 };
				break;
			case 1:
				name = "Ice Pike";
				description = "Frozen pikes rize from the ground catching the enemy off guard.";
				isMagic = true;
				strength = 23;
				mpCost = 12;
				rangeSize = new Vector2(1, 2);
				rangeActive = new int[] { 1,1};
				break;
			default:
				name = "Dab";
				description = "Physical: Kills off the user's brain cells to make the enemy facepalm so hard it hurts them";
				isMagic = false;
				strength = 7;
				mpCost = 5;
				rangeSize = new Vector2(1, 1);
				rangeActive = new int[] { 1 };
				break;
		}
	}
}

public class FormationData
{
	[XmlAttribute("PlayerID")]
	public int id;
	[XmlElement("TileVector")]
	public Vector2 tiles;
	[XmlElement("TileID")]
	public int tileID;
	public FormationData(int createID)
	{
		id = createID;
		switch (id)
		{
			case 0:
				tiles = new Vector2(3, 2);
				break;
			default:
				tiles = Vector2.zero;
				break;
		}
		tileID = Mathf.FloorToInt(tiles.y + tiles.x * 8);
	}
}
