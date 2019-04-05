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


	public CharacterData(string characterString)
	{
		string[] cols = characterString.Split('\t');
		id = int.Parse(cols[0]);
		name = cols[1];
		level = int.Parse(cols[2]);
		exp = int.Parse(cols[3]);
		hp = int.Parse(cols[4]);
		mp = int.Parse(cols[5]);
		attack = int.Parse(cols[6]);
		defense = int.Parse(cols[7]);
		resistance = int.Parse(cols[8]);
		speed = int.Parse(cols[9]);
		currentHp = int.Parse(cols[10]);
		currentMp = int.Parse(cols[11]);
		modelId = int.Parse(cols[12]);

		attacksLearned = new List<int>();
		string[] attacks = cols[13].Split(',');
		for (int i = 0; i < attacks.Length; i++)
		{
			attacksLearned.Add(int.Parse(attacks[i]));
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

	public EnemyData(string monsterString)
	{
		string[] cols = monsterString.Split('\t');
		id = int.Parse(cols[0]);
		name = cols[1];
		level = int.Parse(cols[2]);
		hp = int.Parse(cols[3]);
		mp = int.Parse(cols[4]);
		attack = int.Parse(cols[5]);
		defense = int.Parse(cols[6]);
		resistance = int.Parse(cols[7]);
		speed = int.Parse(cols[8]);

		expGain = int.Parse(cols[9]);
		goldGain = int.Parse(cols[10]);

		modelId = int.Parse(cols[11]);

		attacksLearned = new List<int>();

		string[] attacks = cols[12].Split(',');
		for (int i = 0; i < attacks.Length; i++)
		{
			attacksLearned.Add(int.Parse(attacks[i]));
		}

		attackAmount = attacksLearned.Count;
	}
}

public class AttackData
{
	[XmlAttribute("AttackID")]
	public int id;
	[XmlElement("Name")]
	public string nameKey;
	[XmlElement("Description")]
	public string descKey;
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

	public AttackData(string attackString)
	{
		string[] cols = attackString.Split('\t');
		
		
		

		id = int.Parse(cols[0]);
		nameKey = cols[1];
		descKey = cols[2];
		
		
		/*if (magic == 1) isMagic = true;
		else isMagic = false;*/
		isMagic = bool.Parse(cols[3]);
		strength = int.Parse(cols[4]);
		mpCost = int.Parse(cols[5]);

		string[] rSizes = cols[6].Split(',');
		rangeSize = new Vector2(int.Parse(rSizes[0]), int.Parse(rSizes[1]));

		
		string[] rActives = cols[7].Split(',');
		List<int> tempRange = new List<int>();

		for (int i = 0; i < rActives.Length; i++)
		{
			tempRange.Add(int.Parse(rActives[i]));
		}

		rangeActive = tempRange.ToArray();
		//Debug.Log("end" + attackString);
	}
}
/*
public class ItemData
{
	public enum ItemEffect { Heal20, Heal50, Heal100, Heal200, Heal500, Recover50, Recover100, Recover300, CurePoison };//Heal is HP, Recover is MP Cure is Status effect
	[XmlAttribute("ItemID")]
	public int id;
	[XmlElement("Name")]
	public string name;
	[XmlElement("Description")]
	public string description;
	[XmlElement("Effect")]
	public ItemEffect effect;

	public ItemData(int createID)
	{
		id = createID;
		switch(id)
		{
			case 0:
				name = "Cheap Potion";
				effect = ItemEffect.Heal20;
				break;
			case 1:
				name = "Potion";
				effect = ItemEffect.Heal50;
				break;
			case 2:
				name = "High Potion";
				effect = ItemEffect.Heal100;
				break;
			case 3:
				break;
			default:
				break;
		}
	}
}
*/
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
	public FormationData(string formString)
	{
		//Debug.Log(formString);
		string[] cols = formString.Split('\t');
		/*for (int i = 0; i < cols.Length; i++)
		{
			Debug.Log(cols[i]);
		}*/
		
		id = int.Parse(cols[0]);
		
		classID = int.Parse(cols[1]);
		
		isMonster = bool.Parse(cols[2]);
		
		
		string[] tileVec = cols[3].Split(',');
		tiles = new Vector2(int.Parse(tileVec[0]), int.Parse(tileVec[1]));

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
	public FormationData[] formations;
	

	public FullFormationData(string uniFormString, int formCount)
	{
		//Debug.Log(uniFormString);
		string[] cols = uniFormString.Split('\t');// This string is for FullFormations
		
		id = int.Parse(cols[0]);
		iterations = int.Parse(cols[1]);

		List<FormationData> formation = new List<FormationData>();

		string fullText = DataManager.LoadTextFromFile("Data/Formations");

		string[] linesText = DataManager.ReadLinesFromString(fullText);

		for (int i = 1; i < iterations + 1; i++)
		{
			//Make the createID of FormationData the one that is next
			

			formation.Add(new FormationData(( linesText[formCount + i] )));//Should work
			
		}
		formations = formation.ToArray();
		//Debug.Log(formations.Length);
		//This can be done because its being created inside a for in gameData
	}
}

public class EncounterMap
{
	[XmlAttribute("MapEncountersID")]
	public int id;
	[XmlElement("FormationGroups")]
	public int[] groupIDs;
	public EncounterMap(string encounterString)
	{
		string[] cols = encounterString.Split('\t');

		id = int.Parse(cols[0]);

		List<int> tempGroup = new List<int>();

		string[] groupMembers = cols[1].Split(',');
		for (int i = 0; i < groupMembers.Length; i++)
		{
			tempGroup.Add(int.Parse(groupMembers[i]));
		}

		groupIDs = tempGroup.ToArray();
	}
}



