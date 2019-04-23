using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;

//[System.Serializable]
public class GameData
{

	//Party
	[XmlArray("Party")]
	[XmlArrayItem("Character")]
	public List<CharacterData> Party;
	//Party
	[XmlArray("Stats")]
	[XmlArrayItem("Char")]
	public List<StatSpread> CharStats;
	//Party Formation
	[XmlArray("PartyFormation")]
	[XmlArrayItem("Member")]
	public List<FormationData> Formation;
	//Monsters
	[XmlArray("Monsters")]
	[XmlArrayItem("Monster")]
	public List<EnemyData> EnemyCollection;
	//Enemy encounters (Formations what enemies, what level etc.)
	[XmlArray("FullFormations")]
	[XmlArrayItem("FullFormation")]
	public List<FullFormationData> FullFormationsCollection;
	//Encounter groups that can appear on a map
	[XmlArray("MapEncounters")]
	[XmlArrayItem("MapEncounter")]
	public List<EncounterMap> MapEncounterCollection;
	//Attacks
	[XmlArray("Attacks")]
	[XmlArrayItem("Attack")]
	public List<AttackData> AttackList;
	//Level Exp Requirements
	[XmlArray("Requirements")]
	[XmlArrayItem("Requirement")]
	public List<ExpRequirements> LevelRequirement;
	//Item & Inventory
	[XmlArray("Items")]
	[XmlArrayItem("Item")]
	public List<ItemData> ItemsCollection;
	[XmlArray("Inventory")]
	[XmlArrayItem("InventorySlot")]
	public List<InventoryData> ItemInventory;
	//Dialogue
	[XmlArray("Dialogues")]
	[XmlArrayItem("Dialogue")]
	public List<DialogueData> DialogueCollection;

	public GameData()
	{
		//Debug.Log("test1");
		LoadStatSpread();
		//Debug.Log("test2");

		LoadCharacters();
		LoadMonsters();

		LoadAttacks();

		/*
		Formation = new List<FormationData>();
		for (int i = 0; i < 4; i++) Formation.Add(new FormationData(i));
		*/


		LoadFormations();

		LoadEncounters();

		LoadRequirements();

		LoadItems();
		LoadInventory();
	}

	public void LoadStatSpread()
	{
		CharStats = new List<StatSpread>();
		string fullText = DataManager.LoadTextFromFile("Data/StatSpread");
		string[] linesText = DataManager.ReadLinesFromString(fullText);
		for (int i = 1; i < linesText.Length; i++) CharStats.Add(new StatSpread(linesText[i]));
	}

	public void LoadCharacters()
	{
		Party = new List<CharacterData>();
		string fullText = DataManager.LoadTextFromFile("Data/Characters"); //No need to put Resources as it´s already loading from inside Resources 
																		   //Debug.Log(fullText);
																		   //Now we need to seperate the text into lines so:
		string[] linesText = DataManager.ReadLinesFromString(fullText);
		for (int i = 1; i < linesText.Length; i++) Party.Add(new CharacterData(linesText[i], CharStats[i-1]));
	}

	public void LoadMonsters()
	{
		EnemyCollection = new List<EnemyData>();
		string fullText = DataManager.LoadTextFromFile("Data/Monsters");

		string[] linesText = DataManager.ReadLinesFromString(fullText);
		for (int i = 1; i < linesText.Length; i++) EnemyCollection.Add(new EnemyData(linesText[i]));
	}

	public void LoadAttacks()
	{
		AttackList = new List<AttackData>();
		string fullText = DataManager.LoadTextFromFile("Data/Attacks");

		string[] linesText = DataManager.ReadLinesFromString(fullText);
		for (int i = 1; i < linesText.Length; i++) AttackList.Add(new AttackData(linesText[i]));
	}

