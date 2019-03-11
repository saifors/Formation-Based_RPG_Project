using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;

[System.Serializable]
public class GameData
{

	//Party
	[XmlArray("Party")]
	[XmlArrayItem("Character")]
	public List<CharacterData> Party;
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
	//Attacks
	[XmlArray("Attacks")]
	[XmlArrayItem("Attack")]
	public List<AttackData> AttackList;

	public GameData()
	{
		Party = new List<CharacterData>();
		for (int i = 0; i < 4; i++) Party.Add(new CharacterData(i));

		Formation = new List<FormationData>();
		for (int i = 0; i < 4; i++) Formation.Add(new FormationData(i));

		EnemyCollection = new List<EnemyData>();
		for (int i = 0; i < 10; i++) EnemyCollection.Add(new EnemyData(i));

		FullFormationsCollection = new List<FullFormationData>();
		for (int i = 0; i < 10; i++) FullFormationsCollection.Add(new FullFormationData(i));

		AttackList = new List<AttackData>();
		for (int i = 0; i < 15; i++) AttackList.Add(new AttackData(i));
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
			DataManager.SaveToXML<GameData>(data, fileName, path);
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
			data = (GameData)DataManager.LoadFromXML<GameData>(fileName, path);
			Debug.Log("Load Successful");
		}
		catch (Exception e)
		{
			//Debug.LogError("[GameDataManager] Load error: " + e);
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

