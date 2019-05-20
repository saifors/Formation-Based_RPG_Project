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


	public CharacterData(string characterString, StatSpread statSpread)
	{
		//Debug.Log("test2.2");
		string[] cols = characterString.Split('\t');
		id = int.Parse(cols[0]);
		name = cols[1];
		level = int.Parse(cols[2]);
		exp = int.Parse(cols[3]);
		//Debug.Log("test3");
		hp = statSpread.hpBase;
		//Debug.Log("test4");
		mp = statSpread.mpBase;
		attack = statSpread.attackBase;
		defense = statSpread.defenseBase;
		resistance = statSpread.resistanceBase;
		speed = statSpread.speedBase;
		currentHp = hp;
		currentMp = mp;
		
		modelId = int.Parse(cols[12]);
//Debug.Log("test5");
		attacksLearned = statSpread.baseAttacks;
		//Debug.Log("test6");
		attackAmount = attacksLearned.Count;
		//Debug.Log("test7");
	}
	
}

public class StatSpread
{
	[XmlAttribute("StatID")]
	public int id;

	[XmlElement("HPBase")]
	public int hpBase;
	[XmlElement("MPBase")]
	public int mpBase;
	[XmlElement("AttackBase")]
	public int attackBase;
	[XmlElement("DefenseBase")]
	public int defenseBase;
	[XmlElement("ResistanceBase")]
	public int resistanceBase;
	[XmlElement("SpeedBase")]
	public int speedBase;

	[XmlElement("StartAttacks")]
	public List<int> baseAttacks;

	[XmlElement("HPGrowth")]
	public int hpGrowth;
	[XmlElement("MPGrowth")]
	public int mpGrowth;
	[XmlElement("AttackGrowth")]
	public int attackGrowth;
	[XmlElement("DefenseBase")]
	public int defenseGrowth;
	[XmlElement("ResistanceGrowth")]
	public int resistanceGrowth;
	[XmlElement("SpeedGrowth")]
	public int speedGrowth;

	[XmlElement("LearnableAttacks")]
	public List<int> learnAttacks;
	[XmlElement("LevelAttacks")]
	public List<int> levelAttacks;

	public StatSpread(string statString)
	{
		string[] cols = statString.Split('\t');
		id = int.Parse(cols[0]);
		hpBase = int.Parse(cols[1]);
		mpBase = int.Parse(cols[2]);
		attackBase = int.Parse(cols[3]);
		defenseBase = int.Parse(cols[4]);
		resistanceBase = int.Parse(cols[5]);
		speedBase = int.Parse(cols[6]);

		baseAttacks = new List<int>();
		string[] bAttacks = cols[7].Split(',');
		for (int i = 0; i < bAttacks.Length; i++)
		{
			baseAttacks.Add(int.Parse(bAttacks[i]));
		}

		hpGrowth = int.Parse(cols[8]);
		mpGrowth = int.Parse(cols[9]);
		attackGrowth = int.Parse(cols[10]);
		defenseGrowth = int.Parse(cols[11]);
		resistanceGrowth = int.Parse(cols[12]);
		speedGrowth = int.Parse(cols[13]);

		learnAttacks = new List<int>();
		string[] leAttacks = cols[14].Split(',');
		for (int i = 0; i < leAttacks.Length; i++)
		{
			learnAttacks.Add(int.Parse(leAttacks[i]));
		}

		levelAttacks = new List<int>();
		string[] lvAttacks = cols[15].Split(',');
		for (int i = 0; i < lvAttacks.Length; i++)
		{
			levelAttacks.Add(int.Parse(lvAttacks[i]));
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

	[XmlElement("Model")]
	public int modelId;
	[XmlElement("Attacks")]
	public List<int> attacksLearned;
	public int attackAmount;
	[XmlElement("Drops")]
	public int[] itemDrops;

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

		if(cols.Length > 13)
		{
			attackAmount = attacksLearned.Count;
			string[] drops = cols[13].Split(',');
			itemDrops = new int[drops.Length];
			for (int i = 0; i < drops.Length; i++)
			{
				itemDrops[i] = int.Parse(drops[i]);
			}
		}
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
	[XmlElement("Particle")]
	public int partID;
	[XmlElement("SFX")]
	public int soundID;

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

		partID = int.Parse(cols[8]);
		soundID = int.Parse(cols[9]);
	}
}

public class ItemData
{
	public enum ItemEffect { Heal20_F, Heal50_F, Heal100_F, Heal200_F, Heal500_F, Heal50_P, Heal100_P, Recover50_F, Recover100_F, Recover300_F, Recover50_P, Recover100_P, CurePoison, None };//Heal is HP, Recover is MP Cure is Status effect
	[XmlAttribute("ItemID")]
	public int id;
	[XmlElement("Name")]
	public string name;
	[XmlElement("Description")]
	public string description;
	[XmlElement("Effect")]
	public ItemEffect effect;
	[XmlElement("Rarity")]
	public int rarity;

	public ItemData(string itemString)
	{
		string[] cols = itemString.Split('\t');

		id = int.Parse(cols[0]);
		name = cols[1];
		description = cols[2];
		effect = (ItemEffect)System.Enum.Parse(typeof(ItemEffect), cols[3]);
		rarity = int.Parse(cols[4]);
	}
}

public class InventoryData
{
	[XmlAttribute("SlotID")]
	public int slot;
	[XmlElement("ItemID")]
	public int itemId;
	[XmlElement("Amount")]
	public int amount;
	/*[XmlElement("Item")]
	public ItemData item;*/

	public InventoryData(string inventoryString)
	{
		string[] cols = inventoryString.Split('\t');

		slot = int.Parse(cols[0]);
		itemId = int.Parse(cols[1]);
		amount = int.Parse(cols[2]);
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
	[XmlElement("Music")]
	public int music;

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

		music = int.Parse(cols[2]);
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
	[XmlElement("BattleMusic")]
	public int battleMusicID;
	[XmlElement("OverworldMusic")]
	public int owMusicID;
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

		battleMusicID = int.Parse(cols[2]);
		owMusicID = int.Parse(cols[3]);
	}
}

public class ExpRequirements
{
	[XmlAttribute("level")]
	public int level;
	[XmlElement("experience")]
	public int exp;
	public ExpRequirements(string expString)
	{
		string[] cols = expString.Split('\t');

		level = int.Parse(cols[0]);
		exp = int.Parse(cols[1]);
	}
}

public class EventData
{
	public int id;
	public bool hasBeenTriggered;
	public EventData(string eventsString)
	{
		string[] cols = eventsString.Split('\t');

		id = int.Parse(cols[0]);
		hasBeenTriggered = bool.Parse(cols[1]);
		//Debug.Log(id + " is " + hasBeenTriggered);
	}
}