	public void LoadFormations()
	{
		int formCount;
		formCount = 0;
		FullFormationsCollection = new List<FullFormationData>();
		string fullText = DataManager.LoadTextFromFile("Data/FullFormations");

		string[] linesText = DataManager.ReadLinesFromString(fullText);
		for (int i = 1; i < linesText.Length; i++)
		{			
			if (i <= 1) formCount = 0;
			else
			{
				formCount += FullFormationsCollection[i - 2].formations.Length; // FUCK THIS SHIIIIT
			}
			FullFormationsCollection.Add(new FullFormationData(linesText[i], formCount));
		}
	}

	public void LoadEncounters()
	{
		MapEncounterCollection = new List<EncounterMap>();
		string fullText = DataManager.LoadTextFromFile("Data/EncounterMaps");

		string[] linesText = DataManager.ReadLinesFromString(fullText);

		for (int i = 1; i < linesText.Length; i++) MapEncounterCollection.Add(new EncounterMap(linesText[i]));
	}

	public void LoadRequirements()
	{
		LevelRequirement = new List<ExpRequirements>();
		string fullText = DataManager.LoadTextFromFile("Data/ExpRequirements");

		string[] linesText = DataManager.ReadLinesFromString(fullText);

		for (int i = 1; i < linesText.Length; i++) LevelRequirement.Add(new ExpRequirements(linesText[i]));
	}

	public void LoadItems()
	{
		ItemsCollection = new List<ItemData>();
		string fullText = DataManager.LoadTextFromFile("Data/Items");

		string[] linesText = DataManager.ReadLinesFromString(fullText);
		for (int i = 1; i < linesText.Length; i++) ItemsCollection.Add(new ItemData(linesText[i]));
	}

	public void LoadInventory()
	{
		ItemInventory = new List<InventoryData>();
		string fullText = DataManager.LoadTextFromFile("Data/Inventory");

		string[] linesText = DataManager.ReadLinesFromString(fullText);
		for (int i = 1; i < linesText.Length; i++) ItemInventory.Add(new InventoryData(linesText[i]));
	}

	public void LoadDialogue()
	{
		DialogueCollection = new List<DialogueData>();
		string fullText = DataManager.LoadTextFromFile("Data/Dialogue");

		string[] linesText = DataManager.ReadLinesFromString(fullText);
		for (int i = 1; i < linesText.Length; i++) DialogueCollection.Add(new DialogueData(linesText[i]));
	}
}


public static class GameDataManager
{
	//public static GameData data; //OPtional

	public static void Save(GameData data, string fileName) //Other alternatives to string filename is an int slot and have a local string filenname that uses slot int
	{
		Debug.Log("[GameDataManager] Save");

		string path = Application.persistentDataPath + "/Data";
		Debug.Log("[GameDataManager] path " + path);

		try //try doing the following
		{
			//DataManager.SaveToXML<GameData>(data, fileName, path);
			DataManager.SaveToText<GameData>(data, fileName, path);
			Debug.Log("[GameDataManager] Save successful");
		}
		catch (Exception e) //in case of failure get what the issue is
		{
			Debug.LogError("[GameDataManager] Save error" + e);
		}

	}
	public static GameData Load(string fileName)
	{
		Debug.Log("[GameDataManager] Load");
		GameData data;

		string path = Application.persistentDataPath + "/Data";
		Debug.Log("[GameDataManager] path " + path);

		try //Optional depending on how load works
		{
			//data = (GameData)DataManager.LoadFromXML<GameData>(fileName, path);
			data = (GameData)DataManager.LoadFromText<GameData>(fileName, path);
			Debug.Log("Load Successful");
		}
		catch (Exception e)
		{
			Debug.LogError("[GameDataManager] Load error: " + e);
			data = NewGame();
		}

		return data;
	}
	public static GameData NewGame()
	{
		return new GameData();
	}
	public static void Delete(string fileName)
	{
		string path = Application.persistentDataPath + "/Data";
		if (File.Exists(path + "/" + fileName))
		{
			File.Delete(path + "/" + fileName);
		}
	}

	public static bool ExistsGame(string fileName)
	{
		string path = Application.persistentDataPath + "/Data";
		if (File.Exists(path + "/" + fileName))
		{
			return true;
		}
		else return false;
	}
}

