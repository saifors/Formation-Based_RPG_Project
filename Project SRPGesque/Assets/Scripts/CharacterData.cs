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
	[XmlElement("Model")]
	public int modelId;
	[XmlElement("Attacks")]
	public List<int> attacksLearned;
	public int attackAmount;
	

	public CharacterData(int createID)
	{
		id = createID;
		attacksLearned = new List<int>();

		switch (id)
		{
			case 0:
				name = "A'en";
				level = 1;
				exp = 0;

				hp = 100;
				mp = 100;
				attack = 10;
				defense = 10;
				resistance = 10;
				speed = 10;

				currentHp = hp;
				currentMp = mp;

				modelId = 0;

				attacksLearned.Add(2);
				attacksLearned.Add(1);
				attacksLearned.Add(0);

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

				modelId = 1;

				attacksLearned.Add(0);
				attacksLearned.Add(1);
				attacksLearned.Add(2);

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

				modelId = 2;

				attacksLearned.Add(0);
				attacksLearned.Add(1);
				attacksLearned.Add(2);

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

				modelId = 3;

				attacksLearned.Add(0);
				attacksLearned.Add(1);
				attacksLearned.Add(2);

				break;
			default:
				break;
		}
		attackAmount = attacksLearned.Count;
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

	[XmlElement("Model")]
	public int modelId;
	[XmlElement("Attacks")]
	public List<int> attacksLearned;
	public int attackAmount;

	public EnemyData(int createID)
	{
		id = createID;
		attacksLearned = new List<int>();
		switch (id)
		{
			case 0:
				name = "Slime";
				level = 1;
				hp = 100;
				mp = 80;
				attack = 10;
				defense = 10;
				resistance = 10;
				speed = 10;
				expGain = 20;
				goldGain = 50;

				modelId = 4;

				attacksLearned.Add(0);
				attacksLearned.Add(1);
				attacksLearned.Add(2);
				break;
			default:
				name = "MissingNo.";
				level = 1;
				hp = 100;
				mp = 50;
				attack = 5;
				defense = 5;
				resistance = 5;
				speed = 5;
				expGain = 250;
				goldGain = 550;

				modelId = 5;

				attacksLearned.Add(0);
				attacksLearned.Add(1);
				attacksLearned.Add(2);
				break;
		}
		attackAmount = attacksLearned.Count;
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
				description = "Magic: Summons an arrow of fire from above to pierce through the foe's body.";
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
	[XmlAttribute("FormationCharID")]
	public int id;
	[XmlElement("Monster/CharID")]
	public int classID;
	[XmlElement("IsMonster")]
	public bool isMonster;
	[XmlElement("TileVector")]
	public Vector2 tiles;
	[XmlElement("TileID")]
	public int tileID;
	public FormationData(int createID)
	{
		id = createID;
		switch (id)
		{
			//Players X >= 3
			case 0://A'en
				isMonster = false;
				tiles = new Vector2(3, 2);
				break;
			case 1://Leech
				isMonster = false;
				tiles = new Vector2(3, 4);
				break;
			case 2://Fenia
				isMonster = false;
				tiles = new Vector2(4, 2);
				break;
			case 3://Entkid
				isMonster = false;
				tiles = new Vector2(3, 3);
				break;
			//From here on enemies //X < 3
			case 4:
				isMonster = true;
				classID = 0;
				tiles = new Vector2(2,4);
				break;
			case 5:
				isMonster = true;
				classID = 1;
				tiles = new Vector2(0, 3);
				break;
			case 6://New Group
				isMonster = true;
				classID = 1;
				tiles = new Vector2(1, 5);
				break;
			case 7:
				isMonster = true;
				classID = 0;
				tiles = new Vector2(1, 7);
				break;
			case 8:
				isMonster = true;
				classID = 1;
				tiles = new Vector2(2, 6);
				break;
			default:
				isMonster = true;
				classID = 0;
				tiles = Vector2.zero;
				break;
		}
		if (!isMonster) classID = id;
		tileID = Mathf.FloorToInt(tiles.y + tiles.x * 8);
	}
}

public class FullFormationData
{
	//Complication: is this shit even doable, should I make a seperate Formation Data?
	//Load up a list*1 of lists*2 where one will be all the types of groups you'll encounter and 2 the enemies in those groups 
	[XmlAttribute("Encounter")]
	public int id;
	[XmlElement("Amount")]
	public int iterations; //Iterations = how many Formation Datas does this take into the group of formations
	[XmlElement("EnemyFormation")]
	public List<FormationData> formation;
	

	public FullFormationData(int createID, int formCounter)
	{
		id = createID;
		
		formation = new List<FormationData>();

		switch(id)
		{
			case 0:
				//Player Full formation
				iterations = 4;		
				break;
			case 1:
				//Enemy Formations form here on out
				iterations = 2;
				break;
			case 2:
				iterations = 3;
				break;
			default:
				iterations = 1;
				break;
		}
		
		for (int i = 0; i < iterations; i++)
		{
			//Make the createID of FormationData the one that is next

			// HERE IS THE FUCKING CUNT-ASS GARBAGE  PROBLEM
			formation.Add(new FormationData(formCounter + i));
		}
		//Debug.Log(formation.Count);
		//This can be done because its being created inside a for in gameData
	}
}
