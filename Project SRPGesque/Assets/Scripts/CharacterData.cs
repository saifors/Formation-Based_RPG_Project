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

	public CharacterData(int createID)
	{
		id = createID;

		switch (id)
		{
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
			default:
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
			default:
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
}
